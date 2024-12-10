import { apiServer } from "../utils/api";
import { getAccessToken } from "./auth";

type BaseDoctorErrorResponse = {
    type: "error";
    message?: any;
}

export type DoctorRequest = {
    full_name: string;
    user_name: string;
    email: string;
    phone: string;
}

export type Doctor = {
    id?: number;
    full_name?: string;
    username?: string;
    email?: string;
    phone?: string;
    role?: number;
    position?: string;
    certificate?: string;
    image?: string;
    gender?: 'M' | 'F';
    roleId: number;
    userId: number;
}

export type DoctorResponse = BaseDoctorErrorResponse | (Doctor & {
    type: "ok";
})

export type DoctorsResponse = BaseDoctorErrorResponse | (Doctor[] & {
    type: "ok";
})

export async function getDoctors(): Promise<DoctorsResponse> {
    try {
        const token = getAccessToken();
        const response: Response = await fetch(apiServer + "doctor", {
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