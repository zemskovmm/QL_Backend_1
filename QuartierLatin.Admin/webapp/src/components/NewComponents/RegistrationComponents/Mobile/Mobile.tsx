import React, { FC } from "react";
import pageStyles from "src/styles/page.module.css";
import { Input } from "reactstrap";

export type MobileProps = {
    errors?: string[];
};

export const Mobile: FC<MobileProps> = ({ errors = [] }) => {
    return (
        <label className="d-flex flex-column mb-2">
            <Input
                className={errors[0] ? pageStyles.inputError : pageStyles.inputGrey}
                type="text"
                placeholder="+7 000 000 00 00"
            />
            <div className={pageStyles.labelText}>
                Мы отправим SMS с кодом подтверждения на этот номер телефона.
                {errors.map((x) => (
                    <span style={{ color: "tomato" }} className="ml-2">
                        {x}
                    </span>
                ))}
            </div>
        </label>
    );
};
