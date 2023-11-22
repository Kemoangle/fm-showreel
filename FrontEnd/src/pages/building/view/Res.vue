<script lang="ts" setup>
import { Restriction } from '@/model/restriction';

interface Props {
    restrictionsData: Restriction[] | undefined;
}

const props = defineProps<Props>();
</script>

<template>
    <section>
        <VCard>
            <VCardItem>
                <template #prepend>
                    <VIcon icon="mdi-chart-timeline-variant" color="success"/>
                </template>
                <VBtn class="float-right" icon="small">
                    <VIcon icon="mdi-add-box" :size="40"/>
                </VBtn>
                <VCardTitle>Restriction</VCardTitle>
                
            </VCardItem>

            <VDivider />

            <VTable class="text-no-wrap">
                <!-- 👉 table head -->
                <thead>
                    <tr>
                        <th scope="col">#</th>
                        <th scope="col">RESTRICTION NAME</th>
                        <th scope="col">EXCEPT</th>
                        <th scope="col">ACTION</th>
                    </tr>
                </thead>

                <!-- 👉 table body -->
                <tbody>
                    <tr v-for="(res, index) in restrictionsData" :key="res.id">
                        <!-- 👉 User -->
                        <td>{{ index + 1 }}</td>
                        <td>
                            <div class="d-flex align-center">
                                {{ res.restrictionName }}
                            </div>
                        </td>

                        <!-- 👉 Email -->
                        <td class="">
                            {{ res.restrictionName }}
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
                <tfoot v-show="!restrictionsData">
                    <tr>
                        <td colspan="7" class="text-center">No data available</td>
                    </tr>
                </tfoot>
            </VTable>

            <VDivider />
        </VCard>
    </section>
</template>

<style lang="scss" scoped>
.card-list {
  --v-card-list-gap: 14px;
}
</style>
