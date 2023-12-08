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

// const { showSnackbar } = useSnackbar();

// const getAll = () => {
//     buildingStore.getPageBuilding(keySearch.value, currentPage.value, pageSize.value);
// };
// watchEffect(() => {
//     totalPages.value = buildingStore.data.totalPages;
//     totalItems.value = buildingStore.data.totalItems;
//     if (currentPage.value > totalPages.value) currentPage.value = totalPages.value;
// });

// onMounted(() => {
//     getAll();
// });
// const changePage = (newPage: number) => {
//     currentPage.value = newPage;
//     getAll();
// };

// watch(currentPage, () => {
//     getAll();
// });

// watch(pageSize, () => {
//     getAll();
// });

// const handleSubmit = async (buildingData: Building) => {
//     if (buildingData.id && buildingData.id > 0) {
//         await buildingStore.updateBuilding(buildingData).catch((error) => {
//             showSnackbar(error.data.Building[0], 'error');
//         });
//     } else {
//         await buildingStore.addBuilding(buildingData).catch((error) => {
//             showSnackbar(error.data.Building[0], 'error');
//         });
//     }

//     getAll();
// };

// const handleUpdate = (id: number) => {
//     idUpdate.value = id;
//     isAddNewBuilding.value = true;
// };

// const deleteBuilding = (id: number) => {
//     Swal.fire({
//         title: "Are you sure?",
//         text: "You won't be able to revert this!",
//         icon: "warning",
//         showCancelButton: true,
//         confirmButtonColor: "#3085d6",
//         cancelButtonColor: "#d33",
//         confirmButtonText: "Yes, delete it!"
//         }).then((result) => {
//         if (result.isConfirmed) {
//             buildingStore.deleteBuilding(id).then((response) => {
//                 Swal.fire({
//                     title: "Deleted!",
//                     icon: "success"
//                     });
//                 getAll();
//             });

//         }
//     });
// };

const list = [
    {
        id: 1,
        title: 'hello',
        duration: 1,
        keyNo: 1,
        category: 'ko',
        remark: '2',
        order: 1,
    },
    {
        id: 2,
        title: 'hello',
        duration: 1,
        keyNo: 1,
        category: 'ko',
        remark: '2',
        order: 2,
    },
];
</script>

<template>
    <section>
        <VCard class="mb-6">
            <VCardText>
                <VRow>
                    <VCol cols="12" sm="4">
                        <VBtn
                            variant="tonal"
                            :to="{
                                name: 'playlist-generator',
                            }"
                            color="secondary"
                        >
                            <!-- @click="handleUpdate(0)" -->
                            Generator playlist
                        </VBtn>
                    </VCol>
                    <VCol cols="12" sm="8" class="display">
                        <VTextField
                            placeholder="Search"
                            density="compact"
                            class="me-3"
                            v-model="keySearch"
                        />
                        <!-- @input="getAll" -->
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
                        <th scope="col">STT</th>
                        <th scope="col">TITLE</th>
                        <th scope="col">DURATION</th>
                        <th scope="col">KEY NO</th>
                        <th scope="col">CATEGORY</th>
                        <th scope="col">REMARK</th>
                    </tr>
                </thead>

                <!-- 👉 table body -->
                <tbody>
                    <tr class="handle" v-for="(playlist, index) in list" :key="playlist.order">
                        <td>
                            {{ index + 1 }}
                        </td>
                        <td>
                            {{ playlist.title }}
                        </td>
                        <td class="text-medium-emphasis">
                            {{ playlist.duration }}
                        </td>
                        <td>
                            {{ playlist.keyNo }}
                        </td>
                        <td class="text-capitalize">
                            {{ playlist.category }}
                        </td>
                        <td>
                            {{ playlist.remark }}
                        </td>
                    </tr>
                    <tr key="j">
                        <td></td>
                        <td></td>
                        <td class="text-medium-emphasis">
                            {{ list.reduce((a, b) => a + (b?.duration || 0), 0) }}
                        </td>
                        <td></td>
                        <td class="text-capitalize"></td>
                        <td></td>
                    </tr>

                    <!-- 👉 table footer  -->
                    <!-- <tfoot v-show="!videoStore.data.videos">
                    <tr>
                        <td colspan="7" class="text-center">
                            <v-row align="center" justify="center" class="fill-height">
                                <v-col cols="12" class="text-center">
                                    <VProgressCircular indeterminate color="info" />
                                </v-col>
                            </v-row>
                        </td>
                    </tr>
                </tfoot> -->
                </tbody>
            </VTable>

            <VDivider />
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
