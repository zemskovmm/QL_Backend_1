import { RootStore } from "src/stores/RootStore";
import { action, observable, runInAction } from "mobx";
import { AdminRouteNames } from "src/routes";
import { validate, IsNotEmpty } from "@keroosha/class-validator";
import { reduceValidationErrorsToErrors } from "src/utilities";
import { AuthErrorFields, AuthorizeStore, ResettableFieldsStore, StoreWithErrors } from "src/stores/interfaces";

export class AdminLoginStore implements AuthorizeStore, ResettableFieldsStore, StoreWithErrors<AuthErrorFields> {
    @observable root;

    @IsNotEmpty({ message: "Обязательно для заполнения" })
    @observable
    login;
    @IsNotEmpty({ message: "Обязательно для заполнения" })
    @observable
    password;

    @observable adminApi;
    @observable errors: AuthErrorFields;

    constructor(root: RootStore) {
        this.root = root;
        this.login = "";
        this.password = "";
        this.adminApi = this.root.adminRpc;
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
        if (this.root.adminRpc.isAuthorized) this.JumpToDashboard();

        const errors = await validate(this);
        if (errors.length !== 0) {
            runInAction(() => (this.errors = reduceValidationErrorsToErrors(errors)));
            return;
        }

        const res = await this.adminApi.userLogin.loginAdmin(this.login, this.password);
        if (res.success) {
            runInAction(() => {
                this.root.adminRpc.setUserToken(res.value);
                this.JumpToDashboard();
                this.resetFields();
            });
            return;
        }

        this.errors.apiError = [res.error.description];
    }

    @action LogOut() {
        this.root.adminRpc.resetUserToken();
        window.location.reload();
    }
}
