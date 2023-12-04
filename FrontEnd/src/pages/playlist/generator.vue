<script setup lang="ts">
import PopupCreateListVideo from '@/components/PopupCreateListVideo.vue';
import PopupViewListVideo from '@/components/PopupViewListVideo.vue';
import { useSnackbar } from '@/components/Snackbar.vue';
import { IListPlaylist, IPlaylist, IVideos } from '@/model/generatorPlaylist';
import { useVideoListStore } from '@/store/useVideoListStore';
import { listVideo1, listVideo2, listVideo3, listVideo4 } from '@/utils/constant';
import { generatorPlaylist } from '@/utils/generatorPlaylist';
import _ from 'lodash';
import { VueDraggableNext } from 'vue-draggable-next';

const { showSnackbar } = useSnackbar();

const useStoreVideo = useVideoListStore();

const selectedBuilding = ref<string[]>([]);
const selectedListVideo = ref();

const isDialogListVideoVisible = ref(false);
const isDialogListVideoCurrent = ref(false);
const isViewPlaylistGeneric = ref(false);
const playlistGeneric = ref<IPlaylist[]>([]);

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
        restriction: [
            {
                category: 'Movie',
                except: ['Mac'],
                exclude: ['Mo'],
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
        restriction: [
            {
                category: 'Movie',
                except: [],
                exclude: [],
            },
        ],
    },
    {
        title: 'UOL United Sq Off Twr_(RN)',
        value: 'UOLUnitedSqOffTwr',
        landlordAds: [
            {
                key: 'sUOL029',
                name: 'UOL - UPOPP',
                durations: 15,
                loop: 4,
                category: 'UOL',
                restriction: '',
            },
            {
                key: 'sUOL032',
                name: 'sUOL032 UOL - Pinetree Hill 15s',
                durations: 15,
                loop: 2,
                category: 'UOL',
                restriction: '',
            },
        ],
    },
];

const listVideos = [
    { title: 'listvideo 1', value: 'admin', videos: listVideo1 },
    { title: 'listvideo 2', value: 'author', videos: listVideo2 },
    { title: 'listvideo 3', value: 'editor', videos: listVideo3 },
    { title: 'listvideo 4', value: 'video4', videos: listVideo4 },
];

onMounted(() => {
    // check all buildings
    buildings.forEach((building) => {
        selectedBuilding.value.push(building.value);
    });
    console.log(useStoreVideo.data);
    
});

// 👉 listPlaylist list
const listPlaylist = ref<IListPlaylist[]>();

const handleClickShowListVideo = () => {
    isDialogListVideoCurrent.value = !isDialogListVideoCurrent.value;
};

const handleCloseDialog = () => {
    isDialogListVideoVisible.value = !isDialogListVideoVisible.value;
};

const handleCloseDialogShowListVideo = () => {
    isDialogListVideoCurrent.value = !isDialogListVideoCurrent.value;
};

const checkPlaylistInvalid = (playlist: IPlaylist[]) => {
    const exportPlaylist = new generatorPlaylist();

    const listVideoCurrent = listVideos.find((x) => x.value == selectedListVideo.value)?.videos;

    const isCheckCategoriesCloselyTogether =
        exportPlaylist.checkCategoriesCloselyTogether(playlist);

    const listVideo: IVideos[] = playlist.map((x) => ({
        name: x.name,
        restriction: listVideoCurrent?.find((l) => l.key == x.key)?.restriction || '',
        key: x.key,
        loop: 1,
    }));

    const isCheckNoBackToBack = exportPlaylist.checkNoBackToBack(listVideo);
    if (isCheckCategoriesCloselyTogether) {
        showSnackbar('Category conflicts in playlists, please double-check?', 'error');
        return false;
    }

    if (isCheckNoBackToBack.status) {
        showSnackbar('Back to back conflicts in playlists, please double-check?', 'error');
        return false;
    }
    return true;
};

const handleSaveOnePlaylist = (playlist: IPlaylist[]) => {
    checkPlaylistInvalid(playlist);
};

const handleCreatePlaylistGeneric = () => {
    if (_.isEmpty(selectedListVideo.value)) {
        showSnackbar("You haven't selected a list video yet !", 'error');
        return;
    }

    isViewPlaylistGeneric.value = true;
    playlistGeneric.value = [];

    const exportPlaylist = new generatorPlaylist();
    const playlist = exportPlaylist.createListVideo(
        listVideos.find((x) => x.value == selectedListVideo.value)?.videos as IVideos[]
    );

    playlist.forEach((l, index) => {
        if (l) {
            playlistGeneric.value.push({
                category: l.category,
                durations: l.durations,
                key: l.key,
                remarks: '1',
                name: l.name,
                order: index,
            });
        }
    });
};

const convertPlaylistToListVideo = (playlist: IPlaylist[]) => {
    const newListVideo: IVideos[] = [];

    playlist.forEach((element: unknown) => {
        newListVideo.push(element as IVideos);
    });
    return newListVideo;
};

