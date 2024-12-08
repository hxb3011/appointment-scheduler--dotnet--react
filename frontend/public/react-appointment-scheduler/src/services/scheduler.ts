import { apiServer } from "../utils/api";
import { getAccessToken } from "./auth";

type BaseErrorResponse = {
    type: "error";
    message?: any;
}

export type Part = {
    id?: number;
    start?: string;
    end?: string;
}

export type PartsResponse = BaseErrorResponse | (Part[] & {
    type: "ok";
})

export async function getParts(): Promise<PartsResponse> {
    try {
        const token = getAccessToken();
        const response: Response = await fetch(apiServer + "scheduler/part", {
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