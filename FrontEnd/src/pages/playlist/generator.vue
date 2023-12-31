<script setup lang="ts">
import { useSnackbar } from '@/components/Snackbar.vue';
import ViewListVideo from '@/components/ViewListVideo.vue';
import PopupChangeNamePlaylist from '@/components/PopupChangeNamePlaylist.vue';
import {
    Building,
    IBuildingLandlord,
    IBuildingLandlordGroup,
    IBuildingRestriction,
    IBuildingRestrictionGroup,
    IListBuildingSelect,
} from '@/model/building';
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

interface IListPlaylistSelect extends IPostPlaylistStore {
    value: number;
    title: string;
}

const { showSnackbar } = useSnackbar();

const useStoreVideo = useVideoListStore();
const useBuilding = useBuildingStore();
const usePlaylist = usePlaylistStore();

const selectedBuilding = ref<number[]>([]);
const selectedPlaylist = ref<number>();
const selectedListVideo = ref<number>();

const isDialogListVideoVisible = ref(false);
const isDialogListVideoCurrent = ref(false);
const isViewPlaylistGeneric = ref(false);
const isLoading = ref(false);
const playlistGeneric = ref<IPlaylist[]>([]);

const isDialogChangeNamePlaylist = ref(false);
const playlistActive = ref<IListPlaylist>();

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
const listPlaylistGeneric = ref<IPostPlaylistStore[]>([]);

const listVideos = ref<IListVideoSelect[]>([]);

const namePlaylist = ref<string[]>();
const namePlaylistGeneric = ref<string>('playlist_generic_' + getTimestamp());

// --------------- watch --------------

watch(selectedListVideo, async (value, oldValue) => {
    if (value != oldValue) {
        isLoading.value = true;
        listVideoActive.value = [];
        playlistGeneric.value = [];
        isViewPlaylistGeneric.value = false;
        if (selectedListVideo.value) {
            const data: unknown = await useStoreVideo.getVideoByListId(selectedListVideo.value);
            if (data as IVideoInList[]) {
                listVideoActive.value = data as IVideoInList[];
                isLoading.value = false;
            } else {
                isLoading.value = false;
            }
        } else {
            isLoading.value = false;
        }
    }
});

watch(selectedPlaylist, (value, oldValue) => {
    if (value) {
        selectedListVideo.value = undefined;
        listPlaylist.value = undefined;
    }
});

watch(selectedListVideo, (value, oldValue) => {
    if (value) {
        selectedPlaylist.value = undefined;
        listPlaylist.value = undefined;
    }
});
// --------------------------------------

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

const convertValueListPlaylistGenericSelect = (data: IPostPlaylistStore[]) => {
    return data.map((x) => ({
        ...x,
        title: x.title,
        value: x.id,
    })) as IListPlaylistSelect[];
};

onMounted(async () => {
    // check all buildings
    listPlaylistGeneric.value = convertValueListPlaylistGenericSelect(
        await usePlaylist.getPlaylists('', 1, 50)
    );
    listVideos.value = convertValueListVideoSelect(await useStoreVideo.getListVideoStore());
    buildings.value = convertValueListBuildingSelect(await useBuilding.getAllBuildingStore());
    buildings.value.forEach((building, index) => {
        selectedBuilding.value.push(building.value);
    });
});

// 👉 listPlaylist list
const listPlaylist = ref<IListPlaylist[]>();

const handleClickShowListVideo = () => {
    isDialogListVideoCurrent.value = !isDialogListVideoCurrent.value;
};

