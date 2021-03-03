import { RootStore } from "src/stores/RootStore";
import { action, observable } from "mobx";
import { StringMap } from "mobx-state-router/dist/types/router-store";

export type HeaderPill = {
    color: string,
    text: string,
}

export type HeaderTitleLink = {
    title: string,
    route: string,
    args?: StringMap,
}

export enum HeaderTitleType {
    text = 'plain-text',
    linkGroup = 'link-group',
}

export type HeaderEntry = {
    type: HeaderTitleType,
    title?: string,
    linkGroup?: HeaderTitleLink[],
    subtitle: string,
    subtitleMobile?: string,
    pill?: HeaderPill,
}

export type Header = {
    [id: string]: HeaderEntry,
}

export class HeaderStore {
    @observable headerOpen: boolean;

    constructor(private readonly root: RootStore) {
        this.root = root;
        this.headerOpen = false;
    }
    
    @action menuOpen() {
        this.headerOpen = !this.headerOpen
    }
}