import React, { FC } from "react";
import styles from "./logo.module.css";
import img from "src/assets/logo-mob-removebg-preview.png";

export type LogoProps = {
    color?: "light" | "dark";
    className?: string;
};

export const Logo: FC<LogoProps> = ({ color = "light", className = "" }) => (
    <div className={styles[color] + " d-flex " + styles[className]}>
        <div className={styles.Logo__avatar}>
            <img src={img} alt="" />
        </div>
        <div className={styles.Logo__text + " d-flex flex-column"}>
            <div className={styles.Logo__title}>Btw i use archlinux</div>
            <div className={styles.Logo__subtitle}>sample text</div>
        </div>
    </div>
);
