import React, { FC, Suspense } from "react";

const Fallback = () => <div>Загрузка...</div>;

export const SuspensePlaceholder: FC = ({ children }) => <Suspense fallback={Fallback}>{children}</Suspense>;
