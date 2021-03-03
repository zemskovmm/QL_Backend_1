import React from "react";
import { Route, RouterState } from "mobx-state-router";
import {
    AdminAuthorizedOnlyHook,
    AdminRouteToRootIfAuthorizedHook,
    EnsureProfileLoadedHook,
    UserAllowOnlyFilledProfilesHook,
    UserAuthorizedOnlyHook,
    UserRouteToRootIfAuthorizedHook,
} from "src/routing/hooks";
import { convertRoutes } from "src/routing/route";
import { AnonNotFoundPage } from "src/pages/anon/AnonNotFoundPage";
import { AccountConfirmPage, AdminLoginPage, EditProfilePage, LoginPage, RegisterPage } from "./routing/LazyRoutes";

export enum AnonRouteNames {
    notFound = "not-found",
    root = "root",
}

export enum UserRouteNames {
    login = "user-login",
    register = "user-register",
    confirm = "user-confirm",
    mainPage = "user-main-page",
    profile = "user-profile",
}

export enum AdminRouteNames {
    login = "admin-login",
    mainPage = "admin-main-page",
}

export const AnonViewMap = {
    [AnonRouteNames.notFound]: <AnonNotFoundPage />,
    [AnonRouteNames.root]: <></>,
};

export const UserViewMap = {
    [UserRouteNames.login]: <LoginPage />,
    [UserRouteNames.register]: <RegisterPage />,
    [UserRouteNames.profile]: <EditProfilePage />,
    [UserRouteNames.confirm]: <AccountConfirmPage />,
};

export const AdminViewMap = {
    [AdminRouteNames.login]: <AdminLoginPage />,
};

const AnonRoutes: Route[] = convertRoutes([
    {
        pattern: "/not-found",
        name: AnonRouteNames.notFound,
    },
    {
        pattern: "/",
        name: AnonRouteNames.root,
    },
]);

const UserRoutes: Route[] = convertRoutes([
    {
        pattern: "/register",
        name: UserRouteNames.register,
        hooks: [UserRouteToRootIfAuthorizedHook],
    },
    {
        pattern: "/login",
        name: UserRouteNames.login,
        hooks: [UserRouteToRootIfAuthorizedHook],
    },
    {
        pattern: "/confirm",
        name: UserRouteNames.confirm,
        onEnter: async (root, to) => {
            if (await root.userConfirmAccountStore.confirmAccount(to.queryParams["code"]))
                throw new RouterState(UserRouteNames.login);
        },
        hooks: [],
    },
    {
        pattern: "/personal",
        name: UserRouteNames.mainPage,
        hooks: [UserAuthorizedOnlyHook, EnsureProfileLoadedHook, UserAllowOnlyFilledProfilesHook],
        onEnter: (root) => {
            root.routerStore.goTo(new RouterState(UserRouteNames.profile));
        },
    },
    {
        pattern: "/personal/profile",
        name: UserRouteNames.profile,
        hooks: [UserAuthorizedOnlyHook, EnsureProfileLoadedHook, UserAllowOnlyFilledProfilesHook],
    },
]);

const AdminRoutes: Route[] = convertRoutes([
    {
        pattern: "/admin",
        name: AdminRouteNames.mainPage,
        hooks: [AdminAuthorizedOnlyHook],
        onEnter: (root) => {
            root.routerStore.goTo(new RouterState(AdminRouteNames.mainPage));
        },
    },
    {
        pattern: "/admin/login",
        name: AdminRouteNames.login,
        hooks: [AdminRouteToRootIfAuthorizedHook],
    },
]);

export const Routes: Route[] = AnonRoutes.concat(UserRoutes).concat(AdminRoutes);
