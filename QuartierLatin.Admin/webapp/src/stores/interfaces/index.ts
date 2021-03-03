export interface AuthErrorFields {
    password?: "required"[];
    login?: ("required" | "invalid-email")[];
    apiError?: string[];
}

export interface StoreWithErrors<T> {
    errors: T;
}

export interface ResettableFieldsStore {
    resetFields: () => void;
}

export interface AuthorizeStore {
    Login: () => void;
    LogOut: () => void;
    login: string;
    password: string;
}
