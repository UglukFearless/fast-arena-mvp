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
            <p v-if="actionMessage" class="hero-inventory__state-card" :class="{ 'hero-inventory__state-card_error': actionMessageIsError }">
                {{ actionMessage }}
            </p>

            <div class="hero-inventory__layout">
                <div class="hero-inventory__slots">
                    <HeroInventoryEquipment
                        :equipment-slots="equipmentSlots"
                        :is-read-only="isReadOnly"
                        :is-acting="isActing"
                        :active-pocket-slot="activePocketSlot"
                        @slot-click="onPocketClick"
                    />

                    <HeroInventoryPockets
                        :pockets="consumablePockets"
                        :is-read-only="isReadOnly"
                        :is-acting="isActing"
                        :active-pocket-slot="activePocketSlot"
                        @pocket-click="onPocketClick"
                    />
                </div>

                <HeroInventoryBackpack
                    :inventory-items="inventoryItems"
                    :is-read-only="isReadOnly"
                    :is-acting="isActing"
                    :active-inventory-cell-id="activeInventoryCellId"
                    @item-click="onInventoryItemClick"
                />
            </div>

            <div class="hero-inventory__money">
                <img
                    v-if="moneyIcon"
                    class="hero-inventory__money-icon"
                    :src="moneyIcon"
                    alt="Монеты"
                />
                <span v-else class="hero-inventory__money-icon hero-inventory__money-icon_fallback">¤</span>
                <span class="hero-inventory__money-value">
                    {{ hero.moneyAmount }}
                </span>
            </div>
        </template>
    </section>
</template>

<script setup lang="ts">
import { ApiException, EquipmentSlotType, HeroAliveState, HeroEquipmentClient, HeroInfoDto, HeroItemCellDto, HeroPocketSlotDto, ItemType } from '@/api/clients';
import HeroInventoryBackpack from '@/components/HeroInfo/HeroInventoryBackpack.vue';
import HeroInventoryEquipment from '@/components/HeroInfo/HeroInventoryEquipment.vue';
import HeroInventoryPockets from '@/components/HeroInfo/HeroInventoryPockets.vue';
import { ApiSettings } from '@/utils/constants';
import authFetch from '@/utils/http-helper';
import { computed, PropType, ref } from 'vue';

const props = defineProps({
    hero: {
        required: true,
        type: Object as PropType<HeroInfoDto>,
    },
});

const emit = defineEmits(['equipped', 'unequipped']);
const heroEquipmentClient = new HeroEquipmentClient(ApiSettings.BaseUrl, authFetch);

const isReadOnly = computed(() => props.hero.isAlive === HeroAliveState.DEAD);
const isActing = ref(false);
const activeInventoryCellId = ref<string | null>(null);
const activePocketSlot = ref<EquipmentSlotType | null>(null);
const actionMessage = ref('');
const actionMessageIsError = ref(false);

const consumableSlotTypes: EquipmentSlotType[] = [
    EquipmentSlotType.POCKET_1,
    EquipmentSlotType.POCKET_2,
    EquipmentSlotType.POCKET_3,
];

const equipmentSlotTypes: EquipmentSlotType[] = [
    EquipmentSlotType.RIGHT_HAND,
    EquipmentSlotType.LEFT_HAND,
];

const consumablePockets = computed(() => {
    return props.hero.pockets.filter(p => consumableSlotTypes.includes(p.slot));
});

const equipmentSlots = computed<HeroPocketSlotDto[]>(() => {
    return equipmentSlotTypes.map(slot => {
        const existingSlot = props.hero.pockets.find(p => p.slot === slot);
        return existingSlot || { slot, item: undefined };
    });
});

const inventoryItems = computed(() => {
    return props.hero.items.filter(i => i.item && i.item.type !== ItemType.MONEY);
});

const moneyIcon = computed(() => {
    const moneyItem = props.hero.items.find(i => i.item && i.item.type === ItemType.MONEY);
    return moneyItem?.item?.itemImage || '';
});

function setActionMessage(message: string, isError: boolean) {
    actionMessage.value = message;
    actionMessageIsError.value = isError;
}
async function onInventoryItemClick(item: HeroItemCellDto) {
    const slotResolution = resolveTargetSlot(item);
    if (!slotResolution.canEquip || slotResolution.slot == null) {
        setActionMessage(slotResolution.message, true);
        return;
    }

    isActing.value = true;
    activeInventoryCellId.value = item.id;
    activePocketSlot.value = null;
    setActionMessage('', false);

    try {
        await heroEquipmentClient.equip({ heroItemCellId: item.id });
        emit('equipped', {
            heroItemCellId: item.id,
            slot: slotResolution.slot,
        });
    } catch (error) {
        setActionMessage(getErrorMessage(error, 'Не удалось экипировать предмет.'), true);
    } finally {
        isActing.value = false;
        activeInventoryCellId.value = null;
    }
}

