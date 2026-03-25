<template>
    <div class="statistic-table-row">
        <div class="statistic-table-row__hero">
            <div>
                <img v-if="!row.isAlive"
                    class="statistic-table-row__skull" 
                    src="@/assets/gui/death-skull.svg" />
                <img class="statistic-table-row__portrait" :src="row.portraitUrl" />
            </div>
            <span>
                {{ row.heroName }}
            </span>
        </div>
        <div class="statistic-table-row__value">
            <span>{{ row.value + ' ' + symbols }}</span>
        </div>
    </div>
</template>

<script setup lang="ts">
import { StatisticDataRowDto } from '@/api/clients';
import { useStatisticStore } from '@/stores/statistic';
import { computed, PropType } from 'vue';

const statisticStore = useStatisticStore();
const symbols = computed(() => statisticStore.valueSymbols);

const props = defineProps({
    row: {
        required: true,
        type: Object as PropType<StatisticDataRowDto>,
    }
});

</script>

<style lang="scss" scoped>

.statistic-table-row {
    width: 100%;
    display: flex;
    border: 2px solid #444;
    border-top: none;

    &__hero {
        flex: 1;
        border-right: 2px solid #444;
        padding: 8px 12px;
        font-weight: 700;
        display: flex;
        align-items: center;
        position: relative;
        gap: 8px;
    }

    &__skull {
        width: 24px;
        position: absolute;
        left: 3px;
        top: 3px;
    }
    
    &__portrait {
        width: 64px;
    }

    &__value {
        flex: 1;
        padding: 8px 12px;
        font-weight: 700;
        font-size: 24px;
        align-items: center;
        display: flex;
    }
}

</style>