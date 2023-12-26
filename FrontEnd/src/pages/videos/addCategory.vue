<script setup lang="ts">
import { PerfectScrollbar } from 'vue3-perfect-scrollbar';
// eslint-disable-next-line @typescript-eslint/consistent-type-imports
import type { VForm } from 'vuetify/components';

import { useSnackbar } from '@/components/Snackbar.vue';
import { Category, SubCategory } from '@/model/category';
import axiosIns from '@/plugins/axios';
import { useCategoryStore } from '@/store/useCategoryStore';
import { requiredValidator } from '@validators';

interface Emit {
    (e: 'update:isDrawerOpenCategory', value: boolean): void;
    (e: 'categoryData', value: Category): void;
}

interface Props {
    isDrawerOpenCategory: boolean;
    categoryId: number;
}
const props = defineProps<Props>();

const emit = defineEmits<Emit>();
const categoryStore = useCategoryStore();
const isFormValid = ref(false);
const refForm = ref<VForm>();
const { showSnackbar } = useSnackbar();

const categoryData = ref<Category>({
    id: 0,
    name: '',
    parent: 0,
    subCategory: [],
});

watch(props, async (oldId, newId) => {
    console.log('ssss');
    refForm.value?.reset();
    refForm.value?.resetValidation();
    if (newId.categoryId > 0) {
        axiosIns.get<Category>('Category/' + newId.categoryId).then((reponse: any) => {
            categoryData.value = reponse.data;
        });
    } else {
        categoryData.value.id = 0;
        categoryData.value.subCategory = [];
        refForm.value?.reset();
        refForm.value?.resetValidation();
    }
});

onMounted(() => {
    refForm.value?.reset();
    refForm.value?.resetValidation();
});

// 👉 drawer close
const closeNavigationDrawer = () => {
    emit('update:isDrawerOpenCategory', false);

    nextTick(() => {
        refForm.value?.reset();
        refForm.value?.resetValidation();
    });
};

const onSubmit = async () => {
    refForm.value?.validate().then(async ({ valid }) => {
        if (valid) {
            const { subCategory, ...rest } = categoryData.value;
            if (categoryData.value.id && categoryData.value.id > 0) {
                await categoryStore
                    .updateCategory(rest)
                    .then((response) => {
                        categoryStore.updateSubCategory(subCategory, response.id).then((data) => {
                            emit('categoryData', categoryData.value);
                            emit('update:isDrawerOpenCategory', false);
                            nextTick(() => {
                                refForm.value?.reset();
                                refForm.value?.resetValidation();
                            });
                        });
                    })
                    .catch((error) => {
                        showSnackbar(error.data.Category[0], 'error');
                    });
            } else {
                await categoryStore
                    .addCategory(rest)
                    .then((response) => {
                        categoryStore.updateSubCategory(subCategory, response.id).then((data) => {
                            emit('categoryData', categoryData.value);
                            emit('update:isDrawerOpenCategory', false);
                            nextTick(() => {
                                refForm.value?.reset();
                                refForm.value?.resetValidation();
                            });
                        });
                    })
                    .catch((error) => {
                        showSnackbar(error.data.Category[0], 'error');
                    });
            }
        }
    });
};

const handleDrawerModelValueUpdate = (val: boolean) => {
    emit('update:isDrawerOpenCategory', val);
};

const addInput = () => {
    if (categoryData.value.subCategory === undefined) {
        categoryData.value.subCategory = [];
    }
    const newSubCategory: SubCategory = {
        id: 0,
        name: '',
    };
    categoryData.value.subCategory.push({ ...newSubCategory });
};

const removeSubCategory = (index: number) => {
    if (categoryData.value.subCategory) {
        categoryData.value.subCategory.splice(index, 1);
    }
};
</script>

<template>
    <section>
        <VNavigationDrawer
            temporary
            :width="400"
            location="end"
            class="scrollable-content"
            :model-value="props.isDrawerOpenCategory"
            @update:model-value="handleDrawerModelValueUpdate"
        >
            <!-- 👉 Title -->
            <div class="d-flex align-center bg-var-theme-background px-5 py-2">
                <h6 class="text-h6">Category</h6>
                <VSpacer />
                <VBtn size="small" color="secondary" variant="text" icon="mdi-close" />
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
                                        v-model="categoryData.name"
                                        :rules="[requiredValidator]"
                                        label="Category Name"
                                    />
                                </VCol>
                                <VCol>
                                    <VBtn color="primary" prepend-icon="mdi-add" @click="addInput">
                                        Add Sub Category
                                    </VBtn>
                                </VCol>
                                <VCol
                                    cols="12"
                                    v-if="categoryData.subCategory"
                                    v-for="(item, index) in categoryData.subCategory"
                                    :key="index"
                                    class="d-flex align-center"
                                >
                                    <VTextField
                                        v-model="item.name"
                                        label=" Sub Category Name"
                                        :rules="[requiredValidator]"
                                    />

                                    <VBtn
                                        style="background-color: transparent;"
                                        variant="text"
                                        class="ml-1"
                                        @click="removeSubCategory(index)"
                                    >
                                        <VIcon
                                            icon="mdi-close-outline"
                                            color="error"
                                            :size="20"
                                            style="height: auto;"
                                        />
                                    </VBtn>
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
