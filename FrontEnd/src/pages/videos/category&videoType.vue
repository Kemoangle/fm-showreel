<script setup lang="ts">
import { useSnackbar } from '@/components/Snackbar.vue';
import { Category } from '@/model/category';
import { VideoType } from '@/model/videoType';
import { useCategoryStore } from '@/store/useCategoryStore';
import { useVideoTypeStore } from '@/store/useVideoTypeStore';
import { onMounted, ref } from 'vue';
import AddCategory from './addCategory.vue';
import AddVideoType from './addVideoType.vue';

const categoryStore = useCategoryStore();
const keySearchCategory = ref('');
const pageSizeCategory = ref(10);
const currentPageCategory = ref(1);
const totalPageCategory = ref(1);
const totalItemCategory = ref();

const videoTypeStore = useVideoTypeStore();
const keySearchVideoType = ref('');
const pageSizeVideoType = ref(10);
const currentPageVideoType = ref(1);
const totalPageVideoType = ref(1);
const totalItemVideoType = ref();

const idUpdateVideotype = ref(0);
const isDrawerOpenVideoType = ref(false);

const idUpdateCategory = ref(0);
const isDrawerOpenCategory = ref(false);
const { showSnackbar } = useSnackbar();

const getAll = () => {
    categoryStore.getPageCategory(
        keySearchCategory.value,
        currentPageCategory.value,
        pageSizeCategory.value
    );
    videoTypeStore.getPageVideoType(
        keySearchVideoType.value,
        currentPageVideoType.value,
        pageSizeVideoType.value
    );
};
watchEffect(() => {
    totalPageCategory.value = categoryStore.data.totalPages;
    totalItemCategory.value = categoryStore.data.totalItems;
    if (currentPageCategory.value > totalPageCategory.value)
        currentPageCategory.value = totalPageCategory.value;
    //videoType
    totalPageVideoType.value = videoTypeStore.data.totalPages;
    totalItemVideoType.value = videoTypeStore.data.totalItems;
    if (currentPageVideoType.value > totalPageVideoType.value)
        currentPageVideoType.value = totalPageVideoType.value;
});

onMounted(() => {
    getAll();
});
const changePage = (newPage: number) => {
    currentPageCategory.value = newPage;
    getAll();
};

const changePageVideoType = (newPage: number) => {
    currentPageVideoType.value = newPage;
    getAll();
};

watch(currentPageCategory, () => {
    getAll();
});

watch(pageSizeCategory, () => {
    getAll();
});
watch(currentPageVideoType, () => {
    getAll();
});

watch(pageSizeVideoType, () => {
    getAll();
});

const openFromVideo = (id: number) => {
    isDrawerOpenVideoType.value = true;
    idUpdateVideotype.value = id;
};
const openFromCategory = (id: number) => {
    isDrawerOpenCategory.value = true;
    idUpdateCategory.value = id;
};
const submitVideoType = async (videoTypeData: VideoType) => {
    console.log(videoTypeData.name);
    if (videoTypeData.id && videoTypeData.id > 0) {
        await videoTypeStore.updateVideoType(videoTypeData).catch((error) => {
            showSnackbar(error.data.VideoType[0], 'error');
        });
    } else {
        await videoTypeStore.addVideoType(videoTypeData).catch((error) => {
            showSnackbar(error.data.VideoType[0], 'error');
        });
    }
    getAll();
};

const submitCategory = async (categoryData: Category) => {
    console.log(categoryData.name);
    if (categoryData.id && categoryData.id > 0) {
        await categoryStore.updateCategory(categoryData).catch((error) => {
            showSnackbar(error.data.Category[0], 'error');
        });
    } else {
        await categoryStore.addCategory(categoryData).catch((error) => {
            showSnackbar(error.data.Category[0], 'error');
        });
    }

    getAll();
};
</script>

