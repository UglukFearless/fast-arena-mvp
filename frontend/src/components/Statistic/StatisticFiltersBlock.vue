<template>
    <div class="statistic-filters-block">
        <div class="statistic-filters-block__container">
            <StatisticFilterComponent
                v-for="filter in filters"
                :key="filter.key"
                :filter="filter"
                @update:modelValue="onFilterChanged(filter.key, $event)"
            />
        </div>
    </div>
</template>

<script lang="ts" setup>
import { HeroAliveParam, HeroOwnerParam, Parameter } from '@/api/clients';
import StatisticFilterComponent from '@/components/Statistic/StatisticFilter.vue';
import { StatisticFilter, StatisticFiltersState, StatisticFilterValue } from '@/model/StatisticFilter';
import { useStatisticStore } from '@/stores/statistic';

const statisticStore = useStatisticStore();

const filters: StatisticFilter[] = [
    new StatisticFilter('parameter', 'Параметр:', statisticStore.parameter as Parameter, [
        { value: Parameter.WINS, label: 'Количество побед' },
        { value: Parameter.GOLD, label: 'Богатство' },
        { value: Parameter.LEVEL, label: 'Опыт' },
    ]),
    new StatisticFilter('aliveParam', 'Жив/мёртв:', statisticStore.aliveParam as HeroAliveParam, [
        { value: HeroAliveParam.ALL, label: 'Все' },
        { value: HeroAliveParam.ALIVE, label: 'Живые' },
        { value: HeroAliveParam.DEAD, label: 'Мёртвые' },
    ]),
    new StatisticFilter('ownerParam', 'Хозяин:', statisticStore.ownerParam as HeroOwnerParam, [
        { value: HeroOwnerParam.ANY, label: 'Все' },
        { value: HeroOwnerParam.MINE, label: 'Мои' },
    ]),
    new StatisticFilter('desc', 'Порядок:', statisticStore.desc, [
        { value: false, label: 'По возрастанию' },
        { value: true, label: 'По убыванию' },
    ]),
];

async function onFilterChanged(filterKey: string, value: StatisticFilterValue) {
    const targetFilter = filters.find((filter) => filter.key === filterKey);

    if (!targetFilter) {
        return;
    }

    targetFilter.value = value;

    const currentFiltersState: StatisticFiltersState = {
        parameter: filters.find((filter) => filter.key === 'parameter')?.value as Parameter,
        aliveParam: filters.find((filter) => filter.key === 'aliveParam')?.value as HeroAliveParam,
        ownerParam: filters.find((filter) => filter.key === 'ownerParam')?.value as HeroOwnerParam,
        desc: filters.find((filter) => filter.key === 'desc')?.value as boolean,
    };

    await statisticStore.applyFilters(currentFiltersState);

    console.log('Statistic filters state', currentFiltersState);
}
</script>

<style lang="scss" scoped>

.statistic-filters-block__container {
    width: 1280px;
    max-width: 100%;
    margin: 0 auto;
    display: flex;
    flex-wrap: wrap;
    gap: 12px;
}

.statistic-filters-block {
    padding: 24px 0;
    box-shadow: 10px 3px 10px 0px rgba(0, 0, 0, .1);
    margin-bottom: 32px;
}

</style>