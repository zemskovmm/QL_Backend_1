import React, { FC } from "react";
import pageStyles from "src/styles/page.module.css";
import { computeInputStyleByErrorArray,  mapChangeEventToValue } from "src/utilities";
import { Input } from "reactstrap";

export type PasswordProps = {
    value: string;
    repeatValue: string;
    onChange?: (x: string) => void;
    onRepeatChange?: (x: string) => void;
    errors?: string[];
    repeatPasswordErrors?: string[];
};

export const ConfirmationCodeInput = () => (
    <label className="d-flex flex-column mb-2">
        <Input className={pageStyles.inputGrey} type="text" placeholder="0000-0000" />
        <div className={pageStyles.labelText}>Полученный код</div>
    </label>
);

export const Password: FC<PasswordProps> = ({
    onChange,
    value,
    onRepeatChange,
    repeatValue,
    errors = [],
    repeatPasswordErrors = [],
}) => {
    return (
        <>
            <div className={pageStyles.noGuttersXl + " d-flex row mb-2"}>
                <label className="d-flex flex-column col-xl-6 col-12 mb-xl-0 mb-2">
                    <Input
                        value={value}
                        onChange={mapChangeEventToValue(onChange ?? (() => {}))}
                        className={computeInputStyleByErrorArray(errors)}
                        type="password"
                        placeholder='Пароль'
                    />
                    <div className={pageStyles.labelText}>
                        Пожалуйста введите ваш пароль.
                        {errors.map((x) => (
                            <span style={{ color: "tomato" }} className="ml-2">
                                {x}
                            </span>
                        ))}
                    </div>
                </label>
                <label className="d-flex flex-column col-xl-6 col-12 ">
                    <Input
                        value={repeatValue}
                        onChange={mapChangeEventToValue(onRepeatChange ?? (() => {}))}
                        className={computeInputStyleByErrorArray(repeatPasswordErrors)}
                        type="password"
                        placeholder='Повторите пароль'
                    />
                    <div className={pageStyles.labelText}>
                        Пожалуйста, подтвердите свой пароль.
                        {repeatPasswordErrors.map((x) => (
                            <span style={{ color: "tomato" }} className="ml-2">
                                {x}
                            </span>
                        ))}
                    </div>
                </label>
            </div>
        </>
    );
};
