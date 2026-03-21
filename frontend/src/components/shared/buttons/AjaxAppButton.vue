<template>
    <AppButton @click="passClickEvent">
        <component :state="state" :message="message" :is="contentComponentName" />
    </AppButton>
</template>

<script setup lang="ts">
import { computed, PropType } from 'vue';
import { AjaxActionState } from '@/model/AjaxActionState';

import AppButton from '@/components/shared/buttons/AppButton.vue';
import AppLabel from '@/components/shared/AppLabel.vue';
import AppSpinner from '@/components/shared/AppSpinner.vue';

const props = defineProps({
    message: {
        type: String,
    },
    state: {
        type: String as PropType<AjaxActionState>,
    },
});

const emit = defineEmits(['click']);

function passClickEvent(event: Event) {
    emit('click', event);
}

const contentComponentName = computed(() => {
    switch(props.state) {
        case (AjaxActionState.READY):
            return AppLabel;
        case (AjaxActionState.BUSY):
            return AppSpinner;
        default:
            return AppLabel;
    }
});

</script>