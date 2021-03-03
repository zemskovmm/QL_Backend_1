import { RootStore } from "src/stores/RootStore";
import { action, observable, runInAction } from "mobx";
import { AdminRouteNames } from "src/routes";
import { validate, IsNotEmpty } from "@keroosha/class-validator";
import { reduceValidationErrorsToErrors } from "src/utilities";
import { AuthErrorFields, AuthorizeStore, ResettableFieldsStore, StoreWithErrors } from "src/stores/interfaces";
import { RouterState } from "mobx-state-router";

export class AdminLoginStore implements AuthorizeStore, ResettableFieldsStore, StoreWithErrors<AuthErrorFields> {
    @observable root;

    @IsNotEmpty({ message: "Обязательно для заполнения" })
    @observable
    login;
    @IsNotEmpty({ message: "Обязательно для заполнения" })
    @observable
    password;

    @observable errors: AuthErrorFields;

    constructor(root: RootStore) {
        this.root = root;
        this.login = "";
        this.password = "";
        this.errors = {};
    }

    @action resetFields() {
        this.login = "";
        this.password = "";
    }

    @action JumpToDashboard() {
        const { routerStore } = this.root;
        routerStore.goTo(AdminRouteNames.mainPage);
    }

    @action async Login() {
        // if (this.root.adminRpc.isAuthorized) this.JumpToDashboard();

        const errors = await validate(this);
        if (errors.length !== 0) {
            runInAction(() => (this.errors = reduceValidationErrorsToErrors(errors)));
            return;
        }

        const res = await fetch("/Auth/Login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({ username: this.login, password: this.password, rememberMe: false }),
        });

        if (res.ok) this.JumpToDashboard();
        this.errors.apiError = ["Wrong password"];
    }

    @action async LogOut() {
        const res = await fetch("/Auth/Logout", {
            method: "GET",
        });
        window.location.reload();
    }
}
