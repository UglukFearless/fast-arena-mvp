<template>
    <div class="statistic-table-title">
        <h1>{{ title }}</h1>
    </div>
</template>

<script setup lang="ts">
import { useStatisticStore } from '@/stores/statistic';
import { computed } from 'vue';

const statisticStore = useStatisticStore();
const title = computed(() => statisticStore.title);

</script>

<style lang="scss" scoped>

.statistic-table-title {
    width: 100%;
    text-align: center;
    margin-bottom: 12px;
}

</style>