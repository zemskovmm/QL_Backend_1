import { action, observable, runInAction } from "mobx";
import { RootStore } from "src/stores/RootStore";

export class UserProfileStore {
    @observable root;
    @observable profile?: any = {};

    constructor(root: RootStore) {
        this.root = root;
    }

    private get api() {
        // return this.root.userRpc;
        return "";
    }

    @action async loadProfile() {
        // const res = await this.api.userProfile.getProfile();
        // runInAction(() => {
        //     this.profile = res;
        // });
    }

    @action async updateInfo(userProfile: any) {
        // await this.api.userProfile.updateProfile(userProfile);
        await this.loadProfile();
    }
}
