<script setup lang="ts">
import { PerfectScrollbar } from 'vue3-perfect-scrollbar';
// eslint-disable-next-line @typescript-eslint/consistent-type-imports
import type { VForm } from 'vuetify/components';

import { Category } from '@/model/category';
import { Video } from '@/model/video';
import axiosIns from '@/plugins/axios';
import { useCategoryStore } from '@/store/useCategoryStore';
import { useVideoStore } from '@/store/useVideoStore';
import { requiredValidator } from '@validators';

const categories = ref();

interface Emit {
    (e: 'update:isDrawerOpen', value: boolean): void;
    (e: 'videoData', value: Video): void;
}

interface Props {
    isDrawerOpen: boolean;
    videoId?: number;
}

const categoryStore = useCategoryStore();
const videoStore = useVideoStore();

const props = defineProps<Props>();
const emit = defineEmits<Emit>();

const isFormValid = ref(false);
const refForm = ref<VForm>();

const videoData = ref<Video | any>({
    id: 0,
    title: '',
    duration: 0,
    keyNo: '',
    rule: '',
    category: [{id: 0, name: ''}]
});

watchEffect(()=>{
    console.log('oldProps.videoId:',props.videoId);
})

watch(props, async (oldId, newId) => {
    if (newId.videoId) {
        videoData.value = videoStore.getVideoById(newId.videoId)        
    }else{
        refForm.value?.reset();
        refForm.value?.resetValidation();
        videoData.value.category = new Array<Category>;
    }
    await axiosIns.get<Category[]>('Category').then((response) => {
        categories.value = response;
    });
});

onMounted(() => {});
    
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
            emit('videoData', videoData.value);
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
            <h6 class="text-h6">Video</h6>
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
                            <!-- 👉 Title -->
                            <VCol cols="12">
                                <VTextField
                                    v-model="videoData.title"
                                    :rules="[requiredValidator]"
                                    label="Title"
                                />
                            </VCol>

                            <VCol cols="12">
                                <VTextField
                                    v-model="videoData.duration"
                                    :rules="[requiredValidator]"
                                    label="Duration"
                                />
                            </VCol>

                            <VCol cols="12">
                                <VTextField
                                    v-model="videoData.keyNo"
                                    :rules="[requiredValidator]"
                                    label="Key no"
                                />
                            </VCol>

                            <VCol cols="12">
                                <VTextField
                                    v-model="videoData.rule"
                                    :rules="[requiredValidator]"
                                    label="Rule"
                                />
                            </VCol>
                            
                            <VCol cols="12">
                                <VAutocomplete  
                                    v-model="videoData.category"
                                    chips
                                    closable-chips
                                    multiple
                                    :items="categories"
                                    item-title="name"
                                    item-value="id"
                                    label="Category"
                                    return-object
                                >
                                </VAutocomplete>
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
