<template>
    <ShopColumn
        :badge-amount="badgeAmount"
        :badge-icon="moneyIcon"
    >
        <ShopItemCard
            v-for="item in items"
            :key="item.selectionId"
            :image-src="item.imageSrc"
            :name="item.name"
            :description="item.description"
            :meta-text="item.quantity > 1 ? `x${item.quantity}` : ''"
            :price-value="item.totalPrice"
            :price-icon="moneyIcon"
            :clickable="item.clickable ?? true"
            @action="$emit('remove-from-buy', item.selectionId)"
        />
    </ShopColumn>
</template>

<script setup lang="ts">
import ShopColumn from '@/components/Shop/ShopColumn.vue';
import ShopItemCard from '@/components/Shop/ShopItemCard.vue';
import { PropType } from 'vue';

type BuyLineItem = {
    selectionId: string;
    itemId: string;
    imageSrc: string;
    name: string;
    description: string;
    quantity: number;
    totalPrice: number;
    clickable?: boolean;
};

defineProps({
    items: {
        type: Array as PropType<BuyLineItem[]>,
        required: true,
    },
    moneyIcon: {
        type: String,
        default: '',
    },
    badgeAmount: {
        type: Number,
        required: true,
    },
});

defineEmits<{ (e: 'remove-from-buy', selectionId: string): void }>();
</script>
