<template>
    <div v-if="isRewardThere" class="fight-reward-panel">
        <p v-html="rewardHtml" />
    </div>
</template>

<script setup lang="ts">
import { useMonsterFight } from '@/stores/monster-fight';
import { computed } from 'vue';

const monsterFightStore = useMonsterFight();

const isRewardThere = computed(() => {
    return !!monsterFightStore.fight?.reward && !!monsterFightStore.fight?.reward.items.length;
});

const rewardHtml = computed(() => {
    return buildRewardHtml();
});

function buildRewardHtml() {
    let result = ''

    if (!isRewardThere.value) {
        return result;
    }

    monsterFightStore.fight!.reward!.items.forEach(i => {
        result += `Вы получили <b>${i.amound} ${i.item.name}</b>! `;
    });

    return result;
}

</script>

<style lang="scss">

.fight-reward-panel {
    padding: 16px 32px;
    font-size: 18px;
    justify-content: center;
    margin: 16px auto;
    border-radius: 8px;
    box-shadow: 2px 2px 12px rgba(185, 185, 185, 0.5);
    max-height: 200px;
    min-width: 350px;
    overflow: auto;

    p {
        margin: 12px auto;
        max-width: 100%;
        text-align: center;
    }
}

</style>