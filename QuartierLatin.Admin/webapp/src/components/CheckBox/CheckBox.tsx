import React, { FC } from "react";
import pageStyles from "src/styles/page.module.css";

type CheckBoxProps = {
    value: boolean;
    onChange: (x: boolean) => void;
    label: string;
};

export const CheckBox: FC<CheckBoxProps> = ({ onChange, value, label }) => (
    <div className="d-flex mr-3 align-items-center" onClick={() => onChange(!value)}>
        <input className={pageStyles.inputHidden} type="checkbox" />
        <div className={pageStyles.checkBox + " mr-2"}>
            {value && <img src="/icon/icon-done.svg" alt="icon-done" />}
        </div>
        <span className={pageStyles.checkBoxText}>{label}</span>
    </div>
);
