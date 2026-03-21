<template>
    <div class="base-layout__content-wrapper">
        <AppHeader />
        <slot name="main" />
    </div>
</template>

<script setup lang="ts">
import AppHeader from '@/components/shared/AppHeader.vue';
</script>

<style lang="scss">

body {
    background: #F2F5FF;
    margin: 0;
    padding: 0;
}
</style>