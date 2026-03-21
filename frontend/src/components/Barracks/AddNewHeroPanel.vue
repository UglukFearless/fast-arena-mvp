<template>
    <div @click="goToCreationHeroForm" class="add-new-hero-panel">
        <img class="add-new-hero-panel__icon" src="@/assets/gui/plus.svg" />
        <p class="add-new-hero-panel__text">
            Создать нового
        </p>
    </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router';

const router = useRouter();

function goToCreationHeroForm() {
    router.push('/create-hero');
}
</script>

<style lang="scss">
.add-new-hero-panel {
    background: white;
    margin: 12px;
    padding: 12px;
    box-shadow: 0px 0px 8px rgba(155, 155, 155, 0.4);
    border-radius: 8px;
    max-width: 100vw;
    width: 290px;
    cursor: pointer;
    display: inline-flex;
    justify-content: center;
    align-items: center;

    &:hover {
        box-shadow: 0px 0px 2px rgba(155, 155, 155, 0.6);
    }

    &__icon {
        width: 128px;
    }

    &__text {
        padding-left: 12px;
        font-size: 18px;
        font-weight: bold;
    }
}
</style>