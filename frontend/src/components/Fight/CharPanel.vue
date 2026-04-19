<template>
    <div class="char-panel">
        <div class="char-panel__presentation">
            <img class="char-panel__portraite" :src="charInfo?.portraitUrl" />
            <span class="char-panel__name"> {{ charInfo?.name }}</span>
        </div>
        <div class="char-panel__conditions">
            <div class="char-panel__condition-item">
                <span class="char-panel__condition-title">MAX:</span>
                <span> {{ charInfo?.maxHealth }} </span>
                /
                <span> {{ charInfo?.maxAbility }} </span>
            </div>
            <div class="char-panel__condition-item">
                <span class="char-panel__condition-title">NOW:</span>
                <span> {{ charInfo?.health }} </span>
                /
                <span> {{ charInfo?.ability }} </span>
            </div>
        </div>
        <div v-if="activeEffects.length" class="char-panel__effects">
            <div
                v-for="effect in activeEffects"
                :key="effect.definitionId"
                class="char-panel__effect"
            >
                <img
                    class="char-panel__effect-image"
                    :src="effect.imageUrl"
                    :alt="`Эффект ${effect.type}`"
                />
                <span class="char-panel__effect-rounds">{{ effect.remainingRounds }}</span>
            </div>
        </div>
    </div>
</template>

<script lang="ts" setup>
import { ActiveEffectDto } from '@/api/clients';
import { CharInfo } from '@/stores/monster-fight';
import { PropType } from 'vue';


const props = defineProps({
    charInfo: {
        required: true,
        type: Object as PropType<CharInfo | null>,
    },
    activeEffects: {
        type: Array as PropType<ActiveEffectDto[]>,
        default: () => [],
    },
});

</script>

<style lang="scss">
.char-panel {
    flex: 1;
    padding: 16px 32px 32px;
    font-size: 18px;
    display: inline-flex;
    flex-direction: column;
    margin: 0 16px;
    border-radius: 8px;
    box-shadow: 2px 2px 12px rgba(185, 185, 185, 0.5);

    &__presentation {
        padding-bottom: 2px;
        display: flex;
        align-items: center;
        border-bottom: 1px solid #aaa;
    }

    &__portraite {
        width: 84px;
        border: 2px solid black;
        box-shadow: 0px 0px 5px 0px rgba(0, 0, 0, .2);
    }

    &__name {
        padding-left: 12px;
        font-size: 18px;
        font-weight: bold;
    }

    &__conditions {}

    &__condition-item {
        display: flex;
    }
    
    &__condition-title {
        flex-grow: 1;
        padding-right: 4px;
        min-width: 75px;
    }

    &__effects {
        margin-top: 8px;
        display: flex;
        flex-wrap: wrap;
        gap: 6px;
    }

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