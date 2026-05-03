<template>
    <div v-if="equipmentSlots.length" class="equipment-panel">
        <div class="equipment-panel__title">Экипировка</div>
        <div class="equipment-panel__grid">
            <article
                v-for="slot in equipmentSlots"
                :key="slot.slot"
                class="equipment-panel__slot"
                :class="{ 'equipment-panel__slot_empty': !slot.item }"
            >
                <template v-if="slot.item">
                    <img
                        class="equipment-panel__image"
                        :src="slot.item.item.itemImage"
                        :alt="slot.item.item.name"
                    />
                    <span class="equipment-panel__name">{{ slot.item.item.name }}</span>
                </template>
                <template v-else>
                    <span class="equipment-panel__empty-label">{{ getEquipmentSlotTitle(slot.slot) }}</span>
                    <span class="equipment-panel__empty-state">Пусто</span>
                </template>
            </article>
        </div>
    </div>
</template>

<script lang="ts" setup>
import { EquipmentSlotType, HeroPocketSlotDto } from '@/api/clients';
import { PropType } from 'vue';

const props = defineProps({
    equipmentSlots: {
        type: Array as PropType<HeroPocketSlotDto[]>,
        default: () => [],
    },
});

function getEquipmentSlotTitle(slot: EquipmentSlotType) {
    if (slot === EquipmentSlotType.RIGHT_HAND) {
        return 'Правая рука';
    }

    if (slot === EquipmentSlotType.LEFT_HAND) {
        return 'Левая рука';
    }

    return 'Слот';
}
</script>

<style lang="scss" scoped>
.equipment-panel {
    margin-top: 10px;
    display: flex;
    flex-direction: column;
    gap: 6px;

    &__title {
        font-size: 14px;
        font-weight: 700;
    }

    &__grid {
        display: grid;
        grid-template-columns: repeat(2, minmax(90px, 1fr));
        gap: 6px;
    }

    &__slot {
        min-height: 68px;
        border: 1px solid #1c2d42;
        border-radius: 6px;
        background: #dce4ef;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        padding: 4px;
        gap: 3px;

        &_empty {
            border-style: dashed;
            background: #cdd7e4;
        }
    }

    &__image {
        width: 24px;
        height: 24px;
        object-fit: contain;
    }

    &__name {
        text-align: center;
        font-size: 10px;
        line-height: 1.2;
        font-weight: 600;
    }

    &__empty-label {
        text-align: center;
        font-size: 11px;
        font-weight: 700;
    }

    &__empty-state {
        font-size: 10px;
        color: #4d6177;
    }
}
</style>
