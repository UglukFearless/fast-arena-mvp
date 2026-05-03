<template>
    <div v-if="activeEffects.length" class="active-effects-panel">
        <div
            v-for="effect in activeEffects"
            :key="effect.definitionId"
            class="active-effects-panel__effect"
        >
            <img
                class="active-effects-panel__effect-image"
                :src="effect.imageUrl"
                :alt="`Эффект ${effect.type}`"
            />
            <span class="active-effects-panel__effect-rounds">{{ effect.remainingRounds }}</span>
        </div>
    </div>
</template>

<script lang="ts" setup>
import { ActiveEffectDto } from '@/api/clients';
import { PropType } from 'vue';

defineProps({
    activeEffects: {
        type: Array as PropType<ActiveEffectDto[]>,
        default: () => [],
    },
});
</script>

<style lang="scss">
.active-effects-panel {
    margin-top: 8px;
    display: flex;
    flex-wrap: wrap;
    gap: 6px;

    &__effect {
        position: relative;
        width: 28px;
        height: 28px;
        border-radius: 4px;
        border: 1px solid #1c2d42;
        background: #dce4ef;
        display: inline-flex;
        align-items: center;
        justify-content: center;
        overflow: hidden;
    }

    &__effect-image {
        width: 100%;
        height: 100%;
        object-fit: contain;
    }

    &__effect-rounds {
        position: absolute;
        right: 0;
        bottom: 0;
        background: rgba(12, 23, 38, 0.85);
        color: #fff;
        font-size: 10px;
        line-height: 1;
        padding: 2px;
    }
}
</style>
