import { RootStore } from "src/stores/RootStore";
import { action, computed, observable, runInAction } from "mobx";
import { validate, IsNotEmpty } from "@keroosha/class-validator";
import { reduceValidationErrorsToErrors } from "src/utilities";
import { UserRouteNames } from "src/routes";

export class UpdateProfileAvatarStore {
    @observable uploadMode = false;

    @computed get userStore() {
        return this.root.userStore;
    }

    @computed get userAvatar() {
        return this.userStore.profile && this.userStore.profile.avatarImage
            ? this.userStore.profile.avatarImage
            : undefined;
    }

    constructor(private readonly root: RootStore) {}

    @action async uploadAvatar(file: File) {
        const buffer = await file.arrayBuffer();
        // const res = await this.root.userRpc.userProfile.uploadUserPhoto(Array.from(new Uint8Array(buffer)));
        // if (!res.success) return;
        // await this.userStore.loadProfile();
        this.uploadMode = false;
    }

    @action async removeAvatar() {
        // const res = await this.root.userRpc.userProfile.removeUserPhoto();
        // if (!res.success) return;

        await this.userStore.loadProfile();
    }
}

type UpdateProfileStoreErrors = {
    firstName?: string[];
    lastName?: string[];
};

export class UpdateProfileStore {
    @observable error?: string;
    @observable errors: UpdateProfileStoreErrors;
    @observable userAvatarStore;

    @IsNotEmpty({ message: "Обязательно для заполнения" }) @observable firstName;
    @IsNotEmpty({ message: "Обязательно для заполнения" }) @observable lastName;
    @observable email;

    constructor(private readonly root: RootStore) {
        this.firstName = "";
        this.lastName = "";
        this.email = "";
        this.errors = {};
        this.userAvatarStore = new UpdateProfileAvatarStore(root);
    }

    @computed get loading() {
        return false;
    }

    @computed get initialized() {
        return true;
    }

    @action async loadAll() {
        runInAction(() => {
            if (this.root.userStore.profile) {
                const { name, email } = this.root.userStore.profile;

                const [firstName = "", lastName = ""] = name.split(" ");

                this.firstName = firstName;
                this.lastName = lastName;
                this.email = email;
            }
        });
    }

    @action
    async updateProfile() {
        const { firstName, lastName, email } = this;
        const { userStore } = this.root;
        if (!this.root.userStore.profile?.id) return;
        const { id } = this.root.userStore.profile;

        const errors = await validate(this);
        if (errors.length !== 0) {
            this.errors = reduceValidationErrorsToErrors(errors);
            return;
        }

        const avatarImage = "boo";
        // const res = await this.root.userRpc.userProfile.updateProfile({
        //     id,
        //     email,
        //     name: `${firstName} ${lastName}`,
        // } as UserProfileDto);
        // if (!res.success) {
        //     runInAction(() => (this.error = res.error.description));
        //     return;
        // }

        await userStore.loadProfile();
        await this.root.routerStore.goTo(UserRouteNames.mainPage);
    }
}
