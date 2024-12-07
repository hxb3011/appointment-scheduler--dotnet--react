import { apiServer } from "../utils/api";

type BaseAuthErrorResponse = {
    type: "error";
    message?: string;
}

export type AuthRequest = FormData
export type RegisterRequest = FormData;

export type AuthResponse = BaseAuthErrorResponse | {
    type: "ok";
    access_token: string;
}

export type RegisterResponse = BaseAuthErrorResponse | {
    type: "ok";
    message?: string;
}

export async function login(request: AuthRequest): Promise<AuthResponse> {
    try {
        const response: Response = await fetch(apiServer + "auth/authenticate", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                username: request.get("username"),
                password: request.get("password"),
            }),
        });
        if ((response.status / 400) == 1) return {
            type: "error",
            message: await response.json()
        };
        if (!response.ok) return {
            type: "error",
            message: `HTTP error! status: ${response.status}`
        };
        const result = await response.json();
        return { type: "ok", ...result }
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
            body: JSON.stringify({
                phone: request.get("username"),
                email: request.get("email"),
                full_name: request.get("full_name"),
                password: request.get("password"),
                otp: 123456
            })
        });
        if ((response.status / 400) == 1) return {
            type: "error",
            message: await response.json()
        };
        if (!response.ok) return {
            type: "error",
            message: `HTTP error! status: ${response.status}`
        };
        const result = await response.json();
        return { type: "ok", message: result.toString() };
    } catch (error) {
        return {
            type: "error",
            message: (error as any).toString()
        };
    }
}

export function setAccessToken(token?: string): void {
    console.log("setAccessToken");
    if (token) sessionStorage.setItem("access_token", token);
    else sessionStorage.removeItem("access_token");
}

export function getAccessToken(): string | null {
    return sessionStorage.getItem("access_token");
}