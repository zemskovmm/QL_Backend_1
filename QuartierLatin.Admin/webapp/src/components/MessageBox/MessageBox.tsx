import React, { FC } from "react";
import { Button, Modal, ModalBody, ModalFooter, ModalHeader } from "reactstrap";
import pageStyles from "src/styles/page.module.css";

export type MessageBoxProps = {
    header: string,
    message: string,
    isOpen: boolean,
    yesButton: string,
    yes: () => void,
    showNo?: boolean,
    noButton?: string,
    no?: () => void,
}

export const MessageBox: FC<MessageBoxProps> = ({ header, message, yesButton, isOpen, yes, showNo, noButton, no }) =>
    <Modal isOpen={isOpen}
           toggle={() => yes()}>
        <ModalHeader toggle={() => yes()}>
            {header}
        </ModalHeader>
        <ModalBody>
            {message}
        </ModalBody>
        <ModalFooter>
            <Button className={pageStyles.buttonDarkBlue}
                    onClick={() => yes()}>
                {yesButton}
            </Button>
            {showNo && (
                <Button className={pageStyles.buttonGray}
                        onClick={() => no!()}>
                    {noButton}
                </Button>
            )}
        </ModalFooter>
    </Modal>;