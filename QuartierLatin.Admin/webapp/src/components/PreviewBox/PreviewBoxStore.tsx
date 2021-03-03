import { action, observable } from "mobx";

export class PreviewBoxStore {
    @observable isPreviewBoxOpen: boolean = false;
    @observable imageUrl: string = '';

    @action show(imageUrl: string) {
        this.imageUrl = imageUrl;
        this.isPreviewBoxOpen = true;
    }

    @action hide(): void {
        this.isPreviewBoxOpen = false;
    }
}