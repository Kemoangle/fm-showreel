<script setup lang="ts">
import PopupCreateListVideo from '@/components/PopupCreateListVideo.vue';
import { useVideoListStore } from '@/store/useVideoListStore';
import { onMounted, ref } from 'vue';

const videoListStore = useVideoListStore();

const keySearch = ref('');
const pageSize = ref(3);
const currentPage = ref(1);
const totalPages = ref(1);
const totalItems = ref();

const selectedListVideo = ref();
const isDialogListVideoVisible = ref(false);
const isDialogListVideoCurrent = ref(false);
const handleCloseDialog = () => {
    isDialogListVideoVisible.value = !isDialogListVideoVisible.value;
};
const handleCloseDialogShowListVideo = () => {
    isDialogListVideoCurrent.value = !isDialogListVideoCurrent.value;
};
const handleClickCreateListVideo = () => {
    isDialogListVideoVisible.value = !isDialogListVideoVisible.value;
};

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
                            @click="handleClickCreateListVideo"
                        >
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
                        <th scope="col">Remark</th>
                        <th scope="col">Create time</th>
                        <th scope="col">Last update time</th>
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
                            <div class="d-flex align-center">
                                {{ item.title }}
                            </div>
                        </td>

                        <td>
                            {{ item.remark }}
                        </td>
                        <td>
                            {{ item.createTime }}
                        </td>
                        <td>
                            {{ item.lastUpdateTime }}
                        </td>

                        <td class="text-center">
                            <VBtn size="x-small" color="default" variant="plain" icon>
                                <VIcon size="24" icon="mdi-dots-vertical" />

                                <VMenu activator="parent">
                                    <VList>
                                        <VListItem >
                                            <template #prepend>
                                                <VIcon
                                                    icon="mdi-pencil-outline"
                                                    :size="20"
                                                    class="me-3"
                                                />
                                            </template>
                                            <VListItemTitle>Edit</VListItemTitle>
                                        </VListItem>

                                        <VListItem >
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
        <PopupCreateListVideo
            :is-dialog-visible="isDialogListVideoVisible"
            @close="handleCloseDialog"
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
