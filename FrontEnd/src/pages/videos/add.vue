<script setup lang="ts">
import { PerfectScrollbar } from 'vue3-perfect-scrollbar';
// eslint-disable-next-line @typescript-eslint/consistent-type-imports
import type { VForm } from 'vuetify/components';

import { Building } from '@/model/building';
import { Category } from '@/model/category';
import { Video } from '@/model/video';
import axiosIns from '@/plugins/axios';
import { useVideoTypeStore } from '@/store/useVideoTypeStore';
import { requiredValidator } from '@validators';

const categories = ref();
const subCategories = ref();
const buildings = ref();

interface Emit {
    (e: 'update:isDrawerOpen', value: boolean): void;
    (e: 'videoData', value: Video): void;
}

interface Props {
    isDrawerOpen: boolean;
    videoId?: number;
}

const props = defineProps<Props>();
const emit = defineEmits<Emit>();

const isFormValid = ref(false);
const refForm = ref<VForm>();

const checkAutocomplete = ref(false);
const videoTypeStore = useVideoTypeStore();
const videoData = ref<Video | any>({
    id: 0,
    title: '',
    duration: '',
    keyNo: '',
    remark: '',
    videoTypeId: 0,
    category: [],
    doNotPlay: [],
    noBackToBack: [],
    subCategory: []
});

watch(props, async (oldId, newId) => {
    refForm.value?.reset();
    refForm.value?.resetValidation();
    checkAutocomplete.value = false;

    await axiosIns.get<Category[]>('Category/GetParent').then((response) => {
        categories.value = response;
    });
    await axiosIns.get<Building[]>('Building/getBuilding').then((response) => {
        buildings.value = response;
    });
    if (newId.videoId) {
        axiosIns.get('http://localhost:5124/api/Video/' + newId.videoId).then((response: any) => {
            videoData.value = response;
            axiosIns.get<Category[]>('Category/GetSub', {
                params: {
                    categories: response.category.map((cat: Category) => cat.id)
                }
            }).then((data) => {
                subCategories.value = data;
                videoData.value.subCategory = activateAutocomplete(response.subCategory, subCategories.value);
            })
            videoData.value.category = activateAutocomplete(response.category, categories.value);
            videoData.value.noBackToBack = activateAutocomplete(response.noBackToBack, categories.value);
            videoData.value.doNotPlay = activateAutocomplete(response.doNotPlay, buildings.value);
        });
    } else {
        videoData.value.id = 0;
        
    }
});

const activateAutocomplete = (arr1: any[], arr2: any[]): any[] => {
    return arr2.filter(item2 => arr1.some(item1 => item1.id === item2.id));
};

onMounted(() => {
    console.log(videoData.value);
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
    checkAutocomplete.value = true;
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

const getSubCategory = () => {
    if(videoData.value.category){
        videoData.value.subCategory = [];
        const categoryIds = videoData.value.category.map((cat: Category) => cat.id);

        if (categoryIds) {
            axiosIns.get<Category[]>('Category/GetSub', {
                params: {
                    categories: categoryIds
                }
            }).then((response: any) => {
                subCategories.value = response;
            }).catch((error: any) => {
                console.error('Error fetching subcategories:', error);
            });
        }
    }else{
        subCategories.value = [];
    }
    refForm.value?.resetValidation();
}
</script>

<template>
    <section>
        <VNavigationDrawer
            temporary
            :width="500"
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
                                        type="number"
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
                                    <VTextField v-model="videoData.remark" label="Remark" />
                                </VCol>

                                <VCol cols="12">
                                    <VAutocomplete
                                        v-model="videoData.category"
                                        chips
                                        closable-chips
                                        :items="categories"
                                        item-title="name"
                                        label="Category"
                                        :menu-props="{ maxHeight: 250 }"
                                        multiple
                                        return-object
                                        @update:model-value="getSubCategory"

                                        :rules="[checkAutocomplete? requiredValidator:true]"
                                    >
                                        <template #chip="{ props, item }">
                                            <VChip v-bind="props" :text="item.raw.name" />
                                        </template>

                                        <template #item="{ props, item }">
                                            <VListItem v-bind="props" :title="item?.raw?.name" />
                                        </template>
                                    </VAutocomplete>
                                </VCol>

                                <VCol cols="12" v-if="videoData.category.length > 0">
                                    <VAutocomplete
                                        v-model="videoData.subCategory"
                                        chips
                                        closable-chips
                                        :items="subCategories"
                                        item-title="name"
                                        label="Sub Category"
                                        :menu-props="{ maxHeight: 250 }"
                                        multiple
                                        return-object
                                        :rules="[checkAutocomplete? requiredValidator:true]"
                                    >
                                        <template #chip="{ props, item }">
                                            <VChip v-bind="props" :text="item.raw.name" />
                                        </template>

                                        <template #item="{ props, item }">
                                            <VListItem v-bind="props" :title="item?.raw?.name" />
                                        </template>
                                    </VAutocomplete>
                                </VCol>
                                
                                <VCol cols="12">
                                    <VAutocomplete
                                        v-model="videoData.noBackToBack"
                                        chips
                                        closable-chips
                                        :items="categories"
                                        item-title="name"
                                        label="No back to back with"
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
                                <VCol cols="12">
                                    <VAutocomplete
                                        v-model="videoData.doNotPlay"
                                        chips
                                        closable-chips
                                        :items="buildings"
                                        item-title="buildingName"
                                        label="Do not play on"
                                        :menu-props="{ maxHeight: 250 }"
                                        multiple
                                        return-object
                                    >
                                        <template #chip="{ props, item }">
                                            <VChip v-bind="props" :text="item.raw.buildingName" />
                                        </template>

                                        <template #item="{ props, item }">
                                            <VListItem v-bind="props" :title="item?.raw?.buildingName" />
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
<style lang="scss">
.v-autocomplete {
  .v-chip {
    white-space: wrap;
  }

  .v-chip.v-chip--density-default {
    block-size: unset !important;
  }
}
</style>
