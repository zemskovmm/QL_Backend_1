import React, { FC } from "react";
import { useRootStore } from "src/utilities";

export const AccountConfirmPage: FC = () => {
    const { userConfirmAccountStore } = useRootStore().rootStore;

    if (!userConfirmAccountStore.loading && userConfirmAccountStore.error)
        return <div style={{ color: "tomato" }}>{userConfirmAccountStore.error}</div>;

    return <>Проверяем...</>;
};

export default AccountConfirmPage;
