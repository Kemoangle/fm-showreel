<script setup lang="ts">
import { useSnackbar } from '@/components/Snackbar.vue';
import { Video } from '@/model/video';
import { useCategoryStore } from '@/store/useCategoryStore';
import { useRuleStore } from '@/store/useRuleStore';
import { useVideoStore } from '@/store/useVideoStore';
import Swal from 'sweetalert2';
import { onMounted, ref } from 'vue';
import Add from './add.vue';

const videoStore = useVideoStore();
const categoryStore = useCategoryStore();
const ruleStore = useRuleStore();

const idUpdate = ref(0);
const searching = ref(false);
const isAddNewVideo = ref(false);
const keySearch = ref('');

const pageSize = ref(10);
const currentPage = ref(1);
const totalPages = ref(1);
const totalItems = ref();
const { showSnackbar } = useSnackbar();

const getAll = async () => {
    await videoStore.getPageVideo(keySearch.value, currentPage.value, pageSize.value);
    console.log(videoStore.data.videos);
};
watchEffect(() => {
    totalPages.value = videoStore.data.totalPages;
    totalItems.value = videoStore.data.totalItems;
    if (currentPage.value > totalPages.value) currentPage.value = totalPages.value;
});

onMounted(() => {
    videoStore.data.videos = !videoStore.data.videos;
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

const handleUpdate = (id: number) => {
    idUpdate.value = id;
    isAddNewVideo.value = true;
};

const deleteVideo = (id: number) => {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!',
    }).then((result) => {
        if (result.isConfirmed) {
            videoStore.deleteVideo(id).then((response) => {
                getAll();
                Swal.fire({
                    title: 'Deleted!',
                    icon: 'success',
                });
            });
        }
    });
};

const addNewVideo = async (videoData: Video) => {
    const { category,doNotPlay,noBackToBack, ...rest } = videoData;
    showSnackbar('Processing...', 'warning');
    if (videoData.id && videoData.id > 0) {
        await videoStore
            .updateVideo(rest)
            .then((response) => {
                categoryStore.updateVideoCategory(videoData.id, category);
                ruleStore.UpdateDoNotPlay(doNotPlay,videoData.id);
                ruleStore.UpdateNoBackToBack(noBackToBack,videoData.id);
            })
            .catch((error) => {
                showSnackbar(error.data.Video[0], 'error');
            });
    } else {
        await videoStore
            .addVideo(rest)
            .then((response) => {
                categoryStore.updateVideoCategory(response.id, category);
                ruleStore.UpdateDoNotPlay(doNotPlay,response.id);
                ruleStore.UpdateNoBackToBack(noBackToBack,response.id);
            })
            .catch((error) => {
                showSnackbar(error.data.Video[0], 'error');
            });
    }
    showSnackbar('Operation completed!', 'success');
    getAll();
};

const randomColor = () => {
    const colors = ['primary', 'secondary', 'success', 'info', 'warning', 'error'];
    const randomColor = colors[Math.floor(Math.random() * colors.length)];
    return randomColor;
};

const handleSearch = async () => {
    searching.value = true;
    videoStore.data.videos = [];
    await new Promise((resolve) => setTimeout(resolve, 1000));
    searching.value = false;
    getAll();
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
                            color="secondary"
                            prepend-icon="mdi-tray-arrow-down"
                            @click="handleUpdate(0)"
                        >
                            Create New Video
                        </VBtn>
                    </VCol>
                   
                    <VCol cols="12" sm="6" class="display">
                        <VTextField
                            placeholder="Search"
                            density="compact"
                            class="me-3"
                            v-model="keySearch"
                            :loading="videoStore.isLoading"
                        />
                        <!-- <VProgressLinear
                            v-if="searching"
                            indeterminate
                            color="primary"
                        /> -->
                        
                    </VCol>
                    <VCol cols="12" sm="2" class="display">
                        <VBtn
                            variant="tonal"
                            color="secondary"
                            prepend-icon="mdi-search"
                            @click="getAll"
                        >
                        </VBtn>
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
                        <th scope="col">TITLE</th>
                        <th scope="col" class="text-center">DURATION</th>
                        <th scope="col" class="text-center">KEY NO</th>
                        <th scope="col">RULE</th>
                        <th scope="col">CATEGORIES</th>
                        <th scope="col" class="text-left">VIDEO TYPE</th>
                        <th scope="col" class="text-center">ACTION</th>
                    </tr>
                </thead>

                <!-- 👉 table body -->
                <tbody>
                    <tr v-for="(video, index) in videoStore.data.videos" :key="video.id">
                        <td>
                            {{ (currentPage - 1) * pageSize + index + 1 }}
                        </td>

                        <td>
                            <div class="d-flex align-center">
                                {{ video.title }}
                            </div>
                        </td>

                        <td class="text-center">
                            {{ video.duration }}
                        </td>

                        <td class="text-center">
                            {{ video.keyNo }}
                        </td>

                        <td style="color: rgb(236, 114, 114);">
                            <p v-if="video.doNotPlay.length">Do pot play on
                                (<span v-for="(item, idx) in video.doNotPlay" :key="item.id">{{item.buildingName}}<span v-if="idx < video.doNotPlay.length - 1">/ </span>
                                </span>)
                            </p>
                            <p v-if="video.noBackToBack.length">No back to back with
                                (<span v-for="(item, idx) in video.noBackToBack" :key="item.id">{{item.name}}<span v-if="idx < video.noBackToBack.length - 1">/ </span>
                                </span>)
                            </p>
                        </td>

                        <td>
                            <VChip
                                size="small"
                                class="text-capitalize ml-2 chip-wrap"
                                v-for="item in video.category"
                                :key="item.id"
                                :color="randomColor()"
                            >
                                {{ item.name }}
                            </VChip>
                        </td>

                        <td class="text-left">
                            {{ video.videoType.name }}
                        </td>

                        <td class="text-center">
                            <VBtn size="x-small" color="default" variant="plain" icon>
                                <VIcon size="24" icon="mdi-dots-vertical" />

                                <VMenu activator="parent">
                                    <VList>
                                        <VListItem @click="handleUpdate(video.id)">
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

                                        <VListItem @click="deleteVideo(video.id)">
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
                <tfoot v-show="videoStore.isLoading">
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
        <Add
            v-model:isDrawerOpen="isAddNewVideo"
            @video-data="addNewVideo"
            v-model:videoId="idUpdate"
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
