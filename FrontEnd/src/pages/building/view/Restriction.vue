<script lang="ts" setup>
import { useSnackbar } from '@/components/Snackbar.vue';
import { useRestrictionStore } from '@/store/useRestrictionStore';
import Swal from 'sweetalert2';
import AddRestriction from './addRestriction.vue';

interface Props {
    buildingId: any;
}
const idUpdate = ref(0);
const isDrawerOpen = ref(false);
const props = defineProps<Props>();
const restrictionStore = useRestrictionStore();
const { showSnackbar } = useSnackbar();

const handleUpdate = (id: number) => {
    idUpdate.value = id;
    isDrawerOpen.value = true;
};

onMounted(() =>{
    restrictionStore.getRestrictionByBuildingId(props.buildingId);
})

const deleteRestriction = async (id: number) => {
    await Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
        }).then((result) => {
        if (result.isConfirmed) {
            restrictionStore.deleteRestriction(id).then((response) => {
                Swal.fire({
                    title: "Deleted!",
                    icon: "success"
                }); 
            }); 
        } 
    });    
};

const capitalizeFirstLetter = (str: string) =>{
    if (str) {
        return str.charAt(0).toUpperCase() + str.slice(1);
      } else {
        return '';
      }
}

const handleSubmit = async (restrictionData: any) => {
    restrictionData.buildingId = props.buildingId;
    restrictionData.name = 'No ' + restrictionData.category.name;
    restrictionData.categoryId = restrictionData.category.id;
    delete(restrictionData.category);
    console.log(restrictionData.except)
    const { except, ...rest } = restrictionData;
    

    if (restrictionData.id && restrictionData.id > 0) {
        await restrictionStore.updateBuildingRestriction(rest)
        .then((response) => {
            restrictionStore.UpdateRestrictionExcept(except, response.id).then(data =>{
                restrictionStore.getRestrictionByBuildingId(props.buildingId);
            });
        })
        .catch((error) => {
            showSnackbar(error.data.Restriction[0], 'error');
        });
    } else {
        await restrictionStore.addBuildingRestriction(rest)
        .then((response) => {
            restrictionStore.UpdateRestrictionExcept(except, response.id).then(data =>{
                restrictionStore.getRestrictionByBuildingId(props.buildingId);
            });
        })
        .catch((error) => {
            showSnackbar(error.data.Restriction[0], 'error');
        });
    }
};
</script>

<template>
    <section>
        <VCard>
            <VCardItem>
                <template #prepend>
                    <VIcon icon="mdi-chart-timeline-variant" color="success" />
                </template>
                <VBtn variant="tonal" color="secondary" 
                    prepend-icon="mdi-tray-arrow-down"
                    style="float: inline-end;" 
                    @click="handleUpdate(0)">
                    Add Restriction
                </VBtn>
                <VCardTitle>Restriction</VCardTitle>
            </VCardItem>

            <VDivider />

            <VTable class="text-no-wrap">
                <!-- 👉 table head -->
                <thead>
                    <tr>
                        <th scope="col">#</th>
                        <th scope="col">Restriction</th>
                        <th scope="col">Except/Exclude</th>
                        <th scope="col">Action</th>
                    </tr>
                </thead>

                <!-- 👉 table body -->
                <tbody>
                    <tr v-for="(item, index) in restrictionStore.data" :key="item.id">
                        <!-- 👉 User -->
                        <td>{{ index + 1 }}</td>
                        <td>
                            <div class="d-flex align-center">
                                {{ item.name }}
                            </div>
                        </td>

                        <td class="">
                            {{ capitalizeFirstLetter(item.type)}}
                            <span v-if="item.except.length">
                                (<span v-for="(v,idx) in item.except" :key="v.id">
                                {{ v.name}}
                                <span v-if="idx < item.except.length - 1">/</span>
                                </span>)
                            </span>
                        </td>
                        <td>
                            <VBtn size="x-small" color="default" variant="plain" icon>
                                <VIcon size="24" icon="mdi-dots-vertical" />

                                <VMenu activator="parent">
                                    <VList>
                                        <VListItem @click="handleUpdate(item.id)">
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

                                        <VListItem @click="deleteRestriction(item.id)">
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
                <tfoot v-show="!restrictionStore.data">
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
        </VCard>
        <AddRestriction
            v-model:isDrawerOpen="isDrawerOpen"
            @restriction-data="handleSubmit"
            v-model:restrictionId="idUpdate"
        />
    </section>
</template>
