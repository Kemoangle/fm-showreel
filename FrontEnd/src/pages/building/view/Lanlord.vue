<script lang="ts" setup>
import { LandlordAds } from '@/model/landlordAds';

interface Props {
    landlordAdsData: LandlordAds[];
}

const props = defineProps<Props>();
</script>

<template>
    <section>
        <VCard>
            <VCardItem>
                <template #prepend>
                    <VIcon icon="mdi-chart-timeline-variant" color="success" />
                </template>
                <VBtn variant="tonal" color="secondary" prepend-icon="mdi-tray-arrow-down" style="float: inline-end;">
                    Add Video
                </VBtn>
                <VCardTitle>LandlordAds</VCardTitle>
            </VCardItem>

            <VDivider />

            <VTable class="text-no-wrap">
                <!-- 👉 table head -->
                <thead>
                    <tr>
                        <th scope="col">#</th>
                        <th scope="col">Video Title</th>
                        <th scope="col">Duration</th>
                        <th scope="col">Key No</th>
                        <th scope="col">Loop</th>
                        <th scope="col">Start Date</th>
                        <th scope="col">End Date</th>
                        <th scope="col">Action</th>
                    </tr>
                </thead>

                <!-- 👉 table body -->
                <tbody>
                    <tr v-for="(item, index) in landlordAdsData" :key="item.id">
                        <!-- 👉 User -->
                        <td>{{ index + 1 }}</td>
                        <td>
                            <div class="d-flex align-center">
                                {{ item.video?.title }}
                            </div>
                        </td>

                        <!-- 👉 Email -->
                        <td class="">
                            {{ item.video?.duration }}
                        </td>

                        <td class="">
                            {{ item.video?.keyNo }}
                        </td>

                        <td class="">
                            {{ item.loop }}
                        </td>

                        <td class="">
                            {{ item.startDate }}
                        </td>

                        <td class="">
                            {{ item.endDate }}
                        </td>
                        <td>
                            <VBtn size="x-small" color="default" variant="plain" icon>
                                <VIcon size="24" icon="mdi-dots-vertical" />

                                <VMenu activator="parent">
                                    <VList>
                                        <VListItem href="javascript:void(0)">
                                            <template #prepend>
                                                <VIcon
                                                    icon="mdi-pencil-outline"
                                                    :size="20"
                                                    class="me-3"
                                                />
                                            </template>
                                            <VListItemTitle>Edit</VListItemTitle>
                                        </VListItem>

                                        <VListItem href="javascript:void(0)">
                                            <template #prepend>
                                                <VIcon
                                                    icon="mdi-delete-outline"
                                                    :size="20"
                                                    class="me-3"
                                                />
                                            </template>

                                            <VListItemTitle>Delete</VListItemTitle>
                                        </VListItem>
                                    </VList>
                                </VMenu>
                            </VBtn>
                        </td>
                    </tr>
                </tbody>

                <!-- 👉 table footer  -->
                <tfoot v-show="!landlordAdsData">
                    <tr>
                        <td colspan="7" class="text-center">No data available</td>
                    </tr>
                </tfoot>
            </VTable>

            <VDivider />
        </VCard>
    </section>
</template>
