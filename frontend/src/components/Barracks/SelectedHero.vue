<template>
    <div v-if="selectedHero" class="selected-hero__container">
        <img 
            @click="removeSelectedHero" 
            class="selected-hero__cancel" 
            :class="{ 'selected-hero__cancel_unvisible': !mouseOver }"
            src="@/assets/gui/cancel.svg" />
        <div class="selected-hero__info">
            <img class="selected-hero__portrait" :src="selectedHero.portraitUrl" />
            <div class="selected-hero__props">
                <div class="selected-hero__props-item">
                    <span class="selected-hero__props-name">Имя:</span>
                    <span class="selected-hero__props-value">{{ selectedHero.name }}</span>
                </div>
                <div class="selected-hero__props-item">
                    <span class="selected-hero__props-name">Уровень:</span>
                    <span class="selected-hero__props-value">{{ selectedHero.level }}</span>
                </div>
                <div class="selected-hero__props-item">
                    <span class="selected-hero__props-progress-wrapper">
                        <span 
                            class="selected-hero__props-progress"
                            :style="{ 'width': progress + '%' }"
                            >
                        </span>
                    </span>
                </div>
                <div class="selected-hero__props-item">
                    <span class="selected-hero__props-name">Здоровье:</span>
                    <span class="selected-hero__props-value">{{ selectedHero.maxHealth }}</span>
                </div>
                <div class="selected-hero__props-item">
                    <span class="selected-hero__props-name">Золото:</span>
                    <span class="selected-hero__props-value">{{ heroGold }}</span>
                </div>
                <div class="selected-hero__props-item">
                    <span class="selected-hero__props-name">Побед:</span>
                    <span class="selected-hero__props-value">{{ wins }}</span>
                </div>
            </div>
        </div>
        <div class="selected-hero__actions">
            <AppButton @click="goInfo(selectedHero.id)">
                Инфо
            </AppButton>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useRouter } from 'vue-router';
import AppButton from '../shared/buttons/AppButton.vue';
import { useHeroStore } from '@/stores/hero';
import { HeroLevelProgressDto, ItemType, MonsterFightResultType } from '@/api/clients';

defineProps({
    mouseOver: {
        type: Boolean,
    }
});

const heroStore = useHeroStore();
const router = useRouter();
const selectedHero = computed(() => heroStore.selectedHero);

async function removeSelectedHero() {
    await heroStore.unselect();
}

function goInfo(heroId: string) {
    router.push({
        name: 'hero-info',
        params: {
            id: heroId,
        }
    });
}

const progress = computed(() => {
    if (!selectedHero.value)
        return 0;
    return (selectedHero.value!.experience 
        - (selectedHero.value!.levelProgressInfo as HeroLevelProgressDto).previousAmound )*100
        /
        ((selectedHero.value!.levelProgressInfo as HeroLevelProgressDto).nextAmound 
            - (selectedHero.value!.levelProgressInfo as HeroLevelProgressDto).previousAmound);
});

const wins = computed(() => {
    if (!selectedHero.value?.results)
        return 0;

    return selectedHero.value.results.filter(r => r.type === MonsterFightResultType.VICTORY).length;
});

const heroGold = computed(() => {
    if (!selectedHero.value?.items)
        return 'Неизвестно';

    const moneyItemCell = selectedHero.value.items.find(ic => ic.item?.type === ItemType.MONEY);

    if (!moneyItemCell)
        return 0;

    return moneyItemCell.amount;
});
</script>

<style lang="scss">
.selected-hero {

    &__container {
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        max-width: 100%;
        min-height: 120px;
        position: relative;
    }

    &__cancel {
        width: 32px;
        position: absolute;
        top: 18px;
        left: 0px;
        cursor: pointer;

        &_unvisible {
            display: none;
        }
    }

    &__info {
        display: flex;
        justify-content: center;
        align-items: center;
        max-width: 100%;
    }

    &__portrait {
        width: 84px;
        margin: 0 12px;
        border: 1px solid black;
        box-shadow: 0px 0px 5px 0px rgba(0, 0, 0, .2);
    }

    &__props {
        display: flex;
        max-width: 160px;
        flex-direction: column;
        margin: 0 12px;
        flex-wrap: wrap;

        &-item {
            display: flex;
            align-items: center;
            font-size: 18px;
        }

        &-name {
            font-weight: bold;
            flex: 1;
            padding: 2px 8px 2px 0;
        }

        &-value {
            flex: 2;
        }

        &-progress-wrapper {
            width: 100%;
            border: 2px solid rgb(14, 10, 62);
            height: 8px;
            border-radius: 4px;
            position: relative;
        }

        &-progress {
            width: 50%;
            background-color: rgb(14, 10, 62);
            height: 100%;
            display: inline-block;
            position: absolute;
        }
    }

    &__actions {
        width: 100%;
        padding-top: 10px;
        border-top: 1px solid #aaa;
        display: flex;
        justify-content: space-evenly;
        align-items: center;
        min-height: 50px;
        flex-wrap: wrap;
    }
}
</style>