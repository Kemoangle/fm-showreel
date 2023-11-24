<script setup lang="ts">
import { VueDraggableNext } from 'vue-draggable-next';
import { generatorPlaylist } from '@/utils/generatorPlaylist';
import { IVideos, IListInfoCompany, IListPlaylist, IPlaylist } from '@/model/generatorPlaylist';
const selectedCompany = ref();

// 👉 companies filters
const companies = [
    { title: 'company 1', value: 'admin' },
    { title: 'company 2', value: 'author' },
    { title: 'company 3', value: 'editor' },
    { title: 'company 4', value: 'maintainer' },
    { title: 'company 5', value: 'subscriber' },
];

const listInfoCompany = [
    {
        companyName: 'company 1',
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
                restriction: '',
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
    const exportPlaylist = new generatorPlaylist();
    console.log(exportPlaylist.createListVideo(listInfoCompany[0].listVideos as IVideos[]));
});

// 👉 listPlaylist list
const listPlaylist = ref<IListPlaylist[]>();

const list = [
    { name: 'John', id: 1 },
    { name: 'Joao', id: 2 },
    { name: 'Jean', id: 3 },
    { name: 'Gerard', id: 4 },
];

// const log = (event: Event) => {
//     console.log(event);
// };

const handleGeneratorPlaylist = () => {
    const exportPlaylist = new generatorPlaylist();
    const newPlaylistCompany = [];

    const genPlaylist = (list: IVideos[]) => {
        const playlist: IPlaylist[] = [];

        list.forEach((l) => {
            playlist.push({
                category: l.category,
                duration: l.durations,
                keyNo: '1',
                remasks: '1',
                title: l.name,
            });
        });

        return playlist;
    };

    for (let i = 0; i < 10; i++) {
        const playlist = exportPlaylist.createListVideo(listInfoCompany[0].listVideos as IVideos[]);

        newPlaylistCompany.push({
            companyName: 'company ' + (i + 1),
            playlist: genPlaylist(playlist),
        });
    }
    listPlaylist.value = newPlaylistCompany;
};
</script>

<template>
    <section>
        <VCard title="Filters" class="mb-6">
            <VCardText>
                <VRow>
                    <VCol cols="12" sm="4">
                        <VBtn variant="tonal" color="secondary" prepend-icon="mdi-tray-arrow-up">
                            Import Excel Companies
                        </VBtn>
                    </VCol>
                    <VCol cols="12" sm="4">
                        <VSelect
                            v-model="selectedCompany"
                            label="Select Company"
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
            </VCardText>
        </VCard>

        <!-- <VueDraggableNext class="dragArea list-group w-full" :list="list" @change="log">
            <div
                class="list-group-item bg-gray-300 m-1 p-3 rounded-md text-center"
                v-for="element in list"
                :key="element.name"
            >
                {{ element.name }}
            </div>
        </VueDraggableNext> -->
        <!-- <VueDraggableNext v-model="list">
            <transition-group>
                <div v-for="element in list" :key="element.id">{{ element.name }}</div>
            </transition-group>
        </VueDraggableNext> -->

        <VCard
            :title="`Playlist ${company.companyName}`"
            v-for="company in listPlaylist"
            :key="company.companyName"
            class="mb-6 position-relative"
        >
            <VBtn color="primary" class="position-absolute"> Save </VBtn>
            <VTable class="text-no-wrap">
                <thead>
                    <tr>
                        <th scope="col">TITLE</th>
                        <th scope="col">DURATION</th>
                        <th scope="col">KEY NO</th>
                        <th scope="col">CATEGORY</th>
                        <th scope="col">REMASK</th>
                    </tr>
                </thead>

                <tbody>
                    <tr v-for="paylist in company.playlist" :key="paylist.keyNo">
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
                    <!-- <VueDraggableNext>
                        <transition-group>
                            <tr v-for="paylist in company.playlist" :key="paylist.keyNo">
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
                        </transition-group>
                    </VueDraggableNext> -->
                </tbody>

                <!-- 👉 table footer  -->
                <tfoot v-show="!company.playlist.length">
                    <tr>
                        <td colspan="7" class="text-center">No data available</td>
                    </tr>
                </tfoot>
            </VTable>
        </VCard>
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
