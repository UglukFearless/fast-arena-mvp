<template>
    <div class="statistic-filter">
        <span class="statistic-filter__title">{{ filter.title }}</span>
        <select class="statistic-filter__select" v-model="filter.value" @change="updateValue(filter.value)">
            <option 
                class="statistic-filter__option"
                v-for="option in filter.options" 
                :key="option.label"
                :value="option.value">
                {{ option.label }}
            </option>
        </select>
    </div>
</template>

<script lang="ts" setup>
import { PropType } from 'vue';
import { StatisticFilter, StatisticFilterValue } from '@/model/StatisticFilter';

const props = defineProps({
    filter: {
        required: true,
        type: Object as PropType<StatisticFilter>,
    }
});

const emit = defineEmits<{
    (event: 'update:modelValue', value: StatisticFilterValue): void;
}>();

function updateValue(filterValue: StatisticFilterValue) {
    emit('update:modelValue', filterValue);
}
</script>

<style lang="scss" scoped>

.statistic-filter {
    display: flex;
    align-items: center;
    gap: 8px;

    &__title {
        font-weight: 700;
    }

    &__select {
        padding: 4px 8px;
        border-radius: 4px;
        border: 2px solid #444;
        background-color: #fff;
        font-size: 14px;
        cursor: pointer;

        &:focus {
            outline: none;
            border-color: #000;
        }
    }
}

</style>