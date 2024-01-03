<script setup lang="ts">
import { PerfectScrollbar } from 'vue3-perfect-scrollbar';
// eslint-disable-next-line @typescript-eslint/consistent-type-imports
import type { VForm } from 'vuetify/components';

import { Building } from '@/model/building';
import { LandlordAds } from '@/model/landlordAds';
import { Video } from '@/model/video';
import axiosIns from '@/plugins/axios';
import { useVideoStore } from '@/store/useVideoStore';
import { requiredValidator } from '@validators';
import { nextTick, onMounted, ref, watch } from 'vue';

interface Emit {
    (e: 'update:isDrawerOpen', value: boolean): void;
    (e: 'landlordData', value: Building): void;
}

interface Props {
    isDrawerOpen: boolean;
    landlordAdsId?: number;
    videoExist: number[];
}

const props = defineProps<Props>();
const emit = defineEmits<Emit>();
const videoStore = useVideoStore();
const videoData = ref<Video[]>([]);
const isFormValid = ref(false);
const refForm = ref<VForm>();

const landlordData = ref<LandlordAds | any>({
    id: 0,
    loop: 0,
    videoId: 0,
    buildingId: 0,
    startDate: new Date(),
    endDate: new Date(),
});

watch(props, async (oldId, newId) => {
    createVideos();
    refForm.value?.reset();
    refForm.value?.resetValidation();
    if (newId.landlordAdsId && newId.landlordAdsId > 0 ) {
        await axiosIns.get('LandlordAds/GetLandlordAdsById/' + newId.landlordAdsId).then((response: any) => {
            landlordData.value = response.data;
        });
    }
    else{
        landlordData.value.id = 0;
        landlordData.value.startDate = '';
        landlordData.value.endDate = '';
    }
});

const createVideos = () => {
    videoStore.getAllVideos();
    videoData.value = videoStore.video;
    if(props.videoExist.length > 0 && props.videoExist[0] > 0){
        videoData.value = videoStore.video.filter(v => !props.videoExist.includes(v.id));
    }
};

onMounted(() => {
    createVideos();
    refForm.value?.reset();
    refForm.value?.resetValidation();
});


// 👉 drawer close
const closeNavigationDrawer = () => {
    emit('update:isDrawerOpen', false);

    nextTick(() => {
        refForm.value?.reset();
        refForm.value?.resetValidation();
    });
};


const onSubmit = () => {
    refForm.value?.validate().then(({ valid }) => {
        if (valid) {
            emit('landlordData', landlordData.value);
            emit('update:isDrawerOpen', false);
            nextTick(() => {
                refForm.value?.reset();
                refForm.value?.resetValidation();
            });
        }
    });
};

const handleDrawerModelValueUpdate = (val: boolean) => {
    emit('update:isDrawerOpen', val);
};

</script>

<template>
    <section>
        <VNavigationDrawer
        temporary
        :width="400"
        location="end"
        class="scrollable-content"
        :model-value="props.isDrawerOpen"
        @update:model-value="handleDrawerModelValueUpdate"
    >
        <!-- 👉 Title -->
        <div class="d-flex align-center bg-var-theme-background px-5 py-2">
            <h6 class="text-h6">Building </h6>
            <VSpacer />
            <VBtn
                size="small"
                color="secondary"
                variant="text"
                icon="mdi-close"
                @click="closeNavigationDrawer"
            />
        </div>

        <PerfectScrollbar :options="{ wheelPropagation: false }">
            <VCard flat>
                <VCardText>
                    <!-- 👉 Form -->
                    <VForm ref="refForm" v-model="isFormValid" @submit.prevent="onSubmit">
                        <VRow>
                            <!-- 👉 buildingName -->
                            <VCol cols="12">
                                <VAutocomplete
                                    v-model="landlordData.video"
                                    :items="videoData"
                                    item-title="title"
                                    label="Video"
                                    :menu-props="{ maxHeight: 250 }"
                                    return-object
                                    :rules="[requiredValidator]"
                                />
                            </VCol>
                            <VCol cols="12">
                                <VTextField
                                    v-model="landlordData.loop"
                                    :rules="[requiredValidator]"
                                    label="Loop"
                                />
                            </VCol>
                            <VCol cols="12">
                                <AppDateTimePicker
                                    v-model="landlordData.startDate"
                                    label="Start Date"
                                    
                                />
                            </VCol>
                            <VCol cols="12">
                                <AppDateTimePicker
                                    v-model="landlordData.endDate"
                                    label="End Date"
                                />
                            </VCol>
                            <!-- 👉 Submit and Cancel -->
                            <VCol cols="12">
                                <VBtn type="submit" class="me-3"> Submit </VBtn>
                                <VBtn
                                    type="reset"
                                    variant="tonal"
                                    color="secondary"
                                    @click="closeNavigationDrawer"
                                >
                                    Cancel
                                </VBtn>
                            </VCol>
                        </VRow>
                    </VForm>
                </VCardText>
            </VCard>
        </PerfectScrollbar>
    </VNavigationDrawer>
    </section>
    
</template>
