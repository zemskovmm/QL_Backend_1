import { RootStore } from "src/stores/RootStore";
import { action, observable, runInAction } from "mobx";
import { UserRouteNames } from "src/routes";
import { validate, IsEmail, IsNotEmpty } from "@keroosha/class-validator";;
import { reduceValidationErrorsToErrors } from "src/utilities";
import { AuthErrorFields, AuthorizeStore, ResettableFieldsStore, StoreWithErrors } from "src/stores/interfaces";

export class UserLoginStore implements AuthorizeStore, ResettableFieldsStore, StoreWithErrors<AuthErrorFields> {
    @observable root;
    @IsEmail({}, { message: "Неверный адрес электронной почты" })
    @observable
    login;
    @IsNotEmpty({ message: "Обязателен для заполнения" })
    @observable
    password;
    @observable userLoginApi;
    @observable userId?: string;
    @observable errors: AuthErrorFields;

    @observable private authPromise: Promise<unknown> | undefined;

    constructor(root: RootStore) {
        this.root = root;
        this.login = "";
        this.password = "";
        this.errors = {};
        this.userLoginApi = root.userRpc.userLogin;
    }

    @action resetFields() {
        this.login = "";
        this.password = "";
        this.errors = {};
    }

    @action JumpToDashboard() {
        const { routerStore } = this.root;
        routerStore.goTo(UserRouteNames.mainPage);
    }

    @action async Login() {
        if (this.root.userRpc.isAuthorized) this.JumpToDashboard();

        const errors = await validate(this);
        if (errors.length !== 0) {
            runInAction(() => (this.errors = reduceValidationErrorsToErrors(errors)));
            return;
        }

        const res = await this.userLoginApi.login(this.login, this.password);
        if (res.success) {
            runInAction(() => {
                this.root.userRpc.setUserToken(res.value);
                this.JumpToDashboard();
                this.resetFields();
            });
            return;
        }

        this.errors.apiError = [res.error.description];
    }

    @action.bound
    private async CheckAuth() {
        if (!this.root.userRpc.isAuthorized || this.authPromise) return;
        // TODO: Check auth method missing in API
    }

    @action LogOut() {
        this.root.userRpc.resetUserToken();
        window.location.reload();
    }
}
