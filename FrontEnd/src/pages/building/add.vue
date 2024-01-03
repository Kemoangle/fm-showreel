<script setup lang="ts">
import { PerfectScrollbar } from 'vue3-perfect-scrollbar';
// eslint-disable-next-line @typescript-eslint/consistent-type-imports
import type { VForm } from 'vuetify/components';

import { Building } from '@/model/building';
import axiosIns from '@/plugins/axios';
import { requiredValidator } from '@validators';

interface Emit {
    (e: 'update:isDrawerOpen', value: boolean): void;
    (e: 'buildingData', value: Building): void;
}

interface Props {
    isDrawerOpen: boolean;
    buildingId?: number;
}

const props = defineProps<Props>();
const emit = defineEmits<Emit>();

const isFormValid = ref(false);
const refForm = ref<VForm>();

const buildingData = ref<Building | any>({
    id: 0,
    buildingName: '',
    address: '',
    district: '',
    remark: '',
    postalCode: 0,
    zone: 'City',
});

watch(props, async (oldId, newId) => {
    refForm.value?.reset();
    refForm.value?.resetValidation();
    if (newId.buildingId && newId.buildingId > 0) {
        axiosIns.get<Building>('Building/' + newId.buildingId).then((reponse) => {
            buildingData.value = reponse.data;
        });
    } else {
        buildingData.value.id = 0;
        refForm.value?.reset();
        refForm.value?.resetValidation();
    }
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
            emit('buildingData', buildingData.value);
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
                <h6 class="text-h6">Building</h6>

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
                                    <VTextField
                                        v-model="buildingData.buildingName"
                                        :rules="[requiredValidator]"
                                        label="Building Name"
                                    />
                                </VCol>

                                <!-- 👉 address -->
                                <VCol cols="12">
                                    <VTextField
                                        v-model="buildingData.address"
                                        :rules="[requiredValidator]"
                                        label="Address"
                                    />
                                </VCol>

                                <!-- 👉 district -->
                                <VCol cols="12">
                                    <VTextField
                                        v-model="buildingData.district"
                                        :rules="[requiredValidator]"
                                        label="District"
                                    />
                                </VCol>

                                <!-- 👉 remark -->
                                <VCol cols="12">
                                    <VTextField
                                        v-model="buildingData.remark"
                                        label="Remark"
                                    />
                                </VCol>
                                <VCol cols="12">
                                    <VTextField
                                        v-model="buildingData.postalCode"
                                        label="Postal Code"
                                    />
                                </VCol>
                                <!-- 👉 Zone -->
                                <VCol cols="12">
                                    <VSelect
                                        v-model="buildingData.zone"
                                        label="Select Zone"
                                        :rules="[requiredValidator]"
                                        :items="[
                                            'City',
                                            'West',
                                            'South',
                                            'Central',
                                            'East',
                                            'North',
                                        ]"
                                        :menu-props="{ maxHeight: 200 }"
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
