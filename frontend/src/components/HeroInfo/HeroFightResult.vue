<template>
    <article class="fight-result">
        <img
            class="fight-result__portrait"
            :src="result.monster.portraitUrl"
            :alt="result.monster.name"
        />

        <div class="fight-result__content">
            <div class="fight-result__head">
                <div>
                    <h4 class="fight-result__name">{{ result.monster.name }}</h4>
                    <p class="fight-result__meta">Уровень {{ result.monster.level }}</p>
                </div>
                <span class="fight-result__badge" :class="badgeClass">{{ label }}</span>
            </div>

            <div class="fight-result__stats">
                <span>HP: {{ result.monster.maxHealth }}</span>
                <span>Мастерство: {{ result.monster.maxAbility }}</span>
            </div>
        </div>
    </article>
</template>

<script setup lang="ts">
import { MonsterFightResultDto, MonsterFightResultType } from '@/api/clients';
import { computed, PropType } from 'vue';

const props = defineProps({
    result: {
        required: true,
        type: Object as PropType<MonsterFightResultDto>,
    },
});

const label = computed(() => {
    switch (props.result.type) {
        case MonsterFightResultType.VICTORY: return 'Победа';
        case MonsterFightResultType.DEFEAT:  return 'Поражение';
        case MonsterFightResultType.DRAW:    return 'Ничья';
        default:                             return 'Неизвестно';
    }
});

const badgeClass = computed(() => {
    switch (props.result.type) {
        case MonsterFightResultType.VICTORY: return 'fight-result__badge_victory';
        case MonsterFightResultType.DEFEAT:  return 'fight-result__badge_defeat';
        case MonsterFightResultType.DRAW:    return 'fight-result__badge_draw';
        default:                             return '';
    }
});
</script>

<style lang="scss" scoped>
.fight-result {
    display: flex;
    gap: 16px;
    background: white;
    border-radius: 12px;
    box-shadow: 0px 0px 8px rgba(155, 155, 155, 0.4);
    padding: 20px;

    @media (max-width: 640px) {
        flex-direction: column;
    }

    &__portrait {
        width: 96px;
        height: 96px;
        object-fit: cover;
        border-radius: 10px;
        border: 1px solid #111;
        flex-shrink: 0;
    }

    &__content {
        flex: 1;
        min-width: 0;
    }

    &__head {
        display: flex;
        justify-content: space-between;
        gap: 12px;
        align-items: flex-start;
        margin-bottom: 14px;

        @media (max-width: 640px) {
            flex-direction: column;
        }
    }

    &__name {
        margin: 0 0 4px;
        font-size: 24px;
    }

    &__meta {
        margin: 0;
        color: #5e5e5e;
    }

    &__badge {
        align-self: flex-start;
        padding: 6px 12px;
        border-radius: 999px;
        font-weight: 700;
        white-space: nowrap;

        &_victory {
            background: #d9efdc;
            color: #1f6b2c;
        }

        &_defeat {
            background: #f3dada;
            color: #8b2525;
        }

        &_draw {
            background: #ececec;
            color: #464646;
        }
    }

    &__stats {
        display: flex;
        flex-wrap: wrap;
        gap: 10px 18px;
        color: #2f2f2f;
    }
}
</style>
