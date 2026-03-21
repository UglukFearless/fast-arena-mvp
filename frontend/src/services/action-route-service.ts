import { ActivityActionType } from "@/api/clients";

export interface IActionRouteService {
    getRouteByActionType(type: ActivityActionType): string;
};

class ActionRouteService implements IActionRouteService {
    getRouteByActionType(type?: ActivityActionType): string {
        if (!type)
            return '/';
        switch (type) {
            case 0:
                return '/fight';
              default:
                throw Error('Unknown type of activity action ' + type);
          }
    }
}

const actionRouteService = new ActionRouteService();

export default actionRouteService;