<script setup lang="ts">
import { useSnackbar } from '@/components/Snackbar.vue';
import ViewListVideo from '@/components/ViewListVideo.vue';
import { Building, IBuildingLandlord, IBuildingRestriction } from '@/model/building';
import { IListPlaylist, IPlaylist, IVideos } from '@/model/generatorPlaylist';
import { IPostPlaylistStore } from '@/model/playlist';
import { Restriction } from '@/model/restriction';
import { Video } from '@/model/video';
import { IVideoInList, VideoList } from '@/model/videoList';
import { useBuildingStore } from '@/store/useBuildingStore';
import { usePlaylistStore } from '@/store/usePlayListStore';
import { useVideoListStore } from '@/store/useVideoListStore';
import { getTimestamp } from '@/utils/functions';
import { generatorPlaylist } from '@/utils/generatorPlaylist';
import _ from 'lodash';
import { VueDraggableNext } from 'vue-draggable-next';

interface IListVideoSelect extends VideoList {
    value: number;
}

interface IListBuildingSelect extends Building {
    value: number;
    title: string;
    landlordAds: Video;
}

const { showSnackbar } = useSnackbar();

const useStoreVideo = useVideoListStore();
const useBuilding = useBuildingStore();
const usePlaylist = usePlaylistStore();

const selectedBuilding = ref<number[]>([]);
const selectedListVideo = ref<number>();

const isDialogListVideoVisible = ref(false);
const isDialogListVideoCurrent = ref(false);
const isViewPlaylistGeneric = ref(false);
const playlistGeneric = ref<IPlaylist[]>([]);

const listVideoActive = ref<IVideoInList[]>([]);
const listVideoPlaylist = ref<IVideos[]>([]);

const dragOptions = () => {
    return {
        animation: 0,
        group: 'description',
        disabled: false,
        ghostClass: 'ghost',
    };
};

const isDragging = ref(false);

const buildings = ref<IListBuildingSelect[]>([]);

const listVideos = ref<IListVideoSelect[]>([]);

const namePlaylist = ref<string[]>();
const namePlaylistGeneric = ref<string>('playlist_generic_' + getTimestamp());

// --------------- watch --------------

watch(selectedListVideo, async (value, oldValue) => {
    if (value != oldValue) {
        listVideoActive.value = [];
        playlistGeneric.value = [];
        isViewPlaylistGeneric.value = false;
        if (selectedListVideo.value) {
            const data: unknown = await useStoreVideo.getVideoByListId(selectedListVideo.value);
            if (data as IVideoInList[]) {
                listVideoActive.value = data as IVideoInList[];
            }
        }
    }
});

const convertValueListVideoSelect = (data: VideoList[]) => {
    return data.map((x) => ({ ...x, value: x.id })) as IListVideoSelect[];
};

const convertValueListBuildingSelect = (data: Building[]) => {
    return data.map((x) => ({
        ...x,
        title: x.buildingName,
        value: x.id,
    })) as IListBuildingSelect[];
};

onMounted(async () => {
    // check all buildings
    listVideos.value = convertValueListVideoSelect(await useStoreVideo.getListVideoStore());
    buildings.value = convertValueListBuildingSelect(await useBuilding.getAllBuildingStore());
    buildings.value.forEach((building) => {
        selectedBuilding.value.push(building.value);
    });
});

// 👉 listPlaylist list
const listPlaylist = ref<IListPlaylist[]>();

const handleClickShowListVideo = () => {
    isDialogListVideoCurrent.value = !isDialogListVideoCurrent.value;
};

