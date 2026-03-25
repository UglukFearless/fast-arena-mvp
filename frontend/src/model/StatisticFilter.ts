import { HeroAliveParam, HeroOwnerParam, Parameter } from '@/api/clients';

export type StatisticFilterValue = string | number | boolean;

export interface StatisticFiltersState {
    parameter: Parameter;
    aliveParam: HeroAliveParam;
    ownerParam: HeroOwnerParam;
    desc: boolean;
}

export interface IStatisticFilterOption {
    label: string;
    value: StatisticFilterValue;
}

export class StatisticFilter {
    key: string;
    title: string;
    value: StatisticFilterValue;
    options: IStatisticFilterOption[];

    constructor(
        key: string,
        title: string,
        value: StatisticFilterValue,
        options: IStatisticFilterOption[]
    ) {
        this.key = key;
        this.title = title;
        this.value = value;
        this.options = options;
    }
}
