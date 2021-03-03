import {computed, observable, runInAction} from "mobx";

export class RequestTracking {
    @observable private __loadingCounter: number = 0;

    @computed get isLoading(): boolean {
        return this.__loadingCounter > 0;
    }

    public async track<T>(cb: () => Promise<T>): Promise<T> {
        runInAction(() => this.__loadingCounter++);
        try {
            return await cb();
        } finally {
            runInAction(() => this.__loadingCounter--);
        }
    }
}