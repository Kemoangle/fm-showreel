<script setup lang="ts">
import { useCategoryStore } from '@/store/useCategoryStore';
import { onMounted, ref } from 'vue';

const categoryStore = useCategoryStore();


const keySearch = ref('');

const pageSize = ref(3);
const currentPage = ref(1);
const totalPages = ref(1);
const totalItems = ref();

const getAll = () => {
    categoryStore.getPageCategory(keySearch.value, currentPage.value, pageSize.value);
};
watchEffect(() => {
    totalPages.value = categoryStore.data.totalPages;
    totalItems.value = categoryStore.data.totalItems;
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
                        >
                            Create New Category
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
                        <th scope="col">Category name</th>
                        <th scope="col" class="text-center">Sub Category</th>
                        <th scope="col" class="text-center">ACTION</th>
                    </tr>
                </thead>

                <!-- 👉 table body -->
                <tbody>
                    <tr v-for="(category, index) in categoryStore.data.categories" :key="category.id">
                        <td>
                            {{ (currentPage - 1) * pageSize + index + 1 }}
                        </td>

                        <td>
                            <div class="d-flex align-center">
                                {{ category.name }}
                            </div>
                        </td>

                        <td>
                            <VChip
                                color="success"
                                size="small"
                                class="text-capitalize ml-2"
                                v-for="item in category.subCategory"
                                :key="item.id"
                            >
                                {{ item.name }}
                            </VChip>
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
                <tfoot v-show="!categoryStore.data.categories">
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
