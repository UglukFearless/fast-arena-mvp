<template>
    <div class="statistic-table-header">
        <div class="statistic-table-header__cell">Герой</div>
        <div class="statistic-table-header__cell">{{ title }}</div>
        <div class="statistic-table-header__cell statistic-table-header__cell_actions">Действия</div>
    </div>
</template>

<script setup lang="ts">
import { useStatisticStore } from '@/stores/statistic';
import { computed } from 'vue';

const statisticStore = useStatisticStore();
const title = computed(() => statisticStore.valueTitle);
</script>

<style lang="scss" scoped>

.statistic-table-header {
    width: 100%;
    display: flex;

    &__cell {
        font-size: 16px;
        flex: 1;
        border: 2px solid #444;
        padding: 8px 12px;
        font-weight: 700;

        &_actions {
            flex: 0 0 140px;
        }
    }
}

</style>