<template>
    <section class="shop">
        <div class="shop__content">
            <h2 class="shop__title">Магазин</h2>

            <p v-if="loading" class="shop__state-card">Загружаем товары...</p>
            <p v-else-if="error" class="shop__state-card shop__state-card_error">{{ error }}</p>

            <template v-else>
                <div class="shop__action shop__action_top">
                    <button
                        class="shop__deal-button"
                        :disabled="isHeroBalanceInsufficient || (sellSelections.length === 0 && buySelections.length === 0)"
                        @click="confirmDeal"
                    >
                        Совершить сделку
                    </button>
                </div>

                <div class="shop__layout">
                    <ShopHeroItemsColumn
                        :items="heroItems"
                        :badge-amount="heroMoneyAfterBalance"
                        :money-icon="moneyIcon"
                        @add-to-sell="addToSell"
                    />

                    <ShopSellLineColumn
                        :items="sellLineItems"
                        :money-icon="moneyIcon"
                        :badge-amount="sellColumnOfferTotal"
                        :badge-is-alert="isHeroBalanceInsufficient"
                        @remove-from-sell="removeFromSell"
                    />

                    <ShopBuyLineColumn
                        :items="buyLineItems"
                        :money-icon="moneyIcon"
                        :badge-amount="buyColumnOfferTotal"
                        @remove-from-buy="removeFromBuy"
                    />

                    <ShopCatalogColumn
                        :items="shopStore.items ?? []"
                        :money-icon="moneyIcon"
                        @add-to-buy="addToBuy"
                    />
                </div>
            </template>
        </div>
    </section>
</template>

<script setup lang="ts">
import { HeroClient } from '@/api/clients';
import ShopBuyLineColumn from '@/components/Shop/ShopBuyLineColumn.vue';
import ShopCatalogColumn from '@/components/Shop/ShopCatalogColumn.vue';
import ShopHeroItemsColumn from '@/components/Shop/ShopHeroItemsColumn.vue';
import ShopSellLineColumn from '@/components/Shop/ShopSellLineColumn.vue';
import { useHeroStore } from '@/stores/hero';
import { useShopStore } from '@/stores/shop';
import { ApiSettings } from '@/utils/constants';
import authFetch from '@/utils/http-helper';
import { computed, onMounted, ref } from 'vue';

const shopStore = useShopStore();
const heroStore = useHeroStore();
const heroClient = new HeroClient(ApiSettings.BaseUrl, authFetch);

type SellSelectionState = {
    cellId: string;
    quantity: number;
};

type BuySelectionState = {
    selectionId: string;
    itemId: string;
    quantity: number;
};

const sellSelections = ref<SellSelectionState[]>([]);
const buySelections = ref<BuySelectionState[]>([]);
const buySelectionSeq = ref(0);
const heroMoneyAmount = ref(0);
const loadingHeroMoney = ref(false);
const heroMoneyError = ref<string | null>(null);

const loading = computed(() => shopStore.loading || shopStore.loadingHeroItems || shopStore.loadingTransaction || loadingHeroMoney.value);
const error = computed(() => heroMoneyError.value || shopStore.error);

const heroItemsSource = computed(() => {
    return shopStore.heroItems ?? [];
});

const heroItems = computed(() => {
    const selectedSellByCellId = new Map<string, number>();
    sellSelections.value.forEach(selection => {
        const prev = selectedSellByCellId.get(selection.cellId) ?? 0;
        selectedSellByCellId.set(selection.cellId, prev + selection.quantity);
    });

    return heroItemsSource.value
        .map(item => {
            const selectedQuantity = selectedSellByCellId.get(item.heroItemCellId) ?? 0;
            const remainingAmount = item.amount - selectedQuantity;
            return {
                ...item,
                amount: Math.max(0, remainingAmount),
            };
        })
        .filter(item => item.amount > 0);
});

const moneyIcon = computed(() => {
    return shopStore.moneyItem?.itemImage || '';
});

const selectedSellLineItems = computed(() => {
    const cellsById = new Map(heroItemsSource.value.map(i => [i.heroItemCellId, i]));

    return sellSelections.value
        .map(selection => {
            const cell = cellsById.get(selection.cellId);
            if (!cell) {
                return null;
            }

            const maxQuantity = Math.max(1, cell.amount);
            const quantity = Math.min(selection.quantity, maxQuantity);

            return {
                key: `sell-${selection.cellId}`,
                cellId: selection.cellId,
                imageSrc: cell.itemImage,
                name: cell.name,
                description: cell.description,
                quantity,
                totalPrice: cell.buyPrice * quantity,
                clickable: true,
            };
        })
        .filter((item): item is {
            key: string;
            cellId: string;
            imageSrc: string;
            name: string;
            description: string;
            quantity: number;
            totalPrice: number;
            clickable: boolean;
        } => !!item);
});

