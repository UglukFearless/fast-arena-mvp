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
import { StatisticFiltersState } from "@/model/StatisticFilter";


export const useStatisticStore = defineStore('statistic', {
    state: () => ({
        data: null as StatisticDataDto | null,
        parameter: Parameter.WINS as Parameter | undefined,
        aliveParam: HeroAliveParam.ALL as HeroAliveParam | undefined,
        ownerParam: HeroOwnerParam.ANY as HeroOwnerParam | undefined,
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
        async fetchData() {
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
        async init() {
            await this.fetchData();
        },
        async update() {
            await this.fetchData();
        },
        async applyFilters(filters: StatisticFiltersState) {
            this.parameter = filters.parameter;
            this.aliveParam = filters.aliveParam;
            this.ownerParam = filters.ownerParam;
            this.desc = filters.desc;

            await this.fetchData();
        },
        async setParameter(parameter: Parameter) {
            this.parameter = parameter;
        },
        async setAliveParam(aliveParam: HeroAliveParam) {
            this.aliveParam = aliveParam;
        },
        async setOwnerParam(ownerParam: HeroOwnerParam) {
            this.ownerParam = ownerParam;
        },
        async setDesc(desc: boolean) {
            this.desc = desc;
        },
    }
});