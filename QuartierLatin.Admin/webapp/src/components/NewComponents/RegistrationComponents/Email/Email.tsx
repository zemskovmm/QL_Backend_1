import React, { FC } from "react";
import pageStyles from "src/styles/page.module.css";
import {  mapChangeEventToValue } from "src/utilities";
import { Input } from "reactstrap";

export type EmailProps = {
    value: string;
    onChange: (x: string) => void;
    errors?: string[];
};

export const Email: FC<EmailProps> = ({ value, onChange, errors = [] }) => {
    return (
        <label className="d-flex flex-column mb-2">
            <Input
                value={value}
                onChange={mapChangeEventToValue(onChange)}
                className={errors[0] ? pageStyles.inputError : pageStyles.inputGrey}
                type="text"
                placeholder='example@example.com'
            />
            <div className={pageStyles.labelText}>
                На указанный адрес электронной почты будет отправлено письмо с кодом подтверждения.
                {errors.map((x) => (
                    <span style={{ color: "tomato" }} className="ml-2">
                        {x}
                    </span>
                ))}
            </div>
        </label>
    );
};
