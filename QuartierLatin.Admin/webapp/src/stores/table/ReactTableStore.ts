import { action, computed, observable } from "mobx";
import { Loadable } from "src/stores/table/LoadableStore";

export interface IReactTableStore<T> {
    totalPages: number;
    pageSize: number;
    isLoading: boolean;
    items: T[];

    setPageSize(size: number, page: number): void;
}

export abstract class ReactTableStore<T> extends Loadable implements IReactTableStore<T> {
    @observable page: number;
    @observable pageSize: number;
    @observable totalPages: number;
    @observable totalItemsCount: number;
    @observable items: T[];
    @observable search: string = "";

    @computed get skip() {
        return this.page * this.pageSize;
    }

    @computed get take() {
        return this.pageSize;
    }

    public abstract refresh(): Promise<void>;

    public async load(): Promise<void> {
        await this.refresh();
    }

    protected constructor() {
        super();
        this.page = 0;
        this.pageSize = 10;
        this.items = [];
        this.totalPages = 0;
        this.totalItemsCount = 0;
    }

    @action fillItems(items: T[], totalCount: number) {
        this.totalItemsCount = totalCount;
        this.items = items;
        this.totalPages = Math.ceil(totalCount / this.pageSize);
        this.isLoaded = true;
    }

    @action setPageSize(size: any, page: any) {
        this.pageSize = size;
        this.page = page;
        this.refresh();
    }

    public getItemById(id: string) {
        return this.items.filter((x: any) => x.id == id)[0];
    }
}

class LiveReloadHelper {
    isReloading = false;
    private onReload: () => Promise<void>;
    constructor(onReload: () => Promise<void>) {
        this.onReload = onReload;
    }

    reload(): void {
        if (this.isReloading) return;
        this.onReload()
            .catch((e) => {
                this.isReloading = false;
                console.log(e);
            })
            .then(() => {
                this.isReloading = false;
            });
    }
}

export abstract class LiveReloadableReactTableStore<T> extends ReactTableStore<T> {
    _reloadHelper: LiveReloadHelper;
    constructor(onLiveReload: () => Promise<void>) {
        super();
        this._reloadHelper = new LiveReloadHelper(async () => {
            if (this.items && this.items.length > 0) {
                await onLiveReload();
            }
        });
    }

    @action liveReload() {
        this._reloadHelper.reload();
    }
}
