import { Building } from '@/model/building';
import { Category } from '@/model/category';
import axiosIns from '@/plugins/axios';
import { buildings, categories, checkAutocomplete, getSubCategory, props, refForm, subCategories, videoData } from './add.vue';

watch(props, async (oldId, newId) => {
refForm.value?.reset();
refForm.value?.resetValidation();
checkAutocomplete.value = false;

await axiosIns.get<Category[]>('Category').then((response) => {
categories.value = response;
});
await axiosIns.get<Building[]>('Building/getBuilding').then((response) => {
buildings.value = response;
});
getSubCategory();
if (newId.videoId) {
axiosIns.get('http://localhost:5124/api/Video/' + newId.videoId).then((response: any) => {
videoData.value = response;
axiosIns.get<Category[]>('Category', {
params: {
categories: response.category.map((cat: Category) => cat.id)
}
}).then((response) => {
subCategories.value = response;
});

const matchingCategories = response.category.map((category: any) => {
return categories.value.find((c: Category) => c.name === category.name);
});
videoData.value.category = matchingCategories.filter(
(category: any) => category !== null
);

const matchingSubCategories = response.subCategory.map((category: any) => {
return subCategories.value.find((c: Category) => c.name === category.name);
});
videoData.value.subCategory = matchingSubCategories.filter(
(category: any) => category !== null
);

const matchingNoBackToBack = response.noBackToBack.map((category: any) => {
return categories.value.find((c: Category) => c.name == category.name);
});
videoData.value.noBackToBack = matchingNoBackToBack.filter(
(category: any) => category !== null
);

const matchingDoNotPlay = response.doNotPlay.map((building: any) => {
return buildings.value.find((c: Building) => c.buildingName === building.buildingName);
});
videoData.value.doNotPlay = matchingDoNotPlay.filter(
(building: any) => building !== null
);
});
} else {
videoData.value.id = 0;
}
});
