<template>
    <div class="statistic-filter">
        <span class="statistic-filter__title">{{ filter.title }}</span>
        <select class="statistic-filter__select" v-model="filter.value" @change="updateValue(filter.value)">
            <option 
                class="statistic-filter__option"
                v-for="option in filter.options" 
                :value="option.value">
                {{ option.label }}
            </option>
        </select>
    </div>
</template>

<script lang="ts" setup>
import { PropType } from 'vue';

const props = defineProps({
    filter: {
        required: true,
        type: Object as PropType<any>,
    }
});

const emit = defineEmits(['update:modelValue']);

function updateValue(filterValue: string | number) {
    if (typeof filterValue === 'string') {
        filterValue = Number.parseInt(filterValue);
    }
    emit('update:modelValue', filterValue);
}
</script>

<style lang="scss" scoped>
</style>