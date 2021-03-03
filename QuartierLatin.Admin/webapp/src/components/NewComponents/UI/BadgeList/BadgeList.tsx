import React, { FC } from "react";
import styles from "./badgeList.module.css";

export type BadgeListProps = { statuses: BadgeTypes[] };

export enum BadgeTypes {
    planned = "badge-planned",
    cannot = "badge-cannot",
    active = "badge-active",
    can = "badge-can",
    completed = "badge-completed",
    voted = "badge-voted",
}

export const statusList = {
    "badge-planned": styles.badge__grey,
    "badge-cannot": styles.badge__red,
    "badge-active": styles.badge__green,
    "badge-can": styles.badge__green,
    "badge-completed": styles.badge__grey,
    "badge-voted": styles.badge__green,
};

export const NameList = {
    "badge-planned": 'Запланировано',
    "badge-cannot": 'Вы не можете проголосовать',
    "badge-active": 'Активен',
    "badge-can": 'Вы можете проголосовать',
    "badge-completed": 'Завершено',
    "badge-voted": 'Вы уже голосовали',
};

export const BadgeList: FC<BadgeListProps> = ({ statuses }) => {
    const badgeMap = statuses.map((x, index) => (
        <span key={index + "badge"} className={styles.badge + " " + statusList[x]}>
            {NameList[x]}
        </span>
    ));
    return <div className={styles.badgeList + " d-flex flex-wrap"}>{badgeMap}</div>;
};
