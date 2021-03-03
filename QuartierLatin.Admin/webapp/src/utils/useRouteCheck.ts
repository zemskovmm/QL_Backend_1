import { useRootStore } from "src/utilities";
import { AdminRouteNames, AnonRouteNames, UserRouteNames } from "src/routes";

const routeCheck = (routes: { [id: number]: string }) => (routeName: string) =>
    Object.values(routes).includes(routeName);

export const useIsUserRoute = () => {
    const {
        rootStore: { routerStore },
    } = useRootStore();

    return routeCheck(UserRouteNames)(routerStore.getCurrentRoute().name);
};
export const useIsAdminRoute = () => {
    const {
        rootStore: { routerStore },
    } = useRootStore();

    return routeCheck(AdminRouteNames)(routerStore.getCurrentRoute().name);
};

export const useIsAnonRoute = () => {
    const {
        rootStore: { routerStore },
    } = useRootStore();

    return routeCheck(AnonRouteNames)(routerStore.getCurrentRoute().name);
};
