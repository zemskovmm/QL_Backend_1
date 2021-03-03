import React, { FC } from "react";
import { useObserver } from "mobx-react";
import { FormGroup, Input } from "reactstrap";
import pageStyles from "src/styles/page.module.css";
import { mapChangeEventToValue } from "src/utilities";

type ValidatedInputErrorsProps = {
    placeholder: string,
    errors?: string[],
    className?: string,
}

export const ValidatedInputErrors: FC<ValidatedInputErrorsProps> = ({ placeholder, errors, className }) =>
    useObserver(() => {
        return <div className={pageStyles.labelText + " " + (className ?? "")}>
            <span>{placeholder}</span> &nbsp;
            {errors?.map(error => (
                <span key={error}
                      style={{ color: "tomato" }}>
                    {error}
                </span>
            ))}
        </div>
    });

type ValidatedInputProps = {
    value: string,
    placeholder: string,
    callback?: (x: string) => void,
    errors?: string[],
    type?: "textarea" | "number",
    disabled?: boolean,
}

export const ValidatedInput: FC<ValidatedInputProps> = ({ value, placeholder, errors, callback, type, disabled = false }) =>
    useObserver(() => {
        return <FormGroup className="mb-2">
            <Input type={type ?? undefined}
                   disabled={disabled}
                   className={pageStyles.inputGrey}
                   id={"create-commission-email"}
                   placeholder={placeholder}
                   value={value}
                   onChange={!!callback ? mapChangeEventToValue(callback) : () => {}} />
            <ValidatedInputErrors
                placeholder={placeholder}
                errors={errors} />
        </FormGroup>
    });
