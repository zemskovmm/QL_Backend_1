import { RootStore } from "src/stores/RootStore";
import React, { ChangeEvent, FC, FormEvent, useContext, useEffect } from "react";
import { MobXProviderContext } from "mobx-react";
import { TransitionHook } from "mobx-state-router/dist/types/router-store";
import { UserProfileStore } from "src/stores/user/UserProfileStore";
import { ValidationError } from "@keroosha/class-validator";
import pageStyles from "src/styles/page.module.css";

export type TransitionHookCallback<T> = (module: T) => Promise<unknown> | void;
export type TransitionHookWithCallback<T> = (callback?: TransitionHookCallback<T>) => TransitionHook;

export const mapChangeEventToValue = <T extends {}>(callback: (x: string) => void) => (
    x: ChangeEvent<HTMLInputElement>
) => callback(x.target.value);

export const preventDefaultEvent = <T extends {}>(callback: (x: FormEvent<HTMLFormElement>) => void) => (
    x: FormEvent<HTMLFormElement>
) => {
    x.preventDefault();
    callback(x);
};

export const useRootStore = () => useContext(MobXProviderContext) as { rootStore: RootStore };

export const useEnsureStoreLoaded = (x: { loading: boolean; initialized: boolean; load: () => void }) =>
    useEffect(() => {
        if (!x.loading && !x.initialized) x.load();
    }, [x]);

// Transition hooks

export const redirectIfProfileNotFilled: TransitionHookCallback<[UserProfileStore, unknown]> = async ([user]) => {
    if (!user.profile) await user.loadProfile();
};

export const reduceValidationErrorsToErrors = <T extends {}>(errors: ValidationError[]) =>
    errors.reduce((acc, x) => ({ ...acc, [x.property]: Object.values(x.constraints ?? {}) }), {} as T);

export const computeInputStyleByErrorArray = (x: unknown[]) =>
    x.length > 0 ? pageStyles.inputError : pageStyles.inputGrey;

export function valueOrNull<T>(value: T | undefined): T | null {
    if (value === undefined) return null;
    return value;
}

export const toDTL = (millis: number) => new Date(millis).toISOString().slice(0, -1);

export function addEvent(eventName: string, callback: (e: any) => void) {
    const element = document as any;
    if (element.addEventListener) {
        element.addEventListener(eventName, (e: any) => callback(e || window.event), false);
    } else if (element.attachEvent) {
        element.attachEvent("on" + eventName, (e: any) => callback(e || window.event));
    } else {
        element["on" + eventName] = (e: any) => callback(e || window.event);
    }
}