const checkPlaylistInvalid = (playlist: IPlaylist[], nameBuilding?: string) => {
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
        showSnackbar(
            `Category conflicts in playlists ${nameBuilding}, please double-check?`,
            'error'
        );
        return false;
    }

    if (isCheckNoBackToBack.status) {
        showSnackbar(
            `Back to back conflicts in playlists ${nameBuilding}, please double-check?`,
            'error'
        );
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
        const buildingsId = listPlaylist.value ? listPlaylist.value[indexBuilding].buildingsId : [];
        const data: IPostPlaylistStore = {
            id: 0,
            jsonPlaylist: JSON.stringify(playlist),
            status: 'active',
            title: nameTimestamp,
            creator: 'string',
            parentId: selectedPlaylist.value || 0,
            buildingsId: buildingsId,
        };

        usePlaylist
            .addNewPlaylist([data])
            .then((data) => {
                showSnackbar(`Save playlist ${name} Successfully`, 'success');
                if (listPlaylist.value && listPlaylist.value[indexBuilding]) {
                    listPlaylist.value[indexBuilding].isSave = true;
                }
            })
            .catch((err) => {
                if (err.data.error) {
                    showSnackbar(err.data.error, 'error');
                } else {
                    showSnackbar(`Something went wrong!`, 'error');
                }
            });
    }
};

