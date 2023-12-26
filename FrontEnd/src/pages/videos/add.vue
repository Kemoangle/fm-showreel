<script setup lang="ts">
import { PerfectScrollbar } from 'vue3-perfect-scrollbar';
// eslint-disable-next-line @typescript-eslint/consistent-type-imports
import type { VForm } from 'vuetify/components';

import { Building } from '@/model/building';
import { Category } from '@/model/category';
import { Video } from '@/model/video';
import axiosIns from '@/plugins/axios';
import { useCategoryStore } from '@/store/useCategoryStore';
import { requiredValidator } from '@validators';

const listCategory = ref<any[]>([]);
const categories = ref<Category[]>([]);
const subCategories = ref<Category[]>([]);
const buildings = ref<Building[]>([]);

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
const categoryStore = useCategoryStore();

const isFormValid = ref(false);
const refForm = ref<VForm>();
const menu = ref(false);
const keySearchCategory = ref('');

const checkAutocomplete = ref(false);
const isRequiredCategory = ref(false);

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
    subCategory: [],
});

watch(props, async (oldId, newId) => {
    refForm.value?.reset();
    refForm.value?.resetValidation();
    getCategory();
    checkAutocomplete.value = false;
    isRequiredCategory.value = false;

    await axiosIns.get<Category[]>('Category/GetParent').then((response: any) => {
        categories.value = response.data;
    });
    await axiosIns.get<Building[]>('Building/getBuilding').then((response: any) => {
        buildings.value = response.data;
    });
    if (newId.videoId) {
        axiosIns.get('Video/' + newId.videoId).then((response: any) => {
            videoData.value = response.data;
            axiosIns
                .get<Category[]>('Category/GetSub', {
                    params: {
                        categories: response.data.category.map((cat: Category) => cat.id),
                    },
                })
                .then((data: any) => {
                    subCategories.value = data.data;
                    videoData.value.subCategory = activateAutocomplete(
                        response.data.subCategory,
                        subCategories.value
                    );
                });
            videoData.value.category = activateAutocomplete(response.data.category, categories.value);
            videoData.value.noBackToBack = activateAutocomplete(
                response.data.noBackToBack,
                categories.value
            );
            videoData.value.doNotPlay = activateAutocomplete(response.data.doNotPlay, buildings.value);
            fetchCategory(videoData.value.category);
            fetchSubCategories(videoData.value.subCategory);

        });
    } else {
        videoData.value.id = 0;
        videoData.value.category = [];
        videoData.value.subCategory = [];
    }
});

const activateAutocomplete = (arr1: any[], arr2: any[]): any[] => {
    return arr2.filter((item2) => arr1.some((item1) => item1.id === item2.id));
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
    
    const activeCategories = listCategory.value.filter((category: any) => category.active);
    videoData.value.category = activeCategories;
    delete(videoData.value.category.subCategory);
    const activeSubCategories = activeCategories
        .map((category: any) => category.subCategory)
        .flat()
        .filter((subCategory: any) => subCategory && subCategory.active);
    videoData.value.subCategory = activeSubCategories;
    checkCategory();

    refForm.value?.validate().then(({ valid }) => {
        if (valid && !isRequiredCategory.value) {
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

const fetchCategory = async (category: any[]) => {
    if (listCategory.value) {
        listCategory.value=listCategory.value.map((x:any)=>({...x, active:false}));

        listCategory.value.forEach((element: any) => {
            if (category.some(category => category.id === element.id)) {
                    element.active = true;
            }
        });
    }
}
const fetchSubCategories = async (subCategories: any[]) => {
    if (listCategory.value) {
        listCategory.value.forEach((element: any) => {
            if (element.subCategory && Array.isArray(element.subCategory)) {
                element.subCategory.forEach((e: any) => {
                    e.active = false;
                    if (subCategories.some(subCategory => subCategory.id === e.id)) {
                        e.active = true;
                    }
                });
            }
        });
    }
}

const checkCategory = () =>{
    isRequiredCategory.value = false;
    if(videoData.value.category.length < 1){
        isRequiredCategory.value = true;
    }
}

const handleClickCategory = (category: any) => {
    if (category.subCategory && Array.isArray(category.subCategory) && !category.active) {
      category.subCategory.forEach((subCategory: any) => {
        subCategory.active = false;
      });
    }
    if (category.active) {
      const isIdInArray = videoData.value.category.some((existingCategory: any) => existingCategory.id === category.id);
      if (!isIdInArray) {
        videoData.value.category.push(category);
      }
    }
}

const handleClickSubCategory = (category: any) => {
    if (category.active) {
      const isIdInArray = videoData.value.subCategory.some((existingCategory: any) => existingCategory.id === category.id);
      if (!isIdInArray) {
        videoData.value.subCategory.push(category);
      }
    }
}

const getCategory = async() =>{
    await axiosIns.get<Category[]>('Category/GetAllCategory', {
        params: {keySearch: keySearchCategory.value}
    }).then((response: any) => {
        listCategory.value = response.data;
        fetchCategory(videoData.value.category);
        fetchSubCategories(videoData.value.subCategory);
    });
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
                                    <v-menu v-model="menu" :close-on-content-click="false" 
                                        location="bottom" 
                                        height="300px" 
                                        width="300px"
                                    >
                                        <template v-slot:activator="{ props }">
                                            <VBtn color="info" v-bind="props" append-icon="mdi-menu-down"> Categories </VBtn>  
                                        </template>
                                        
                                            <VList>
                                                <VTextField
                                                    placeholder="Search"
                                                    density="compact"
                                                    @input="getCategory"
                                                    v-model="keySearchCategory"
                                                />
                                                <div v-if="!categoryStore.isLoading" v-for="category in listCategory">
                                                    <div class="father">
                                                        <VCheckbox :id="category.name" v-model="category.active" @change="handleClickCategory(category)"/>
                                                        <label style="cursor: pointer;" :for="category.name">{{ category.name }}</label>
                                                        <VIcon
                                                            icon="mdi-menu-down"
                                                            :size="20"
                                                            class="me-3"
                                                            color="primary"
                                                        >
                                                        </VIcon>
                                                    </div>
                                                    <div class="children" v-if="category.active">
                                                        <div class="children-item" v-for="children in category.subCategory">
                                                            <VCheckbox :id="children.name" v-model="children.active" @change="handleClickSubCategory(children)"/>
                                                            <label style="cursor: pointer;" :for="children.name">{{ children.name }}</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </VList>
                                    </v-menu>
                                    <span class="ml-3" style="color: rgb(235, 98, 98);" v-if="isRequiredCategory">This field is required</span>
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
                                            <VListItem
                                                v-bind="props"
                                                :title="item?.raw?.buildingName"
                                            />
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

.father {
  display: flex;
  align-items: center;
  padding-inline-start: 10px;
}

p {
  padding: 0;
  margin: 0;
}

.children {
  &-item {
    display: flex;
    align-items: center;
    padding-inline-start: 40px;
  }
}
</style>
