<script setup lang="ts">
import { Video } from '@/model/video';
import axiosIns from '@/plugins/axios';
import { useCategoryStore } from '@/store/useCategoryStore';
import { useVideoStore } from '@/store/useVideoStore';
import { onMounted, ref } from 'vue';
import Add from './add.vue';

const videoStore = useVideoStore();
const categoryStore = useCategoryStore();

const idUpdate = ref(0);

const isAddNewVideo = ref(false);
const keySearch = ref('');

const pageSize = ref(3);
const currentPage = ref(1);
const totalPages = ref(1);
const totalItems = ref();

const getAll = () => {
    videoStore.getPageVideo(keySearch.value, currentPage.value, pageSize.value);
};
watchEffect(() => {
    totalPages.value = videoStore.data.totalPages;
    totalItems.value = videoStore.data.totalItems;
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

const handleUpdate = (id: number) => {
    idUpdate.value = id;
    isAddNewVideo.value = true;
};

const deleteVideo = (id: number) => {
    videoStore.deleteVideo(id).then((response) => {
        getAll();
    });
};

const addNewVideo = (videoData: Video) => {
    const {category, ...rest} = videoData;
    if (videoData.id && videoData.id > 0) {
        
        videoStore.updateVideo(rest).then((response) => {
            getAll();
        });
        axiosIns.patch('Category/' + videoData.id, videoData.category).then((response) => {
            getAll();
        });
    } else {
        axiosIns.post('Video', rest).then((response) => {
            getAll();
        });
    }
};
</script>

<template>
    <section>
        <VCard>
            <VCardText class="d-flex flex-wrap gap-4">
                <VSpacer />
                <div class="app-user-search-filter d-flex align-center">
                    <!-- 👉 Search  -->
                    <VTextField
                        placeholder="Building Name"
                        density="compact"
                        class="me-3"
                        @input="getAll"
                        v-model="keySearch"
                    />

                    <!-- 👉 Add user button -->
                    <VBtn @click="isAddNewVideo = true"> Add New Video </VBtn>
                </div>
            </VCardText>

            <VDivider />

            <VTable class="text-no-wrap">
                <!-- 👉 table head -->
                <thead>
                    <tr>
                        <th scope="col">#</th>
                        <th scope="col">TITLE</th>
                        <th scope="col">KEY NO</th>
                        <th scope="col">RULE</th>
                        <th scope="col">CATEGORIES</th>
                        <th scope="col">ACTION</th>
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

                        <td class="">
                            {{ video.keyNo }}
                        </td>

                        <td>
                            <span class="">{{ video.rule }}</span>
                        </td>

                        <td>
                                <VChip
                                    color="success"
                                    size="small"
                                    class="text-capitalize ml-2"
                                    v-for="item in video.category" :key="item.id" 
                                >
                                {{ item.name }}
                                </VChip>
                        </td>

                        <td>
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
                <tfoot v-show="!videoStore.data.videos">
                    <tr>
                        <td colspan="7" class="text-center">No data available</td>
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
