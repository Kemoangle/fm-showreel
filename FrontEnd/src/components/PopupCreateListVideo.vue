<script lang="ts" setup>
import { Video } from '@/model/video';
import { VideoList } from '@/model/videoList';
import { VideoVideolist } from '@/model/videoVideolist';
import { useVideoStore } from '@/store/useVideoStore';

interface IVideo {
    id: number;
    name: string;
    isActive: boolean;
}

interface IProps {
    isDialogVisible: boolean;
}

interface Emit {
    (e: 'close', value: boolean): void;
    (e: 'update:isDialogVisible', value: boolean): void;
    (e: 'submit:save', value: boolean): void;
}

"".toLocaleLowerCase();
const props = defineProps<IProps>();
const emit = defineEmits<Emit>();
const videoStore = useVideoStore();
const videos = ref<IVideo[]>([]);
const searchVideo = ref('');
const VideoVideoList = ref<VideoVideolist[]>([]);
const videoList = ref<VideoList>()

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
    videoStore.getAllVideos();
};

onMounted(() => {
    createVideos();
});

const handleSave = () => {
    emit('update:isDialogVisible', false);

    emit('submit:save', false);
};

const handleClose = () => {
    emit('update:isDialogVisible', false);
};

const getListVideo = (videos: Video[]) => {
    const listName: string[] = [];
    if (videos.length) {
        videos.forEach((video) => {
            if (video.isActive && video.title) {
                listName.push(video.title);
            }
        });
        return listName;
    }
    return [];
};
</script>

<template>
    <VDialog v-model="props.isDialogVisible" max-width="1200">
        <!-- Dialog Content -->
        <VForm>

        <VCard title="Create list Video">
            <VCardText>
                <VRow>
                    <VCol cols="12" sm="6" md="6">
                        <div class="sticky mb-4">
                            <VTextField 
                                v-model="searchVideo" 
                                label="Search Video"
                                clearable
                            />
                        </div>
                        <VCard>
                            
                            <VCardText class="max-height-500">
                                
                                
                                <div v-for="video in videoStore.video" class="row">
                                    <VRow>
                                        <VCol cols="8" sm="8" md="8">
                                            <VCheckbox
                                                v-model="video.isActive"
                                                :key="video.id"
                                                :label="video.title"
                                                v-show="video.title.toLocaleLowerCase().includes(searchVideo.toLocaleLowerCase().trim())"
                                            />
                                        </VCol>
                                        <VCol cols="4" sm="4" md="4">
                                            <VTextField
                                                v-if="video.isActive"
                                                />
                                                <!-- label="Loop"
                                                type="number" -->
                                        </VCol>
                                    </VRow>
                                </div>
                            </VCardText>
                        </VCard>
                    </VCol>
                    <VCol cols="12" sm="6" md="6">
                        <VTextField
                            label="Video list title"
                            clearable
                            class="mb-4"
                        />
                        <VCard>
                            <VCardText class="max-height-500">
                                <VList :items="getListVideo(videoStore.video)" />
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

.v-field__input {
  /* stylelint-disable-next-line liberty/use-logical-spec */
  min-height: 10px !important;
}

.v-text-field input {
  /* stylelint-disable-next-line liberty/use-logical-spec */
  height: 10px;
}
</style>
