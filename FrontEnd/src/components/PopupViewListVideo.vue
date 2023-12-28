<script lang="ts" setup>
interface IVideo {
    id: number;
    name: string;
    isActive: boolean;
}

interface IProps {
    isDialogVisible: boolean;
    dataVideo: any[];
}

interface Emit {
    (e: 'close', value: boolean): void;
    (e: 'submit:save', value: boolean): void;
}

const props = defineProps<IProps>();
const emit = defineEmits<Emit>();

const videos = ref<IVideo[]>([]);

const createVideos = () => {
    const arr: IVideo[] = [];
    for (let i = 0; i < 100; i++) {
        arr.push({
            id: i + 1,
            name: 'video ' + (i + 1),
            isActive: false,
        });
    }
    videos.value = arr;
};

onMounted(() => {
    createVideos();
});

const handleSave = () => {
    emit('submit:save', false);
};

const handleClose = () => {
    emit('close', false);
};
</script>

<template>
    <VDialog v-model="props.isDialogVisible" max-width="1000">
        <!-- Dialog Content -->
        <VCard title="View list Video">
            <VCardText>
                <VTable class="text-no-wrap">
                    <!-- 👉 table head -->
                    <thead>
                        <tr>
                            <th scope="col">TITLE</th>
                            <th scope="col">DURATION</th>
                            <th scope="col">Loop</th>
                            <th scope="col">KEY NO</th>
                            <th scope="col">REMARK</th>
                        </tr>
                    </thead>

                    <!-- 👉 table body -->
                    <tbody>
                        <tr v-for="(video, index) in dataVideo" :key="video.key">
                            <td>
                                <div class="d-flex align-center">
                                    {{ video.name }}
                                </div>
                            </td>

                            <td class="">
                                {{ video.durations }}
                            </td>
                            <td class="">
                                {{ video.loop }}
                            </td>
                            <td>
                                <span class="">{{ video.key }}</span>
                            </td>
                            <td>
                                {{ video.remark }}
                            </td>
                        </tr>
                    </tbody>

                    <!-- 👉 table footer  -->
                    <!-- <tfoot v-show="!buildingStore.data.buildings">
                    <tr>
                        <td colspan="7" class="text-center">No data available</td>
                    </tr>
                </tfoot> -->
                </VTable>
            </VCardText>

            <VCardActions>
                <VSpacer />
                <VBtn color="error" @click="handleClose"> Close </VBtn>
                <VBtn color="success" @click="handleSave"> Save </VBtn>
            </VCardActions>
        </VCard>
    </VDialog>
</template>

<style lang="scss" scoped>
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
</style>
