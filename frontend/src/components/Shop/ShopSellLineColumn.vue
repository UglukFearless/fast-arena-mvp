<template>
    <ShopColumn
        :badge-amount="badgeAmount"
        :badge-icon="moneyIcon"
        :badge-is-alert="badgeIsAlert"
    >
        <ShopItemCard
            v-for="item in items"
            :key="item.key"
            :image-src="item.imageSrc"
            :name="item.name"
            :description="item.description"
            :meta-text="item.quantity > 1 ? `x${item.quantity}` : ''"
            :price-value="item.totalPrice"
            :price-icon="moneyIcon"
            :clickable="item.clickable ?? true"
            @action="$emit('remove-from-sell', item.cellId)"
        />
    </ShopColumn>
</template>

<script setup lang="ts">
import ShopColumn from '@/components/Shop/ShopColumn.vue';
import ShopItemCard from '@/components/Shop/ShopItemCard.vue';
import { PropType } from 'vue';

type SellLineItem = {
    key: string;
    cellId: string;
    imageSrc: string;
    name: string;
    description: string;
    quantity: number;
    totalPrice: number;
    clickable?: boolean;
};

defineProps({
    items: {
        type: Array as PropType<SellLineItem[]>,
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
    badgeIsAlert: {
        type: Boolean,
        default: false,
    },
});

defineEmits<{ (e: 'remove-from-sell', cellId: string): void }>();
</script>
