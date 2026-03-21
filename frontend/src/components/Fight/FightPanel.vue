<template>
    <main >
        <CharsPanel />
        <RoundPanel />
    </main>
</template>

<script setup lang="ts">
import RoundPanel from './RoundPanel.vue';
import CharsPanel from './CharsPanel.vue';
import { useMonsterFight } from '@/stores/monster-fight';
import { onMounted } from 'vue';

const monsterFightStore = useMonsterFight();

onMounted(async () => {
    await monsterFightStore.getCurrent();
});

</script>