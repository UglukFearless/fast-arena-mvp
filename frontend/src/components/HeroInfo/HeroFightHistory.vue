<template>
    <div class="fight-history">
        <div class="fight-history__header">
            <h3 class="fight-history__title">История сражений</h3>
            <span class="fight-history__count">{{ results.length }}</span>
        </div>

        <p v-if="!results.length" class="fight-history__empty">
            У этого героя пока нет истории боёв.
        </p>

        <div v-else class="fight-history__list">
            <HeroFightResult
                v-for="result in sortedResults"
                :key="result.id"
                :result="result"
            />
        </div>
    </div>
</template>

<script setup lang="ts">
import { MonsterFightResultDto } from '@/api/clients';
import { computed, PropType } from 'vue';
import HeroFightResult from './HeroFightResult.vue';

const props = defineProps({
    results: {
        required: true,
        type: Array as PropType<MonsterFightResultDto[]>,
    },
});

const sortedResults = computed(() =>
    [...props.results].sort((a, b) => b.order - a.order)
);
</script>

<style lang="scss" scoped>
.fight-history {
    display: flex;
    flex-direction: column;
    gap: 16px;

    &__header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        gap: 12px;
    }

    &__title {
        margin: 0;
        font-size: 28px;
    }

    &__count {
        min-width: 40px;
        height: 40px;
        border-radius: 50%;
        background: #111827;
        color: white;
        display: inline-flex;
        justify-content: center;
        align-items: center;
        font-weight: 700;
    }

    &__empty {
        margin: 0;
        font-size: 20px;
        background: white;
        border-radius: 12px;
        box-shadow: 0px 0px 8px rgba(155, 155, 155, 0.4);
        padding: 20px;
    }

    &__list {
        display: flex;
        flex-direction: column;
        gap: 16px;
    }
}
</style>
