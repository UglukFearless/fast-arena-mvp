<template>
    <BaseLayout>
        <template v-slot:main>
            <StatisticFiltersBlock />
            <StatisticTable />
        </template>
    </BaseLayout>
</template>

<script setup lang="ts">
import BaseLayout from '@/layouts/BaseLayout.vue';
import StatisticFiltersBlock from '@/components/Statistic/StatisticFiltersBlock.vue';
import StatisticTable from '@/components/Statistic/StatisticTable.vue';
</script>