const selectedBuyLineItems = computed(() => {
    const shopById = new Map((shopStore.items ?? []).map(i => [i.itemId, i]));

    return buySelections.value
        .map(selection => {
            const item = shopById.get(selection.itemId);
            if (!item) {
                return null;
            }

            const quantity = Math.max(1, selection.quantity);
            return {
                selectionId: selection.selectionId,
                itemId: selection.itemId,
                imageSrc: item.itemImage,
                name: item.name,
                description: item.description,
                quantity,
                totalPrice: item.sellPrice * quantity,
                clickable: true,
            };
        })
        .filter((item): item is {
            selectionId: string;
            itemId: string;
            imageSrc: string;
            name: string;
            description: string;
            quantity: number;
            totalPrice: number;
            clickable: boolean;
        } => !!item);
});

const sellGoodsTotal = computed(() => {
    return selectedSellLineItems.value.reduce((sum, item) => sum + item.totalPrice, 0);
});

const buyGoodsTotal = computed(() => {
    return selectedBuyLineItems.value.reduce((sum, item) => sum + item.totalPrice, 0);
});

const heroMoneyTotal = computed(() => heroMoneyAmount.value);

const neededHeroBalanceMoney = computed(() => {
    return Math.max(0, buyGoodsTotal.value - sellGoodsTotal.value);
});

const heroBalanceMoneyUsed = computed(() => {
    return Math.min(heroMoneyTotal.value, neededHeroBalanceMoney.value);
});

const isHeroBalanceInsufficient = computed(() => {
    return heroBalanceMoneyUsed.value < neededHeroBalanceMoney.value;
});

const heroMoneyAfterBalance = computed(() => {
    return heroMoneyTotal.value - heroBalanceMoneyUsed.value;
});

const sellColumnOfferTotal = computed(() => {
    return sellGoodsTotal.value + heroBalanceMoneyUsed.value;
});

const shopBalanceMoneyUsed = computed(() => {
    return Math.max(0, sellGoodsTotal.value - buyGoodsTotal.value);
});

const buyColumnOfferTotal = computed(() => {
    return buyGoodsTotal.value + shopBalanceMoneyUsed.value;
});

const sellLineItems = computed(() => {
    const items: Array<{
        key: string;
        cellId: string;
        imageSrc: string;
        name: string;
        description: string;
        quantity: number;
        totalPrice: number;
        clickable: boolean;
    }> = [];

    if (heroBalanceMoneyUsed.value > 0 && shopStore.moneyItem) {
        items.push({
            key: 'sell-hero-money-balance',
            cellId: 'sell-hero-money-balance',
            imageSrc: shopStore.moneyItem.itemImage,
            name: shopStore.moneyItem.name,
            description: shopStore.moneyItem.description,
            quantity: heroBalanceMoneyUsed.value,
            totalPrice: heroBalanceMoneyUsed.value,
            clickable: false,
        });
    }

    return [...items, ...selectedSellLineItems.value];
});

const buyLineItems = computed(() => {
    const items: Array<{
        selectionId: string;
        itemId: string;
        imageSrc: string;
        name: string;
        description: string;
        quantity: number;
        totalPrice: number;
        clickable: boolean;
    }> = [];

    if (shopBalanceMoneyUsed.value > 0 && shopStore.moneyItem) {
        items.push({
            selectionId: 'buy-shop-money-balance',
            itemId: shopStore.moneyItem.id,
            imageSrc: shopStore.moneyItem.itemImage,
            name: shopStore.moneyItem.name,
            description: shopStore.moneyItem.description,
            quantity: shopBalanceMoneyUsed.value,
            totalPrice: shopBalanceMoneyUsed.value,
            clickable: false,
        });
    }

    return [...items, ...selectedBuyLineItems.value];
});

function addToSell(cellId: string) {
    const cell = heroItems.value.find(i => i.heroItemCellId === cellId);
    if (!cell) {
        return;
    }

    const sourceCell = heroItemsSource.value.find(i => i.heroItemCellId === cellId);
    if (!sourceCell) {
        return;
    }

    const existing = sellSelections.value.find(i => i.cellId === cellId);
    if (!existing) {
        sellSelections.value = [...sellSelections.value, { cellId, quantity: 1 }];
        return;
    }

    if (!cell.canBeFolded) {
        return;
    }

    if (existing.quantity >= sourceCell.amount) {
        return;
    }

    sellSelections.value = sellSelections.value.map(i => i.cellId === cellId
        ? { ...i, quantity: i.quantity + 1 }
        : i);
}

function removeFromSell(cellId: string) {
    const existing = sellSelections.value.find(i => i.cellId === cellId);
    if (!existing) {
        return;
    }

    if (existing.quantity > 1) {
        sellSelections.value = sellSelections.value.map(i => i.cellId === cellId
            ? { ...i, quantity: i.quantity - 1 }
            : i);
        return;
    }

    sellSelections.value = sellSelections.value.filter(i => i.cellId !== cellId);
}

