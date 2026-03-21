import { HeroAliveState, HeroClient, HeroCreationModel, HeroDto } from "@/api/clients";
import { ApiSettings } from "@/utils/constants";
import authFetch from "@/utils/http-helper";
import { defineStore } from "pinia";

export const useHeroStore = defineStore('hero', {
    state: () => ({
        heroes: null as HeroDto[] | null,
        selectedHeroId: null as string | null,
    }),
    getters: {
        selectedHero: (state): HeroDto | undefined => {
            return state.heroes?.find(h => h.id === state.selectedHeroId);
        },
        heroesExceptSelected: (state): HeroDto[] | undefined => {
            return state.heroes?.filter(h => h.id !== state.selectedHeroId && h.isAlive === HeroAliveState.ALIVE);
        },
        deadHeroes: (state): HeroDto[] | undefined => {
            return state.heroes?.filter(h => h.isAlive === HeroAliveState.DEAD);
        },
    },
    actions: {
        async create(model: HeroCreationModel): Promise<HeroDto> {
            try {
                const heroClient = new HeroClient(ApiSettings.BaseUrl, authFetch);
                return await heroClient.create(model);
            } catch(e) {
                alert("При создании героя возникла ошибка!");
                throw e;
            }
        },
        async refreshHeroes() {
            const heroClient = new HeroClient(ApiSettings.BaseUrl, authFetch);
            this.heroes = await heroClient.get();

            const selectedHero = await heroClient.getSelected();
            if (selectedHero)
                this.selectedHeroId = selectedHero.id as string;
            else
                this.selectedHeroId = null;
        },
        setHeroes(heroes: HeroDto[]) {
            this.heroes = heroes;
        },
        setSelectedHero(id: string | null) {
            this.selectedHeroId = id;
        },
        async select(id: string) {
            try {
                const heroClient = new HeroClient(ApiSettings.BaseUrl, authFetch);
                await heroClient.select(id);
                this.selectedHeroId = id;
            } catch (error) {
                alert((error as Error).message);
                throw error;
            }
        },
        async unselect() {
            try {
                const heroClient = new HeroClient(ApiSettings.BaseUrl, authFetch);
                await heroClient.unselect();
                this.selectedHeroId = null;
            } catch (error) {
                alert((error as Error).message);
                throw error;
            }
        },
        $reset() {
            this.heroes = null;
            this.selectedHeroId = null;
        },
    },
});