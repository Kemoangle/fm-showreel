<script setup lang="ts">
import { PerfectScrollbar } from 'vue3-perfect-scrollbar';
// eslint-disable-next-line @typescript-eslint/consistent-type-imports
import type { VForm } from 'vuetify/components';

import { Category } from '@/model/category';
import { VideoType } from '@/model/videoType';
import axiosIns from '@/plugins/axios';
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

const isFormValid = ref(false);
const refForm = ref<VForm>();

const categoryData = ref<VideoType | any>({
    id: 0,
    name: '',
});

watch(props, async (oldId, newId) => {
    refForm.value?.reset();
    refForm.value?.resetValidation();
    if (newId.categoryId && newId.categoryId > 0) {
        axiosIns.get<Category>('Category/' + newId.categoryId).then((reponse) => {
            categoryData.value = reponse;
        });
    } else {
        categoryData.value.id = 0;
        refForm.value?.reset();
        refForm.value?.resetValidation();
    }
});
onMounted(() => {});

// 👉 drawer close
const closeNavigationDrawer = () => {
    emit('update:isDrawerOpenCategory', false);

    nextTick(() => {
        refForm.value?.reset();
        refForm.value?.resetValidation();
    });
};

const onSubmit = () => {
    refForm.value?.validate().then(({ valid }) => {
        if (valid) {
            emit('categoryData', categoryData.value);
            emit('update:isDrawerOpenCategory', false);
            nextTick(() => {
                refForm.value?.reset();
                refForm.value?.resetValidation();
            });
        }
    });
};

const handleDrawerModelValueUpdate = (val: boolean) => {
    emit('update:isDrawerOpenCategory', val);
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
                                        v-model="categoryData.name"
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
