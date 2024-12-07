import { apiServer } from "../utils/api";

type BaseAuthErrorResponse = {
    type: "error";
    message?: any;
}

export type AuthRequest = FormData
export type RegisterRequest = FormData;

export type AuthResponse = BaseAuthErrorResponse | {
    type: "ok";
    access_token?: string;
}

export type RegisterResponse = BaseAuthErrorResponse | {
    type: "ok";
    message?: any;
}

export async function login(request: AuthRequest): Promise<AuthResponse> {
    try {
        const response: Response = await fetch(apiServer + "auth/token", {
            method: "POST",
            body: request,
        });
        if ((response.status / 400) == 1) return {
            type: "error",
            message: await response.text()
        };
        if (!response.ok) return {
            type: "error",
            message: `HTTP error! status: ${response.status}`
        };
        const result = await response.json();
        result.type = "ok";
        return result;
    } catch (error) {
        return {
            type: "error",
            message: (error as any).toString()
        };
    }
}

export async function register(request: RegisterRequest): Promise<RegisterResponse> {
    try {
        const response: Response = await fetch(apiServer + "auth/register", {
            method: "POST",
            body: request
        });
        if ((response.status / 400) == 1) return {
            type: "error",
            message: await response.text()
        };
        if (!response.ok) return {
            type: "error",
            message: `HTTP error! status: ${response.status}`
        };
        return { type: "ok", message: response.text() };
    } catch (error) {
        return {
            type: "error",
            message: (error as any).toString()
        };
    }
}

export function setAccessToken(token?: string): void {
    if (token) sessionStorage.setItem("access_token", token);
    else sessionStorage.removeItem("access_token");
}

export function getAccessToken(): string | null {
    return sessionStorage.getItem("access_token");
}