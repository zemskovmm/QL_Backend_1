import { RootStore } from "src/stores/RootStore";
import { action, observable, runInAction } from "mobx";
import { validate, IsEmail, Equals, MinLength } from "@keroosha/class-validator";
import { reduceValidationErrorsToErrors } from "src/utilities";

type RegisterStoreErrors = {
    email?: string[];
    mobile?: string[];
    password?: string[];
    repeatPassword?: string[];
    serverResponseError?: string;
};
export class RegisterStore {
    @observable root;

    @observable formError;

    @IsEmail({}, { message: "Неверный адрес электронной почты" })
    @observable
    email;

    @observable errors: RegisterStoreErrors;

    @MinLength(8, { message: "Слабый пароль" })
    @observable
    password;
    @Equals("password", { message: "Пароли не совпадают" })
    @observable
    repeatPassword;

    // @IsPhoneNumber('+7', { message: "not valid Number" })
    // @observable mobile;

    @action resetFields() {
        this.password = "";
        this.repeatPassword = "";
        this.email = "";
        this.formError = "";
        this.errors = {};
    }

    constructor(root: RootStore) {
        this.root = root;
        this.password = "";
        this.repeatPassword = "";
        this.email = "";
        this.formError = "";
        this.errors = {};
    }

    @action async register() {
        const { routerStore } = this.root;
        const errors = await validate(this);
        runInAction(() => {
            if (errors.length === 0) return;
            this.errors = reduceValidationErrorsToErrors(errors);
            return;
        });
        // const res = await this.userLoginApi.register(this.email, this.password);

        // if (res.success) {
        //     this.resetFields();
        //     routerStore.goTo(UserRouteNames.login);
        // }

        // runInAction(() => (this.errors.serverResponseError = res.error.description));
    }
}