function addToBuy(itemId: string) {
    const item = (shopStore.items ?? []).find(i => i.itemId === itemId);
    if (!item) {
        return;
    }

    if (item.canBeFolded) {
        const existing = buySelections.value.find(i => i.itemId === itemId);
        if (existing) {
            buySelections.value = buySelections.value.map(i => i.selectionId === existing.selectionId
                ? { ...i, quantity: i.quantity + 1 }
                : i);
            return;
        }
    }

    buySelectionSeq.value += 1;
    buySelections.value = [...buySelections.value, {
        selectionId: `buy-selection-${buySelectionSeq.value}`,
        itemId,
        quantity: 1,
    }];
}

function removeFromBuy(selectionId: string) {
    if (selectionId === 'buy-shop-money-balance') {
        return;
    }

    const index = buySelections.value.findIndex(i => i.selectionId === selectionId);
    if (index >= 0) {
        const selected = buySelections.value[index];
        if (selected.quantity > 1) {
            buySelections.value = buySelections.value.map(i => i.selectionId === selectionId
                ? { ...i, quantity: i.quantity - 1 }
                : i);
            return;
        }

        const next = [...buySelections.value];
        next.splice(index, 1);
        buySelections.value = next;
    }
}

function confirmDeal() {
    if (isHeroBalanceInsufficient.value) {
        return;
    }

    if (!confirm('Вы уверены? Совершить сделку?')) {
        return;
    }

    executeDeal();
}

async function executeDeal() {
    try {
        await shopStore.executeTransaction(
            sellSelections.value.map(selection => ({
                heroItemCellId: selection.cellId,
                quantity: selection.quantity,
            })),
            buySelections.value.map(selection => ({
                itemId: selection.itemId,
                quantity: selection.quantity,
            })),
        );

        sellSelections.value = [];
        buySelections.value = [];
        buySelectionSeq.value = 0;

        await Promise.all([
            shopStore.fetchItems(),
            shopStore.fetchHeroItems(),
            fetchHeroMoneyAmount(),
        ]);
    } catch (e: any) {
        alert(e?.message ?? 'Не удалось совершить сделку.');
    }
}

async function fetchHeroMoneyAmount() {
    heroMoneyError.value = null;

    if (!heroStore.heroes) {
        await heroStore.refreshHeroes();
    }

    if (!heroStore.selectedHeroId) {
        heroMoneyAmount.value = 0;
        heroMoneyError.value = 'Сначала выберите героя в казарме.';
        return;
    }

    loadingHeroMoney.value = true;

    try {
        const heroInfo = await heroClient.getInfo(heroStore.selectedHeroId);
        heroMoneyAmount.value = heroInfo.moneyAmount;
    } catch (e: any) {
        heroMoneyAmount.value = 0;
        heroMoneyError.value = e?.message ?? 'Не удалось загрузить баланс героя.';
    } finally {
        loadingHeroMoney.value = false;
    }
}

onMounted(async () => {
    await Promise.all([
        shopStore.fetchItems(),
        shopStore.fetchHeroItems(),
        fetchHeroMoneyAmount(),
    ]);
});
</script>

<style lang="scss">
.shop {
    &__content {
        width: 1280px;
        margin: 0 auto;
        max-width: 100%;
        padding: 24px 16px;
    }

    &__title {
        margin: 0 0 24px;
    }

    &__layout {
        display: grid;
        grid-template-columns: repeat(4, minmax(0, 1fr));
        gap: 16px;

        @media (max-width: 1280px) {
            grid-template-columns: repeat(2, minmax(0, 1fr));
        }

        @media (max-width: 760px) {
            grid-template-columns: 1fr;
        }
    }

    &__state-card {
        margin: 0;
        color: #666;
        background: white;
        border: 1px dashed #d8dee6;
        border-radius: 10px;
        margin-bottom: 24px;

        @media (max-width: 1280px) {
            grid-template-columns: repeat(2, minmax(0, 1fr));
        }

        @media (max-width: 760px) {
            grid-template-columns: 1fr;
        }
    }

    &__action {
        display: flex;
        justify-content: center;

        &_top {
            margin-bottom: 24px;
        }
    }

    &__deal-button {
        padding: 12px 32px;
        font-size: 16px;
        font-weight: 600;
        background: #BBCBF4;
        color: #111827;
        border: none;
        border-radius: 8px;
        cursor: pointer;
        transition: background 0.2s;

        &:hover:not(:disabled) {
            background: #a7b9ea;
        }

        &:disabled {
            background: #ccc;
            cursor: not-allowed;
            opacity: 0.6;
        }
    }

    &__state-card {
        margin: 0;
        color: #666;
        background: white;
        border: 1px dashed #d8dee6;
        border-radius: 10px;
        padding: 10px;

        &_error {
            color: #c0392b;
        }
    }
}
</style>
