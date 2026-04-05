<template>
    <div class="shop-money-badge" :class="{ 'shop-money-badge_alert': isAlert }">
        <img
            v-if="icon"
            class="shop-money-badge__icon"
            :src="icon"
            alt="Монеты"
        />
        <span v-else class="shop-money-badge__icon shop-money-badge__icon_fallback">¤</span>
        <span v-if="!showInfinitySign && amount !== undefined" class="shop-money-badge__value">{{ amount }}</span>
        <span v-if="showInfinitySign" class="shop-money-badge__infinity">∞</span>
    </div>
</template>

<script setup lang="ts">
const props = defineProps({
    icon: {
        type: String,
        default: '',
    },
    amount: {
        type: Number,
        default: undefined,
    },
    isAlert: {
        type: Boolean,
        default: false,
    },
    showInfinitySign: {
        type: Boolean,
        default: false,
    },
});
</script>

<style lang="scss" scoped>
.shop-money-badge {
    width: 100%;
    box-sizing: border-box;
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 8px;
    border: 1px solid #d8dee6;
    background: white;
    border-radius: 10px;
    padding: 8px 12px;
    box-shadow: 0px 0px 6px rgba(155, 155, 155, 0.35);

    &_alert {
        border-color: #c0392b;
        background: #fff0ee;
        color: #8e2a1f;
    }

    &__icon {
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

    &__value {
        font-size: 18px;
        font-weight: 700;
    }

    &__infinity {
        font-size: 20px;
        font-weight: 700;
        line-height: 1;
    }
}
</style>
