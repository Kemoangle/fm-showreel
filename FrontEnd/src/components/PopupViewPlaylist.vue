<script lang="ts" setup>
import { IPlaylist } from '@/model/generatorPlaylist';
import { IPostPlaylistStore } from '@/model/playlist';
import { VueDraggableNext } from 'vue-draggable-next';
import _ from 'lodash';
import { usePlaylistStore } from '@/store/usePlayListStore';
import { useSnackbar } from './Snackbar.vue';

interface IProps {
    isDrawerOpen: boolean;
    data: IPostPlaylistStore | undefined;
}

interface Emit {
    (e: 'update:isDrawerOpen', value: boolean): void;
}

const { showSnackbar } = useSnackbar();

const props = defineProps<IProps>();
const emit = defineEmits<Emit>();
const isDragging = ref(false);
const isLoading = ref(false);

const playlistStore = usePlaylistStore();

const handleClose = () => {
    emit('update:isDrawerOpen', false);
};

const playlist = ref<IPlaylist[]>([]);

watch(props, async (oldId, newId) => {
    if (props.data) {
        playlist.value = JSON.parse(props.data.jsonPlaylist) as IPlaylist[];
    }
});

const handleUpdate = () => {
    if (props.data) {
        isLoading.value = true;
        playlistStore
            .updatePlaylist({ ...props.data, jsonPlaylist: JSON.stringify(playlist.value) })
            .then((res: any) => {
                isLoading.value = false;

                if (res?.id) {
                    showSnackbar('Update playlist successfully!', 'success');
                }
            })
            .catch((err) => {
                isLoading.value = false;

                showSnackbar('Something went wrong!', 'error');
            });
    }
};

const dragOptions = () => {
    return {
        animation: 0,
        group: 'description',
        disabled: false,
        ghostClass: 'ghost',
    };
};
</script>

<template>
    <VDialog v-model="props.isDrawerOpen" max-width="1200">
        <VCard title=" ">
            <div class="position-absolute-left">
                <VTextField v-if="data" v-model="data.title" />
            </div>
            <VBtn
                color="primary"
                :disabled="isLoading"
                :loading="isLoading"
                class="position-absolute-right"
                @click="handleUpdate"
            >
                Save
            </VBtn>
            <VCardText class="pt-8">
                <VTable class="text-no-wrap">
                    <thead>
                        <tr>
                            <th scope="col">STT</th>
                            <th scope="col">TITLE</th>
                            <th scope="col">DURATION</th>
                            <th scope="col">KEY NO</th>
                            <th scope="col">CATEGORY</th>
                            <th scope="col">REMARK</th>
                        </tr>
                    </thead>

                    <VueDraggableNext
                        class="list-group"
                        tag="tbody"
                        handle=".handle"
                        :list="playlist"
                        v-bind="dragOptions"
                        @start="isDragging = true"
                        @end="isDragging = false"
                    >
                        <transition-group type="transition" name="flip-list">
                            <tr class="handle" v-for="(pl, index) in playlist" :key="pl.order">
                                <td>
                                    {{ index + 1 }}
                                </td>
                                <td>
                                    {{ pl.name }}
                                </td>
                                <td class="text-medium-emphasis">
                                    {{ pl.durations }}
                                </td>
                                <td>
                                    {{ pl.key }}
                                </td>
                                <td class="text-capitalize">
                                    {{ pl.category?.map((x: any) => x.name).join(', ') }}
                                </td>
                                <td>
                                    <VTextField class="input-remark" v-model="pl.remarks" />
                                </td>
                            </tr>
                            <tr key="j">
                                <td></td>
                                <td></td>
                                <td class="text-medium-emphasis">
                                    {{ playlist.reduce((a, b) => a + (b?.durations || 0), 0) }}
                                </td>
                                <td></td>
                                <td class="text-capitalize"></td>
                                <td></td>
                            </tr>
                        </transition-group>
                    </VueDraggableNext>

                    <tfoot v-show="!playlist.length">
                        <tr>
                            <td colspan="7" class="text-center">No data available</td>
                        </tr>
                    </tfoot>
                </VTable>
            </VCardText>

            <VCardActions>
                <VSpacer />
                <VBtn color="error" @click="handleClose"> Close </VBtn>
            </VCardActions>
        </VCard>
    </VDialog>
</template>

<style lang="scss">
.max-height-500 {
    position: relative;
    max-block-size: 500px;
    overflow-y: auto;
}

.sticky {
    position: sticky;
    z-index: 1;
    background-color: white;
    inset-block-start: 0;
}

.flip-list-move {
    transition: transform 0.5s;
}

.no-move {
    transition: transform 0s;
}

.ghost {
    background: #c8ebfb;
    opacity: 0.5;
}

.position-absolute-right {
    position: absolute;
    inset-block-start: 20px;
    inset-inline-end: 20px;
}
.position-absolute-left {
    position: absolute;
    left: 20px;
    top: 20px;
    input {
        min-width: 400px !important;
        min-height: 30px !important;
    }
}
</style>
