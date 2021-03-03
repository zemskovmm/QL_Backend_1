import * as React from "react";
import { RouterView } from "mobx-state-router";
import { useRootStore } from "src/utilities";
import { UserViewMap } from "src/routes";
import { useObserver } from "mobx-react";
import { Header } from "src/components/NewComponents/Header/Header";
import { useIsUserRoute } from "src/utils/useRouteCheck";
import { User } from "src/components/NewComponents/UI/User/User";
import { Footer } from "src/components/NewComponents/Footer/Footer";
import { SuspensePlaceholder } from "src/components/SuspensePlaceholder/SuspensePlaceholder";

export const UserRoutes = () => {
    return <></>;
};

export const UserShell = () => {
    const {
        rootStore: { routerStore, userRpc },
    } = useRootStore();
    const [routes, userWidget] = userRpc.isAuthorized ? [<UserRoutes />, <User />] : [];

    return useObserver(() => (
        <>
            {useIsUserRoute() && <Header HeaderNav={routes} UserWidget={userWidget} />}
            <SuspensePlaceholder>
                <RouterView routerStore={routerStore} viewMap={UserViewMap} />
            </SuspensePlaceholder>
            <Footer />
        </>
    ));
};
