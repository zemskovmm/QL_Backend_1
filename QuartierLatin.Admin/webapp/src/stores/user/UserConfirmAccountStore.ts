import { observable } from "mobx";
import { RootStore } from "src/stores/RootStore";

export class UserConfirmAccountStore {
    @observable root;
    @observable loading = true;
    @observable error: string | undefined;

    constructor(root: RootStore) {
        this.root = root;
    }

    reset() {
        this.error = undefined;
        this.loading = true;
    }

    setError(error: string) {
        this.error = error;
        this.loading = false;
    }

    async confirmAccount(code?: string) {
        this.reset();
        if (!code) {
            this.setError("Необходим код подтверждения");
            return false;
        }

        const { success, error } = await this.root.userRpc.userLogin.confirmAccount(code);
        if (!success) {
            this.setError(error.description);
            return false;
        }
        return true;
    }
}
