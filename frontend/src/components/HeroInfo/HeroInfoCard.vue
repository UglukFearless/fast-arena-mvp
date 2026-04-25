<template>
    <div class="hero-card">
        <img class="hero-card__portrait" :src="hero.portraitUrl" :alt="hero.name" />

        <div class="hero-card__body">
            <div class="hero-card__head">
                <h2 class="hero-card__name">{{ hero.name }}</h2>
                <span class="hero-card__status" :class="statusClass">{{ aliveLabel }}</span>
            </div>

            <div class="hero-card__stats">
                <div class="hero-card__stat">
                    <span class="hero-card__stat-label">Уровень</span>
                    <span class="hero-card__stat-value">{{ hero.level }}</span>
                </div>
                <div class="hero-card__stat">
                    <span class="hero-card__stat-label">Здоровье</span>
                    <span class="hero-card__stat-value">{{ hero.maxHealth }}</span>
                </div>
                <div class="hero-card__stat">
                    <span class="hero-card__stat-label">Мастерство</span>
                    <span class="hero-card__stat-value">{{ hero.maxAbility }}</span>
                </div>
                <div class="hero-card__stat">
                    <span class="hero-card__stat-label">Боёв</span>
                    <span class="hero-card__stat-value">{{ hero.results.length }}</span>
                </div>
            </div>

            <div class="hero-card__level-progress">
                <div class="hero-card__level-progress-head">
                    <span class="hero-card__level-progress-label">Прогресс уровня</span>
                    <span class="hero-card__level-progress-value">
                        {{ hero.experience }} / {{ nextLevelExperience }} (до {{ nextLevel }} ур.)
                    </span>
                </div>
                <div class="hero-card__level-progress-bar" role="progressbar" :aria-valuenow="Math.round(levelProgressPercent)" aria-valuemin="0" aria-valuemax="100">
                    <div class="hero-card__level-progress-fill" :style="{ width: `${levelProgressPercent}%` }" />
                </div>
            </div>

            <div v-if="killerName" class="hero-card__killer">
                <span class="hero-card__killer-label">Убит монстром:</span>
                <span class="hero-card__killer-name">{{ killerName }}</span>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { HeroAliveState, HeroInfoDto, MonsterFightResultType } from '@/api/clients';
import { computed, PropType } from 'vue';

const props = defineProps({
    hero: {
        required: true,
        type: Object as PropType<HeroInfoDto>,
    },
});

const aliveLabel = computed(() =>
    props.hero.isAlive === HeroAliveState.ALIVE ? 'Жив' : 'Мёртв'
);

const statusClass = computed(() =>
    props.hero.isAlive === HeroAliveState.ALIVE
        ? 'hero-card__status_alive'
        : 'hero-card__status_dead'
);

const killerName = computed(() => {
    const record = props.hero.results.find(r => r.type === MonsterFightResultType.DEFEAT);
    return record?.monster.name ?? '';
});

const nextLevel = computed(() => props.hero.level + 1);

const previousLevelExperience = computed(() => props.hero.levelProgressInfo?.previousAmound ?? 0);
const nextLevelExperience = computed(() => props.hero.levelProgressInfo?.nextAmound ?? props.hero.experience);

const levelProgressPercent = computed(() => {
    const range = nextLevelExperience.value - previousLevelExperience.value;
    if (range <= 0) {
        return 100;
    }

    const normalizedCurrent = Math.min(
        Math.max(props.hero.experience, previousLevelExperience.value),
        nextLevelExperience.value,
    );

    return ((normalizedCurrent - previousLevelExperience.value) / range) * 100;
});
</script>

<style lang="scss" scoped>
.hero-card {
    display: flex;
    gap: 24px;
    align-items: flex-start;
    background: white;
    border-radius: 12px;
    box-shadow: 0px 0px 8px rgba(155, 155, 155, 0.4);
    padding: 20px;

    @media (max-width: 720px) {
        flex-direction: column;
        align-items: center;
    }

    &__portrait {
        width: 180px;
        border-radius: 10px;
        border: 1px solid #111;
        background: #f2f2f2;
        flex-shrink: 0;
    }

    &__body {
        flex: 1;
        min-width: 0;
        display: flex;
        flex-direction: column;
        gap: 20px;
    }

    &__head {
        display: flex;
        justify-content: space-between;
        align-items: flex-start;
        gap: 12px;

        @media (max-width: 720px) {
            flex-direction: column;
        }
    }

    &__name {
        margin: 0;
        font-size: 34px;
    }

    &__status {
        border-radius: 999px;
        padding: 8px 14px;
        font-weight: 700;
        white-space: nowrap;

        &_alive {
            background: #d9efdc;
            color: #1f6b2c;
        }

        &_dead {
            background: #f3dada;
            color: #8b2525;
        }
    }

    &__stats {
        display: grid;
        grid-template-columns: repeat(2, minmax(0, 1fr));
        gap: 16px;

        @media (max-width: 560px) {
            grid-template-columns: 1fr;
        }
    }

    &__stat {
        display: flex;
        flex-direction: column;
        gap: 6px;
        padding: 14px 16px;
        background: #f6f7fb;
        border-radius: 10px;

        &-label {
            font-weight: bold;
            color: #5e5e5e;
        }

        &-value {
            font-size: 24px;
        }
    }

    &__killer {
        display: flex;
        align-items: center;
        gap: 10px;

        &-label {
            font-weight: bold;
            color: #5e5e5e;
        }

        &-name {
            font-size: 20px;
            font-weight: 700;
            color: #8b2525;
        }
    }

    &__level-progress {
        display: flex;
        flex-direction: column;
        gap: 10px;

        &-head {
            display: flex;
            justify-content: space-between;
            align-items: baseline;
            gap: 12px;

            @media (max-width: 560px) {
                flex-direction: column;
                align-items: flex-start;
            }
        }

        &-label {
            font-weight: bold;
            color: #5e5e5e;
        }

        &-value {
            font-size: 18px;
            font-weight: 600;
            color: #1d2940;
        }

        &-bar {
            height: 14px;
            border-radius: 999px;
            background: #e5e9f1;
            overflow: hidden;
        }

        &-fill {
            height: 100%;
            border-radius: 999px;
            background: linear-gradient(90deg, #2f974a 0%, #67cf82 100%);
            transition: width 0.25s ease;
        }
    }
}
</style>
