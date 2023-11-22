<script setup lang="ts">
import { Building } from '@/model/building';
import { useBuildingStore } from '@/store/useBuildingStore';
import { onMounted, ref } from 'vue';
import Add from './add.vue';


const buildingStore = useBuildingStore();
const idUpdate = ref(0);

const isAddNewBuilding = ref(false);
const keySearch = ref('');
const rowPerPage = ref(10);
const currentPage = ref(1);
const totalPage = ref(1);
const totalUsers = ref(0);

// 👉 Computing pagination data
const paginationData = computed(() => {
    const firstIndex = buildingStore.data.length
        ? (currentPage.value - 1) * rowPerPage.value + 1
        : 0;
    const lastIndex = buildingStore.data.length + (currentPage.value - 1) * rowPerPage.value;

    return `${firstIndex}-${lastIndex} of ${totalUsers.value}`;
});

// SECTION Checkbox toggle
const selectedRows = ref<string[]>([]);
const selectAllUser = ref(false);

const addNewBuilding = (buildingData: Building) => {
    console.log(buildingData);
    if (buildingData.id && buildingData.id > 0) {
        buildingStore.updateBuilding(buildingData);
    } else {
        buildingStore.addBuilding(buildingData);
    }
};
const handleUpdate = (id: number) => {
    idUpdate.value = id;
    isAddNewBuilding.value = true;
};
onMounted(() => {
    buildingStore.getBuilding('');
    totalPage.value = buildingStore.data.totalPage;
    totalUsers.value = buildingStore.data.length;
});

const search = () => {
    buildingStore.getBuilding(keySearch.value);
};
</script>

<template>
    <section>
        <VCard>
            <VCardText class="d-flex flex-wrap gap-4">
                <!-- 👉 Export button -->
                <VBtn variant="tonal" color="secondary" prepend-icon="mdi-tray-arrow-up">
                    Export
                </VBtn>

                <VSpacer />

                <div class="app-user-search-filter d-flex align-center">
                    <!-- 👉 Search  -->
                    <VTextField
                        placeholder="Building Name"
                        density="compact"
                        class="me-3"
                        @input="search"
                        v-model="keySearch"
                    />

                    <!-- 👉 Add user button -->
                    <VBtn @click="isAddNewBuilding = true"> Add New Building </VBtn>
                </div>
            </VCardText>

            <VDivider />

            <VTable class="text-no-wrap">
                <!-- 👉 table head -->
                <thead>
                    <tr>
                        <th scope="col">
                            <div style="width: 1.875rem;">
                                <VCheckbox />
                            </div>
                        </th>
                        <th scope="col">NAME</th>
                        <th scope="col">ADDRESS</th>
                        <th scope="col">DISTRICT</th>
                        <th scope="col">POSTAL CODE</th>
                        <th scope="col">ZONE</th>
                        <th scope="col">REMAKE</th>
                        <th scope="col">ACTION</th>
                    </tr>
                </thead>

                <!-- 👉 table body -->
                <tbody>
                    <tr v-for="building in buildingStore.data" :key="building.id">
                        <!-- 👉 Checkbox -->
                        <td>
                            <div style="width: 1.875rem;">
                                <VCheckbox :id="`check${building.id}`" />
                            </div>
                        </td>

                        <!-- 👉 User -->
                        <td>
                            <div class="d-flex align-center">
                                {{ building.buildingName }}
                            </div>
                        </td>

                        <!-- 👉 Email -->
                        <td class="">
                            {{ building.address }}
                        </td>

                        <!-- 👉 Role -->
                        <td>
                            <span class="">{{ building.district }}</span>
                        </td>
                        <td>
                            {{ building.postalCode }}
                        </td>
                        <td>
                            {{ building.zone }}
                        </td>
                        <td>
                            {{ building.remark }}
                        </td>
                        
                        <td>
                            <VBtn size="x-small" color="default" variant="plain" icon>
                                <VIcon size="24" icon="mdi-dots-vertical" />

                                <VMenu activator="parent">
                                    <VList>
                                        <VListItem :to="{ name: 'building-view-id', params: { id: building.id } }">
                                            <template #prepend>
                                                <VIcon
                                                    icon="mdi-eye-outline"
                                                    :size="20"
                                                    class="me-3"
                                                />
                                            </template>
                                            <VListItemTitle>View</VListItemTitle>
                                        </VListItem>

                                        <VListItem @click="handleUpdate(building.id)">
                                            <template #prepend>
                                                <VIcon
                                                    icon="mdi-pencil-outline"
                                                    :size="20"
                                                    class="me-3"
                                                />
                                            </template>
                                            <VListItemTitle>Edit</VListItemTitle>
                                        </VListItem>

                                        <VListItem
                                            @click="buildingStore.deleteBuilding(building.id)"
                                        >
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
                <tfoot v-show="!buildingStore.data.length">
                    <tr>
                        <td colspan="7" class="text-center">No data available</td>
                    </tr>
                </tfoot>
            </VTable>

            <VDivider />

            <VCardText class="d-flex align-center flex-wrap justify-end gap-4 pa-2">
                <div class="d-flex align-center me-3" style="width: 171px;">
                    <span class="text-no-wrap me-3">Rows per page:</span>

                    <VSelect
                        v-model="rowPerPage"
                        density="compact"
                        variant="plain"
                        class="user-pagination-select"
                        :items="[10, 20, 30, 50]"
                    />
                </div>

                <div class="d-flex align-center">
                    <h6 class="text-sm font-weight-regular">
                        {{ paginationData }}
                    </h6>

                    <VPagination
                        v-model="currentPage"
                        size="small"
                        :total-visible="1"
                        :length="totalPage"
                        @next="selectedRows = []"
                        @prev="selectedRows = []"
                    />
                </div>
            </VCardText>
        </VCard>
        <Add
            v-model:isDrawerOpen="isAddNewBuilding"
            @building-data="addNewBuilding"
            v-model:buildingId="idUpdate"
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
