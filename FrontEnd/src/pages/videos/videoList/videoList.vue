<script setup lang="ts">
import PopupCreateListVideo from '@/components/PopupCreateListVideo.vue';
import { useSnackbar } from '@/components/Snackbar.vue';
import ViewListVideo from '@/components/ViewListVideo.vue';
import { VideoList } from '@/model/videoList';
import { useVideoListStore } from '@/store/useVideoListStore';
import moment from 'moment';
import Swal from 'sweetalert2';
import { onMounted, ref } from 'vue';

const videoListStore = useVideoListStore();
const videoListId = ref(0);
const isViewListVideo = ref(false);
const searching = ref(false);
const keySearch = ref('');
const pageSize = ref(10);
const currentPage = ref(1);
const totalPages = ref(1);
const totalItems = ref();
const { showSnackbar } = useSnackbar();
const idUpdate = ref(0);

const isDialogListVideoVisible = ref(false);

const getAll = () => {
    videoListStore.getPageVideoList(keySearch.value, currentPage.value, pageSize.value);
};
watchEffect(() => {
    totalPages.value = videoListStore.data.totalPages;
    totalItems.value = videoListStore.data.totalItems;
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

const handleSubmit = async (videoListData: VideoList) => {
    const { videoVideoList, ...rest } = videoListData;
    if (videoListData.id && videoListData.id > 0) {
        await videoListStore
            .updateVideoList(rest)
            .then((response) => {
                videoListStore.addVideoVideoList(videoVideoList, response.id);
            })
            .catch((error) => {
                showSnackbar(error.data.VideoList[0], 'error');
            });
    } else {
        await videoListStore
            .addVideoList(rest)
            .then((response) => {
                videoListStore.addVideoVideoList(videoVideoList, response.id);
            })
            .catch((error) => {
                showSnackbar(error.data.VideoList[0], 'error');
            });
    }
    getAll();
};

const viewListVideo = (id: number) => {
    videoListId.value = id;
    isViewListVideo.value = true;
};

const deleteList = async (id: number) => {
    await Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!',
    }).then((result) => {
        if (result.isConfirmed) {
            videoListStore.deleteList(id).then((response) => {
                getAll();
                Swal.fire({
                    title: 'Deleted!',
                    icon: 'success',
                });
            });
        }
    });
};

const openForm = (id: number) => {
    idUpdate.value = id;
    isDialogListVideoVisible.value = true;
};
</script>

<template>
    <section>
        <VCard class="mb-6">
            <VCardText>
                <VRow>
                    <VCol cols="12" sm="4">
                        <VBtn color="primary" prepend-icon="mdi-plus-thick" @click="openForm(0)">
                            Create List Video
                        </VBtn>
                    </VCol>
                    <VCol cols="12" sm="8" class="display">
                        <VTextField
                            placeholder="Search"
                            density="compact"
                            class="me-3"
                            @input="getAll"
                            v-model="keySearch"
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
                        <th scope="col">#</th>
                        <th scope="col">Title</th>
                        <th scope="col">Create time</th>
                        <th scope="col" class="text-center">Action</th>
                    </tr>
                </thead>

                <!-- 👉 table body -->
                <tbody>
                    <tr v-for="(item, index) in videoListStore.data.videolist" :key="item.id">
                        <td>
                            {{ (currentPage - 1) * pageSize + index + 1 }}
                        </td>

                        <td>
                            <div class="d-flex flex-column">
                                <h6 class="text-sm font-weight-medium">
                                    <a
                                        @click="viewListVideo(item.id)"
                                        class="font-weight-medium user-list-name"
                                        style="cursor: pointer"
                                    >
                                        {{ item.title }}
                                    </a>
                                </h6>
                            </div>
                        </td>

                        <td>
                            {{ moment(item.createdTime).format('DD-MM-YYYY') }}
                        </td>

                        <td class="text-center">
                            <VBtn size="x-small" color="default" variant="plain" icon>
                                <VIcon size="24" icon="mdi-dots-vertical" />

                                <VMenu activator="parent">
                                    <VList>
                                        <VListItem @click="openForm(item.id)">
                                            <template #prepend>
                                                <VIcon
                                                    icon="mdi-pencil-outline"
                                                    :size="20"
                                                    class="me-3"
                                                    color="warning"
                                                />
                                            </template>
                                            <VListItemTitle>Edit</VListItemTitle>
                                        </VListItem>
                                        <VListItem @click="deleteList(item.id)">
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
                </tbody>

                <!-- 👉 table footer  -->
                <tfoot v-show="!videoListStore.data.videolist">
                    <tr>
                        <td colspan="7" class="text-center">No data available</td>
                    </tr>
                </tfoot>
            </VTable>
            <VDivider />
            <!-- SECTION Pagination -->
            <VCardText class="d-flex flex-wrap justify-end gap-4 pa-2">
                <!-- 👉 Rows per page -->
                <div class="d-flex align-center me-3" style="width: 171px">
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
        <PopupCreateListVideo
            v-model:is-dialog-visible="isDialogListVideoVisible"
            @video-list-data="handleSubmit"
            :video-list-id="idUpdate"
        />
        <ViewListVideo v-model:videoListId="videoListId" v-model:isDrawerOpen="isViewListVideo" />
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
