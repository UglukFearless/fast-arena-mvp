import { ActivityClient, ActivityDto } from "@/api/clients";
import { ApiSettings } from "@/utils/constants";
import authFetch from "@/utils/http-helper";
import { defineStore } from "pinia";

// Temp

export const useActivityStore = defineStore('activity', {
    state:() => ({
        activities: [] as ActivityDto[],
    }),
    actions: {
        async init() {
            const activityClient = new ActivityClient(
                ApiSettings.BaseUrl,
                authFetch,
            );
            this.activities = await activityClient.get();
        },
    }
});