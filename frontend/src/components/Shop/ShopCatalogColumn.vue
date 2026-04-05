<template>
    <ShopColumn
        :badge-amount="undefined"
        :badge-icon="moneyIcon"
        :badge-show-infinity-sign="true"
    >
        <ShopItemCard
            v-for="item in items"
            :key="item.itemId"
            :image-src="item.itemImage"
            :name="item.name"
            :description="item.description"
            :price-value="item.sellPrice"
            :price-icon="moneyIcon"
            :clickable="true"
            @action="$emit('add-to-buy', item.itemId)"
        />
    </ShopColumn>
</template>

<script setup lang="ts">
import { ShopItemDto } from '@/api/clients';
import ShopColumn from '@/components/Shop/ShopColumn.vue';
import ShopItemCard from '@/components/Shop/ShopItemCard.vue';
import { PropType } from 'vue';

defineProps({
    items: {
        type: Array as PropType<ShopItemDto[]>,
        required: true,
    },
    moneyIcon: {
        type: String,
        default: '',
    },
});

defineEmits<{ (e: 'add-to-buy', itemId: string): void }>();
</script>
