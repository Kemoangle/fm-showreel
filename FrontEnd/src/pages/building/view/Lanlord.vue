<script lang="ts" setup>
import { useSnackbar } from '@/components/Snackbar.vue';
import { LandlordAds } from '@/model/landlordAds';
import { useLanlordAdsStore } from '@/store/useLandlordStore';
import moment from 'moment';
import Swal from 'sweetalert2';
import { onMounted, ref } from 'vue';
import AddLandlordAds from './addLandlordAds.vue';

interface Props {
    buildingId: any;
}
const idUpdate = ref(0);
const isDrawerOpen = ref(false);
const props = defineProps<Props>();
const lanlordAdsStore = useLanlordAdsStore();
const { showSnackbar } = useSnackbar();

const handleUpdate = (id: number) => {
    idUpdate.value = id;
    isDrawerOpen.value = true;
};

const handleSubmit = async (landlordData: LandlordAds) => {
    landlordData.buildingId = props.buildingId;
    landlordData.videoId = landlordData.video?.id;
    delete(landlordData.video);
    if (landlordData.id && landlordData.id > 0) {
        await lanlordAdsStore.updateLandlordAds(landlordData)
        .then(response =>{
            lanlordAdsStore.getAllLandlordAds(props.buildingId);
        }).catch((error) => {
            showSnackbar(error.data.Landlordad[0], 'error');
        });
    } else{
        await lanlordAdsStore.addLandlordAds(landlordData)
        .then(response =>{
            lanlordAdsStore.getAllLandlordAds(props.buildingId);
        }).catch((error) => {
            showSnackbar(error.data.Landlordad[0], 'error');
        });
    }
};

onMounted(() =>{
    lanlordAdsStore.getAllLandlordAds(props.buildingId);
})

const deleteLandlordAds = async (id: number) => {
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
            lanlordAdsStore.deleteLandlordAds(id).then((response) => {
                lanlordAdsStore.getAllLandlordAds(props.buildingId);
                Swal.fire({
                    title: "Deleted!",
                    icon: "success"
                });
            }); 
            
        }
        });
};

const dataTransmission = (): number[] =>{
    const videoIds: number[] = lanlordAdsStore.data.map((ad) => ad.video?.id || 0);  
    return videoIds;
}
</script>

<template>
    <section>
        <VCard>
            <VCardItem>
                <template #prepend>
                    <VIcon icon="mdi-home-add" color="success" />
                </template>
                <VBtn variant="tonal" color="primary" 
                    prepend-icon="mdi-plus-thick"
                    style="float: inline-end;" 
                    @click="handleUpdate(0)">
                    Add Video
                </VBtn>
                <VCardTitle>LandlordAds</VCardTitle>
            </VCardItem>

            <VDivider />

            <VTable class="text-no-wrap">
                <!-- 👉 table head -->
                <thead>
                    <tr>
                        <th scope="col">#</th>
                        <th scope="col">VIDEO TITLE</th>
                        <th scope="col">DURATION</th>
                        <th scope="col">KEY NO</th>
                        <th scope="col">LOOP</th>
                        <th scope="col">START DATE</th>
                        <th scope="col">END DATE</th>
                        <th scope="col">ACTION</th>
                    </tr>
                </thead>

                <!-- 👉 table body -->
                <tbody>
                    <tr v-for="(item, index) in lanlordAdsStore.data" :key="item.id">
                        <!-- 👉 User -->
                        <td>{{ index + 1 }}</td>
                        <td>
                            <div class="d-flex align-center">
                                {{ item.video?.title }}
                            </div>
                        </td>

                        <!-- 👉 Email -->
                        <td class="">
                            {{ item.video?.duration }}
                        </td>

                        <td class="">
                            {{ item.video?.keyNo }}
                        </td>

                        <td class="">
                            {{ item.loop }}
                        </td>

                        <td class="">
                            {{ item.startDate ? moment(item.startDate).format('DD-MM-YYYY') : "" }}
                        </td>

                        <td class="">
                            {{ item.endDate ? moment(item.endDate).format('DD-MM-YYYY') : "" }}
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

                                        <VListItem @click="deleteLandlordAds(item.id)">
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
                <tfoot v-show="!lanlordAdsStore.data">
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
        <AddLandlordAds
            v-model:isDrawerOpen="isDrawerOpen"
            @landlord-data="handleSubmit"
            v-model:landlordAdsId="idUpdate"
            :videoExist="dataTransmission()"
        />
    </section>
</template>