const handleGeneratorPlaylistBuildings = (playlist: IPlaylist[]) => {
    if (checkPlaylistInvalid(playlist)) {
        const exportPlaylist = new generatorPlaylist();
        const newPlaylistBuilding: IListPlaylist[] = [];

        isViewPlaylistGeneric.value = false;

        const genPlaylist = (listVideo: IVideos[], landLordAds: IVideos[]) => {
            const playlist: IPlaylist[] = [];
            let newList = [...listVideo];
            if (!_.isEmpty(landLordAds)) {
                newList = exportPlaylist.addLandLordAds(newList, landLordAds);
            }

            newList.forEach((l, index) => {
                if (l) {
                    playlist.push({
                        category: l.category,
                        durations: l.durations,
                        key: l.key,
                        remarks: '1',
                        name: l.name,
                        order: index,
                    });
                }
            });

            return playlist;
        };

        buildings.forEach((building, index) => {
            if (selectedBuilding.value.includes(building.value)) {
                newPlaylistBuilding.push({
                    id: index + 1,
                    buildingName: 'building ' + building.title,
                    playlist: genPlaylist(
                        convertPlaylistToListVideo(playlistGeneric.value),
                        building.landlordAds
                    ),
                });
            }
        });

        listPlaylist.value = newPlaylistBuilding;
    }
};

const handleViewPlaylistGeneric = () => {
    isViewPlaylistGeneric.value = !isViewPlaylistGeneric.value;
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
                        >
                            <template v-slot:selection="{ item, index }">
                                <span>{{ item.title }}</span>
                                <span
                                    v-if="index === 1"
                                    class="text-grey text-caption align-self-center"
                                >
                                    (+{{ selectedBuilding.length - 1 }} others)
                                </span>
                            </template>
                        </VSelect>
                    </VCol>
                </VRow>
                <VRow>
                    <VCol cols="12" sm="3" v-if="_.isEmpty(playlistGeneric)">
                        <VBtn
                            variant="tonal"
                            color="secondary"
                            @click="handleCreatePlaylistGeneric"
                        >
                            Generator Playlist Generic
                        </VBtn>
                    </VCol>
                    <VCol cols="12" sm="3" v-else>
                        <VBtn variant="tonal" color="secondary" @click="handleViewPlaylistGeneric">
                            {{
                                isViewPlaylistGeneric
                                    ? 'View All Playlist'
                                    : 'View Playlist Generic'
                            }}
                        </VBtn>
                    </VCol>
                    <VCol
                        cols="12"
                        sm="3"
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
            title="Playlist Generic"
            class="mb-6 position-relative"
            v-if="!_.isEmpty(playlistGeneric) && isViewPlaylistGeneric"
        >
            <VBtn
                color="primary"
                class="position-absolute"
                @click="handleGeneratorPlaylistBuildings(playlistGeneric)"
            >
                Generator PlayList Buildings
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
                    handle=".handle"
                    :list="playlistGeneric"
                    v-bind="dragOptions"
                    @start="isDragging = true"
                    @end="isDragging = false"
                >
                    <transition-group type="transition" name="flip-list">
                        <tr
                            class="handle"
                            v-for="(paylist, index) in playlistGeneric"
                            :key="paylist.order"
                        >
                            <td>
                                {{ index + 1 }}
                            </td>
                            <td>
                                {{ paylist.name }}
                            </td>

                            <td class="text-medium-emphasis">
                                {{ paylist.durations }}
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
                        <tr key="j">
                            <td></td>
                            <td></td>
                            <td class="text-medium-emphasis">
                                {{ playlistGeneric.reduce((a, b) => a + (b?.durations || 0), 0) }}
                            </td>
                            <td></td>
                            <td class="text-capitalize"></td>
                            <td></td>
                        </tr>
                    </transition-group>
                </VueDraggableNext>

                <tfoot v-show="!playlistGeneric.length">
                    <tr>
                        <td colspan="7" class="text-center">No data available</td>
                    </tr>
                </tfoot>
            </VTable>
        </VCard>

        <VCard
            :title="`Playlist ${building.buildingName}`"
            v-for="building in listPlaylist"
            :key="building.buildingName"
            class="mb-6 position-relative"
            v-if="!isViewPlaylistGeneric"
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
                    handle=".handle"
                    :list="building.playlist"
                    v-bind="dragOptions"
                    @start="isDragging = true"
                    @end="isDragging = false"
                >
                    <transition-group type="transition" name="flip-list">
                        <tr
                            class="handle"
                            v-for="(paylist, index) in building.playlist"
                            :key="paylist.order"
                        >
                            <td>
                                {{ index + 1 }}
                            </td>
                            <td>
                                {{ paylist.name }}
                            </td>
                            <td class="text-medium-emphasis">
                                {{ paylist.durations }}
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
                        <tr key="j">
                            <td></td>
                            <td></td>
                            <td class="text-medium-emphasis">
                                {{ building.playlist.reduce((a, b) => a + (b?.durations || 0), 0) }}
                            </td>
                            <td></td>
                            <td class="text-capitalize"></td>
                            <td></td>
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
