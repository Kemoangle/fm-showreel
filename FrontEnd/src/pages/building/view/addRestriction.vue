<script setup lang="ts">
import { PerfectScrollbar } from 'vue3-perfect-scrollbar';
// eslint-disable-next-line @typescript-eslint/consistent-type-imports
import type { VForm } from 'vuetify/components';

import { Building } from '@/model/building';
import { VideoType } from '@/model/videoType';
import axiosIns from '@/plugins/axios';
import { useCategoryStore } from '@/store/useCategoryStore';
import { requiredValidator } from '@validators';

interface Emit {
    (e: 'update:isDrawerOpen', value: boolean): void;
    (e: 'restrictionData', value: Building): void;
}

interface Props {
    isDrawerOpen: boolean;
    restrictionId?: number;
}

const props = defineProps<Props>();
const emit = defineEmits<Emit>();
const categoryStore = useCategoryStore();

const isFormValid = ref(false);
const refForm = ref<VForm>();
const videoTypes = ref<VideoType[]>([]);
const restrictionData = ref<any>({
    id: 0,
    categoryId: 0,
    category: null,
    buildingId: 0,
    name:'',
    type: 'Except',
    except: [],
});

watch(props, async (oldId, newId) => {
    
});

const getVideoTypes = () => {
    console.log(restrictionData.value.category);
        if(restrictionData.value.category){
            axiosIns.get<VideoType[]>('VideoType/GetVideoTypeByCategory/' + restrictionData.value.category.id).then((reponse: any) => {
                videoTypes.value = reponse;
        });
    }
    
}

const createVideos = () => {
    categoryStore.getAllCategory();
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
            emit('restrictionData', restrictionData.value);
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
                            <VCol cols="12">
                                <VAutocomplete
                                    v-model="restrictionData.category"
                                    :items="categoryStore.data"
                                    item-title="name"
                                    label="Restriction"
                                    :menu-props="{ maxHeight: 250 }"
                                    :rules="[requiredValidator]"
                                    @update:model-value="getVideoTypes"
                                    return-object
                                />
                            </VCol>
                            <VCol cols="12">
                                <VSelect
                                        v-model="restrictionData.type"
                                        label="Select Type"
                                        :items="[
                                            'Except',
                                            'Exclude',
                                        ]"
                                        :menu-props="{ maxHeight: 200 }"
                                    />
                                    
                            </VCol>
                            <VCol cols="12">
                                <VAutocomplete
                                        v-model="restrictionData.except"
                                        chips
                                        closable-chips
                                        :items="videoTypes"
                                        item-title="name"
                                        label="Except/Exclude"
                                        :menu-props="{ maxHeight: 250 }"
                                        multiple
                                        return-object
                                    >
                                        <template #chip="{ props, item }">
                                            <VChip v-bind="props" :text="item.raw.name" />
                                        </template>

                                        <template #item="{ props, item }">
                                            <VListItem v-bind="props" :title="item?.raw?.name" />
                                        </template>
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
     </section>
    
</template>
