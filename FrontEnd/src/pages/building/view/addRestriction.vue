<script setup lang="ts">
import { PerfectScrollbar } from 'vue3-perfect-scrollbar';
// eslint-disable-next-line @typescript-eslint/consistent-type-imports
import type { VForm } from 'vuetify/components';

import { Category } from '@/model/category';
import { Restriction } from '@/model/restriction';
import axiosIns from '@/plugins/axios';
import { useCategoryStore } from '@/store/useCategoryStore';
import { requiredValidator } from '@validators';

interface Emit {
    (e: 'update:isDrawerOpen', value: boolean): void;
    (e: 'restrictionData', value: Restriction): void;
}

interface Props {
    isDrawerOpen: boolean;
    restrictionId?: number;
    categoryExist: number[];
}

const props = defineProps<Props>();
const emit = defineEmits<Emit>();
const categoryStore = useCategoryStore();
const categoriesData = ref<Category[]>([]);
const isFormValid = ref(false);
const refForm = ref<VForm>();
const subCategories = ref<Category[]>([]);
const restrictionData = ref<Restriction>({
    id: 0,
    buildingId: 0,
    categoryId: 0,
    type: 'Except',
    arrCategory: [],
    category: { id: 0 },
});

watch(props, async (oldId, newId) => {
    createVideos();
    refForm.value?.reset();
    refForm.value?.resetValidation();
    if (newId.restrictionId) {
        await axiosIns
            .get('Restriction/GetBuildingRestrictionById/' + newId.restrictionId)
            .then((response: any) => {
                categoryStore.getSubCategory(response.data.category.id).then((data: any) => {
                    subCategories.value = data;
                    restrictionData.value = response.data;
                    restrictionData.value.arrCategory = activateCategories(
                        response.data.arrCategory,
                        subCategories.value
                    );
                });
            });
    } else {
        restrictionData.value.id = 0;
    }
});

const activateCategories = (arrCategory: any[], subCategories: any[]): any[] => {
    return subCategories.filter((subCategory) =>
        arrCategory.some((category) => category.id === subCategory.id)
    );
};

const getSubCategories = () => {
    if (restrictionData.value.category) {
        restrictionData.value.arrCategory = [];
        restrictionData.value.type = undefined;
        categoryStore.getSubCategory(restrictionData.value.category.id).then((response: any) => {
            subCategories.value = response;
        });
    }
};

const createVideos = () => {
    categoryStore.getAllCategory();
    if(props.categoryExist.length > 0 && props.categoryExist[0] > 0){
        categoriesData.value = categoryStore.data.filter((c: Category) => !props.categoryExist.includes(c.id));
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
                <h6 class="text-h6">Restriction</h6>
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
                                        :items="categoriesData"
                                        item-title="name"
                                        label="Restriction"
                                        :menu-props="{ maxHeight: 250 }"
                                        :rules="[requiredValidator]"
                                        @update:model-value="getSubCategories"
                                        return-object
                                    />
                                </VCol>
                                <VCol cols="12">
                                    <VSelect
                                        v-model="restrictionData.type"
                                        label="Action"
                                        :items="['Except', 'Exclude']"
                                        :menu-props="{ maxHeight: 200 }"
                                        :rules="[
                                            restrictionData.category &&
                                            restrictionData.arrCategory &&
                                            restrictionData.arrCategory.length > 0
                                                ? requiredValidator
                                                : true,
                                        ]"
                                    />
                                </VCol>
                                <VCol cols="12">
                                    <VAutocomplete
                                        v-model="restrictionData.arrCategory"
                                        chips
                                        closable-chips
                                        :items="subCategories"
                                        item-title="name"
                                        :label="
                                            restrictionData.type == 'Except'
                                                ? 'Except'
                                                : restrictionData.type == 'Exclude'
                                                ? 'Exclude'
                                                : 'Except/Exclude'
                                        "
                                        :menu-props="{ maxHeight: 250 }"
                                        multiple
                                        return-object
                                        :rules="[
                                            restrictionData.category && restrictionData.type
                                                ? requiredValidator
                                                : true,
                                        ]"
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
