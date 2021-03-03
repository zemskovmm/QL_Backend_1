import React, { FC, useCallback } from "react";
import LoginStyles from "./styles.module.css";
import { mapChangeEventToValue, preventDefaultEvent, useRootStore } from "src/utilities";
import { useObserver } from "mobx-react";
import { RouterLink } from "mobx-state-router";
import { UserRouteNames } from "src/routes";
import { PageLayout } from "src/components/LoginLayout";
import pageStyles from "src/styles/page.module.css";
import { AuthErrorFields, AuthorizeStore, StoreWithErrors } from "src/stores/interfaces";
import { HeaderTitleType } from "src/stores/ui/header";
import { HeaderTitle } from "src/components/NewComponents/Header/Header";

type LoginFormObserverProps = {
    store: AuthorizeStore & StoreWithErrors<AuthErrorFields>;
    showRegisterButton?: boolean;
};
export const LoginFormObserver: FC<LoginFormObserverProps> = ({ store, showRegisterButton = true }) =>
    useObserver(() => {
        const onSubmit = useCallback(
            preventDefaultEvent(() => store.Login()),
            [store]
        );

        return (
            <PageLayout>
                <div>
                    <form onSubmit={onSubmit}>
                        <div>
                            <label className="d-flex flex-column mb-2">
                                <input
                                    className={pageStyles.inputGrey}
                                    type="text"
                                    name="election-email"
                                    id="email"
                                    placeholder="Номер телефона или адрес электронной почты"
                                    value={store.login}
                                    onChange={mapChangeEventToValue((x) => (store.login = x))}
                                />
                                <div className={pageStyles.labelText}>
                                    Пожалуйста, введите номер телефона или email.
                                    {store.errors.login?.map((x) => (
                                        <span style={{ color: "tomato" }} className="ml-2">
                                            {x}
                                        </span>
                                    ))}
                                </div>
                            </label>
                        </div>
                        <div>
                            <label className="d-flex flex-column mb-2">
                                <input
                                    className={pageStyles.inputGrey}
                                    type="password"
                                    name="password"
                                    id="election-password"
                                    placeholder="Пароль"
                                    value={store.password}
                                    onChange={mapChangeEventToValue((x) => (store.password = x))}
                                />
                                <div className={pageStyles.labelText}>
                                    Введите пароль, который был использован при регистрации.
                                    {store.errors.password?.map((x) => (
                                        <span style={{ color: "tomato" }} className="ml-2">
                                            {x}
                                        </span>
                                    ))}
                                </div>
                            </label>
                        </div>
                        <div>
                            {store.errors.apiError?.map((x) => (
                                <span style={{ color: "tomato" }}>{x}</span>
                            ))}
                        </div>
                        <div>
                            <button className={pageStyles.buttonDarkBlue + " mr-3"} type="submit">
                                Войти на сайт
                            </button>
                            {showRegisterButton && (
                                <RouterLink routeName={UserRouteNames.register} className={LoginStyles.marginLeft1}>
                                    <button className={pageStyles.buttonDarkBlue} type="button">
                                        Регистрация
                                    </button>
                                </RouterLink>
                            )}
                        </div>
                    </form>
                </div>
            </PageLayout>
        );
    });

export const LoginPage = () => {
    const {
        rootStore: { loginStore: store },
    } = useRootStore();
    return (
        <>
            <HeaderTitle
                entry={{
                    type: HeaderTitleType.text,
                    title: "Авторизация пользователя",
                    subtitle: "Пожалуйста, введите логин и пароль, чтобы воспользоваться сервисом.",
                }}
            />
            <LoginFormObserver store={store} />
        </>
    );
};

export default LoginPage;
