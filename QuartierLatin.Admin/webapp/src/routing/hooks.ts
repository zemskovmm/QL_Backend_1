import { RouteTransitionHook } from "src/routing/route";
import { RouterState } from "mobx-state-router";
import { AdminRouteNames, UserRouteNames } from "src/routes";

export const UserAuthorizedOnlyHook: RouteTransitionHook = (root) => {
    // if (!root.userRpc.isAuthorized) throw new RouterState(UserRouteNames.login);
};

export const EnsureProfileLoadedHook: RouteTransitionHook = async (root) => {
    if (!root.userStore.profile) await root.userStore.loadProfile();
};

export const UserRouteToRootIfAuthorizedHook: RouteTransitionHook = (root) => {
    // if (root.userRpc.isAuthorized) {
    //     root.routerStore.goTo(new RouterState(UserRouteNames.mainPage));
    // }
};

export const UserAllowOnlyFilledProfilesHook: RouteTransitionHook = async (root) => {
    const profile = root.userStore.profile;
    const profileFilled = profile && profile.name;
    if (!profileFilled) throw new RouterState(UserRouteNames.profile);
};

export const AdminAuthorizedOnlyHook: RouteTransitionHook = async (root) => {
    const res = await fetch("/Heartbeat/pulseAdmin", { method: "GET" });
    if (!res.ok) throw new RouterState(AdminRouteNames.login);
    // if (!root.adminRpc.isAuthorized) throw new RouterState(AdminRouteNames.login);
};

export const AdminRouteToRootIfAuthorizedHook: RouteTransitionHook = async (root) => {
    const res = await fetch("/Heartbeat/pulseAdmin", { method: "GET" });
    if (res.ok) throw new RouterState(AdminRouteNames.mainPage);
    // if (root.adminRpc.isAuthorized) {
    //     root.routerStore.goTo(new RouterState(AdminRouteNames.mainPage));
    // }
};
