<template>
    <ShopColumn
        :badge-amount="badgeAmount"
        :badge-icon="moneyIcon"
    >
        <ShopItemCard
            v-for="item in items"
            :key="item.heroItemCellId"
            :image-src="item.itemImage"
            :name="item.name"
            :description="item.description"
            :meta-text="item.canBeFolded ? `x${item.amount}` : ''"
            :price-value="item.buyPrice"
            :price-icon="moneyIcon"
            :clickable="true"
            @action="$emit('add-to-sell', item.heroItemCellId)"
        />
    </ShopColumn>
</template>

<script setup lang="ts">
import { ShopHeroItemDto } from '@/api/clients';
import ShopColumn from '@/components/Shop/ShopColumn.vue';
import ShopItemCard from '@/components/Shop/ShopItemCard.vue';
import { PropType } from 'vue';

defineProps({
    items: {
        type: Array as PropType<ShopHeroItemDto[]>,
        required: true,
    },
    badgeAmount: {
        type: Number,
        required: true,
    },
    moneyIcon: {
        type: String,
        default: '',
    },
});

defineEmits<{ (e: 'add-to-sell', cellId: string): void }>(); 
</script>
