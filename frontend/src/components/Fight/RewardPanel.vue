<template>
    <div v-if="isRewardThere" class="fight-reward-panel">
        <p class="fight-reward-panel__title">Получено после боя</p>

        <div class="fight-reward-panel__list">
            <div
                v-for="rewardItem in rewardItems"
                :key="buildRewardItemKey(rewardItem)"
                class="fight-reward-panel__item"
            >
                <img
                    class="fight-reward-panel__image"
                    :src="rewardItem.item.itemImage"
                    :alt="rewardItem.item.name"
                />

                <span class="fight-reward-panel__name">{{ rewardItem.item.name }}</span>
                <span class="fight-reward-panel__amount">x{{ rewardItem.amound }}</span>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { GivenItemDto, ItemType } from '@/api/clients';
import { useMonsterFight } from '@/stores/monster-fight';
import { computed } from 'vue';

const monsterFightStore = useMonsterFight();

const rewardItems = computed((): GivenItemDto[] => {
    const items = monsterFightStore.fight?.reward?.items ?? [];

    return [...items].sort((left, right) => {
        if (left.item.type === ItemType.MONEY && right.item.type !== ItemType.MONEY) {
            return -1;
        }

        if (left.item.type !== ItemType.MONEY && right.item.type === ItemType.MONEY) {
            return 1;
        }

        return left.item.name.localeCompare(right.item.name);
    });
});

const isRewardThere = computed(() => {
    return rewardItems.value.length > 0;
});

function buildRewardItemKey(rewardItem: GivenItemDto): string {
    return `${rewardItem.item.id}-${rewardItem.amound}`;
}
</script>

<style lang="scss">
.fight-reward-panel {
    margin: 16px auto 0;
    padding-top: 16px;
    border-top: 1px solid #d8d8d8;

    &__title {
        margin: 0 0 12px;
        text-align: center;
        font-size: 16px;
        font-weight: 700;
        color: #2f2f2f;
    }

    &__list {
        display: flex;
        flex-direction: column;
        gap: 8px;
    }

    &__item {
        display: grid;
        grid-template-columns: 36px 1fr auto;
        align-items: center;
        gap: 12px;
        padding: 8px 10px;
        border-radius: 10px;
        background: #f7f7f7;
    }

    &__image {
        width: 36px;
        height: 36px;
        object-fit: contain;
    }

    &__name {
        color: #2f2f2f;
        line-height: 1.2;
    }

    &__amount {
        font-weight: 700;
        color: #4b4b4b;
        white-space: nowrap;
    }
}
</style>