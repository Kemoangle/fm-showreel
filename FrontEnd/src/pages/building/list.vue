<script setup lang="ts">
import { useSnackbar } from '@/components/Snackbar.vue';
import { Building } from '@/model/building';
import { useBuildingStore } from '@/store/useBuildingStore';
import Swal from 'sweetalert2';
import { onMounted, ref } from 'vue';
import Add from './add.vue';

const buildingStore = useBuildingStore();
const idUpdate = ref(0);

const isAddNewBuilding = ref(false);
const keySearch = ref('');
const router = useRoute();

const pageSize = ref(10);
const currentPage = ref(1);
const totalPages = ref(1);
const totalItems = ref();

const { showSnackbar } = useSnackbar();

const getAll = () => {
    buildingStore.getPageBuilding(keySearch.value, currentPage.value, pageSize.value);
};
watchEffect(() => {
    totalPages.value = buildingStore.data.totalPages;
    totalItems.value = buildingStore.data.totalItems;
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

const handleSubmit = async (buildingData: Building) => {
    if (buildingData.id && buildingData.id > 0) {
        await buildingStore.updateBuilding(buildingData).catch((error) => {
            showSnackbar(error.data.Building[0], 'error');
        });
    } else {
        await buildingStore.addBuilding(buildingData).catch((error) => {
            showSnackbar(error.data.Building[0], 'error');
        });
    }

    getAll();
};

const handleUpdate = (id: number) => {
    idUpdate.value = id;
    isAddNewBuilding.value = true;
};

const deleteBuilding = (id: number) => {
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
            buildingStore.deleteBuilding(id).then((response) => {
                Swal.fire({
                    title: 'Deleted!',
                    icon: 'success',
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
                            color="secondary"
                            prepend-icon="mdi-plus-thick"
                            @click="handleUpdate(0)"
                        >
                            Create New Building
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
                        <th scope="col">NAME</th>
                        <th scope="col">ADDRESS</th>
                        <th scope="col">DISTRICT</th>
                        <th scope="col">POSTAL CODE</th>
                        <th scope="col">ZONE</th>
                        <th scope="col">REMAKE</th>
                        <th scope="col" class="text-center">ACTION</th>
                    </tr>
                </thead>

                <!-- 👉 table body -->
                <tbody>
                    <tr
                        v-for="(building, index) in buildingStore.data.buildings"
                        :key="building.id"
                    >
                        <td>
                            {{ (currentPage - 1) * pageSize + index + 1 }}
                        </td>

                        <td>
                            <div class="d-flex align-center">
                                {{ building.buildingName }}
                            </div>
                        </td>

                        <td class="">
                            {{ building.address }}
                        </td>

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

                        <td class="text-center">
                            <VBtn size="x-small" color="default" variant="plain" icon>
                                <VIcon size="24" icon="mdi-dots-vertical" />

                                <VMenu activator="parent">
                                    <VList>
                                        <VListItem
                                            :to="{
                                                name: 'building-view-id',
                                                params: { id: building.id },
                                            }"
                                        >
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

                                        <VListItem @click="handleUpdate(building.id)">
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

                                        <VListItem @click="deleteBuilding(building.id)">
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
                <tfoot v-show="!buildingStore.data.buildings">
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
                        rounded="circle"
                        :total-visible="1"
                        @input="changePage(currentPage)"
                    />
                </div>
            </VCardText>
            <!-- !SECTION -->
        </VCard>
        <Add
            v-model:isDrawerOpen="isAddNewBuilding"
            @building-data="handleSubmit"
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
