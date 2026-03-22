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
    </div>
</template>

<script lang="ts" setup>
import { CharInfo } from '@/stores/monster-fight';
import { PropType } from 'vue';


const props = defineProps({
    charInfo: {
        required: true,
        type: Object as PropType<CharInfo | null>,
    }
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
}
</style>