<template>
    <section>
        <VRow>
            <VCol cols="6">
                <VCard class="mb-2">
                    <VCardText>
                        <VRow>
                            <VCol cols="12" sm="4">
                                <VBtn
                                    variant="tonal"
                                    color="secondary"
                                    prepend-icon="mdi-tray-arrow-down"
                                    @click="openFromCategory(0)"
                                >
                                    Add Category
                                </VBtn>
                            </VCol>
                            <VCol cols="12" sm="8" class="display">
                                <VTextField
                                    placeholder="Search"
                                    density="compact"
                                    class="me-3"
                                    @input="getAll"
                                    v-model="keySearchCategory"
                                />
                            </VCol>
                        </VRow>
                    </VCardText>
                </VCard>
                <VCard>
                    <VTable class="text-no-wrap">
                        <!-- 👉 table head -->
                        <thead>
                            <tr>
                                <th scope="col">#</th>
                                <th scope="col">Category name</th>
                                <th scope="col" class="text-center">ACTION</th>
                            </tr>
                        </thead>

                        <!-- 👉 table body -->
                        <tbody>
                            <tr
                                v-for="(category, index) in categoryStore.data.categories"
                                :key="category.id"
                            >
                                <td>
                                    {{ (currentPageCategory - 1) * pageSizeCategory + index + 1 }}
                                </td>

                                <td>
                                    <div class="d-flex align-center">
                                        {{ category.name }}
                                    </div>
                                </td>

                                <td class="text-center">
                                    <VBtn color="default" @click="openFromCategory(category.id)">
                                        <VIcon
                                            icon="mdi-pencil-outline"
                                            :size="20"
                                            class="me-3"
                                            color="warning"
                                        />
                                    </VBtn>
                                </td>
                            </tr>
                        </tbody>

                        <!-- 👉 table footer  -->
                        <tfoot v-show="!categoryStore.data.categories">
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
                    <VCardText class="d-flex flex-wrap justify-end gap-4 pa-2">
                        <!-- 👉 Rows per page -->
                        <div class="d-flex align-center me-3" style="width: 171px;">
                            <span class="text-no-wrap me-3">Rows per page:</span>

                            <VSelect
                                v-model="pageSizeCategory"
                                density="compact"
                                variant="plain"
                                class="user-pagination-select"
                                :items="[5, 10, 20, 30, 50]"
                            />
                        </div>

                        <!-- 👉 Pagination and pagination meta -->
                        <div class="d-flex align-center">
                            <VPagination
                                v-model="currentPageCategory"
                                :length="totalPageCategory"
                                :total-visible="1"
                                rounded="circle"
                                @input="changePage"
                            />
                        </div>
                    </VCardText>
                </VCard>
            </VCol>
            <VCol cols="6">
                <VCard class="mb-2">
                    <VCardText>
                        <VRow>
                            <VCol cols="12" sm="4">
                                <VBtn
                                    variant="tonal"
                                    color="secondary"
                                    prepend-icon="mdi-tray-arrow-down"
                                    @click="openFromVideo(0)"
                                >
                                    Add Video Type
                                </VBtn>
                            </VCol>
                            <VCol cols="12" sm="8" class="display">
                                <VTextField
                                    placeholder="Search"
                                    density="compact"
                                    class="me-3"
                                    @input="getAll"
                                    v-model="keySearchVideoType"
                                />
                            </VCol>
                        </VRow>
                    </VCardText>
                </VCard>
                <VCard>
                    <VTable class="text-no-wrap">
                        <!-- 👉 table head -->
                        <thead>
                            <tr>
                                <th scope="col">#</th>
                                <th scope="col">Video Type</th>
                                <th scope="col" class="text-center">ACTION</th>
                            </tr>
                        </thead>

                        <!-- 👉 table body -->
                        <tbody>
                            <tr
                                v-for="(videoType, index) in videoTypeStore.data.videoTypes"
                                :key="videoType.id"
                            >
                                <td>
                                    {{ (currentPageVideoType - 1) * pageSizeVideoType + index + 1 }}
                                </td>

                                <td>
                                    <div class="d-flex align-center">
                                        {{ videoType.name }}
                                    </div>
                                </td>

                                <td class="text-center">
                                    <VBtn color="default" @click="openFromVideo(videoType.id)">
                                        <VIcon
                                            icon="mdi-pencil-outline"
                                            :size="20"
                                            class="me-3"
                                            color="warning"
                                        />
                                    </VBtn>
                                </td>
                            </tr>
                        </tbody>

                        <!-- 👉 table footer  -->
                        <tfoot v-show="!videoTypeStore.data.videoTypes">
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
                    <VCardText class="d-flex flex-wrap justify-end gap-4 pa-2">
                        <!-- 👉 Rows per page -->
                        <div class="d-flex align-center me-3" style="width: 171px;">
                            <span class="text-no-wrap me-3">Rows per page:</span>

                            <VSelect
                                v-model="pageSizeVideoType"
                                density="compact"
                                variant="plain"
                                class="user-pagination-select"
                                :items="[5, 10, 20, 30, 50]"
                            />
                        </div>

                        <!-- 👉 Pagination and pagination meta -->
                        <div class="d-flex align-center">
                            <VPagination
                                v-model="currentPageVideoType"
                                :length="totalPageVideoType"
                                :total-visible="1"
                                rounded="circle"
                                @input="changePageVideoType"
                            />
                        </div>
                    </VCardText>
                </VCard>
            </VCol>
        </VRow>
        <AddVideoType
            v-model:is-drawer-open-video-type="isDrawerOpenVideoType"
            @video-type-data="submitVideoType"
            v-model:videoTypeId="idUpdateVideotype"
        />
        <AddCategory
            v-model:is-drawer-open-category="isDrawerOpenCategory"
            @category-data="submitCategory"
            v-model:categoryId="idUpdateCategory"
        />
        <!-- SECTION Pagination -->

        <!-- !SECTION -->
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
