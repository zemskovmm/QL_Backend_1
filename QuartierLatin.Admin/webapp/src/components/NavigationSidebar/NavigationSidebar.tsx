import { FC } from "react";
import styles from "./navigation-sidebar.module.css";
import * as React from "react";
import { Logo } from "src/components/NewComponents/UI/Logo/Logo";

export const LocalizedNavigationDelimiter: FC<{ content: string }> = ({ content }) => (
    <div className={styles.navigationDelimiter}>{content}</div>
);

export const NavigationSidebar: FC = ({ children }) => (
    <div className={styles.navigationSideBar}>
        <Logo color={"dark"} className={"mt-4"} />
        {children}
        <div className={styles.navigationFooter + " mb-3"}>© 2020 - Sample text copyright</div>
    </div>
);
