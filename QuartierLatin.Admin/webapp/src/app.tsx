import React from "react";
import createBrowserHistory from "history/createBrowserHistory";
import "react-datepicker/dist/react-datepicker.css";
import "./global.css";
import "./globalBS.css";
import { observer, Provider } from "mobx-react";
import { RootStore } from "src/stores/RootStore";
import { HistoryAdapter } from "mobx-state-router";
import { UserShell } from "src/components/UserShell/UserShell";
import { AdminShell } from "src/components/AdminShell/AdminShell";
import { AnonShell } from "src/components/AnonShell/AnonShell";
import "mobx-react-lite/batchingForReactDom";

let root: RootStore;

const ensureInitialized = () => {
    if (root) return;
    root = new RootStore();
    const historyAdapter = new HistoryAdapter(root.routerStore, createBrowserHistory());
    historyAdapter.observeRouterStateChanges();
};

export const App = observer(() => {
    ensureInitialized();
    const route = root.routerStore.routerState.routeName;
    return (
        <Provider rootStore={root}>
            {route.startsWith("admin-") ? <AdminShell /> : route.startsWith("user-") ? <UserShell /> : <AnonShell />}
        </Provider>
    );
});
