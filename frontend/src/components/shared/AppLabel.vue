<template>
    <span>
        {{ message }}
    </span>
</template>

<script setup lang="ts">
const props = defineProps({
    message: {
        type: String,
    }
});
</script>