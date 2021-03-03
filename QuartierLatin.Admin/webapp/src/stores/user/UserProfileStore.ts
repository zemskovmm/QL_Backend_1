import { action, observable, runInAction } from "mobx";
import { RootStore } from "src/stores/RootStore";
import { UserProfileDto } from "src/api";

type UserProfile = UserProfileDto;

export class UserProfileStore {
    @observable root;
    @observable profile?: UserProfile;

    constructor(root: RootStore) {
        this.root = root;
    }

    private get api() {
        return this.root.userRpc;
    }

    @action async loadProfile() {
        const res = await this.api.userProfile.getProfile();
        runInAction(() => {
            this.profile = res;
        });
    }

    @action async updateInfo(userProfile: UserProfile) {
        await this.api.userProfile.updateProfile(userProfile);
        await this.loadProfile();
    }
}
