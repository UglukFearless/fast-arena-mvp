<template>
    <section class="statistic-table-content">
        <StatisticTableTitle />
        <StatisticTableHeader />
        <StatisticTableRow v-for="row in rows" :row="row" />
    </section>
</template>

<script setup lang="ts">
import { useStatisticStore } from '@/stores/statistic';
import { computed } from 'vue';

import StatisticTableTitle from '@/components/Statistic/StatisticTableTitle.vue';
import StatisticTableHeader from '@/components/Statistic/StatisticTableHeader.vue';
import StatisticTableRow from '@/components/Statistic/StatisticTableRow.vue';

const statisticStore = useStatisticStore();

statisticStore.init();

const rows = computed(() => statisticStore.rows);
</script>

<style lang="scss" scoped>

.statistic-table-content {
    width: 100%;
    max-width: 1280px;
    margin: 0 auto 24px;
}

</style>