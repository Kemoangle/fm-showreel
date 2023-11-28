<script setup lang="ts">
import { VueDraggableNext } from 'vue-draggable-next';
import { generatorPlaylist } from '@/utils/generatorPlaylist';
import { IVideos, IListInfoCompany, IListPlaylist, IPlaylist } from '@/model/generatorPlaylist';
import PopupCreateListVideo from '@/components/PopupCreateListVideo.vue';
import PopupViewListVideo from '@/components/PopupViewListVideo.vue';
import { listVideo1, listVideo2, listVideo3 } from '@/utils/constant';
const selectedBuilding = ref<string[]>([]);
const selectedListVideo = ref();

const isDialogListVideoVisible = ref(false);
const isDialogListVideoCurrent = ref(false);

const companies = [
    { title: 'building 1', value: 'com1' },
    { title: 'building 2', value: 'com2' },
    { title: 'building 3', value: 'com3' },
];

const listVideos = [
    { title: 'listvideo 1', value: 'admin', videos: listVideo1 },
    { title: 'listvideo 2', value: 'author', videos: listVideo2 },
    { title: 'listvideo 3', value: 'editor', videos: listVideo3 },
];

const listInfoBuilding = [
    {
        buildingName: 'building 1',
        landlordAds: [],
        listVideos: [
            {
                key: 1,
                name: 'Allswell - Red Bull',
                durations: 15,
                loop: 3,
                category: 'F&B',
                restriction: '',
            },
            {
                key: 2,
                name: 'Darlie',
                durations: 15,
                loop: 4,
                category: '',
                restriction: '',
            },
            {
                key: 3,
                name: 'Firefly',
                durations: 25,
                loop: 3,
                category: 'Travel',
                restriction: '',
            },
            {
                key: 4,
                name: 'Skechers',
                durations: 30,
                loop: 4,
                category: '',
                restriction: '',
            },
            {
                key: 5,
                name: 'Shaw - Retribution',
                durations: 15,
                loop: 4,
                category: 'Movie',
                restriction: '',
            },
            {
                key: 6,
                name: 'Tour de France',
                durations: 40,
                loop: 2,
                category: '',
                restriction: '',
            },
            {
                key: 7,
                name: 'Captive Media OOH Video 2',
                durations: 10,
                loop: 1,
                category: '',
                restriction: 'No Back to Back With FMSG Contents',
            },
            {
                key: 8,
                name: 'FMSG Hiring',
                durations: 15,
                loop: 1,
                category: '',
                restriction: '',
            },
        ],
    },
];

onMounted(() => {
    // check all companies
    companies.forEach((building) => {
        selectedBuilding.value.push(building.value);
    });
});

// 👉 listPlaylist list
const listPlaylist = ref<IListPlaylist[]>();

const handleGeneratorPlaylist = () => {
    const exportPlaylist = new generatorPlaylist();
    const newPlaylistBuilding: IListPlaylist[] = [];

    const genPlaylist = (list: IVideos[]) => {
        const playlist: IPlaylist[] = [];

        list.forEach((l) => {
            if (l) {
                playlist.push({
                    category: l.category,
                    duration: l.durations,
                    keyNo: '1',
                    remasks: '1',
                    title: l.name,
                });
            }
        });

        return playlist;
    };

    companies.forEach((building) => {
        if (selectedBuilding.value.includes(building.value)) {
            const playlist = exportPlaylist.createListVideo(
                listVideos.find((x) => x.value == selectedListVideo.value)?.videos as IVideos[]
            );

            newPlaylistBuilding.push({
                buildingName: 'building ' + building.title,
                playlist: genPlaylist(playlist),
            });
        }
    });

    listPlaylist.value = newPlaylistBuilding;
};

const handleClickCreateListVideo = () => {
    isDialogListVideoVisible.value = !isDialogListVideoVisible.value;
};

const handleClickShowListVideo = () => {
    isDialogListVideoCurrent.value = !isDialogListVideoCurrent.value;
};

const handleCloseDialog = () => {
    isDialogListVideoVisible.value = !isDialogListVideoVisible.value;
};

const handleCloseDialogShowListVideo = () => {
    isDialogListVideoCurrent.value = !isDialogListVideoCurrent.value;
};
</script>

