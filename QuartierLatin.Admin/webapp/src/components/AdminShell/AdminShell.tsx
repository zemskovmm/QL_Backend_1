import { useRootStore } from "src/utilities";
import { AdminViewMap } from "src/routes";
import { useObserver } from "mobx-react";
import { RouterView } from "mobx-state-router";
import * as React from "react";
import style from "../../styles/page.module.css";
import { LocalizedNavigationDelimiter, NavigationSidebar } from "../NavigationSidebar/NavigationSidebar";
import { FC } from "react";
import navigationStyle from "src/components/NavigationSidebar/navigation-sidebar.module.css";
import { useIsAdminRoute } from "src/utils/useRouteCheck";
import { Header } from "src/components/NewComponents/Header/Header";
import { SuspensePlaceholder } from "src/components/SuspensePlaceholder/SuspensePlaceholder";
import iconLogOut from "src/assets/icon/meeting_room.svg";
import { Fade } from "reactstrap";

const AdminNavigation: FC<{ onLogout: () => void }> = ({ onLogout }) => (
    <NavigationSidebar>
        <LocalizedNavigationDelimiter content={"Посадочная страница"} />
        <LocalizedNavigationDelimiter content={"Аккаунт администратора"} />
        <a href="#" onClick={onLogout}>
            <img src={iconLogOut} alt="" className="mr-3" />
            Выйти
        </a>
    </NavigationSidebar>
);

export const AdminShell = () => {
    const {
        rootStore: { routerStore, adminRpc, adminLoginStore },
    } = useRootStore();
    return useObserver(() =>
        adminRpc.isAuthorized ? (
            <div className={style.fullHeight}>
                <Fade className={style.fullHeight + " container ml-0"}>
                    <div className={style.flexList + " row " + style.fullHeight}>
                        <div className={"col-lg-3 " + style.darkBackground}>
                            <div className={"sticky-top mt-4"}>
                                <AdminNavigation onLogout={() => adminLoginStore.LogOut()} />
                            </div>
                        </div>
                        <div className={"col-lg-9"}>
                            <SuspensePlaceholder>
                                <RouterView routerStore={routerStore} viewMap={AdminViewMap} />
                            </SuspensePlaceholder>
                        </div>
                    </div>
                </Fade>
            </div>
        ) : (
            <>
                {useIsAdminRoute() && <Header />}
                <SuspensePlaceholder>
                    <RouterView routerStore={routerStore} viewMap={AdminViewMap} />
                </SuspensePlaceholder>
            </>
        )
    );
};