const handleClickSaveAll = () => {
    if (listPlaylist.value) {
        isLoading.value = true;
        for (let i = 0; i < listPlaylist.value.length; i++) {
            const playlist = listPlaylist.value[i].playlist;
            if (!checkPlaylistInvalid(playlist, listPlaylist.value[i].buildingName.join(', '))) {
                isLoading.value = false;
                return;
            }
        }
        // const getObjPlaylist = (child: IListPlaylist): IPostPlaylistStore[] => {
        //     return !child.isSave
        //         ? child.nameTimestamp.map((x) => ({
        //               id: 0,
        //               jsonPlaylist: JSON.stringify(child.playlist),
        //               status: 'active',
        //               title: x,
        //               creator: 'string',
        //               parentId: selectedPlaylist.value || 0,
        //               buildingsId: child.buildingsId,
        //           }))
        //         : [];
        // };

        const data: IPostPlaylistStore[] = [];
        listPlaylist.value.forEach((child) => {
            data.push({
                id: 0,
                jsonPlaylist: JSON.stringify(child.playlist),
                status: 'active',
                title: child.nameTimestamp,
                creator: 'string',
                parentId: selectedPlaylist.value || 0,
                buildingsId: child.buildingsId,
            });
        });

        usePlaylist
            .addNewPlaylist(data)
            .then((data) => {
                if (data) {
                    showSnackbar(`Save all playlist Successfully`, 'success');
                    if (listPlaylist.value) {
                        for (let i = 0; i < listPlaylist.value.length; i++) {
                            listPlaylist.value[i].isSave = true;
                        }
                    }
                } else {
                    showSnackbar(`Something went wrong!`, 'error');
                }
                isLoading.value = false;
            })
            .catch((err) => {
                isLoading.value = false;
                if (err.data.error) {
                    showSnackbar(err.data.error, 'error');
                } else {
                    showSnackbar(`Something went wrong!`, 'error');
                }
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
                ...l,
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

const getListVideoDoNoPlay = (listVideoActive: IVideoInList[], id: number) => {
    return listVideoActive.filter((x) => !x.doNotPlay?.some((y) => y.id == id));
};

const handleGeneratorPlaylistBuildings = (
    playlist: IPlaylist[],
    listVideoGeneric?: IVideoInList[]
) => {
    if (checkPlaylistInvalid(playlist)) {
        isLoading.value = true;
        const exportPlaylist = new generatorPlaylist();
        const newPlaylistBuilding: IListPlaylist[] = [];
        const buildingActive = buildings.value.filter((b) =>
            selectedBuilding.value.includes(b.value)
        );
        const namePlaylistGeneric = listPlaylistGeneric.value.find(
            (x) => x.id == selectedPlaylist.value
        )?.title;

        useBuilding.setListBuildingActive(
            buildingActive.map((x) => x.id),
            function (
                LandlordAds: IBuildingLandlordGroup[],
                dataRestrictions: IBuildingRestrictionGroup[]
            ) {
                isViewPlaylistGeneric.value = false;

                const genPlaylist = (
                    landLordAds: IVideos[],
                    restriction: Restriction[] | undefined,
                    building: Building,
                    playlistGeneric: any[]
                ) => {
                    // check do not play building

                    const newListVideoActive = listVideoGeneric
                        ? getListVideoDoNoPlay(listVideoGeneric, building.id)
                        : getListVideoDoNoPlay(listVideoActive.value, building.id);

                    let listVideoBuilding = listVideoGeneric
                        ? [
                              ...playlistGeneric.filter(
                                  (x) => !x?.doNotPlay?.some((d: any) => d.id == building.id)
                              ),
                          ]
                        : [
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

                LandlordAds.forEach((group, index) => {
                    const pl = genPlaylist(
                        group?.videos,
                        dataRestrictions.find((x) =>
                            group.buildingId.some((b) => x.buildingId.some((v) => v == b))
                        )?.restriction,
                        buildingActive.find((x) => group.buildingId.some((v) => v == x.id)) ||
                            buildingActive[0],
                        playlist
                    );

                    const nameBuilding = buildingActive
                        .filter((x) => group.buildingId.some((v) => v === x.id))
                        .map((x) => x.buildingName || '');

                    newPlaylistBuilding.push({
                        id: index + 1,
                        listBuilding: group.buildingId,
                        buildingName: nameBuilding,
                        nameTimestamp: nameBuilding.join('_') + '-' + namePlaylistGeneric,
                        playlist: pl,
                        isSave: false,
                        buildingsId: group.buildingId,
                    });
                });

                listPlaylist.value = newPlaylistBuilding;
                isLoading.value = false;
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
            JsonListVideo: JSON.stringify(listVideoActive.value),
            status: 'active',
            title: namePlaylistGeneric.value,
            creator: 'string',
            parentId: 0,
        };
        usePlaylist
            .addNewPlaylist([data])
            .then((data) => {
                if (data) {
                    showSnackbar(`Save ${namePlaylistGeneric.value} Successfully`, 'success');
                } else {
                    showSnackbar(`Something went wrong!`, 'error');
                }
            })
            .catch((err) => {
                if (err.data.error) {
                    showSnackbar(err.data.error, 'error');
                } else {
                    showSnackbar(`Something went wrong!`, 'error');
                }
            });
    }
};

const handleClickGenPlaylistBuilding = () => {
    const value = usePlaylist.data.filter(
        (x: IPostPlaylistStore) => x.id == selectedPlaylist.value
    );
    if (value) {
        const playlist = JSON.parse(value[0].jsonPlaylist);
        const listVideo = JSON.parse(value[0].jsonListVideo);
        handleGeneratorPlaylistBuildings(playlist, listVideo);
    }
};

const handleClickEditNameGroupPlaylist = (playlistId: number) => {
    isDialogChangeNamePlaylist.value = true;
    const plActive = listPlaylist.value?.find((x) => x.id == playlistId);
    if (plActive) {
        playlistActive.value = plActive;
    }
};
</script>

<template>
    <section>
        <PopupChangeNamePlaylist
            :playlist="playlistActive"
            v-model:is-dialog-visible="isDialogChangeNamePlaylist"
        />
        <VCard title="Filters" class="mb-6">
            <VProgressCircular
                color="primary"
                class="position-absolute"
                indeterminate
                v-show="isLoading"
            ></VProgressCircular>
            <VCardText>
                <VRow>
                    <VCol cols="12" sm="4">
                        <VAutocomplete
                            v-model="selectedListVideo"
                            label="Select List Video"
                            :items="listVideos"
                            :loading="_.isEmpty(listVideos)"
                            clearable
                            clear-icon="mdi-close"
                            :menu-props="{ maxHeight: 500 }"
                        />
                    </VCol>

                    <VCol cols="12" sm="4">
                        <VAutocomplete
                            v-model="selectedBuilding"
                            label="Select Buildings"
                            :items="buildings"
                            clearable
                            clear-icon="mdi-close"
                            multiple
                            :loading="_.isEmpty(buildings)"
                            :menu-props="{ maxHeight: 500 }"
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
                        </VAutocomplete>
                    </VCol>
                    <VCol cols="12" sm="4">
                        <VAutocomplete
                            v-model="selectedPlaylist"
                            label="Select Playlist"
                            :items="listPlaylistGeneric"
                            clearable
                            clear-icon="mdi-close"
                            :loading="_.isEmpty(listPlaylistGeneric)"
                            :menu-props="{ maxHeight: 500 }"
                        />
                    </VCol>
                </VRow>
                <VRow>
                    <div class="d-flex mt-5 pl-3 gap-3">
                        <div v-if="_.isEmpty(playlistGeneric)">
                            <VBtn
                                color="primary"
                                @click="handleClickGenPlaylistBuilding"
                                v-if="!!selectedPlaylist"
                            >
                                Generator Playlist Buildings
                            </VBtn>

                            <VBtn
                                color="primary"
                                v-else
                                @click="handleCreatePlaylistGeneric"
                                :disabled="_.isEmpty(listVideoActive)"
                            >
                                Generator Playlist Generic
                            </VBtn>
                        </div>
                        <div v-else>
                            <VBtn color="primary" @click="handleViewPlaylistGeneric">
                                {{
                                    isViewPlaylistGeneric
                                        ? 'View All Playlist'
                                        : 'View Playlist Generic'
                                }}
                            </VBtn>
                        </div>
                        <VBtn
                            color="primary"
                            @click="handleClickShowListVideo"
                            v-if="!_.isEmpty(listVideoActive) && selectedListVideo"
                        >
                            View List Video
                        </VBtn>
                        <VBtn
                            color="primary"
                            prepend-icon="mdi-tray-arrow-down"
                            @click="handleClickSaveAll"
                            v-if="listPlaylist"
                        >
                            Save All
                        </VBtn>
                    </div>
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
                <!-- <VBtn color="primary" @click="handleGeneratorPlaylistBuildings(playlistGeneric)">
                    Generator PlayList Buildings
                </VBtn> -->
                <VBtn
                    color="primary"
                    @click="savePlaylistGeneric(playlistGeneric)"
                    icon="mdi-content-save-all"
                    variant="text"
                >
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
            :title="`Playlist ${pl.buildingName.join(', ')}`"
            v-for="(pl, indexBuilding) in listPlaylist"
            :key="indexBuilding"
            class="mb-6 position-relative"
            v-if="!isViewPlaylistGeneric"
        >
            <div class="position-absolute">
                <!-- v-if="pl.listBuilding.length == 1" -->
                <VTextField
                    class="input-name-building"
                    v-model="pl.nameTimestamp"
                    :disabled="pl.isSave"
                />
                <!-- <VBtn v-else @click="handleClickEditNameGroupPlaylist(pl.id)">Edit Name</VBtn> -->
                <VBtn
                    color="primary"
                    size="40"
                    v-if="!pl.isSave"
                    @click="
                        handleSaveOnePlaylist(
                            pl.playlist,
                            pl.buildingName.join(', '),
                            pl.nameTimestamp,
                            indexBuilding
                        )
                    "
                    icon="mdi-content-save-all"
                    variant="text"
                >
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
                    :handle="pl.isSave ? 'don\'t-drag' : '.handle'"
                    :list="pl.playlist"
                    v-bind="dragOptions"
                    @start="isDragging = true"
                    @end="isDragging = false"
                >
                    <transition-group type="transition" name="flip-list">
                        <tr
                            class="handle"
                            v-for="(playlist, index) in pl.playlist"
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
                                {{ pl.playlist.reduce((a, b) => a + (b?.durations || 0), 0) }}
                            </td>
                            <td></td>
                            <td class="text-capitalize"></td>
                            <td></td>
                        </tr>
                    </transition-group>
                </VueDraggableNext>

                <tfoot v-show="!pl.playlist.length">
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
