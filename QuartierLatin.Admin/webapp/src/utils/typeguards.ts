import { ValidationError } from "@keroosha/class-validator";

export const isErrorTypeguard = (x: unknown): x is Error => (x as Error).message !== undefined;

export const isValidationErrorTypeguard = (x: unknown): x is ValidationError[] => Array.isArray(x as ValidationError[]);