<template>
    <section>
        <VCard title="Filters" class="mb-6">
            <VCardText>
                <VRow>
                    <!-- <VCol cols="12" sm="4">
                        <VBtn variant="tonal" color="secondary" prepend-icon="mdi-tray-arrow-up">
                            Import Excel Companies
                        </VBtn>
                    </VCol> -->
                    <VCol cols="12" sm="4">
                        <VSelect
                            v-model="selectedListVideo"
                            label="Select List Video"
                            :items="listVideos"
                            clearable
                            clear-icon="mdi-close"
                        />
                    </VCol>
                    <VCol cols="12" sm="4">
                        <VSelect
                            v-model="selectedBuilding"
                            label="Select Buildings"
                            :items="companies"
                            clearable
                            clear-icon="mdi-close"
                            multiple
                        />
                    </VCol>
                    <VCol cols="12" sm="4">
                        <VBtn
                            variant="tonal"
                            color="secondary"
                            prepend-icon="mdi-tray-arrow-down"
                            @click="handleGeneratorPlaylist"
                        >
                            Generator
                        </VBtn>
                    </VCol>
                </VRow>
                <VRow>
                    <VCol cols="12" sm="4">
                        <VBtn
                            variant="tonal"
                            color="secondary"
                            prepend-icon="mdi-tray-arrow-down"
                            @click="handleClickCreateListVideo"
                        >
                            Create List Video
                        </VBtn>
                    </VCol>
                    <VCol
                        cols="12"
                        sm="4"
                        v-if="listVideos.find((x) => x.value == selectedListVideo)?.videos"
                    >
                        <VBtn
                            variant="tonal"
                            color="secondary"
                            prepend-icon="mdi-tray-arrow-down"
                            @click="handleClickShowListVideo"
                        >
                            View List Video
                        </VBtn>
                    </VCol>
                </VRow>
            </VCardText>
        </VCard>

        <VCard
            :title="`Playlist ${building.buildingName}`"
            v-for="building in listPlaylist"
            :key="building.buildingName"
            class="mb-6 position-relative"
        >
            <VBtn color="primary" class="position-absolute"> Save </VBtn>
            <VTable class="text-no-wrap">
                <thead>
                    <tr>
                        <th scope="col">STT</th>
                        <th scope="col">TITLE</th>
                        <th scope="col">DURATION</th>
                        <th scope="col">KEY NO</th>
                        <th scope="col">CATEGORY</th>
                        <th scope="col">REMASK</th>
                    </tr>
                </thead>

                <tbody>
                    <tr v-for="(paylist, index) in building.playlist" :key="paylist.keyNo">
                        <td>
                            {{ index + 1 }}
                        </td>
                        <td>
                            {{ paylist.title }}
                        </td>

                        <td class="text-medium-emphasis">
                            {{ paylist.duration }}
                        </td>

                        <td>
                            {{ paylist.keyNo }}
                        </td>

                        <td class="text-capitalize">
                            {{ paylist.category }}
                        </td>

                        <td>
                            {{ paylist.remasks }}
                        </td>
                    </tr>
                </tbody>

                <!-- 👉 table footer  -->
                <tfoot v-show="!building.playlist.length">
                    <tr>
                        <td colspan="7" class="text-center">No data available</td>
                    </tr>
                </tfoot>
            </VTable>
        </VCard>

        <PopupCreateListVideo
            :is-dialog-visible="isDialogListVideoVisible"
            @close="handleCloseDialog"
        />

        <PopupViewListVideo
            :data-video="listVideos.find((x) => x.value == selectedListVideo)?.videos || []"
            :is-dialog-visible="isDialogListVideoCurrent"
            @close="handleCloseDialogShowListVideo"
        />
    </section>
</template>

<style lang="scss">
.app-user-search-filter {
    inline-size: 24.0625rem;
}

.text-capitalize {
    text-transform: capitalize;
}

.user-list-name:not(:hover) {
    color: rgba(var(--v-theme-on-background), var(--v-high-emphasis-opacity));
}
</style>

<style lang="scss" scope>
.user-pagination-select {
    .v-field__input,
    .v-field__append-inner {
        padding-block-start: 0.3rem;
    }
}
.position-absolute {
    top: 20px;
    right: 20px;
}
</style>
