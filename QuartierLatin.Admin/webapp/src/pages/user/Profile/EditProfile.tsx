import React, { FC } from "react";
import styles from "./editProfile.module.css";
import pageStyles from "src/styles/page.module.css";
import { preventDefaultEvent, useRootStore } from "src/utilities";
import { Fade, Form } from "reactstrap";
import { useObserver } from "mobx-react";
import { ValidatedInput } from "src/components/ValidatedInput/ValidatedInput";
import { UpdateProfileStore } from "src/stores/user/UpdateProfileStore";
import { HeaderTitleType } from "src/stores/ui/header";
import { HeaderTitle } from "src/components/NewComponents/Header/Header";

const ProfileUpdateBlock: FC<{ store: UpdateProfileStore }> = ({ store }) => {
    return useObserver(() => (
        <div className="d-flex row mb-3">
            <label className="d-flex flex-column mb-3 mb-xl-0 col-12 col-xl-6">
                <ValidatedInput
                    value={store.firstName}
                    errors={store.errors.firstName}
                    callback={(value) => (store.firstName = value)}
                    placeholder={"Имя участника"}
                />
            </label>
            <label className="d-flex flex-column mb-3 mb-xl-0 col-12 col-xl-6">
                <ValidatedInput
                    value={store.lastName}
                    errors={store.errors.lastName}
                    callback={(value) => (store.lastName = value)}
                    placeholder={"Фамилия участника"}
                />
            </label>
        </div>
    ));
};

export const EditProfilePage: FC = () => {
    const {
        rootStore: { updateProfileStore },
    } = useRootStore();
    return useObserver(() => (
        <>
            <HeaderTitle
                entry={{
                    type: HeaderTitleType.text,
                    title: "Редактирование профиля",
                    subtitle: "Здесь необходимо указать свои настоящие имя и фамилию.",
                }}
            />
            <div>
                <Form
                    onSubmit={preventDefaultEvent(() => updateProfileStore.updateProfile())}
                    className={styles.editProfile__back}
                >
                    <div className={styles.editProfile__maxWidth}>
                        <form className={styles.editProfile__form}>
                            <div className="d-flex flex-column">
                                <ProfileUpdateBlock store={updateProfileStore} />
                            </div>
                        </form>
                        <button className={pageStyles.buttonDarkBlue} type="submit">
                            Сохранить изменения
                        </button>{" "}
                        &nbsp;
                        <span className={"text-muted"}>{updateProfileStore.email}</span>
                    </div>
                </Form>
            </div>
        </>
    ));
};

export default EditProfilePage;
