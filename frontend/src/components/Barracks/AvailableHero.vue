<template>
    <div class="available-hero">
        <div class="available-hero__info">
            <img class="available-hero__portrait" :src="hero.portraitUrl" />
            <div class="available-hero__props">
                <div class="available-hero__props-item">
                    <span class="available-hero__props-name">Имя:</span>
                    <span class="available-hero__props-value">{{ hero.name }}</span>
                </div>
                <div class="available-hero__props-item">
                    <span class="available-hero__props-name">Уровень:</span>
                    <span class="available-hero__props-value">{{ hero.level }}</span>
                </div>
                <div class="available-hero__props-item">
                    <span class="available-hero__props-progress-wrapper">
                        <span 
                            class="available-hero__props-progress"
                            :style="{ 'width': progress + '%' }"
                            >
                        </span>
                    </span>
                </div>
                <div class="available-hero__props-item">
                    <span class="available-hero__props-name">Здоровье:</span>
                    <span class="available-hero__props-value">{{ hero.maxHealth }}</span>
                </div>
                <div class="available-hero__props-item">
                    <span class="available-hero__props-name">Золото:</span>
                    <span class="available-hero__props-value">{{ heroGold }}</span>
                </div>
                <div class="available-hero__props-item">
                    <span class="available-hero__props-name">Побед:</span>
                    <span class="available-hero__props-value">{{ wins }}</span>
                </div>
            </div>
        </div>
        <div class="available-hero__actions">
            <AppButton @click="selectHero">
                Выбрать
            </AppButton>
            <AppButton @click="goInfo(hero.id)">
                Инфо
            </AppButton>
        </div>
    </div>
</template>

<script setup lang="ts">
import { HeroDto, HeroLevelProgressDto, ItemType, MonsterFightResultType } from "@/api/clients";
import { computed, PropType } from "vue";

import AppButton from '../shared/buttons/AppButton.vue';
import { useHeroStore } from "@/stores/hero";

const props = defineProps({
    hero: {
        required: true,
        type: Object as PropType<HeroDto>,
    }
});

const heroStore = useHeroStore();

async function selectHero() {
    await heroStore.select(props.hero.id as string);
}

function goInfo(heroId: string) {
    console.log('Go to hero info with id', heroId);
}

const progress = computed(() => {
    return (props.hero.experience 
        - (props.hero.levelProgressInfo as HeroLevelProgressDto).previousAmound )*100
        /
        ((props.hero.levelProgressInfo as HeroLevelProgressDto).nextAmound 
            - (props.hero.levelProgressInfo as HeroLevelProgressDto).previousAmound);
});

const wins = computed(() => {
    if (!props.hero.results)
        return 0;

    return props.hero.results.filter(r => r.type === MonsterFightResultType.VICTORY).length;
});

const heroGold = computed(() => {
    if (!props.hero.items)
        return 'Неизвестно';

    const moneyItemCell = props.hero.items.find(ic => ic.item?.type === ItemType.MONEY);

    if (!moneyItemCell)
        return 0;

    return moneyItemCell.amount;
});

</script>

<style lang="scss">
.available-hero {
    background: white;
    margin: 12px;
    padding: 12px;
    box-shadow: 0px 0px 8px rgba(155, 155, 155, 0.4);
    border-radius: 8px;
    max-width: 100vw;
    width: 290px;
    display: inline-flex;
    justify-content: center;
    align-items: center;
    flex-direction: column;

    &:hover {
        box-shadow: 0px 0px 2px rgba(155, 155, 155, 0.6);
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