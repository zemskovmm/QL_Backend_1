import * as React from "react";
import { RouterView } from "mobx-state-router";
import { useRootStore } from "src/utilities";
import { AnonViewMap, UserRouteNames } from "src/routes";
import { useObserver } from "mobx-react";
import { Header, HeaderLink } from "src/components/NewComponents/Header/Header";
import { useIsAnonRoute } from "src/utils/useRouteCheck";
import { Footer } from "src/components/NewComponents/Footer/Footer";
import { SuspensePlaceholder } from "src/components/SuspensePlaceholder/SuspensePlaceholder";

export const AnonWidget = () => <HeaderLink name={UserRouteNames.login} text={"ВОЙТИ"} />;

export const AnonShell = () => {
    const {
        rootStore: { routerStore, headerStore },
    } = useRootStore();
    return useObserver(() => (
        <>
            {useIsAnonRoute() && <Header HeaderNav={<></>} UserWidget={<AnonWidget />} />}
            <SuspensePlaceholder>
                <RouterView routerStore={routerStore} viewMap={AnonViewMap} />
            </SuspensePlaceholder>
            <Footer />
        </>
    ));
};
