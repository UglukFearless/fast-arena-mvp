<template>
    <div class="sex-input">
        <div class="sex-input__radio-group">
            <input 
                id="sexMale" 
                required 
                @input="updateValue" 
                name="sex" 
                type="radio" 
                value="0" />
            <label for="sexMale">Мужской</label>
        </div>
        <div class="sex-input__radio-group">
            <input 
                id="sexFemale" 
                required 
                @input="updateValue" 
                name="sex" 
                type="radio" 
                value="1" />
            <label for="sexFemale">Женский</label>
        </div>
        <div class="sex-input__radio-group">
            <input 
                id="sexOther" 
                required 
                @input="updateValue" 
                name="sex" 
                type="radio" 
                value="2" />
            <label for="sexOther">Другое</label>
        </div>
    </div>
</template>

<script setup lang="ts">
import { HeroSex } from '@/api/clients';
import { PropType } from 'vue';

defineOptions({
  inheritAttrs: false
});

defineProps({
    modelValue: {
        type: Number as PropType<HeroSex>,
    },
});

const emit = defineEmits(['update:modelValue']);

function updateValue(e: Event) {
    emit('update:modelValue', Number.parseInt((e.target as HTMLInputElement).value));
}

</script>

<style lang="scss">
.sex-input {

    flex: 1;
    font-size: 16px;
    padding: 4px 16px;
    max-width: 50%;
    display: flex;
    flex-direction: column;
    gap: 4px;

    &__radio-group {
        display: flex;
        align-items: center;
        label {
            margin-left: 8px;
            cursor: pointer;
        }

        input {
            cursor: pointer;
        }
    }
}
</style>