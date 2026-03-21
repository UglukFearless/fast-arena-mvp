<template>
    <div class="round-panel">
        <div class="round-panel__current">
            <AvaliableHeroActions />
            <DiceRolls />
            <RoundResult />
            <RewardPanel />
        </div>
        <FightHistoryPanel :round-results="history" />
    </div>
</template>

<script setup lang="ts">
import FightHistoryPanel from './FightHistoryPanel.vue';
import AvaliableHeroActions from './AvaliableHeroActions.vue';
import DiceRolls from './DiceRolls.vue';
import RoundResult from './RoundResult.vue';
import RewardPanel from './RewardPanel.vue';
import { useMonsterFight } from '@/stores/monster-fight';
import { computed } from 'vue';

const monsterFightStore = useMonsterFight();

const history = computed(() => monsterFightStore.history);

</script>

<style lang="scss">
.round-panel {
    display: flex;
    flex-direction: column;
    padding: 16px 32px 32px;

    &__current {
        display: flex;
        padding: 16px 32px 32px;
        font-size: 18px;
        justify-content: center;
        flex-direction: column;
        margin: 16px auto;
        border-radius: 8px;
        box-shadow: 2px 2px 12px rgba(185, 185, 185, 0.5);
        width: 516px;
        max-width: 100%;
    }
}
</style>