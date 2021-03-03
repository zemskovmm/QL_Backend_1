import { observable } from "mobx";
import { RouterState, RouterStore } from "mobx-state-router";
import { UserLoginStore } from "src/stores/user/UserLoginStore";
import { RegisterStore } from "src/stores/user/RegisterStore";
import { UserProfileStore } from "src/stores/user/UserProfileStore";
import { Routes } from "src/routes";
import { HeaderStore } from "src/stores/ui/header";
import { PreviewBoxStore } from "src/components/PreviewBox/PreviewBoxStore";
import { UserConfirmAccountStore } from "src/stores/user/UserConfirmAccountStore";
import { UpdateProfileStore } from "src/stores/user/UpdateProfileStore";
import { AdminLoginStore } from "src/stores/admin/AdminLoginStore";

// TODO Page prefix!
export class RootStore {
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
