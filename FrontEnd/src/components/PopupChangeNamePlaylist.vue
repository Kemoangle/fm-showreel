<script lang="ts" setup>
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

watch(
    props,
    (value) => {
        console.log('hel;l;o');
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
            <VCardText> building </VCardText>

            <VCardActions>
                <VSpacer />
                <VBtn color="error" @click="dialogVisibleUpdate(false)"> Cancel </VBtn>
                <VBtn color="success" @click="handleConfirm()"> Agree </VBtn>
            </VCardActions>
        </VCard>
    </VDialog>
</template>