const checkPlaylistInvalid = (playlist: IPlaylist[]) => {
    const exportPlaylist = new generatorPlaylist();

    const isCheckCategoriesCloselyTogether =
        exportPlaylist.checkCategoriesCloselyTogether(playlist);

    const listVideo: IVideos[] = playlist.map((x) => ({
        name: x.name,
        noBackToBack: listVideoActive.value.find((l) => x.key == l.video.keyNo)?.noBackToBack,
        key: x.key,
        loop: 1,
        subCategory: x.category || [],
        category: x.category || [],
        id: 1,
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

const handleSaveOnePlaylist = (
    playlist: IPlaylist[],
    name: string,
    nameTimestamp: string,
    indexBuilding: number
) => {
    if (checkPlaylistInvalid(playlist)) {
        const data: IPostPlaylistStore = {
            id: 0,
            jsonPlaylist: JSON.stringify(playlist),
            status: 'active',
            title: nameTimestamp,
            creator: 'string',
            parentId: 0,
        };
        usePlaylist
            .addNewPlaylist(data)
            .then((data) => {
                showSnackbar(`Save playlist ${name} Successfully`, 'success');
                if (listPlaylist.value && listPlaylist.value[indexBuilding]) {
                    listPlaylist.value[indexBuilding].isSave = true;
                }
            })
            .catch((err) => {
                showSnackbar(`Something went wrong!`, 'error');
            });
    }
};

const convertListVideoRenderPlaylist = (listVideo: any[]) => {
    return listVideo.map((x) => ({
        ...x.video,
        loop: x.loopNum,
        category: x.category,
        noBackToBack: x.noBackToBack,
        subCategory: x.subCategory,
        doNotPlay: x.doNotPlay,
    }));
};

const handleCreatePlaylistGeneric = () => {
    if (!selectedListVideo.value) {
        showSnackbar("You haven't selected a list video yet!", 'error');
        return;
    }

    isViewPlaylistGeneric.value = true;
    playlistGeneric.value = [];

    const exportPlaylist = new generatorPlaylist();
    const playlist = exportPlaylist.createListVideo(
        convertListVideoRenderPlaylist(listVideoActive.value)
    );

    listVideoPlaylist.value = playlist;

    playlist.forEach((l, index) => {
        if (l) {
            playlistGeneric.value.push({
                category: l.category || [],
                durations: l.duration,
                key: l.keyNo || '',
                remarks: '',
                name: l.title || '',
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
        const buildingActive = buildings.value.filter((b) =>
            selectedBuilding.value.includes(b.value)
        );

        useBuilding.setListBuildingActive(
            buildingActive.map((x) => x.id),
            function (LandlordAds: IBuildingLandlord[], dataRestrictions: IBuildingRestriction[]) {
                isViewPlaylistGeneric.value = false;

                const genPlaylist = (
                    listVideo: Video[],
                    landLordAds: IVideos[],
                    restriction: Restriction[] | undefined,
                    building: Building
                ) => {
                    // check do not play building

                    const newListVideoActive = listVideoActive.value.filter(
                        (x) => !x.doNotPlay?.some((y) => y.id == building.id)
                    );

                    let listVideoBuilding = [
                        ...listVideoPlaylist.value.filter(
                            (x) => !x.doNotPlay?.some((d) => d.id == building.id)
                        ),
                    ];

                    // check restriction building
                    if (!_.isEmpty(restriction) && restriction) {
                        listVideoBuilding = exportPlaylist.handleRestrictionBuilding(
                            restriction,
                            convertListVideoRenderPlaylist(newListVideoActive)
                        );
                    }
                    console.log('listVideoBuilding:', listVideoBuilding);

                    const playlist: IPlaylist[] = [];
                    let newList = [...listVideoBuilding];
                    if (!_.isEmpty(landLordAds)) {
                        newList = exportPlaylist.addLandLordAds(listVideoBuilding, landLordAds);
                        newList.forEach((l: IVideos, index: number) => {
                            if (l) {
                                playlist.push({
                                    category: l.category || [],
                                    durations: l.duration,
                                    key: l.keyNo || '',
                                    remarks: '',
                                    name: l.title || '',
                                    order: index,
                                });
                            }
                        });
                    } else {
                        newList.forEach((l: IVideos, index: number) => {
                            if (l) {
                                playlist.push({
                                    category: l.category || [],
                                    durations: l.duration,
                                    key: l.keyNo || '',
                                    remarks: '',
                                    name: l.title || '',
                                    order: index,
                                });
                            }
                        });
                    }

                    return playlist;
                };

                const timestamp = getTimestamp();

                buildingActive.forEach((building, index) => {
                    const playlist = genPlaylist(
                        convertPlaylistToListVideo(playlistGeneric.value),
                        LandlordAds.find((x) => x.buildingId == building.id)?.videos,
                        dataRestrictions.find((x) => x.buildingId == building.id)?.restriction,
                        building
                    );
                    newPlaylistBuilding.push({
                        id: index + 1,
                        buildingName: 'building ' + building.title,
                        nameTimestamp: 'building ' + building.title + ' - ' + timestamp,
                        playlist,
                        isSave: false,
                    });
                });

                listPlaylist.value = newPlaylistBuilding;
            }
        );
    }
};

const handleViewPlaylistGeneric = () => {
    isViewPlaylistGeneric.value = !isViewPlaylistGeneric.value;
};

const savePlaylistGeneric = (playlist: IPlaylist[]) => {
    if (checkPlaylistInvalid(playlist)) {
        const data: IPostPlaylistStore = {
            id: 0,
            jsonPlaylist: JSON.stringify(playlist),
            status: 'active',
            title: namePlaylistGeneric.value,
            creator: 'string',
            parentId: 0,
        };
        usePlaylist
            .addNewPlaylist(data)
            .then((data) => {
                showSnackbar(`Save ${namePlaylistGeneric.value} Successfully`, 'success');
            })
            .catch((err) => {
                showSnackbar(`Something went wrong!`, 'error');
            });
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
                            :loading="_.isEmpty(listVideos)"
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
                            :loading="_.isEmpty(buildings)"
                        >
                            <template v-slot:selection="{ item, index }">
                                <span>{{ index === 0 ? item.title : '' }}</span>
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
                            color="primary"
                            @click="handleCreatePlaylistGeneric"
                            :disabled="!listVideoActive"
                        >
                            Generator Playlist Generic
                        </VBtn>
                    </VCol>
                    <VCol cols="12" sm="3" v-else>
                        <VBtn color="primary" @click="handleViewPlaylistGeneric">
                            {{
                                isViewPlaylistGeneric
                                    ? 'View All Playlist'
                                    : 'View Playlist Generic'
                            }}
                        </VBtn>
                    </VCol>
                    <VCol cols="12" sm="3" v-if="!_.isEmpty(listVideoActive)">
                        <VBtn
                            color="primary"
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
            <div class="position-absolute">
                <VTextField class="input-name-building" v-model="namePlaylistGeneric" />
                <VBtn color="primary" @click="handleGeneratorPlaylistBuildings(playlistGeneric)">
                    Generator PlayList Buildings
                </VBtn>
                <VBtn color="primary" @click="savePlaylistGeneric(playlistGeneric)"> Save </VBtn>
            </div>
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
                            v-for="(playlist, index) in playlistGeneric"
                            :key="playlist.order"
                        >
                            <td>
                                {{ index + 1 }}
                            </td>
                            <td>
                                {{ playlist.name }}
                            </td>
                            <td class="text-medium-emphasis">
                                {{ playlist.durations }}
                            </td>
                            <td>
                                {{ playlist.key }}
                            </td>
                            <td class="text-capitalize">
                                {{ playlist.category?.map((x) => x.name).join(', ') }}
                            </td>
                            <td>
                                <VTextField class="input-remark" :v-model="playlist.remarks" />
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
            v-for="(building, indexBuilding) in listPlaylist"
            :key="building.buildingName"
            class="mb-6 position-relative"
            v-if="!isViewPlaylistGeneric"
        >
            <div class="position-absolute">
                <VTextField
                    class="input-name-building"
                    v-model="building.nameTimestamp"
                    :disabled="building.isSave"
                />
                <VBtn
                    color="primary"
                    v-if="!building.isSave"
                    @click="
                        handleSaveOnePlaylist(
                            building.playlist,
                            building.buildingName,
                            building.nameTimestamp,
                            indexBuilding
                        )
                    "
                >
                    Save
                </VBtn>
            </div>
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
                    :handle="building.isSave ? 'don\'t-drag' : '.handle'"
                    :list="building.playlist"
                    v-bind="dragOptions"
                    @start="isDragging = true"
                    @end="isDragging = false"
                >
                    <transition-group type="transition" name="flip-list">
                        <tr
                            class="handle"
                            v-for="(playlist, index) in building.playlist"
                            :key="playlist.order"
                        >
                            <td>
                                {{ index + 1 }}
                            </td>
                            <td>
                                {{ playlist.name }}
                            </td>
                            <td class="text-medium-emphasis">
                                {{ playlist.durations }}
                            </td>
                            <td>
                                {{ playlist.key }}
                            </td>
                            <td class="text-capitalize">
                                {{ playlist.category?.map((x) => x.name).join(', ') }}
                            </td>
                            <td>
                                <VTextField class="input-remark" v-model="playlist.remarks" />
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

        <ViewListVideo
            v-if="selectedListVideo"
            v-model:is-drawer-open="isDialogListVideoCurrent"
            :video-list-id="selectedListVideo"
            :data-list-video="listVideoActive"
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
    display: flex;
    gap: 10px;
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

.input-remark {
    input {
        block-size: 35px !important;
        min-block-size: unset;
        padding-block: 0;
        padding-inline: 20px;
    }
}

.input-name-building {
    input {
        block-size: 35px !important;
        inline-size: 400px !important;
        min-block-size: unset;
    }
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
