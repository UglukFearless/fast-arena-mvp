import { HeroActVariant, MonsterFightActionStateDto, MonsterFightActionStateResult, MonsterFightClient, MonsterFightDto } from "@/api/clients";
import { ApiSettings } from "@/utils/constants";
import authFetch from "@/utils/http-helper";
import { defineStore } from "pinia";
import { useActivitySessionStore } from "./activity-session";
import { Router } from "vue-router";

export interface CharInfo {
    name: string,
    maxHealth: number,
    maxAbility: number,
    health: number,
    ability: number
    portraitUrl: string,
};


const activitySessionStore = useActivitySessionStore();

export const useMonsterFight = defineStore('monster-fight', {
    state: () => ({
        monsterFight: null as MonsterFightDto | null,
    }),
    getters: {
        fight: (state): MonsterFightDto | null => {
            return state.monsterFight;
        },
        heroInfo(state): CharInfo | null {
            if (!state.monsterFight)
                return null;

            const currentState = this.currentState as MonsterFightActionStateDto;

            return {
                name: state.monsterFight.hero.name,
                maxHealth: state.monsterFight.hero.maxHealth,
                maxAbility: state.monsterFight.hero.maxAbility,
                health: currentState.heroHealth,
                ability: currentState.heroAbility,
                portraitUrl: state.monsterFight.hero.portraitUrl as string,
            };
        },
        monsterInfo(state): CharInfo | null {
            if (!state.monsterFight)
                return null;

            const currentState = this.currentState as MonsterFightActionStateDto;

            return {
                name: state.monsterFight.monster.name,
                maxHealth: state.monsterFight.monster.maxHealth,
                maxAbility: state.monsterFight.monster.maxAbility,
                health: currentState.monsterHealth,
                ability: currentState.monsterAbility,
                portraitUrl: state.monsterFight.monster.portraitUrl as string,
            };
        },
        currentState: (state): MonsterFightActionStateDto | null => {
            if (!state.monsterFight)
                return null;

            return Object.entries(state.monsterFight?.state)
                .sort(([k1], [k2]) => Number(k2) - Number(k1))[0]?.[1];
        },
        currentOrder: (state): string | null => {
            if (!state.monsterFight)
                return null;

            return Object.entries(state.monsterFight?.state)
                .sort(([k1], [k2]) => Number(k2) - Number(k1))[0]?.[0];
        },
        history(state): string[] {
            if (!state.monsterFight)
                return [];

            return Object.entries(state.monsterFight.state)
                .filter(s => s[0] !== this.currentOrder)
                .filter(s => !!s[1].result)
                .map(s => (s[1].result as MonsterFightActionStateResult).resultText)
                .reverse();
        },
    },
    actions: {
        async getCurrent() {
            try {
                const monsterFightClient = new MonsterFightClient(ApiSettings.BaseUrl, authFetch);
                this.monsterFight = await monsterFightClient.get();
            } catch(e) {
                alert('Что-то пошло не так!');
                throw e;
            }
        },
        async doAction(code: HeroActVariant, router: Router) {
            try {
                const monsterFightClient = new MonsterFightClient(ApiSettings.BaseUrl, authFetch);
                const roundResult = await monsterFightClient.doHeroAction(code);

                if (!roundResult.shoudGoNext) {
                    this.monsterFight!.state[roundResult.stateOrder!] = roundResult.state!;
                    this.monsterFight!.reward = roundResult.reward;
                } else {
                    await activitySessionStore.goNext(router);
                }

            } catch(e) {
                alert('Что-то пошло не так!');
                throw e;
            }
        },
        $reset() {
            this.monsterFight = null;
        }
    }
});