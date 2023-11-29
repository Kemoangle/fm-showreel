<script lang="ts" setup>
import { Video } from '@/model/video';
import { VideoList } from '@/model/videoList';
import { VideoVideolist } from '@/model/videoVideolist';
import { useVideoStore } from '@/store/useVideoStore';
import { requiredValidator } from '@validators';
import type { VForm } from 'vuetify/components';


interface IProps {
    isDialogVisible: boolean;
}

interface Emit {
    (e: 'update:isDialogVisible', value: boolean): void;
    (e: 'videoListData', value: VideoList): void;
}

const refForm = ref<VForm>();
const isFormValid = ref(false);

''.toLocaleLowerCase();
const props = defineProps<IProps>();
const emit = defineEmits<Emit>();
const videoStore = useVideoStore();
const searchVideo = ref('');

const videoListData = ref<VideoList>({});

watch(props, async (oldId, newId) => {
    videoStore.getAllVideos();

});
const createVideos = () => {
    videoStore.getAllVideos();
};

onMounted(() => {
    createVideos();
});

const handleSave = () => {
    refForm.value?.validate().then(({ valid }) => {
        if (valid) {
            emit('videoListData', videoListData.value);
            emit('update:isDialogVisible', false);
            nextTick(() => {
                refForm.value?.reset();
                refForm.value?.resetValidation();
            });
        }
    });
};

const handleClose = () => {
    emit('update:isDialogVisible', false);
};

const getListVideo = (videos: Video[]) => {
    const item: VideoVideolist[] = [];
    const listVideo: Video[] = [];
    if (videos.length) {
        videos.forEach((video) => {
            if (video.isActive) {
                listVideo.push(video);
                item.push({videoId: video.id, loopNum: video.loop})
            }
        });
        videoListData.value.videoVideoList = item;
        return listVideo;
    }
    return [];
};

</script>

<template>
    <VDialog v-model="props.isDialogVisible" max-width="1200">
        <!-- Dialog Content -->
        <VForm ref="refForm" v-model="isFormValid" @submit.prevent="handleSave">
        <VCard title="Create list Video">
            <VCardText>
                <VRow>
                    <VCol cols="12" sm="5" md="5">
                        <div class="sticky mb-4">
                            <VTextField v-model="searchVideo" label="Search Video" clearable />
                        </div>
                        <VCard>
                            <VCardText class="max-height-500">
                                <VCheckbox
                                    v-for="video in videoStore.video"
                                    v-model="video.isActive"
                                    :key="video.id"
                                    :label="video.title"
                                    v-show="video.title.toLocaleLowerCase().includes(searchVideo?.toLocaleLowerCase().trim())"
                                />
                            </VCardText>
                        </VCard>
                    </VCol>
                    <VCol cols="12" sm="7" md="7">
                        <VTextField label="Video list title" clearable class="mb-4" v-model="videoListData.title" :rules="[requiredValidator]"/>
                        <VCard>
                            <VCardText class="max-height-500">
                                <div v-for="video in getListVideo(videoStore.video)">
                                    <VRow>
                                        <VCol cols="9" sm="9" md="9">
                                            <VCardText>{{ video.title }}</VCardText>
                                        </VCol>
                                        <VCol cols="3" sm="3" md="3" class="mt-2">
                                            <VTextField
                                                v-model="video.loop"
                                                label="Loop"
                                                type="number"
                                                density="compact"
                                                min="1"
                                                :rules="[requiredValidator]"
                                                @input="getListVideo(videoStore.video)"
                                            />
                                        </VCol>
                                    </VRow>
                                    <hr />
                                </div>
                            </VCardText>
                        </VCard>
                    </VCol>
                </VRow>
            </VCardText>

            <VCardActions>
                <VSpacer />
                <VBtn color="error" @click="handleClose"> Close </VBtn>
                <VBtn color="success" @click="handleSave"> Save </VBtn>
            </VCardActions>
        </VCard>
    </VForm>
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
