import { lazy } from "react";

// Anon pages.
export const AnonNotFoundPage = lazy(() => import("../pages/anon/AnonNotFoundPage"));

// User pages.
export const LoginPage = lazy(() => import("../pages/user/login/LoginPage"));
export const RegisterPage = lazy(() => import("../pages/user/Register/RegisterPage"));
export const EditProfilePage = lazy(() => import("../pages/user/Profile/EditProfile"));
export const AccountConfirmPage = lazy(() => import("../pages/user/AccountConfirm/AccountConfirmPage"));

// Admin pages.
export const AdminLoginPage = lazy(() => import("../pages/admin/Login/AdminLoginPage"));
export const AdminDashboardPage = lazy(() => import("../pages/admin/dashboard/dashboard-page"));
