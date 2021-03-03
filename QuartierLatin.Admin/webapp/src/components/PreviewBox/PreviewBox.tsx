import React, { FC } from "react";
import { Button, Modal, ModalBody, ModalFooter, ModalHeader } from "reactstrap";
import { useObserver } from "mobx-react";
import pageStyles from "src/styles/page.module.css";
import { PreviewBoxStore } from "src/components/PreviewBox/PreviewBoxStore";

export type PreviewBoxProps = {
    store: PreviewBoxStore;
}

export const PreviewBox: FC<PreviewBoxProps> = ({ store }) => {
    return useObserver(() => (
        <Modal isOpen={store.isPreviewBoxOpen}
               size={'lg'}
               toggle={() => store.hide()}>
            <ModalHeader toggle={() => store.hide()}>
                {'Просмотр работы'}
            </ModalHeader>
            <ModalBody>
                <img style={{ width: '100%' }}
                     src={store.imageUrl}
                     alt={'art contest'} />
            </ModalBody>
            <ModalFooter>
                <Button className={pageStyles.buttonDarkBlue}
                        onClick={() => store.hide()}>
                    {'Закрыть'}
                </Button>
            </ModalFooter>
        </Modal>
    ));
};