import { ItemDto, ShopBuyRequestItemModel, ShopHeroItemDto, ShopItemDto, ShopSellRequestItemModel, ShopTransactionModel } from "@/api/clients";
import { ApiSettings } from "@/utils/constants";
import authFetch from "@/utils/http-helper";
import { defineStore } from "pinia";

type ShopCatalogDto = {
    moneyItem: ItemDto;
    items: ShopItemDto[];
};

export const useShopStore = defineStore('shop', {
    state: () => ({
        items: null as ShopItemDto[] | null,
        heroItems: null as ShopHeroItemDto[] | null,
        moneyItem: null as ItemDto | null,
        loading: false,
        loadingHeroItems: false,
        loadingTransaction: false,
        error: null as string | null,
    }),
    actions: {
        async fetchItems() {
            this.loading = true;
            this.error = null;
            try {
                const response = await authFetch.fetch(`${ApiSettings.BaseUrl}/api/shop/catalog`, {
                    method: "GET",
                    headers: {
                        "Accept": "application/json",
                    },
                });

                if (!response.ok) {
                    const errorText = await response.text();
                    throw new Error(errorText || "Не удалось загрузить товары магазина.");
                }

                const data = await response.json() as ShopCatalogDto;
                this.items = data.items ?? [];
                this.moneyItem = data.moneyItem ?? null;
            } catch (e: any) {
                this.error = e?.message ?? 'Не удалось загрузить товары магазина.';
            } finally {
                this.loading = false;
            }
        },
        async fetchHeroItems() {
            this.loadingHeroItems = true;
            this.error = null;
            try {
                const response = await authFetch.fetch(`${ApiSettings.BaseUrl}/api/shop/hero-items`, {
                    method: "GET",
                    headers: {
                        "Accept": "application/json",
                    },
                });

                if (!response.ok) {
                    throw new Error("Не удалось загрузить предметы героя.");
                }

                this.heroItems = await response.json() as ShopHeroItemDto[];
            } catch (e: any) {
                this.error = e?.message ?? 'Не удалось загрузить предметы героя.';
            } finally {
                this.loadingHeroItems = false;
            }
        },
        async executeTransaction(sellItems: ShopSellRequestItemModel[], buyItems: ShopBuyRequestItemModel[]) {
            this.loadingTransaction = true;
            this.error = null;

            try {
                const model: ShopTransactionModel = {
                    sellItems,
                    buyItems,
                };

                const response = await authFetch.fetch(`${ApiSettings.BaseUrl}/api/shop/transaction`, {
                    method: "POST",
                    headers: {
                        "Accept": "application/json",
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify(model),
                });

                if (!response.ok) {
                    const errorText = await response.text();
                    throw new Error(errorText || "Не удалось совершить сделку.");
                }
            } catch (e: any) {
                this.error = e?.message ?? 'Не удалось совершить сделку.';
                throw e;
            } finally {
                this.loadingTransaction = false;
            }
        },
    },
});
