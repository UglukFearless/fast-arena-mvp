import { ActivityActionType, ActivitySessionClient, ActivitySessionDto } from "@/api/clients";
import actionRouteService from "@/services/action-route-service";
import { ApiSettings } from "@/utils/constants";
import authFetch from "@/utils/http-helper";
import { defineStore } from "pinia";
import { Router } from "vue-router";

export const useActivitySessionStore = defineStore('activity-session', {
    state:() => ({
        session: null as ActivitySessionDto | null,
    }),
    actions: {
        async start(id: string) {
            const sessionClient = new ActivitySessionClient(
                ApiSettings.BaseUrl,
                authFetch,
            );
            this.session = await sessionClient.start(id);
        },
        async getCurrent() {
            const sessionClient = new ActivitySessionClient(
                ApiSettings.BaseUrl,
                authFetch,
            );
            this.session = await sessionClient.getCurrent();
        },
        async goNext(router: Router) {
            this.getCurrent();
            const sesstionRoute = actionRouteService.getRouteByActionType(this.session?.currentAction.type);
            router.push(sesstionRoute);
        }
    }
});
