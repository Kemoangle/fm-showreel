<script setup lang="ts">
import PopupCreateListVideo from '@/components/PopupCreateListVideo.vue';
import PopupViewListVideo from '@/components/PopupViewListVideo.vue';
import { useSnackbar } from '@/components/Snackbar.vue';
import { IListPlaylist, IPlaylist, IVideos } from '@/model/generatorPlaylist';
import { listVideo1, listVideo2, listVideo3 } from '@/utils/constant';
import { generatorPlaylist } from '@/utils/generatorPlaylist';
import _ from 'lodash';
import { VueDraggableNext } from 'vue-draggable-next';

const { showSnackbar } = useSnackbar();

const selectedBuilding = ref<string[]>([]);
const selectedListVideo = ref();

const isDialogListVideoVisible = ref(false);
const isDialogListVideoCurrent = ref(false);

const dragOptions = () => {
    return {
        animation: 0,
        group: 'description',
        disabled: false,
        ghostClass: 'ghost',
    };
};

const isDragging = ref(false);

const buildings = [
    {
        title: 'CIMB',
        value: 'cimb',
        landlordAds: [
            {
                key: 'sSPZ004',
                name: 'JOHN',
                durations: 150,
                loop: 1,
                category: 'F&B',
                restriction: 'No Back to Back With F&B',
            },
        ],
    },
    { title: 'OGW', value: 'ogw', landlordAds: [] },
    {
        title: 'SunPlaza',
        value: 'sunplaza',
        landlordAds: [
            {
                key: 'sSPZ004',
                name: 'The Charcoal Grill Legend',
                durations: 150,
                loop: 4,
                category: 'F&B',
                restriction: 'No Back to Back With F&B',
            },
        ],
    },
];

const listVideos = [
    { title: 'listvideo 1', value: 'admin', videos: listVideo1 },
    { title: 'listvideo 2', value: 'author', videos: listVideo2 },
    { title: 'listvideo 3', value: 'editor', videos: listVideo3 },
];

onMounted(() => {
    // check all buildings
    buildings.forEach((building) => {
        selectedBuilding.value.push(building.value);
    });
});

// 👉 listPlaylist list
const listPlaylist = ref<IListPlaylist[]>();

const handleGeneratorPlaylist = () => {
    const exportPlaylist = new generatorPlaylist();
    const newPlaylistBuilding: IListPlaylist[] = [];

    if (_.isEmpty(selectedBuilding.value)) {
        showSnackbar("You haven't selected a building yet !", 'error');
        return;
    }

    if (_.isEmpty(selectedListVideo.value)) {
        showSnackbar("You haven't selected a list video yet !", 'error');
        return;
    }

    const genPlaylist = (list: IVideos[], landLordAds: IVideos[]) => {
        const playlist: IPlaylist[] = [];
        let newList = [...list];
        if (!_.isEmpty(landLordAds)) {
            newList = exportPlaylist.addLandLordAds(newList, landLordAds);
        }

        newList.forEach((l, index) => {
            if (l) {
                playlist.push({
                    category: l.category,
                    duration: l.durations,
                    key: l.key,
                    remarks: '1',
                    title: l.name,
                    order: index,
                });
            }
        });

        return playlist;
    };

    buildings.forEach((building, index) => {
        if (selectedBuilding.value.includes(building.value)) {
            const playlist = exportPlaylist.createListVideo(
                listVideos.find((x) => x.value == selectedListVideo.value)?.videos as IVideos[]
            );

            newPlaylistBuilding.push({
                id: index + 1,
                buildingName: 'building ' + building.title,
                playlist: genPlaylist(playlist, building.landlordAds),
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

const handleSaveOnePlaylist = (playlist: IPlaylist[]) => {
    console.log(playlist);
    const exportPlaylist = new generatorPlaylist();

    const listVideoCurrent = listVideos.find((x) => x.value == selectedListVideo.value)?.videos;

    const isCheckCategoriesCloselyTogether =
        exportPlaylist.checkCategoriesCloselyTogether(playlist);

    const listVideo: IVideos[] = playlist.map((x) => ({
        name: x.title,
        restriction: listVideoCurrent?.find((l) => l.key == x.key)?.restriction || '',
        key: x.key,
        loop: 1,
    }));

    const isCheckNoBackToBack = exportPlaylist.checkNoBackToBack(listVideo);
    if (isCheckCategoriesCloselyTogether) {
        showSnackbar('Category conflicts in playlists, please double-check?', 'error');
        return;
    }

    if (isCheckNoBackToBack.status) {
        showSnackbar('Back to back conflicts in playlists, please double-check?', 'error');
        return;
    }
};
</script>

<template>
    <section>
        <VCard title="Filters" class="mb-6">
            <VCardText>
                <VRow>
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
                            :items="buildings"
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
            <VBtn
                color="primary"
                class="position-absolute"
                @click="handleSaveOnePlaylist(building.playlist)"
            >
                Save
            </VBtn>
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
                    :list="building.playlist"
                    v-bind="dragOptions"
                    @start="isDragging = true"
                    @end="isDragging = false"
                >
                    <transition-group type="transition" name="flip-list">
                        <tr v-for="(paylist, index) in building.playlist" :key="paylist.order">
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
                                {{ paylist.key }}
                            </td>

                            <td class="text-capitalize">
                                {{ paylist.category }}
                            </td>

                            <td>
                                {{ paylist.remarks }}
                            </td>
                        </tr>
                    </transition-group>
                </VueDraggableNext>

                <tfoot v-show="!building.playlist.length">
                    <tr>
                        <td colspan="7" class="text-center">No data available</td>
                    </tr>
                </tfoot>
            </VTable>
        </VCard>

        <PopupCreateListVideo
            v-model:isDialogVisible="isDialogListVideoVisible"
            @close="handleCloseDialog"
        />
        <!-- :is-dialog-visible="isDialogListVideoVisible" -->

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
.m-0 {
  margin: 0 !important;
}

.user-pagination-select {
  .v-field__input,
  .v-field__append-inner {
    padding-block-start: 0.3rem;
  }
}

.position-absolute {
  inset-block-start: 20px;
  inset-inline-end: 20px;
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

.list-group {
  min-block-size: 20px;
}

.list-group-item {
  cursor: move;
}

.list-group-item i {
  cursor: pointer;
}

// table

.th--table {
  color: rgba(var(--v-theme-on-background), var(--v-high-emphasis-opacity));
  font-size: 12px;
  font-weight: bold;
}

.tr--table {
  border-block-start: thin solid rgba(var(--v-border-color), var(--v-border-opacity));
}

.td--table {
  color: rgba(var(--v-theme-on-background), var(--v-high-emphasis-opacity));
  font-size: 14px;
}
</style>
