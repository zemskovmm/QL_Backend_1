import { useRootStore } from "src/utilities";
import { HeaderTitle } from "src/components/NewComponents/Header/Header";
import { HeaderTitleType } from "src/stores/ui/header";
import { Fade } from "reactstrap";
import React from "react";
import { LoginFormObserver } from "src/pages/user/login/LoginPage";

export const AdminLoginPage = () => {
    const {
        rootStore: { adminLoginStore: store },
    } = useRootStore();
    return (
        <>
            <HeaderTitle
                entry={{
                    type: HeaderTitleType.text,
                    title: "Авторизация администратора",
                    subtitle: "Пожалуйста, введите логин и пароль, чтобы воспользоваться сервисом.",
                }}
            />
            <Fade>
                <LoginFormObserver store={store} showRegisterButton={false} />
            </Fade>
        </>
    );
};

export default AdminLoginPage;
