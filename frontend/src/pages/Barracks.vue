<template>
    <BaseLayout>
        <template v-slot:main>
            <SelectedHeroPanel />
            <AvailableHeroesPanel />
            <CemeteryOfHeroes />
        </template>
    </BaseLayout>
</template>

<script setup lang="ts">
import BaseLayout from '@/layouts/BaseLayout.vue';
import SelectedHeroPanel from '@/components/Barracks/SelectedHeroPanel.vue';
import AvailableHeroesPanel from '@/components/Barracks/AvailableHeroesPanel.vue';
import CemeteryOfHeroes from '@/components/Barracks/CemeteryOfHeroes.vue';
import { useHeroStore } from '@/stores/hero';
import { onMounted } from 'vue';

const heroStore = useHeroStore();

onMounted(async () => {
    await heroStore.refreshHeroes();
});
</script>