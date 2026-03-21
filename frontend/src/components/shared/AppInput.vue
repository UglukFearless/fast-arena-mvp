<template>
    <div class="app-input__wrapper">
        <input
            class="app-input"
            @input="updateValue($event)"
            :placeholder="placeholder"
            :value="modelValue"
            v-bind="$attrs"
            />
        <p v-if="$attrs['invalid']" class="app-input__error-message">
            {{ errorMessage }}
        </p>
    </div>
</template>

<script setup lang="ts">

defineOptions({
  inheritAttrs: false
});

const emit = defineEmits(['update:modelValue']);

function updateValue(e: Event) {
    emit('update:modelValue', (e.target as HTMLInputElement).value);
}

defineProps({
    placeholder: {
        type: String,
    },
    modelValue: {
        type: String,
    },
    errorMessage: {
        type: String,
    },
});

</script>

<style lang="scss">
.app-input {
    width: 100%;
    font-size: 15px;
    padding: 16px;
    border-radius: 4px;
    border: 1px solid #E5ECFF;
    background: #FFF;

    &[readonly] {
        opacity: 0.5;
    }

    &[invalid=true] {
        border: 1px solid rgb(163, 12, 12);
    }
}

.app-input__wrapper {
    width: 100%;
    position: relative;
}

.app-input__error-message {
    font-size: 10px;
    color: rgb(163, 12, 12);
    padding: 0 16px;
    margin: 0;
    line-height: 10px;
    position: absolute;
    top: 100%;
}
</style>