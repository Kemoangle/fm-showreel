<script lang="ts" setup>
import { requiredValidator } from '@/@core/utils/validators';
import { IListPlaylist } from '@/model/generatorPlaylist';

interface Props {
    isDialogVisible: boolean;
    title: string;
    playlist: IListPlaylist | undefined;
}

interface Emit {
    (e: 'update:modelValue', value: boolean): void;
    (e: 'update:isDialogVisible', val: boolean): void;
    (e: 'onAgree'): void;
}

const props = withDefaults(defineProps<Props>(), {
    isDialogVisible: false,
    title: 'Change name building',
});

const listName = ref<string[]>();

watch(
    props,
    (value) => {
        if (value) {
            listName.value = value.playlist?.nameTimestamp;
        }
    },
    { deep: true }
);

const emit = defineEmits<Emit>();
const dialogVisibleUpdate = (val: boolean) => {
    emit('update:isDialogVisible', val);
};

const handleConfirm = () => {
    emit('onAgree');
    dialogVisibleUpdate(false);
};
</script>

<template>
    <VDialog
        :model-value="props.isDialogVisible"
        @update:model-value="dialogVisibleUpdate"
        class="v-dialog-sm"
    >
        <!-- Dialog Content -->
        <VCard :title="title">
            <VCardText>
                <div v-for="(building, index) in listName" v-if="listName">
                    <VTextField
                        v-model="listName[index]"
                        :rules="[requiredValidator]"
                        :label="`building ${playlist?.buildingName[index]}`"
                        class="mt-4"
                    />
                </div>
            </VCardText>

            <VCardActions>
                <VSpacer />
                <VBtn color="primary" variant="elevated" @click="handleConfirm()"> Okay </VBtn>
            </VCardActions>
        </VCard>
    </VDialog>
</template>
