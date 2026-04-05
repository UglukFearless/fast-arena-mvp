<template>
    <section class="hero-inventory">
        <div class="hero-inventory__header">
            <h3 class="hero-inventory__title">Инвентарь</h3>
            <span v-if="isReadOnly" class="hero-inventory__readonly">Только просмотр</span>
        </div>

        <p v-if="!hero.isInventoryVisible" class="hero-inventory__state-card">
            Инвентарь чужого героя пока скрыт.
        </p>

        <template v-else>
            <p v-if="!inventoryItems.length" class="hero-inventory__state-card">
                У героя пока нет предметов в инвентаре.
            </p>

            <div class="hero-inventory__grid">
                <article
                    v-for="cell in gridCells"
                    :key="cell.key"
                    class="hero-inventory__cell"
                    :class="{ 'hero-inventory__cell_placeholder': cell.isPlaceholder }"
                    @mouseenter="onCellMouseEnter(cell)"
                    @mouseleave="onCellMouseLeave()"
                >
                    <template v-if="!cell.isPlaceholder">
                        <img
                            class="hero-inventory__item-image"
                            :src="cell.item.item.itemImage"
                            :alt="cell.item.item.name"
                        />
                        <span class="hero-inventory__item-name">{{ cell.item.item.name }}</span>
                        <span v-if="cell.item.item.canBeFolded" class="hero-inventory__item-amount">x{{ cell.item.amount }}</span>

                        <div v-if="tooltipCellKey === cell.key" class="hero-inventory__tooltip">
                            {{ cell.item.item.description }}
                        </div>
                    </template>
                </article>
            </div>

            <div class="hero-inventory__money">
                <img
                    v-if="moneyIcon"
                    class="hero-inventory__money-icon"
                    :src="moneyIcon"
                    alt="Монеты"
                />
                <span v-else class="hero-inventory__money-icon hero-inventory__money-icon_fallback">¤</span>
                <span class="hero-inventory__money-value">{{ hero.moneyAmount }}</span>
            </div>
        </template>
    </section>
</template>

<script setup lang="ts">
import { HeroAliveState, HeroInfoDto, HeroItemCellDto, ItemType } from '@/api/clients';
import { computed, onUnmounted, PropType, ref } from 'vue';

type FilledGridCell = {
    key: string;
    isPlaceholder: false;
    item: HeroItemCellDto;
};

type PlaceholderGridCell = {
    key: string;
    isPlaceholder: true;
};

type GridCell = FilledGridCell | PlaceholderGridCell;

const GRID_COLUMNS = 6;
const MIN_GRID_ROWS = 3;

const props = defineProps({
    hero: {
        required: true,
        type: Object as PropType<HeroInfoDto>,
    },
});

const isReadOnly = computed(() => props.hero.isAlive === HeroAliveState.DEAD);

const inventoryItems = computed(() => {
    return props.hero.items.filter(i => i.item && i.item.type !== ItemType.MONEY);
});

const moneyIcon = computed(() => {
    const moneyItem = props.hero.items.find(i => i.item && i.item.type === ItemType.MONEY);
    return moneyItem?.item?.itemImage || '';
});

const tooltipCellKey = ref<string | null>(null);
let tooltipTimer: ReturnType<typeof setTimeout> | null = null;

function clearTooltipTimer() {
    if (tooltipTimer) {
        clearTimeout(tooltipTimer);
        tooltipTimer = null;
    }
}

function onCellMouseEnter(cell: GridCell) {
    clearTooltipTimer();

    if (cell.isPlaceholder) {
        tooltipCellKey.value = null;
        return;
    }

    tooltipTimer = setTimeout(() => {
        tooltipCellKey.value = cell.key;
    }, 1000);
}

function onCellMouseLeave() {
    clearTooltipTimer();
    tooltipCellKey.value = null;
}

const gridCells = computed<GridCell[]>(() => {
    const minCells = GRID_COLUMNS * MIN_GRID_ROWS;
    const itemCells: GridCell[] = inventoryItems.value.map(i => ({
        key: i.id,
        isPlaceholder: false,
        item: i,
    }));

    if (itemCells.length >= minCells)
        return itemCells;

    const placeholdersCount = minCells - itemCells.length;
    const placeholders: GridCell[] = Array.from({ length: placeholdersCount }, (_, index) => ({
        key: `placeholder-${index}`,
        isPlaceholder: true,
    }));

    return [...itemCells, ...placeholders];
});

onUnmounted(() => {
    clearTooltipTimer();
});
</script>

<style lang="scss" scoped>
.hero-inventory {
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

    &__readonly {
        border-radius: 999px;
        padding: 6px 12px;
        background: #f3dada;
        color: #8b2525;
        font-weight: 700;
        white-space: nowrap;
    }

    &__state-card {
        margin: 0;
        font-size: 18px;
        background: white;
        border-radius: 12px;
        box-shadow: 0px 0px 8px rgba(155, 155, 155, 0.4);
        padding: 16px;
    }

    &__grid {
        display: grid;
        grid-template-columns: repeat(6, 72px);
        gap: 8px;
        width: fit-content;
        max-width: 100%;

        @media (max-width: 960px) {
            grid-template-columns: repeat(4, 72px);
        }

        @media (max-width: 680px) {
            grid-template-columns: repeat(3, 64px);
        }
    }

    &__cell {
        position: relative;
        width: 100%;
        min-height: 64px;
        background: #dce4ef;
        border-radius: 8px;
        box-shadow: inset 0 0 0 1px rgba(12, 23, 38, 0.25), 0px 1px 4px rgba(12, 23, 38, 0.35);
        border: 1px solid #1c2d42;
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        gap: 3px;
        padding: 6px;

        &_placeholder {
            background: #cdd7e4;
            border-color: #344a63;
            border-style: dashed;
            box-shadow: inset 0 0 0 1px rgba(28, 45, 66, 0.2);
        }
    }

    &__item-image {
        width: 28px;
        height: 28px;
        object-fit: contain;
    }

    &__item-name {
        text-align: center;
        font-size: 11px;
        line-height: 1.2;
        font-weight: 600;
        color: #13253b;
        text-shadow: 0 1px 0 rgba(255, 255, 255, 0.35);
    }

    &__item-amount {
        font-size: 12px;
        font-weight: 700;
        color: #0e223a;
    }

    &__tooltip {
        position: absolute;
        left: 6px;
        right: 6px;
        bottom: calc(100% + 8px);
        z-index: 5;
        padding: 8px 10px;
        border-radius: 8px;
        background: #1b2736;
        color: #f3f7fc;
        font-size: 12px;
        line-height: 1.35;
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
        min-width: 200px;
    }

    &__money {
        align-self: flex-start;
        display: inline-flex;
        align-items: center;
        gap: 8px;
        border: 1px solid #d8dee6;
        background: white;
        border-radius: 10px;
        padding: 8px 12px;
        box-shadow: 0px 0px 6px rgba(155, 155, 155, 0.35);
    }

    &__money-icon {
        width: 32px;
        height: 32px;
        object-fit: contain;

        &_fallback {
            display: inline-flex;
            justify-content: center;
            align-items: center;
            font-weight: 700;
        }
    }

    &__money-value {
        font-size: 18px;
        font-weight: 700;
    }
}
</style>
