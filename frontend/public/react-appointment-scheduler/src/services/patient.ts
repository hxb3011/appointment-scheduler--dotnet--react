import { apiServer } from "../utils/api";
import { getAccessToken } from "./auth";

type BasePatientErrorResponse = {
    type: "error";
    message?: any;
}

export type PatientRequest = {
    full_name: string;
    user_name: string;
    email: string;
    phone: string;
}

export type Patient = {
    id?: number;
    full_name?: string;
    username?: string;
    email?: string;
    phone?: string;
    role?: number;
    image?: string;
}

export type PatientResponse = BasePatientErrorResponse | (Patient & {
    type: "ok";
})

export async function currentUser(): Promise<PatientResponse> {
    try {
        const token = getAccessToken();
        const response: Response = await fetch(apiServer + "patient/current", {
            headers: {
                ...(token ? { "Authorization": `Bearer ${token}` } : {})
            },
            method: "GET"
        });
        if (response.status === 401) return {
            type: "error",
            message: "unauth"
        }
        if ((response.status / 400) === 1) return {
            type: "error",
            message: await response.text()
        };
        if (!response.ok) return {
            type: "error",
            message: `HTTP error! status: ${response.status}; content: ${await response.text()};`
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

export async function currentUserImage(): Promise<{ type: "ok" | "error"; url?: string; }> {
    try {
        const token = getAccessToken();
        const response: Response = await fetch(apiServer + "patient/current/image", {
            headers: {
                ...(token ? { "Authorization": `Bearer ${token}` } : {})
            },
            method: "GET"
        });
        if (!response.ok) return { type: "error" };
        return { type: "ok", url: URL.createObjectURL(await response.blob()) };
    } catch (error) {
        return { type: "error" };
    }
}