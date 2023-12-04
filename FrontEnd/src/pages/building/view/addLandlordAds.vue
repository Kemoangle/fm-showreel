<script setup lang="ts">
import { PerfectScrollbar } from 'vue3-perfect-scrollbar';
// eslint-disable-next-line @typescript-eslint/consistent-type-imports
import type { VForm } from 'vuetify/components';

import { Building } from '@/model/building';
import { LandlordAds } from '@/model/landlordAds';
import { useVideoStore } from '@/store/useVideoStore';
import { requiredValidator } from '@validators';

interface Emit {
    (e: 'update:isDrawerOpen', value: boolean): void;
    (e: 'landlordData', value: Building): void;
}

interface Props {
    isDrawerOpen: boolean;
    landlordAdsId?: number;
}

const props = defineProps<Props>();
const emit = defineEmits<Emit>();
const videoStore = useVideoStore();

const isFormValid = ref(false);
const refForm = ref<VForm>();

const landlordData = ref<LandlordAds | any>({
    id: 0,
    loop: 0,
    videoId: 0,
    buildingId: 0,
    startDate: '',
    endDate: '',
    video: null
});

const createVideos = () => {
    videoStore.getAllVideos();
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
                                    v-model="landlordData.videoId"
                                    :items="videoStore.video"
                                    item-title="title"
                                    item-value="id"
                                    label="Video"
                                    :menu-props="{ maxHeight: 250 }"
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
                                    :rules="[requiredValidator]"
                                />
                            </VCol>
                            <VCol cols="12">
                                <AppDateTimePicker
                                    v-model="landlordData.endDate"
                                    label="End Date"
                                    :rules="[requiredValidator]"
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
</template>
