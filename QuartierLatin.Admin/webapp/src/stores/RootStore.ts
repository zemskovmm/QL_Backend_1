import { action, computed, observable } from "mobx";
import { RouterState, RouterStore } from "mobx-state-router";
import { UserLoginStore } from "src/stores/user/UserLoginStore";
import { RegisterStore } from "src/stores/user/RegisterStore";
import { CoreApi } from "src/api";
import { AdminLoginStore } from "src/stores/admin/AdminLoginStore";
import { UserProfileStore } from "src/stores/user/UserProfileStore";
import { Routes } from "src/routes";
import { HeaderStore } from "src/stores/ui/header";
import { PreviewBoxStore } from "src/components/PreviewBox/PreviewBoxStore";
import { UserConfirmAccountStore } from "src/stores/user/UserConfirmAccountStore";
import { UpdateProfileStore } from "src/stores/user/UpdateProfileStore";

type AuthHeaderKeys = "X-User-Auth" | "X-Admin-Auth" | "X-Jury-Auth";

const apiUrl = "/tsrpc";

export class SecureCoreApi extends CoreApi {
    @observable private token: string | null;
    private localStorageKey;

    @computed get isAuthorized() {
        return this.token != null;
    }

    constructor(path: string, authHeaderKey: AuthHeaderKeys) {
        super(path, async (url: string, request: RequestInit) => {
            request.credentials = "same-origin";
            request.headers = {};
            if (this.token) request.headers[authHeaderKey] = this.token;
            const res = await fetch(url, request);
            if (res.status == 401) {
                window.location.reload();
                this.resetUserToken();
                await new Promise(() => {
                    // Never
                });
            }
            return res;
        });
        const localStorageKey = "vote-auth-token:" + authHeaderKey;
        this.localStorageKey = localStorageKey;
        this.token = window.localStorage.getItem(localStorageKey);
    }

    @action setUserToken(token: string) {
        this.token = token;
        window.localStorage.setItem(this.localStorageKey, token);
    }

    @action resetUserToken() {
        this.token = null;
        window.localStorage.removeItem(this.localStorageKey);
    }
}

// TODO Page prefix!
export class RootStore {
    @observable userRpc = new SecureCoreApi(apiUrl, "X-User-Auth");
    @observable adminRpc = new SecureCoreApi(apiUrl, "X-Admin-Auth");
    @observable anonRpc = new CoreApi(apiUrl);
    @observable routerStore = new RouterStore(this, Routes, new RouterState("not-found"));

    @observable headerStore = new HeaderStore(this);
    @observable previewStore = new PreviewBoxStore();

    // User specific routes
    @observable loginStore = new UserLoginStore(this);
    @observable userStore = new UserProfileStore(this);
    @observable userConfirmAccountStore = new UserConfirmAccountStore(this);
    @observable registerStore = new RegisterStore(this);
    @observable updateProfileStore = new UpdateProfileStore(this);

    // Admin specific routes
    @observable adminLoginStore = new AdminLoginStore(this);
}
