import React, { FC, useCallback, useState } from "react";
import { useThrottle } from "src/utils/throttle-effect";
import { useObserver } from "mobx-react";
import Select from "react-dropdown-select";
import pageStyles from "src/styles/page.module.css";
import { mapChangeEventToValue } from "src/utilities";
import { AutoCompleteSelectStore } from "src/components/AutoCompleteSelect/AutoCompleteSelectStore";

type AutoCompleteSelectProps = {
    store: AutoCompleteSelectStore<any>;
    placeholder: string;
    throttleTimeout?: number;
    className?: string;
};

export const AutoCompleteSelect: FC<AutoCompleteSelectProps> =
    ({ store, placeholder, throttleTimeout = 300, className }) => {

    const onChange = (item: any) => store.value = item;
    const onAddressChoose = useCallback(([x]: any[]) => onChange(x), [onChange]);
    const values = store.value ? [store.value] : [];
    const [query, setQuery] = useState("");

    useThrottle({
        action: (x: string) => store.suggest(x),
        data: query,
        timeout: throttleTimeout,
    }, [query, store]);

    return useObserver(() => (
        <Select
            className={pageStyles.dropdownGray + " " + (className ?? "")}
            values={values}
            options={store.items}
            placeholder={placeholder}
            labelField={store.labelField}
            onChange={onAddressChoose}
            clearable={true}
            searchable={true}
            searchBy={store.labelField}
            valueField={store.valueField}
            additionalProps={{ onChange: mapChangeEventToValue(setQuery) }} />
    ));
}