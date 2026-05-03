<template>
    <section class="hero-inventory-equipment">
        <h4 class="hero-inventory-equipment__section-title">Экипировка</h4>

        <div class="hero-inventory-equipment__grid">
            <article
                v-for="slot in equipmentSlots"
                :key="slot.slot"
                class="hero-inventory-equipment__cell"
                :class="{
                    'hero-inventory-equipment__cell_placeholder': !slot.item,
                    'hero-inventory-equipment__cell_clickable': canUnequipItem(slot),
                    'hero-inventory-equipment__cell_loading': isActing && activePocketSlot === slot.slot,
                }"
                @mouseenter="onSlotMouseEnter(slot)"
                @mouseleave="onCellMouseLeave()"
                @click="onSlotClick(slot)"
            >
                <template v-if="slot.item">
                    <img
                        class="hero-inventory-equipment__item-image"
                        :src="slot.item.item.itemImage"
                        :alt="slot.item.item.name"
                    />
                    <span class="hero-inventory-equipment__item-name">{{ slot.item.item.name }}</span>

                    <div v-if="tooltipCellKey === getSlotTooltipKey(slot)" class="hero-inventory-equipment__tooltip">
                        {{ slot.item.item.description }}
                    </div>
                </template>

                <template v-else>
                    <span class="hero-inventory-equipment__slot-label">{{ getSlotTitle(slot.slot) }}</span>
                    <span class="hero-inventory-equipment__slot-state">Пусто</span>
                </template>
            </article>
        </div>
    </section>
</template>

<script setup lang="ts">
import { EquipmentSlotType, HeroPocketSlotDto } from '@/api/clients';
import { onUnmounted, PropType, ref } from 'vue';

const props = defineProps({
    equipmentSlots: {
        required: true,
        type: Array as PropType<HeroPocketSlotDto[]>,
    },
    isReadOnly: {
        required: true,
        type: Boolean,
    },
    isActing: {
        required: true,
        type: Boolean,
    },
    activePocketSlot: {
        type: Number as PropType<EquipmentSlotType | null>,
        default: null,
    },
});

const emit = defineEmits(['slot-click']);

const tooltipCellKey = ref<string | null>(null);
let tooltipTimer: ReturnType<typeof setTimeout> | null = null;

function clearTooltipTimer() {
    if (tooltipTimer) {
        clearTimeout(tooltipTimer);
        tooltipTimer = null;
    }
}

function onSlotMouseEnter(slot: HeroPocketSlotDto) {
    clearTooltipTimer();

    if (!slot.item) {
        tooltipCellKey.value = null;
        return;
    }

    tooltipTimer = setTimeout(() => {
        tooltipCellKey.value = getSlotTooltipKey(slot);
    }, 1000);
}

function onCellMouseLeave() {
    clearTooltipTimer();
    tooltipCellKey.value = null;
}

function canUnequipItem(slot: HeroPocketSlotDto) {
    return !props.isReadOnly
        && !props.isActing
        && !!slot.item;
}

function onSlotClick(slot: HeroPocketSlotDto) {
    if (!canUnequipItem(slot)) {
        return;
    }

    emit('slot-click', slot);
}

function getSlotTooltipKey(slot: HeroPocketSlotDto) {
    return `equipment-${slot.slot}`;
}

function getSlotTitle(slot: EquipmentSlotType) {
    switch (slot) {
        case EquipmentSlotType.RIGHT_HAND:
            return 'Правая рука';
        case EquipmentSlotType.LEFT_HAND:
            return 'Левая рука';
        default:
            return 'Слот';
    }
}

onUnmounted(() => {
    clearTooltipTimer();
});
</script>

<style lang="scss" scoped>
.hero-inventory-equipment {
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
        grid-template-columns: repeat(2, 110px);
        gap: 8px;

        @media (max-width: 680px) {
            grid-template-columns: repeat(2, 92px);
        }
    }

    &__cell {
        position: relative;
        width: 100%;
        min-height: 84px;
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

    &__slot-label {
        font-size: 12px;
        font-weight: 700;
        color: #18304a;
        text-align: center;
    }

    &__slot-state {
        font-size: 11px;
        color: #4d6177;
        text-align: center;
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
