<script lang="ts" setup>
const listCategory = ref([
    {
        id: 1,
        name: 'koko',
        active: false,
        children: [
            {
                id: 1,
                name: 'children',
                active: false,
            },
        ],
    },
    {
        id: 2,
        name: 'koko',
        active: false,
        children: [
            {
                id: 1,
                name: 'children2',
                active: false,
            },
        ],
    },
]);

const menu = ref(false);
</script>

<template>
    <div>
        <VCard title="Create Awesome 🙌">
            <VCardText>This is your second page.</VCardText>
            <v-menu v-model="menu" :close-on-content-click="false" location="bottom">
                <template v-slot:activator="{ props }">
                    <v-btn color="indigo" v-bind="props"> Menu as Popover </v-btn>
                </template>
                <VCard>
                    <VList>
                        <div v-for="category in listCategory">
                            <div class="father">
                                <VCheckbox v-model="category.active" />
                                <label>{{ category.name }}</label>
                            </div>
                            <div class="children" v-if="category.active">
                                <div class="children-item" v-for="children in category.children">
                                    <VCheckbox />
                                    <label>{{ children.name }}</label>
                                </div>
                            </div>
                        </div>
                    </VList>
                </VCard>
            </v-menu>

            <VCardText>
                Chocolate sesame snaps pie carrot cake pastry pie lollipop muffin. Carrot cake
                dragée chupa chups jujubes. Macaroon liquorice cookie wafer tart marzipan bonbon.
                Gingerbread jelly-o dragée chocolate.
            </VCardText>
        </VCard>
    </div>
</template>

<style lang="scss" scoped>
.father {
  display: flex;
  align-items: center;
  padding-inline-start: 10px;
}

p {
  padding: 0;
  margin: 0;
}

.children {
  &-item {
    display: flex;
    align-items: center;
    padding-inline-start: 40px;
  }
}
</style>
