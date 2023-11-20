<script setup lang="ts">
import { Building } from '@/model/building';
import axiosIns from '@/plugins/axios';
import axios from 'axios';
import { onMounted, ref } from 'vue';
import Add from './add.vue';

const buildings = ref<Building[] | any>([]);

const getAllBuilding = () => {
    axiosIns.get<Building[]>('Building').then((response) => {
        buildings.value = response;
    });
};
const isAddNewUserDrawerVisible = ref(false);
const addNewBuilding = (buildingData: Building) => {
    console.log(buildingData);
    // axiosIns.post('Building',buildingData).then((response) => {
    //     console.log('success');
    // });
    delete buildingData.id;
    axios.post('https://localhost:7284/api/Building',buildingData).then((response) => {
        console.log('success');
    });
    // refetch User
    getAllBuilding();
};
onMounted(() => {
    getAllBuilding();
});
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
                    <VTextField placeholder="Search Building" density="compact" class="me-3" />

                    <!-- 👉 Add user button -->
                    <VBtn @click="isAddNewUserDrawerVisible = true"> Add New Building </VBtn>
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
                        <th scope="col">RESTRICTIONS</th>
                    </tr>
                </thead>

                <!-- 👉 table body -->
                <tbody>
                    <tr v-for="building in buildings" :key="building.id">
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
                            <span class="">{{
                                building.district
                            }}</span>
                        </td>
                        <td>
                            {{ building.postalCode }}
                        </td>
                        <td>
                            {{ building.zone }}
                        </td>
                        <td>
                            <p
                                v-for="r in building.restrictions"
                                :key="r.id"
                                class="d-flex align-center mt-5"
                            >
                                {{ r.restrictionName }}
                            </p>
                        </td>
                    </tr>
                </tbody>

                <!-- 👉 table footer  -->
                <tfoot v-show="!buildings.length">
                    <tr>
                        <td colspan="7" class="text-center">No data available</td>
                    </tr>
                </tfoot>
            </VTable>

            <VDivider />
        </VCard>
        <Add v-model:isDrawerOpen="isAddNewUserDrawerVisible" @building-data="addNewBuilding" />
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
