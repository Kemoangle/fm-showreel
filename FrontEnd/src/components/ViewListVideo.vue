<script lang="ts" setup>
import { useVideoListStore } from '@/store/useVideoListStore';


interface IProps {
    isDrawerOpen: boolean;
    videoListId: number;
}

interface Emit {
    (e: 'update:isDrawerOpen', value: boolean): void;
}

const props = defineProps<IProps>();
const emit = defineEmits<Emit>();

const handleSave = () => {
};

const handleClose = () => {
    emit('update:isDrawerOpen', false);
};


const videoListStore = useVideoListStore();


const videoList = ref<any>([]);

watch(props, async (oldId, newId) => {
    if (newId.videoListId && newId.videoListId > 0 ) {
        videoListStore.getVideoByListId(newId.videoListId).then(response =>{
            videoList.value = response;
        })
    }
});
</script>

<template>
    <VDialog v-model="props.isDrawerOpen" max-width="1000">
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
                            <th scope="col">REMASK</th>
                        </tr>
                    </thead>

                    <!-- 👉 table body -->
                    <tbody>
                        <tr v-for="(video, index) in videoList" :key="video.id">
                            <td>
                                <div class="d-flex align-center">
                                    {{ video.video?.title }}
                                </div>
                            </td>

                            <td class="">
                                {{ video.video?.duration }}
                            </td>
                            <td class="">
                                {{ video.loopNum }}
                            </td>
                            <td>
                                <span class="">{{ video.video?.keyNo }}</span>
                            </td>
                            <td>
                                {{ video.video?.rule }}
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