function resolveTargetSlot(itemCell: HeroItemCellDto): { canEquip: boolean; slot: EquipmentSlotType | null; message: string } {
    const itemType = itemCell.item.type;

    if (itemType === ItemType.POTION) {
        const targetPocket = consumablePockets.value.find(p => !p.item);
        if (!targetPocket) {
            return {
                canEquip: false,
                slot: null,
                message: 'Все карманы заняты.',
            };
        }

        return {
            canEquip: true,
            slot: targetPocket.slot,
            message: '',
        };
    }

    const rightHandSlot = getEquipmentSlot(EquipmentSlotType.RIGHT_HAND);
    const leftHandSlot = getEquipmentSlot(EquipmentSlotType.LEFT_HAND);
    const isRightHandOccupied = !!rightHandSlot?.item;
    const isLeftHandOccupied = !!leftHandSlot?.item;
    const hasTwoHandedWeapon = hasTwoHandedWeaponEquipped();

    if (itemType === ItemType.SHIELD) {
        if (hasTwoHandedWeapon) {
            return {
                canEquip: false,
                slot: null,
                message: 'Нельзя экипировать щит с двуручным оружием.',
            };
        }

        if (isLeftHandOccupied) {
            return {
                canEquip: false,
                slot: null,
                message: 'Левая рука уже занята.',
            };
        }

        return {
            canEquip: true,
            slot: EquipmentSlotType.LEFT_HAND,
            message: '',
        };
    }

    if (itemType === ItemType.WEAPON) {
        if (isTwoHandedWeapon(itemCell)) {
            if (isRightHandOccupied || isLeftHandOccupied) {
                return {
                    canEquip: false,
                    slot: null,
                    message: 'Двуручное оружие требует обе свободные руки.',
                };
            }

            return {
                canEquip: true,
                slot: EquipmentSlotType.RIGHT_HAND,
                message: '',
            };
        }

        if (hasTwoHandedWeapon || isRightHandOccupied) {
            return {
                canEquip: false,
                slot: null,
                message: 'Правая рука уже занята.',
            };
        }

        return {
            canEquip: true,
            slot: EquipmentSlotType.RIGHT_HAND,
            message: '',
        };
    }

    return {
        canEquip: false,
        slot: null,
        message: 'Этот предмет нельзя экипировать из рюкзака.',
    };
}

function getEquipmentSlot(slotType: EquipmentSlotType) {
    return equipmentSlots.value.find(slot => slot.slot === slotType);
}

function hasTwoHandedWeaponEquipped() {
    const rightHandItem = getEquipmentSlot(EquipmentSlotType.RIGHT_HAND)?.item;
    if (!rightHandItem || rightHandItem.item.type !== ItemType.WEAPON) {
        return false;
    }

    return isTwoHandedWeapon(rightHandItem);
}

function isTwoHandedWeapon(itemCell: HeroItemCellDto) {
    if (!itemCell.item || itemCell.item.type !== ItemType.WEAPON) {
        return false;
    }

    const slots = itemCell.item.allowedSlots;
    return Array.isArray(slots)
        && slots.includes(EquipmentSlotType.RIGHT_HAND)
        && slots.includes(EquipmentSlotType.LEFT_HAND);
}

async function onPocketClick(pocket: HeroPocketSlotDto) {
    if (!pocket.item || isReadOnly.value || isActing.value) {
        return;
    }

    isActing.value = true;
    activeInventoryCellId.value = null;
    activePocketSlot.value = pocket.slot;
    setActionMessage('', false);

    try {
        await heroEquipmentClient.unequip({ slot: pocket.slot });
        emit('unequipped', {
            slot: pocket.slot,
        });
    } catch (error) {
        setActionMessage(getErrorMessage(error, 'Не удалось снять предмет.'), true);
    } finally {
        isActing.value = false;
        activePocketSlot.value = null;
    }
}

function getErrorMessage(error: unknown, fallback: string) {
    if (ApiException.isApiException(error)) {
        return error.response || error.message || fallback;
    }

    if (error instanceof Error) {
        return error.message || fallback;
    }

    return fallback;
}
</script>

<style lang="scss" scoped>
.hero-inventory {
    display: flex;
    flex-direction: column;
    gap: 16px;

    &__layout {
        display: flex;
        align-items: flex-start;
        gap: 20px;
        flex-wrap: wrap;
    }

    &__slots {
        display: flex;
        flex-direction: column;
        gap: 16px;
    }

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

        &_error {
            color: #992b2b;
            border: 1px solid #d1a3a3;
        }
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
