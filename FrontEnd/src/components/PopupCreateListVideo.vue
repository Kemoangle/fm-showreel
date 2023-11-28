<script lang="ts" setup>
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

const props = defineProps<IProps>();
const emit = defineEmits<Emit>();

const videos = ref<IVideo[]>([]);
const searchVideo = ref('');

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
    emit('update:isDialogVisible', false);

    emit('submit:save', false);
};

const handleClose = () => {
    emit('update:isDialogVisible', false);
};

const getListVideo = (videos: IVideo[]) => {
    const listName: string[] = [];
    if (videos.length) {
        videos.forEach((video) => {
            if (video.isActive) {
                listName.push(video.name);
            }
        });
        return listName;
    }
    return [];
};
</script>

<template>
    <VDialog v-model="props.isDialogVisible" max-width="1000">
        <!-- Dialog Content -->
        <VCard title="Create list Video">
            <VCardText>
                <VRow>
                    <VCol cols="12" sm="6" md="6">
                        <VCard>
                            <VCardText class="max-height-500">
                                <div class="sticky">
                                    <VTextField v-model="searchVideo" placeholder="Search Video" />
                                </div>
                                <VCheckbox
                                    v-model="video.isActive"
                                    v-for="video in videos"
                                    :key="video.id"
                                    :label="video.name"
                                    v-show="video.name.includes(searchVideo.trim())"
                                />
                            </VCardText>
                        </VCard>
                    </VCol>
                    <VCol cols="12" sm="6" md="6">
                        <VCard>
                            <VCardText class="max-height-500">
                                <VList :items="getListVideo(videos)" />
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
    </VDialog>
</template>

<style lang="scss" scoped>
.max-height-500 {
    max-height: 500px;
    overflow-y: auto;
    position: relative;
}
.sticky {
    position: sticky;
    top: 0;
    z-index: 1;
    background-color: white;
}
</style>
