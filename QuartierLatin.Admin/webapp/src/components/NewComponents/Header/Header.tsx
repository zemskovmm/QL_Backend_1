import React, { FC } from "react";
import styles from "./header.module.css";
import { Logo } from "src/components/NewComponents/UI/Logo/Logo";
import { RouterLink } from "mobx-state-router";
import { RouterLinkProps } from "mobx-state-router/dist/types/components/router-link";
import { useRootStore } from "src/utilities";
import { HeaderEntry, HeaderTitleType } from "src/stores/ui/header";
import { UserRouteNames } from "src/routes";
import { useObserver } from "mobx-react";

export const RouterNavLink: FC<RouterLinkProps> = (props) => {
    const { className, activeClassName, ...pass } = props;
    return (
        <RouterLink
            className={styles.header__headLink + " " + className}
            activeClassName={styles.header__headLink_active}
            {...pass}
        />
    );
};

export const HeaderLink = ({ name, text }: { name: string; text: string }) => (
    <RouterNavLink routeName={name}>{text}</RouterNavLink>
);

type HeaderHeadProps = {
    HeaderNav?: React.ReactNode;
    UserWidget?: React.ReactNode;
};

const HeaderHead: FC<HeaderHeadProps> = ({ HeaderNav, UserWidget }) => {
    const {
        rootStore: { loginStore, headerStore: store },
    } = useRootStore();
    return useObserver(() => {
        const headerMobile = `${styles.header__mobile} ${store.headerOpen ? styles.header__mobile_open : ""}`;
        const headerJustify = HeaderNav && UserWidget ? "justify-content-between" : "justify-content-center";
        return (
            <div
                className={
                    styles.header__head + " d-flex justify-content-lg-between align-items-center " + headerJustify
                }
            >
                <RouterLink className={styles.header__headLogo} routeName={UserRouteNames.mainPage}>
                    <Logo className="headerLogoMobile" />
                </RouterLink>
                {HeaderNav && (
                    <div className={headerMobile}>
                        <button className={styles.header__burger} onClick={() => store.menuOpen()}>
                            <svg
                                width="30"
                                height="20"
                                viewBox="0 0 30 20"
                                fill="none"
                                xmlns="http://www.w3.org/2000/svg"
                            >
                                <rect className={styles.header__burger_1} width="30" height="2" fill="white" />
                                <rect className={styles.header__burger_2} y="9" width="30" height="2" fill="white" />
                                <rect className={styles.header__burger_3} y="18" width="30" height="2" fill="white" />
                            </svg>
                        </button>
                        <div className={styles.header__headMenu} onClick={() => (store.headerOpen = false)}>
                            {HeaderNav}
                            <button
                                className={styles.header__headLink + " d-flex p-0 " + styles.header__headLink_button}
                                onClick={() => loginStore.LogOut()}
                            >
                                ВЫЙТИ ИЗ СИСТЕМЫ
                            </button>
                        </div>
                    </div>
                )}
                {UserWidget && <div className={styles.header__headUser}>{UserWidget}</div>}
            </div>
        );
    });
};

export type HeaderProps = {
    HeaderNav?: React.ReactNode;
    UserWidget?: React.ReactNode;
};

export const Header: FC<HeaderProps> = ({ HeaderNav, UserWidget }) => {
    return useObserver(() => (
        <div className={styles.header__back}>
            <div className={styles.header__maxWidth + " pb-0"}>
                <HeaderHead HeaderNav={HeaderNav} UserWidget={UserWidget} />
            </div>
        </div>
    ));
};

type HeaderStatusProps = {
    entry: HeaderEntry;
};

const HeaderStatus: FC<HeaderStatusProps> = ({ entry }) => {
    const subtitleVar = window.innerWidth > 768 ? entry.subtitle : entry.subtitleMobile ?? entry.subtitle;
    return (
        <div className={styles.header__status + " d-flex flex-wrap align-items-center"}>
            {entry.pill && <div className={styles.header__statusPill}>{entry.pill.text}</div>}
            <div className={styles.header__statusText + " mt-3"}>{subtitleVar}</div>
        </div>
    );
};

type HeaderTitleProps = {
    entry: HeaderEntry;
};

export const HeaderTitle: FC<HeaderTitleProps> = ({ entry }) => {
    return (
        <div className={styles.header__back}>
            <div className={styles.header__maxWidth + " pt-0"}>
                <div className={styles.header__title + " d-flex align-items-center"}>
                    {entry.type === HeaderTitleType.text && (
                        <h1 className={styles.header__h1 + " mr-2"}>{entry.title}</h1>
                    )}
                    {entry.type === HeaderTitleType.linkGroup && (
                        <div className="d-flex flex-wrap">
                            {entry.linkGroup!.map((link, index) => (
                                <RouterNavLink
                                    key={index}
                                    className={styles.header__h1_link + " mr-3"}
                                    routeName={link.route}
                                    params={link.args}
                                >
                                    {link.title}
                                </RouterNavLink>
                            ))}
                        </div>
                    )}
                </div>
                <HeaderStatus entry={entry} />
            </div>
        </div>
    );
};
