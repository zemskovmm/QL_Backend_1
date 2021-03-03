import React from "react";
import { PageLayout } from "src/components/LoginLayout";
import { useRootStore } from "src/utilities";

export const DashboardPage = () => {
    const {
        rootStore: { adminLoginStore },
    } = useRootStore();

    return (
        <PageLayout>
            <button onClick={() => adminLoginStore.LogOut()}>Выйти</button>
        </PageLayout>
    );
};

export default DashboardPage;
