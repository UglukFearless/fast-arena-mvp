<template>
    <div class="fight-history-panel">
        <p v-for="roundResult in roundResults" v-html="roundResult" />
    </div>
</template>

<script setup lang="ts">
import { PropType } from 'vue';


const props = defineProps({
    roundResults: {
        required: true,
        type: Array as PropType<string[]>,
    }
});

</script>

<style lang="scss">

.fight-history-panel {
    padding: 16px 32px;
    font-size: 18px;
    justify-content: center;
    margin: 16px auto;
    border-radius: 8px;
    box-shadow: 2px 2px 12px rgba(185, 185, 185, 0.5);
    max-height: 200px;
    min-width: 350px;
    overflow: auto;

    p {
        margin: 12px auto;
        border-bottom: 1px solid #aaa;
        max-width: 100%;
    }
}

</style>