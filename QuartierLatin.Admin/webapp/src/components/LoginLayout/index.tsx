import React, { FC } from "react";
import style from "./styles.module.css";

export const PageLayout: FC = ({ children }) => <div className={style.loginLayout}>{children}</div>;
