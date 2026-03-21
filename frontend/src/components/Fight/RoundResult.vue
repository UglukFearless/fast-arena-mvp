<template>
    <p class="round-result" v-html="roundResultText" />
</template>

<script setup lang="ts">
import { useMonsterFight } from '@/stores/monster-fight';
import { computed } from 'vue';

const monsterFightStore = useMonsterFight();
const roundResultText = computed(() => monsterFightStore.currentState?.result?.resultText);
</script>

<style lang="scss">
.round-result {
    display: inline-block;
    text-align: center;
    border-top: 1px solid #aaa;
    max-width: 100%;
}
</style>