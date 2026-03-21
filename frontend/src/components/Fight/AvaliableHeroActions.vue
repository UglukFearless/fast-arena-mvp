<template>
    <div class="avaliable-hero-actions">
        <AjaxAppButton 
            class="avaliable-hero-actions__action"
            v-for="action in avaliableActions"
            :message="action.title"
            @click="doAction(action.code)"
            :state="buttonsState"
        />
    </div>
</template>

<script setup lang="ts">
interface HeroActionModel {
    code: HeroActVariant;
    title: string;
}


import { useMonsterFight } from '@/stores/monster-fight';
import { computed, ref } from 'vue';
import AjaxAppButton from '../shared/buttons/AjaxAppButton.vue';
import { AjaxActionState } from '@/model/AjaxActionState';
import { HeroActVariant } from '@/api/clients';
import { useRouter } from 'vue-router';

const monsterFightStore = useMonsterFight();
const router = useRouter();

const avaliableActions = computed(() => {
    return monsterFightStore.currentState?.actVariants.map(convertActionCodeToModel) ?? [];
});

const buttonsState = ref(AjaxActionState.READY);

const convertActionCodeToModel = function(code: HeroActVariant): HeroActionModel {
    switch(code) {
        case 0:
            return {
                code: code,
                title: 'Атаковать',
            };
        case 1:
            return {
                code: code,
                title: 'Принять',
            };
        default:
            throw Error('Неизвестный код действия!');
    }
}

const doAction = async function(code: HeroActVariant) {
    buttonsState.value = AjaxActionState.BUSY;
    await monsterFightStore.doAction(code, router);
    buttonsState.value = AjaxActionState.READY;
}

</script>

<style lang="scss">

.avaliable-hero-actions {
    display: flex;
    gap: 12px;
    max-width: 100%;
    flex-wrap: wrap;
    margin: 6px auto;

    &__action {
        font-size: 16px;
        font-weight: bold;
        margin: 4px auto;
        padding: 8px 32px;
        border: 1px solid black;
    }
}

</style>