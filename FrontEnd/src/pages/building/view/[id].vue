<template>
    <section>
        <VCard v-if="building">
            <VImg />

            <VCardText class="d-flex align-bottom flex-sm-row flex-column justify-center gap-x-4">
                <div class="user-profile-info w-100 mt-16 pt-6 pt-sm-0 mt-sm-0">
                    <h1 class="text-h3 text-center text-sm-start font-weight-medium mb-2">
                        {{ building.buildingName }}
                    </h1>

                    <div
                        class="d-flex align-center justify-center justify-sm-space-between flex-wrap gap-4"
                    >
                        <div
                            class="d-flex flex-wrap justify-center justify-sm-start flex-grow-1 gap-3"
                        >
                            <span class="d-flex align-center mr-5">
                                <VIcon
                                    size="24"
                                    icon="mdi-invert-colors"
                                    class="me-2"
                                    color="warning"
                                />
                                <span class="text-body-1 font-weight-medium">
                                    {{ building.district }}
                                </span>
                            </span>

                            <span class="d-flex align-center mr-5">
                                <VIcon
                                    size="24"
                                    icon="mdi-map-marker-outline"
                                    class="me-2"
                                    color="success"
                                />
                                <span class="text-body-1 font-weight-medium">
                                    {{ building.address }}
                                </span>
                            </span>

                            <span class="d-flex align-center">
                                <VIcon
                                    size="24"
                                    icon="mdi-calendar-blank"
                                    class="me-2"
                                    color="info"
                                />
                                <span class="text-body-1 font-weight-medium">
                                    {{ building.createTime }}
                                </span>
                            </span>
                        </div>
                    </div>
                </div>
            </VCardText>
        </VCard>
        <VRow>
            <VCol cols="12" class="mt-1">
                <Lanlord v-model:buildingId="$route.params.id" />
            </VCol>
            <VCol cols="6" class="mt-1"> </VCol>

            <VCol cols="12" md="6"> </VCol>

            <VCol cols="12"> </VCol>
        </VRow>
    </section>
</template>

<script setup lang="ts">
import { Building } from '@/model/building';
import { LandlordAds } from '@/model/landlordAds';
import { useBuildingStore } from '../../../store/useBuildingStore';
import Lanlord from './Lanlord.vue';

const building = ref<Building>();
const router = useRoute();
const landlordAdsData = ref<LandlordAds[] | any>([]);

const buildingStore = useBuildingStore();
building.value = buildingStore.getBuildingById(Number(router.params.id));
</script>

<style lang="scss">
.user-profile-avatar {
    border: 5px solid rgb(var(--v-theme-surface));
    background-color: rgb(var(--v-theme-surface)) !important;
    inset-block-start: -3rem;

    .v-img__img {
        border-radius: 0.125rem;
    }
}
</style>
