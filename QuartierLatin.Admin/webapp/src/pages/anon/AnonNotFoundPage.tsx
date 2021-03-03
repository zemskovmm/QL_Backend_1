import React, { FC } from "react";
import { useObserver } from "mobx-react";
import pageStyles from "src/styles/page.module.css";
import { UserRouteNames } from "src/routes";
import { RouterLink } from "mobx-state-router";
import { HeaderTitleType } from "src/stores/ui/header";
import { HeaderTitle } from "src/components/NewComponents/Header/Header";

export const AnonNotFoundPage: FC = ({}) => {
    return useObserver(() => (
        <>
            <HeaderTitle
                entry={{
                    type: HeaderTitleType.text,
                    title: "Неизвестная страница",
                    subtitle: "Похоже, что вы заблудились!",
                }}
            />
            <div>
                <div className={pageStyles.page__maxWidth + " container mt-3"}>
                    <h1 className={pageStyles.heading}>404</h1>
                    <p>Мы пытались что-то найти по этому адресу, но ничего не нашли!</p>
                    <RouterLink
                        className={"btn btn-secondary " + pageStyles.buttonDarkBlue}
                        routeName={UserRouteNames.mainPage}
                    >
                        Вернуться на главную страницу
                    </RouterLink>
                </div>
            </div>
        </>
    ));
};

export default AnonNotFoundPage;
