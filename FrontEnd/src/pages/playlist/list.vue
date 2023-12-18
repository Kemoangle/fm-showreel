<script setup lang="ts">
import { useSnackbar } from '@/components/Snackbar.vue';
import Swal from 'sweetalert2';
import { onMounted, ref } from 'vue';
import { useDebounce } from '@vueuse/core'
import { usePlaylistStore } from '@/store/usePlayListStore';
import PopupViewPlaylist from '@/components/PopupViewPlaylist.vue';
import { IPostPlaylistStore } from '@/model/playlist';

const playlistStore = usePlaylistStore();

const keySearch = ref('');
const keySearchDebounce = useDebounce(keySearch, 1000)

const pageSize = ref(10);
const currentPage = ref(1);
const totalPages = ref(1);
const totalItems = ref();
const isViewPlaylist = ref(false)
const playlistActive = ref<IPostPlaylistStore>()

const { showSnackbar } = useSnackbar();

const getAll = () => {
    playlistStore.getPlaylists(keySearchDebounce.value.trim(), currentPage.value, pageSize.value);
};

watchEffect(() => {
    totalPages.value = playlistStore.data.totalPages;
    totalItems.value = playlistStore.data.totalItems;
    if (currentPage.value > totalPages.value) currentPage.value = totalPages.value;
});

onMounted(() => {
    getAll();
});

const changePage = (newPage: number) => {
    currentPage.value = newPage;
    getAll();
};

watch(currentPage, () => {
    getAll();
});

watch(pageSize, () => {
    getAll();
});

watch(keySearchDebounce, () => {
    getAll();
});

// const handleSubmit = async (buildingData: Building) => {
//     if (buildingData.id && buildingData.id > 0) {
//         await playlistStore.updateBuilding(buildingData).catch((error) => {
//             showSnackbar(error.data.Building[0], 'error');
//         });
//     } else {
//         await playlistStore.addBuilding(buildingData).catch((error) => {
//             showSnackbar(error.data.Building[0], 'error');
//         });
//     }

//     getAll();
// };

// const handleUpdate = (id: number) => {
//     idUpdate.value = id;
//     isAddNewBuilding.value = true;
// };

const viewPlaylist = (playlist:IPostPlaylistStore)=>{
    isViewPlaylist.value = true
    playlistActive.value = playlist
}

const deleteBuilding = (id: number) => {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
        }).then((result) => {
        if (result.isConfirmed) {
            playlistStore.removePlaylist(id).then((response) => {
                Swal.fire({
                    title: "Deleted!",
                    icon: "success"
                    });
                getAll();
            });
        }
    });
};

</script>

<template>
    <section>
        <VCard class="mb-6">
            <VCardText>
                <VRow>
                    <VCol cols="12" sm="4">
                        <VBtn
                            variant="tonal"
                            :to="{
                                name: 'playlist-generator',
                            }"
                            color="secondary"
                        >
                            Generator playlist
                        </VBtn>
                    </VCol>
                    <VCol cols="12" sm="8" class="display">
                        <VTextField
                            placeholder="Search"
                            density="compact"
                            class="me-3"
                            v-model="keySearch"
                            :loading="playlistStore.isLoading"
                        />
                    </VCol>
                </VRow>
            </VCardText>
        </VCard>

        <VCard>
            <VDivider />
            <VTable class="text-no-wrap">
                <!-- 👉 table head -->
                <thead>
                    <tr>
                        <th scope="col">STT</th>
                        <th scope="col">TITLE</th>
                        <th scope="col">STATUS</th>
                        <th scope="col">CREATOR</th>
                        <th scope="col">Action</th>
                    </tr>
                </thead>

                <!-- 👉 table body -->
                <tbody>
                    <tr class="handle" v-for="(playlist, index) in playlistStore.data" :key="index">
                        <td>
                            {{ index + 1 }}
                        </td>
                        <td>
                            {{ playlist.title }}
                        </td>
                        <td>
                            {{ playlist.status }}
                        </td>
                        <td>Admin</td>
                        <td>
                            <VBtn size="x-small" color="default" variant="plain" icon>
                                <VIcon size="24" icon="mdi-dots-vertical" />

                                <VMenu activator="parent">
                                    <VList>
                                        <VListItem @click="viewPlaylist(playlist)">
                                            <template #prepend>
                                                <VIcon
                                                    icon="mdi-eye-outline"
                                                    :size="20"
                                                    class="me-3"
                                                    color="info"
                                                />
                                            </template>
                                            <VListItemTitle>View</VListItemTitle>
                                        </VListItem>

                                        <VListItem @click="deleteBuilding(playlist.id)">
                                            <template #prepend>
                                                <VIcon
                                                    icon="mdi-delete-outline"
                                                    :size="20"
                                                    class="me-3"
                                                    color="error"
                                                />
                                            </template>
                                            <VListItemTitle>Delete</VListItemTitle>
                                        </VListItem>
                                    </VList>
                                </VMenu>
                            </VBtn>
                        </td>
                    </tr>

                    <!-- 👉 table footer  -->
                    <tfoot v-show="!playlistStore.data">
                    <tr>
                        <td colspan="7" class="text-center">
                            <v-row align="center" justify="center" class="fill-height">
                                <v-col cols="12" class="text-center">
                                    <VProgressCircular indeterminate color="info" />
                                </v-col>
                            </v-row>
                        </td>
                    </tr>
                </tfoot>
                </tbody>
            </VTable>

            <VDivider />
                <!-- SECTION Pagination -->
                <VCardText class="d-flex flex-wrap justify-end gap-4 pa-2">
                <!-- 👉 Rows per page -->
                <div class="d-flex align-center me-3" style="width: 171px;">
                    <span class="text-no-wrap me-3">Rows per page:</span>

                    <VSelect
                        v-model="pageSize"
                        density="compact"
                        variant="plain"
                        class="user-pagination-select"
                        :items="[5, 10, 20, 30, 50]"
                    />
                </div>

                <!-- 👉 Pagination and pagination meta -->
                <div class="d-flex align-center">
                    <VPagination
                        v-model="currentPage"
                        :length="totalPages"
                        :total-visible="1"
                        rounded="circle"
                        @input="changePage"
                    />
                </div>
            </VCardText>
            <!-- !SECTION -->
        </VCard>
        <PopupViewPlaylist
            :data="playlistActive"
            v-model:is-drawer-open="isViewPlaylist"
        />
    </section>
</template>

<style lang="scss">
.app-user-search-filter {
    inline-size: 24.0625rem;
}

.text-capitalize {
    text-transform: capitalize;
}

.user-list-name:not(:hover) {
    color: rgba(var(--v-theme-on-background), var(--v-high-emphasis-opacity));
}
</style>

<style lang="scss" scope>
.user-pagination-select {
    .v-field__input,
    .v-field__append-inner {
        padding-block-start: 0.3rem;
    }
}
</style>
