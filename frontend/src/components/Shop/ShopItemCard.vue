<template>
    <article
        class="shop-item-card"
        :class="{ 'shop-item-card_clickable': clickable }"
        @mouseenter="onMouseEnter"
        @mouseleave="onMouseLeave"
        @click="onCardClick"
    >
        <img class="shop-item-card__image" :src="imageSrc" :alt="name" />
        <div class="shop-item-card__info">
            <span class="shop-item-card__name">{{ name }}</span>
            <span v-if="metaText" class="shop-item-card__meta">{{ metaText }}</span>
            <span v-if="hasPrice" class="shop-item-card__price">
                <img
                    v-if="priceIcon"
                    class="shop-item-card__price-icon"
                    :src="priceIcon"
                    alt="Монеты"
                />
                <span v-else class="shop-item-card__price-icon shop-item-card__price-icon_fallback">¤</span>
                <span>{{ priceValue }}</span>
            </span>
        </div>

        <div v-if="showTooltip" class="shop-item-card__tooltip">
            {{ description }}
        </div>
    </article>
</template>

<script setup lang="ts">
import { computed, onUnmounted, ref } from 'vue';

const props = defineProps({
    imageSrc: {
        type: String,
        required: true,
    },
    name: {
        type: String,
        required: true,
    },
    description: {
        type: String,
        required: true,
    },
    metaText: {
        type: String,
        default: '',
    },
    priceValue: {
        type: Number,
        default: undefined,
    },
    priceIcon: {
        type: String,
        default: '',
    },
    clickable: {
        type: Boolean,
        default: false,
    },
});

const emit = defineEmits<{ (e: 'action'): void }>();

const hasPrice = computed(() => props.priceValue !== undefined && props.priceValue !== null);

const showTooltip = ref(false);
let tooltipTimer: ReturnType<typeof setTimeout> | null = null;

function onMouseEnter() {
    clearTooltipTimer();
    tooltipTimer = setTimeout(() => {
        showTooltip.value = true;
    }, 1000);
}

function onMouseLeave() {
    clearTooltipTimer();
    showTooltip.value = false;
}

function clearTooltipTimer() {
    if (tooltipTimer) {
        clearTimeout(tooltipTimer);
        tooltipTimer = null;
    }
}

function onCardClick() {
    if (props.clickable) {
        emit('action');
    }
}

onUnmounted(() => {
    clearTooltipTimer();
});
</script>

<style lang="scss" scoped>
.shop-item-card {
    position: relative;
    display: flex;
    align-items: flex-start;
    gap: 10px;
    padding: 10px;
    background: #fff;
    border-radius: 8px;
    box-shadow: 0 1px 4px rgba(0, 0, 0, 0.08);

    &_clickable {
        cursor: pointer;
    }

    &__image {
        width: 64px;
        height: 64px;
        object-fit: contain;
        flex-shrink: 0;
    }

    &__info {
        display: flex;
        flex-direction: column;
        align-items: flex-start;
        gap: 4px;
        text-align: left;
    }

    &__name {
        font-weight: 600;
    }

    &__meta {
        font-size: 12px;
        color: #2b3a4f;
    }

    &__price {
        display: inline-flex;
        align-items: center;
        gap: 6px;
        font-size: 1em;
        font-weight: 700;
        color: #e67e22;
    }

    &__price-icon {
        width: 18px;
        height: 18px;
        object-fit: contain;

        &_fallback {
            display: inline-flex;
            justify-content: center;
            align-items: center;
            font-size: 14px;
        }
    }

    &__tooltip {
        position: absolute;
        left: 8px;
        right: 8px;
        bottom: calc(100% + 8px);
        z-index: 5;
        padding: 8px 10px;
        border-radius: 8px;
        background: #1b2736;
        color: #f3f7fc;
        font-size: 12px;
        line-height: 1.35;
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
    }
}
</style>
