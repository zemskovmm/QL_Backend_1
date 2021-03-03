import React, { FC, useState } from "react";
import { Switch, SwitchPropsValues } from "src/components/NewComponents/RegistrationComponents/Switch/Switch";
import { preventDefaultEvent, useRootStore } from "src/utilities";
import { useObserver } from "mobx-react";
import { Email } from "src/components/NewComponents/RegistrationComponents/Email/Email";
import { Mobile } from "src/components/NewComponents/RegistrationComponents/Mobile/Mobile";
import styles from "./registration.module.css";
import { Password } from "src/components/NewComponents/RegistrationComponents/Password/Password";
import pageStyles from "src/styles/page.module.css";
import { RouterLink } from "mobx-state-router";
import { UserRouteNames } from "src/routes";
import { HeaderTitleType } from "src/stores/ui/header";
import { HeaderTitle } from "src/components/NewComponents/Header/Header";

export const RegisterPage = () => {
    const [switchState, setSwitchState] = useState<SwitchPropsValues>("email");
    const {
        rootStore: { registerStore },
    } = useRootStore();
    return useObserver(() => {
        const FormWidgetSwitch =
            switchState === "email" ? (
                <Email
                    errors={registerStore.errors.email}
                    value={registerStore.email}
                    onChange={(x) => (registerStore.email = x)}
                />
            ) : (
                <Mobile errors={registerStore.errors.mobile} />
            );
        return (
            <>
                <HeaderTitle
                    entry={{
                        type: HeaderTitleType.text,
                        title: "Регистрация",
                        subtitle:
                            "Пожалуйста, подтвердите электронную почту или телефон для прохождения процедуры регистрации.",
                    }}
                />
                <div>
                    <div className={styles.registration__back}>
                        <div className={styles.registration__maxWidth}>
                            <form
                                className={styles.registration__form + " d-flex flex-column"}
                                onSubmit={preventDefaultEvent(() => registerStore.register())}
                            >
                                <Switch value={switchState} onChange={(x) => setSwitchState(x)} />
                                {FormWidgetSwitch}
                                <Password
                                    value={registerStore.password}
                                    repeatValue={registerStore.repeatPassword}
                                    onChange={(x) => (registerStore.password = x)}
                                    onRepeatChange={(x) => (registerStore.repeatPassword = x)}
                                    errors={registerStore.errors.password}
                                    repeatPasswordErrors={registerStore.errors.repeatPassword}
                                />
                                <div>
                                    <button
                                        className={pageStyles.buttonDarkBlue + " " + styles.registration__buttonLeft}
                                        type="submit"
                                    >
                                        Подтвердить адрес электронной почты
                                    </button>
                                    <RouterLink routeName={UserRouteNames.login}>
                                        <button className={pageStyles.buttonDarkBlue} type="button">
                                            Уже зарегистрирован
                                        </button>
                                    </RouterLink>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </>
        );
    });
};

export default RegisterPage;
