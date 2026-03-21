<template>
    <div class="input-group">
        <span v-if="!!name" class="input-group__input-name">
            {{ name }}:
        </span>
        <slot />
    </div>
</template>

<script setup lang="ts">

defineProps({
    name: {
        type: String,
        required: false,
    },
})
</script>

<style lang="scss">
.input-group {
    display: flex;
    padding: 4px 0;
    align-items: center;

    &__input-name {
        flex: 1;
        text-align: right;
        padding: 0 12px;
        max-width: 50%;
    }
}
</style>
