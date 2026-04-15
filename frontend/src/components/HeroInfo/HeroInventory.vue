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
                <HeroInventoryPockets
                    :pockets="hero.pockets"
                    :is-read-only="isReadOnly"
                    :is-acting="isActing"
                    :active-pocket-slot="activePocketSlot"
                    @pocket-click="onPocketClick"
                />

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
    const targetPocket = props.hero.pockets.find(p => !p.item);
    if (!targetPocket) {
        setActionMessage('Все карманы заняты.', true);
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
            slot: targetPocket.slot,
        });
    } catch (error) {
        setActionMessage(getErrorMessage(error, 'Не удалось экипировать предмет.'), true);
    } finally {
        isActing.value = false;
        activeInventoryCellId.value = null;
    }
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
