<template>
    <section class="chemetery__content">
        <h2 class="chemetery__title">Cemetery!</h2>
        <div class="chemetery__content-wrapper">
            <DeadHero v-for="hero in deadHeroes" :hero="hero" />
        </div>
    </section>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useHeroStore } from '@/stores/hero';

import DeadHero from './DeadHero.vue';

const heroStore = useHeroStore();
const deadHeroes = computed(() => heroStore.deadHeroes);
</script>

<style lang="scss">
.chemetery {
    &__content {
        padding: 18px 0;
    }

    &__title {
        text-align: center;
    }

    &__content-wrapper {
        display: flex;
        flex-wrap: wrap;
    }
}
</style>