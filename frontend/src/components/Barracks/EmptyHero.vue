<template>
    <div v-if="!selectedHero" class="empty-hero__panel">
        <img class="empty-hero__image" src="@/assets/gui/uncertainty.svg" />
        <p class="empty-hero__text">
            Требуется выбрать героя для поединка
        </p>
    </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useHeroStore } from '@/stores/hero';

const heroStore = useHeroStore();
const selectedHero = computed(() => heroStore.selectedHero);
</script>

<style lang="scss">
.empty-hero {

    &__panel {
        display: flex;
        justify-content: center;
        align-items: center;
        max-width: 100%;
        min-height: 120px;
    }

    &__image {
        width: 64px;
        margin: 0 12px;
    }

    &__text {
        max-width: 160px;
        font-size: 18px;
        margin: 0 12px;
    }
}
</style>