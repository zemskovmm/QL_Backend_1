import React, { DetailedHTMLProps, FC } from "react";
import styles from "./vote.module.css";
import { BadgeTypes } from "src/components/NewComponents/UI/BadgeList/BadgeList";

export type CandidateProps = {
    image: string;
    name?: string;
    description?: string;
    badges: BadgeTypes[];
    bgColor?: string;
    classElement?: string;
    onClick?: () => void;
};

export const Candidate: FC<CandidateProps> = ({ name, image, description, bgColor, classElement = '', onClick }) => {
    const bgStyle = {
        background: bgColor
    }
    const hasLabel = name || description;
    const imageStyle = hasLabel ? {} : { borderRadius: "5px" }; 
    const classWhite = bgColor ? styles.candidateWhite : ''
    return (
        <div onClick={onClick}
             className={styles.candidate + " d-flex flex-column " + classWhite + ' ' + styles[classElement]}
             style={bgStyle}>
            <img src={image} alt='' style={imageStyle} className={styles.candidate__img} />
            {hasLabel && (
                <div className={styles.candidate__content + " d-flex flex-column"}>
                    <div className={styles.candidate__title}>{name}</div>
                    <div className={styles.candidate__text}>{description}</div>
                </div>
            )}
        </div>
    );
};

export const VoteButton = (x: DetailedHTMLProps<React.ButtonHTMLAttributes<HTMLButtonElement>, HTMLButtonElement>) => (
    <button {...x} className={styles.vote__itemButton}>
        Голосовать
    </button>
);

export const VoteListBlock: FC = ({ children }) => (
    <div className={styles.vote__item + " d-flex align-items-center"}>{children}</div>
);

export const VoteMetricsBlock: FC<{ firstText: string; secondText: string; isWinner?: boolean }> = (
    { firstText, secondText, isWinner }) => {
    const highlightClass = isWinner ? ` ${styles.vote__resultItem_green}` : "";
    return (
        <div className={`${styles.vote__resultItem} ` + highlightClass + " ml-auto d-flex flex-column"}>
            <div className={styles.vote__resultCount}>{firstText}</div>
            <div className={styles.vote__resultName}>{secondText}</div>
        </div>
    );
};

export const VoteResult: FC = ({ children }) => {
    return (
        <div className={styles.vote__item + " d-flex justify-content-between"}>
            <div className={styles.vote__resultList + " " + styles.vote__resultList_green + " d-flex"}>{children}</div>
        </div>
    );
};

export const VoteList: FC = ({ children }) => {
    return <div className={styles.vote__list + " d-flex flex-wrap"}>{children}</div>;
};

export const VoteListLayout: FC = ({ children }) => (
    <div className={styles.vote__back}>
        <div className={styles.vote__maxWidth}>{children}</div>
    </div>
);