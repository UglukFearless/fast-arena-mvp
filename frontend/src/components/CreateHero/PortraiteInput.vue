<template>
    <button class="portrait-input__button" @click="openPortraitSelector($event)">
        <img class="portrait-input__image" :src="heroPortraitUrl" />
    </button>
    
    <div v-if="modalShow" class="portrait-input__modal-section">
        <div class="portrait-input__modal-main">
            <span class="portrait-input__modal-close" @click="closeModal">
                &#10006;
            </span>
            <div class="portrait-input__modal-content">
                <img v-for="portrait in allPortraits" 
                    class="portrait-input__modal-image" 
                    @click="updateValue(portrait.id as string)" 
                    :src="portrait.url" />
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { usePortraitStore } from '@/stores/portrait';
import { computed, onMounted, ref, watch } from 'vue';

const portraitStore = usePortraitStore();
const allPortraits = computed(() => portraitStore.heroPortraits);

const props = defineProps({
    modelValue: {
        type: String,
    },
});

const emit = defineEmits(['update:modelValue']);

const modalShow = ref(false);

const heroPortraitUrlRaw = computed(() => {
    return allPortraits.value?.find(p => p.id === props.modelValue)?.url as string;
});

const heroPortraitUrl = computed(() => {
    return props.modelValue ? heroPortraitUrlRaw.value : require('@/assets/gui/plus.svg');
});

onMounted(async () => {
    await portraitStore.init();
});

function openPortraitSelector(e: Event) {
    e.preventDefault();
    modalShow.value = true;
}

function closeModal() {
    modalShow.value = false;
}

function updateValue(portraitId: string) {
    emit('update:modelValue', portraitId);
    closeModal();
}
</script>

<style lang="scss">
.portrait-input {

    &__button {
        flex: 1;
        cursor: pointer;
        max-width: 50%;
    }

    &__image {
        max-width: 64px;
    }

    &__modal-section {
        position: absolute;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        z-index: 9999;
        background-color: rgba(0, 0, 0, 0.3);
        backdrop-filter: blur(3px);
        display: flex;
        align-items: center;
        justify-content: center;
    }

    &__modal-main {
        background: white;
        padding: 22px;
        box-shadow: 0px 0px 8px rgba(155, 155, 155, 0.4);
        border-radius: 8px;
        max-width: 480px;
        min-height: 128px;
        width: 100vw;
        display: flex;
        justify-content: center;
        flex-direction: column;
        font-size: 18px;
        position: relative;
    }

    &__modal-close {
        font-size: 22px;
        position: absolute;
        top: 0px;
        right: 4px;
        cursor: pointer;
    }

    &__modal-content {
        display: flex;
        flex-wrap: wrap;
    }

    &__modal-image {
        width: 64px;
        cursor: pointer;
        margin: 4px;
    }
}
</style>