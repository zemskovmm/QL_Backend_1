import React, { FC } from "react";
import styles from "./switch.module.css";

export type SwitchPropsValues = "email" | "phone";

export type SwitchProps = {
    value?: SwitchPropsValues;
    onChange: (x: SwitchPropsValues) => void;
};

const isButtonActive = (state: SwitchPropsValues) => (x: SwitchPropsValues) => state === x;

export const Switch: FC<SwitchProps> = ({ value = "email", onChange }) => {
    const isButtonActiveWithState = isButtonActive(value);

    const emailButtonStyles = `${styles.switch__button} col-6 ${
        isButtonActiveWithState("email") ? styles.switch__button_active : ""
    }`;

    const phoneButtonStyles = `${styles.switch__button} col-6 ${
        isButtonActiveWithState("phone") ? styles.switch__button_active : ""
    }`;

    return (
        <div className={styles.switch + " d-flex row m-0 mb-3"}>
            <button className={emailButtonStyles} type="button" onClick={() => onChange("email")}>
                РЕГИСТРАЦИЯ ПО ЭЛЕКТРОННОЙ ПОЧТЕ
            </button>
            <button className={phoneButtonStyles} type="button" onClick={() => onChange("phone")}>
                РЕГИСТРАЦИЯ ПО НОМЕРУ ТЕЛЕФОНА
            </button>
        </div>
    );
};
