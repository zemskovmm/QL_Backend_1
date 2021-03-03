import React, { FC } from "react";
import styles from "./footer.module.css";
import { Logo } from "src/components/NewComponents/UI/Logo/Logo";
import { UserRouteNames } from "src/routes";
import { RouterLink } from "mobx-state-router";

export const Footer: FC = () => (
    <div className={styles.footer__back}>
        <div className={styles.footer__maxWidth + " d-flex"}>
            <div className={styles.footer__col}>
                <RouterLink routeName={UserRouteNames.mainPage}>
                    <Logo />
                </RouterLink>
            </div>
            <div className={styles.footer__col}>
                <div className={styles.footer__colItem}>{/*Страница проекта*/}</div>
            </div>
            <div className={styles.footer__col + " " + styles.footer__colCopy}>© 2020 - Sample text copyright</div>
        </div>
    </div>
);
