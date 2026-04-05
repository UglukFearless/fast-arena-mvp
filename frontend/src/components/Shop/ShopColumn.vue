<template>
    <section class="shop-column">
        <ShopMoneyBadge
            v-if="badgeAmount !== undefined || badgeShowInfinitySign"
            :icon="badgeIcon"
            :amount="badgeAmount"
            :is-alert="badgeIsAlert"
            :show-infinity-sign="badgeShowInfinitySign"
        />
        <div class="shop-column__body">
            <slot />
        </div>
    </section>
</template>

<script setup lang="ts">
import ShopMoneyBadge from '@/components/Shop/ShopMoneyBadge.vue';

defineProps({
    badgeAmount: {
        type: Number,
        default: undefined,
    },
    badgeIcon: {
        type: String,
        default: '',
    },
    badgeIsAlert: {
        type: Boolean,
        default: false,
    },
    badgeShowInfinitySign: {
        type: Boolean,
        default: false,
    },
});
</script>

<style lang="scss" scoped>
.shop-column {
    display: flex;
    flex-direction: column;
    gap: 10px;
    background: #f6f9fd;
    border: 1px solid #d8dee6;
    border-radius: 12px;
    padding: 12px;
    min-height: 420px;

    &__body {
        display: flex;
        flex-direction: column;
        gap: 10px;
    }
}
</style>
