<template>
    <button @click="processClick" class="app-button">
        <slot />
    </button>
</template>

<script setup lang="ts">
const emit = defineEmits(['click']);

function processClick(event: Event) {
    emit('click', event);
}
</script>

<style lang="scss">
.app-button {
    display: flex;
    padding: 8px 20px;
    justify-content: center;
    align-items: center;
    border-radius: 4px;
    background: #BBCBF4;
    border: none;
    cursor: pointer;
}
</style>