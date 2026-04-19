<template>
    <div class="avaliable-hero-actions">
        <AjaxAppButton 
            class="avaliable-hero-actions__action"
            v-for="action in avaliableActions"
            :key="action.code"
            :message="action.title"
            @click="onActionClick(action.code)"
            :state="buttonsState"
        />

        <div v-if="isItemPickerVisible" class="avaliable-hero-actions__item-picker">
            <button
                v-for="pocketItem in pocketItems"
                :key="pocketItem.id"
                class="avaliable-hero-actions__item-button"
                type="button"
                @click="usePocketItem(pocketItem.id)"
            >
                <img
                    class="avaliable-hero-actions__item-image"
                    :src="pocketItem.item.itemImage"
                    :alt="pocketItem.item.name"
                />
                <span>{{ pocketItem.item.name }}</span>
                <span v-if="pocketItem.item.canBeFolded">x{{ pocketItem.amount }}</span>
            </button>
        </div>
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
import { HeroActVariant, HeroItemCellDto, MonsterFightDoActionDataDto } from '@/api/clients';
import { useRouter } from 'vue-router';

const monsterFightStore = useMonsterFight();
const router = useRouter();

const avaliableActions = computed(() => {
    return monsterFightStore.currentState?.actVariants.map(convertActionCodeToModel) ?? [];
});

const buttonsState = ref(AjaxActionState.READY);
const isItemPickerVisible = ref(false);

const pocketItems = computed((): HeroItemCellDto[] => {
    return monsterFightStore.currentState?.pocketItems ?? [];
});

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
        case 2:
            return {
                code: code,
                title: 'Использовать предмет',
            };
        default:
            throw Error('Неизвестный код действия!');
    }
}

const doAction = async function(code: HeroActVariant, actionData?: MonsterFightDoActionDataDto) {
    buttonsState.value = AjaxActionState.BUSY;
    await monsterFightStore.doAction({
        actVariant: code,
        actionData,
    }, router);
    buttonsState.value = AjaxActionState.READY;
}

const onActionClick = async function(code: HeroActVariant) {
    if (code === HeroActVariant.USE_ITEM) {
        isItemPickerVisible.value = !isItemPickerVisible.value;
        return;
    }

    isItemPickerVisible.value = false;
    await doAction(code);
}

const usePocketItem = async function(heroItemCellId: string) {
    isItemPickerVisible.value = false;
    await doAction(HeroActVariant.USE_ITEM, {
        usedPocketItemCellId: heroItemCellId,
    });
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

    &__item-picker {
        width: 100%;
        display: flex;
        flex-wrap: wrap;
        justify-content: center;
        gap: 8px;
    }

    &__item-button {
        border: 1px solid #1c2d42;
        border-radius: 6px;
        background: #dce4ef;
        padding: 6px 10px;
        display: inline-flex;
        align-items: center;
        gap: 6px;
        cursor: pointer;
        font-weight: 600;
    }

    &__item-image {
        width: 20px;
        height: 20px;
        object-fit: contain;
    }
}

</style>