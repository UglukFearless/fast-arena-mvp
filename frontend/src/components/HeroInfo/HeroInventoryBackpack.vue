<template>
    <section class="hero-inventory-backpack">
        <h4 class="hero-inventory-backpack__section-title">Рюкзак</h4>

        <div class="hero-inventory-backpack__grid">
            <article
                v-for="cell in gridCells"
                :key="cell.key"
                class="hero-inventory-backpack__cell"
                :class="{
                    'hero-inventory-backpack__cell_placeholder': cell.isPlaceholder,
                    'hero-inventory-backpack__cell_clickable': !cell.isPlaceholder && canEquipInventoryItem(cell.item),
                    'hero-inventory-backpack__cell_loading': !cell.isPlaceholder && isActing && activeInventoryCellId === cell.item.id,
                }"
                @mouseenter="onCellMouseEnter(cell)"
                @mouseleave="onCellMouseLeave()"
                @click="onCellClick(cell)"
            >
                <template v-if="!cell.isPlaceholder">
                    <img
                        class="hero-inventory-backpack__item-image"
                        :src="cell.item.item.itemImage"
                        :alt="cell.item.item.name"
                    />
                    <span class="hero-inventory-backpack__item-name">{{ cell.item.item.name }}</span>
                    <span v-if="cell.item.item.canBeFolded" class="hero-inventory-backpack__item-amount">x{{ cell.item.amount }}</span>

                    <div v-if="tooltipCellKey === cell.key" class="hero-inventory-backpack__tooltip">
                        {{ cell.item.item.description }}
                    </div>
                </template>
            </article>
        </div>
    </section>
</template>

<script setup lang="ts">
import { HeroItemCellDto, ItemType } from '@/api/clients';
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
    inventoryItems: {
        required: true,
        type: Array as PropType<HeroItemCellDto[]>,
    },
    isReadOnly: {
        required: true,
        type: Boolean,
    },
    isActing: {
        required: true,
        type: Boolean,
    },
    activeInventoryCellId: {
        type: String as PropType<string | null>,
        default: null,
    },
});

const emit = defineEmits(['item-click']);

const tooltipCellKey = ref<string | null>(null);
let tooltipTimer: ReturnType<typeof setTimeout> | null = null;

const gridCells = computed<GridCell[]>(() => {
    const minCells = GRID_COLUMNS * MIN_GRID_ROWS;
    const itemCells: GridCell[] = props.inventoryItems.map(i => ({
        key: i.id,
        isPlaceholder: false,
        item: i,
    }));

    if (itemCells.length >= minCells) {
        return itemCells;
    }

    const placeholdersCount = minCells - itemCells.length;
    const placeholders: GridCell[] = Array.from({ length: placeholdersCount }, (_, index) => ({
        key: `placeholder-${index}`,
        isPlaceholder: true,
    }));

    return [...itemCells, ...placeholders];
});

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

function canEquipInventoryItem(item: HeroItemCellDto) {
    return !props.isReadOnly
        && !props.isActing
        && !!item.item
        && item.item.type === ItemType.POTION;
}

function onCellClick(cell: GridCell) {
    if (cell.isPlaceholder || !canEquipInventoryItem(cell.item)) {
        return;
    }

    emit('item-click', cell.item);
}

onUnmounted(() => {
    clearTooltipTimer();
});
</script>

<style lang="scss" scoped>
.hero-inventory-backpack {
    display: flex;
    flex-direction: column;
    gap: 12px;

    &__section-title {
        margin: 0;
        font-size: 20px;
        color: #18304a;
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

        &_clickable {
            cursor: pointer;
            transition: transform 0.12s ease, box-shadow 0.12s ease, border-color 0.12s ease;

            &:hover {
                transform: translateY(-1px);
                border-color: #0f5f85;
                box-shadow: inset 0 0 0 1px rgba(12, 23, 38, 0.25), 0px 4px 10px rgba(12, 23, 38, 0.28);
            }
        }

        &_loading {
            opacity: 0.6;
        }

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
}
</style>