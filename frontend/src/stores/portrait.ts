import { PortraitClient, PortraitDto } from "@/api/clients";
import { ApiSettings } from "@/utils/constants";
import authFetch from "@/utils/http-helper";
import { defineStore } from "pinia";

export const usePortraitStore = defineStore('portrait', {
    state: () => ({
        heroPortraits: null as PortraitDto[] | null,
    }),
    actions: {
        async init() {
            const portraitClient = new PortraitClient(
                ApiSettings.BaseUrl,
                authFetch,
            );
            this.heroPortraits = await portraitClient.getAllForHeroes();
        }
    }
});