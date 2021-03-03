export interface AutoCompleteSelectStore<TItem> {
    items: TItem[];
    value?: TItem;
    labelField: string,
    valueField: string,
    suggest(query: string): Promise<void>;
}