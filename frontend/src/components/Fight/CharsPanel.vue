<template>
    <div class="chars-panel">
        <CharPanel :char-info="heroInfo" />
        <CharPanel :char-info="monsterInfo" />
    </div>
</template>

<script lang="ts" setup>
import { useMonsterFight } from '@/stores/monster-fight';
import CharPanel from './CharPanel.vue';
import { computed } from 'vue';

const monsterFightStore = useMonsterFight();

const heroInfo = computed(() => monsterFightStore.heroInfo);
const monsterInfo = computed(() => monsterFightStore.monsterInfo);

</script>

<style lang="scss">

.chars-panel {
    display: flex;
    justify-content: center;
    padding: 16px;
    width: 580px;
    max-width: 100%;
    margin: 0 auto;
}

</style>