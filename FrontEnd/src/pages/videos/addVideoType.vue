<script setup lang="ts">
import { PerfectScrollbar } from 'vue3-perfect-scrollbar';
// eslint-disable-next-line @typescript-eslint/consistent-type-imports
import type { VForm } from 'vuetify/components';

import { VideoType } from '@/model/videoType';
import axiosIns from '@/plugins/axios';
import { useVideoTypeStore } from '@/store/useVideoTypeStore';
import { requiredValidator } from '@validators';

interface Emit {
    (e: 'update:isDrawerOpenVideoType', value: boolean): void;
    (e: 'videoTypeData', value: VideoType): void;
}

interface Props {
    isDrawerOpenVideoType: boolean;
    videoTypeId: number;
}
const videoTypeStore = useVideoTypeStore();
const props = defineProps<Props>();
const emit = defineEmits<Emit>();

const isFormValid = ref(false);
const refForm = ref<VForm>();

const videoType = ref<VideoType | any>({
    id: 0,
    name: '',
});

watch(props, async (oldId, newId) => {
    refForm.value?.reset();
    refForm.value?.resetValidation();
    if (newId.videoTypeId && newId.videoTypeId > 0) {
        axiosIns.get<VideoType>('VideoType/' + newId.videoTypeId).then((reponse) => {
            videoType.value = reponse;
        });
    } else {
        videoType.value.id = 0;
        refForm.value?.reset();
        refForm.value?.resetValidation();
    }
});
onMounted(() => {});

// 👉 drawer close
const closeNavigationDrawer = () => {
    emit('update:isDrawerOpenVideoType', false);

    nextTick(() => {
        refForm.value?.reset();
        refForm.value?.resetValidation();
    });
};

const onSubmit = () => {
    refForm.value?.validate().then(({ valid }) => {
        if (valid) {
            emit('videoTypeData', videoType.value);
            emit('update:isDrawerOpenVideoType', false);
            nextTick(() => {
                refForm.value?.reset();
                refForm.value?.resetValidation();
            });
        }
    });
};

const handleDrawerModelValueUpdate = (val: boolean) => {
    emit('update:isDrawerOpenVideoType', val);
};

</script>

<template>
    <section>
        <VNavigationDrawer
            temporary
            :width="400"
            location="end"
            class="scrollable-content"
            :model-value="props.isDrawerOpenVideoType"
            @update:model-value="handleDrawerModelValueUpdate"
        >
            <!-- 👉 Title -->
            <div class="d-flex align-center bg-var-theme-background px-5 py-2">
                <h6 class="text-h6">Video Type</h6>

                <VSpacer />

                <VBtn
                    size="small"
                    color="secondary"
                    variant="text"
                    icon="mdi-close"
                />
            </div>

            <PerfectScrollbar :options="{ wheelPropagation: false }">
                <VCard flat>
                    <VCardText>
                        <!-- 👉 Form -->
                        <VForm ref="refForm" v-model="isFormValid" @submit.prevent="onSubmit">
                            <VRow>
                                <!-- 👉 Name -->
                                <VCol cols="12">
                                    <VTextField
                                        v-model="videoType.name"
                                        :rules="[requiredValidator]"
                                        label="Name"
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
