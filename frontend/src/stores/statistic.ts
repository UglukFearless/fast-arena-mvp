import { 
    HeroAliveParam, 
    HeroOwnerParam, 
    Parameter, 
    StatisticClient, 
    StatisticDataDto, 
    StatisticDataRowDto 
} from "@/api/clients";
import { ApiSettings } from "@/utils/constants";
import authFetch from "@/utils/http-helper";
import { defineStore } from "pinia";


export const useStatisticStore = defineStore('statistic', {
    state: () => ({
        data: null as StatisticDataDto | null,
        parameter: undefined as Parameter | undefined,
        aliveParam: undefined as HeroAliveParam | undefined,
        ownerParam: undefined as HeroOwnerParam | undefined,
        desc: true,
    }),
    getters: {
        rows: (state): StatisticDataRowDto[] => {
            return state.data?.data ?? [];
        },
        title: (state): string => {
            return state.data?.title ?? '';
        },
        valueTitle: (state): string => {
            return state.data?.valueTitle ?? '';
        },
        valueSymbols: (state): string => {
            return state.data?.valueSymbols ?? '';
        },
    },
    actions: {
        async init() {
            const statisticClient = new StatisticClient(
                ApiSettings.BaseUrl,
                authFetch,
            );
            this.data = await statisticClient.get(
                this.parameter,
                this.aliveParam,
                this.ownerParam,
                this.desc,
            );
        },
        async update() {
            const statisticClient = new StatisticClient(
                ApiSettings.BaseUrl,
                authFetch,
            );
            this.data = await statisticClient.get(
                this.parameter,
                this.aliveParam,
                this.ownerParam,
                this.desc,
            );
        },
        async setParameter(parameter: Parameter) {
            this.parameter = parameter;
        },
        async setAliveParam(aliveParam: HeroAliveParam) {
            this.aliveParam = aliveParam;
        },
        async setOwnerParam(ownerParam: HeroOwnerParam) {
            this.ownerParam = ownerParam
        },
        async setDesc(desc: boolean) {
            this.desc = desc;
        },
    }
});