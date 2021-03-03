import React from "react";
import styles from "./user.module.css";
import { useRootStore } from "src/utilities";
import { useObserver } from "mobx-react";
import { RouterLink } from "mobx-state-router";
import { UserRouteNames } from "src/routes";
import iconAvatar from "src/assets/icon/avatar.svg";

export const User = () =>
    useObserver(() => {
        const {
            rootStore: { loginStore, userStore: profileStore },
        } = useRootStore();
        return (
            <>
                <div className="d-flex justify-content-center">
                    <div className="d-flex flex-column justify-content-center">
                        <div className={styles.User__title}>{`${profileStore.profile?.name ?? ""}`}</div>
                        <div className={styles.User__title}>{`[${profileStore.profile?.email ?? ""}]`}</div>
                        <button className={styles.User__subtitle} onClick={() => loginStore.LogOut()}>
                            Выйти из системы
                        </button>
                    </div>
                    <RouterLink className={styles.news__link} routeName={UserRouteNames.profile}>
                        <div className={styles.User__avatar}>
                            <img src={iconAvatar} alt={"avatar"} />
                        </div>
                    </RouterLink>
                </div>
            </>
        );
    });
