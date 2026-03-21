<template>
    <section @mouseover="mouseOver = true" @mouseleave="mouseOver = false" class="selected-hero-section">
        <div class="selected-hero-section__hero-panel">
            <EmptyHero />
            <SelectedHero :mouseOver="mouseOver" />
        </div>
    </section>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import EmptyHero from './EmptyHero.vue';
import SelectedHero from './SelectedHero.vue';

const mouseOver = ref(false);
</script>

<style lang="scss">
.selected-hero-section {
    display: flex;
    justify-content: center;
    border-bottom: 1px solid #aaa;
    margin-bottom: 12px;

    &__hero-panel {
        background: white;
        margin: 12px auto;
        padding: 12px;
        box-shadow: 0px 0px 8px rgba(155, 155, 155, 0.4);
        border-radius: 8px;
        max-width: 100vw;

        &:hover {
            box-shadow: 0px 0px 2px rgba(155, 155, 155, 0.6);
        }
    }
}
</style>