<script lang="ts" setup>
import { VideoList } from '@/model/videoList';
import { useVideoListStore } from '@/store/useVideoListStore';

interface IProps {
    isDrawerOpen: boolean;
    videoListId: number;
    dataListVideo?: VideoList[];
}

interface Emit {
    (e: 'update:isDrawerOpen', value: boolean): void;
}

const props = defineProps<IProps>();
const emit = defineEmits<Emit>();

const handleClose = () => {
    emit('update:isDrawerOpen', false);
};

const videoListStore = useVideoListStore();

const videoList = ref<any>([]);

watch(props, async (oldId, newId) => {
    if (props.dataListVideo) {
        videoList.value = props.dataListVideo;
    } else {
        if (newId.videoListId && newId.videoListId > 0) {
            await videoListStore.getVideoByListId(newId.videoListId).then((response) => {
                videoList.value = response;
            });
        }
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
                            <td style="color: rgb(236, 114, 114)">
                                <p v-if="video.doNotPlay.length">
                                    Do pot play on (<span
                                        v-for="(item, idx) in video.doNotPlay"
                                        :key="item.id"
                                        >{{ item.buildingName
                                        }}<span v-if="idx < video.doNotPlay.length - 1"
                                            >/
                                        </span> </span
                                    >)
                                </p>
                                <p v-if="video.noBackToBack.length">
                                    No back to back with (<span
                                        v-for="(item, idx) in video.noBackToBack"
                                        :key="item.id"
                                        >{{ item.name
                                        }}<span v-if="idx < video.noBackToBack.length - 1"
                                            >/
                                        </span> </span
                                    >)
                                </p>
                            </td>
                        </tr>
                    </tbody>
                </VTable>
            </VCardText>
            <v-row align="center" justify="center" class="fill-height" v-show="!videoList.length">
                <v-col cols="12" class="text-center">
                    <VProgressCircular indeterminate color="info" />
                </v-col>
            </v-row>
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
