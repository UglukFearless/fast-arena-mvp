<template>
    <div class="dice-rolls">
        <div class="dice-rolls__hero">
            <span> Герой </span>
            <span class="dice-rolls__dice-roll">{{ state?.heroDiceRoll ?? '-' }}</span>
        </div>
        <div class="dice-rolls__ballance">
            <span>Баланс</span>
            <span v-bind:class="{ 'dice-rolls__good-balance': isGoodBalance, 'dice-rolls__bad-balance': isBadBalance }" 
                class="dice-rolls__dice-roll">{{ diceRollBallance }}</span>
        </div>
        <div class="dice-rolls__monster">
            <span> Противник </span>
            <span class="dice-rolls__dice-roll">{{ state?.monsterDiceRoll ?? '-' }}</span>
        </div>
    </div>
</template>

<script setup lang="ts">
import { useMonsterFight } from '@/stores/monster-fight';
import { computed } from 'vue';


const monsterFightStore = useMonsterFight();

const state = computed(() => monsterFightStore.currentState);
const diceRollBallance = computed(() => {
    if (!state.value)
        return 0;
    if (!state.value.heroDiceRoll)
        return 0;
    if (!state.value.monsterDiceRoll)
        return 0;

    return state.value.heroDiceRoll - state.value.monsterDiceRoll;
});

const isGoodBalance = computed(() => diceRollBallance.value > 0);
const isBadBalance = computed(() => diceRollBallance.value < 0);

</script>

<style lang="scss">

.dice-rolls {

    padding: 12px 0;
    display: inline-flex;
    justify-content: center;
    gap: 12px;

    &__hero {
        padding: 0 6px;
        border-right: 1px solid #aaa;
        display: inline-flex;
        flex-direction: column;
        text-align: center;
        width: 108px;
    }

    &__monster {
        padding: 0 6px;
        border-left: 1px solid #aaa;
        display: inline-flex;
        flex-direction: column;
        text-align: center;
        width: 108px;
    }

    &__ballance {
        padding: 0 6px;
        display: inline-flex;
        flex-direction: column;
        text-align: center;
        width: 108px;
    }
    &__good-balance {
        background: rgb(9, 164, 56);
    }
    &__bad-balance {
        background: rgb(247, 98, 98);
    }

    &__dice-roll {
        font-size: 24px;
        font-weight: bold;
    }
}

</style>