<template>
    <section class="available-heroes">
        <div class="available-heroes__content">
            <div class="available-heroes__content-wrapper">
                <AddNewHeroPanel />
                <AvailableHero v-for="hero in heroesExceptSelected" :hero="hero" />
            </div>
        </div>  
    </section>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useHeroStore } from '@/stores/hero';
import AddNewHeroPanel from './AddNewHeroPanel.vue';

import AvailableHero from './AvailableHero.vue';


const heroStore = useHeroStore();
const heroesExceptSelected = computed(() => heroStore.heroesExceptSelected);
</script>

<style lang="scss">
.available-heroes {
    border-bottom: 1px solid #aaa;
    margin-bottom: 12px;

    &__content {
        width: 1280px;
        margin: 0 auto;
        max-width: 100%;
    }

    &__content-wrapper {
        display: flex;
        flex-wrap: wrap;
    }
}
</style>