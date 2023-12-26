<script setup lang="ts">
import { useSnackbar } from '@/components/Snackbar.vue';
import { Category } from '@/model/category';
import { useCategoryStore } from '@/store/useCategoryStore';
import { onMounted, ref } from 'vue';
import AddCategory from './addCategory.vue';

const categoryStore = useCategoryStore();
const keySearchCategory = ref('');
const pageSizeCategory = ref(10);
const currentPageCategory = ref(1);
const totalPageCategory = ref(1);
const totalItemCategory = ref();

const idUpdateCategory = ref(0);
const isDrawerOpenCategory = ref(false);
const { showSnackbar } = useSnackbar();

const getCategory = () => {
    categoryStore.getPageCategory(
        keySearchCategory.value,
        currentPageCategory.value,
        pageSizeCategory.value
    );
};

watchEffect(() => {
    totalPageCategory.value = categoryStore.pageCategory.totalPages;
    totalItemCategory.value = categoryStore.pageCategory.totalItems;
    if (currentPageCategory.value > totalPageCategory.value)
        currentPageCategory.value = totalPageCategory.value;
});

onMounted(() => {
    getCategory();
});
const changePage = (newPage: number) => {
    currentPageCategory.value = newPage;
    getCategory();
};

watch(currentPageCategory, () => {
    getCategory();
});

watch(pageSizeCategory, () => {
    getCategory();
});

const openFromCategory = (id: number) => {
    isDrawerOpenCategory.value = true;
    idUpdateCategory.value = id;
};

const submitCategory = async (categoryData: Category) => {
    getCategory();
};

const randomColor = () => {
    const colors = ['primary', 'secondary', 'success', 'info', 'warning', 'error'];
    const randomColor = colors[Math.floor(Math.random() * colors.length)];
    return randomColor;
};

const selectedSubcategory = ref<[]>([]);
const selectedCategory = ref<[]>([]);
const menu = ref(false);

</script>

<template>
    <section>
        <VRow>
            <VCol cols="12">
                <VCard class="mb-2">
                    <VCardText>
                        <VRow>
                            <VCol cols="12" sm="4">
                                <VBtn
                                    color="primary"
                                    prepend-icon="mdi-plus-thick"
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
                                    @input="getCategory"
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
                                <th scope="col">Sub Category</th>
                                <th scope="col" class="text-center">ACTION</th>
                            </tr>
                        </thead>

                        <!-- 👉 table body -->
                        <tbody>
                            <tr
                                v-for="(category, index) in categoryStore.pageCategory.categories"
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

                                <td>
                                    <VChip
                                        size="small"
                                        class="text-capitalize ml-2 chip-wrap"
                                        v-for="item in category.subCategory"
                                        :key="item.id"
                                        :color="randomColor()"
                                    >
                                        {{ item.name }}
                                    </VChip>
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
                        <tfoot v-show="!categoryStore.pageCategory.categories">
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
        </VRow>
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

.father {
  display: flex;
  align-items: center;
  padding-inline-start: 10px;
}

p {
  padding: 0;
  margin: 0;
}

.children {
  &-item {
    display: flex;
    align-items: center;
    padding-inline-start: 40px;
  }
}
</style>
