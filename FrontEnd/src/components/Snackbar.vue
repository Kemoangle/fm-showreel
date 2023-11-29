<script lang="ts">
const show = ref(false);
const message = ref('');
const timeout = ref(1000);
const color = ref<string | undefined>('success');

type SnackbarColor = 'success' | 'warning' | 'error';

export function useSnackbar() {
    function showSnackbar(vMessage: string, vColor?: SnackbarColor, vTimeout?: number) {
        message.value = vMessage;
        color.value = vColor;
        timeout.value = vTimeout || 2000;

        show.value = true;
    }

    return { showSnackbar };
}

export default {
    // eslint-disable-next-line vue/component-api-style
    data() {
        return {
            show,
            message,
            timeout,
            color,
        };
    },
};
</script>

<template>
    <VSnackbar v-model="show" :timeout="timeout" :color="color">
        {{ message }}
    </VSnackbar>
</template>
