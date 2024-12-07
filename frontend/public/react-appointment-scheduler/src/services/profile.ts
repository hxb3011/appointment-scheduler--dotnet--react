import { apiServer } from "../utils/api";
import { getAccessToken } from "./auth";

type BaseProfileErrorResponse = {
    type: "error";
    message?: any;
}

export type ProfileRequest = {
    patient?: number;
    full_name?: string;
    birthdate?: string;
    gender?: 'M' | 'F';
}

export type ProfilesRequest = {
    offset?: number;
    count?: number;
}

export type Profile = {
    id?: number;
    patient?: number;
    full_name?: string;
    // date_of_birth?: string;
    birthdate?: string;
    gender?: 'M' | 'F';
}

export type ReadProfileResponse = BaseProfileErrorResponse | (Profile & {
    type: "ok";
})

export type ProfileResponse = BaseProfileErrorResponse | {
    type: "ok";
    message?: any;
}

export type ProfilesResponse = BaseProfileErrorResponse | (Profile[] & {
    type: "ok";
})

export async function getProfiles(request: ProfilesRequest): Promise<ProfilesResponse> {
    try {
        const token = getAccessToken();
        const params = new URLSearchParams();
        request.count && params.set("count", request.count.toString());
        request.offset && params.set("offset", request.offset.toString());
        const response: Response = await fetch(apiServer + "profile/patient/current" + (params.size ? params.toString() : ''), {
            headers: {
                ...(token ? { "Authorization": `Bearer ${token}` } : {})
            },
            method: "GET"
        });
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
        return result
    } catch (error) {
        return {
            type: "error",
            message: (error as any).toString()
        };
    }
}

export async function createProfile(request: ProfileRequest): Promise<ProfileResponse> {
    try {
        const token = getAccessToken();
        const response: Response = await fetch(apiServer + "profile", {
            headers: {
                "Content-Type": "application/json",
                ...(token ? { "Authorization": `Bearer ${token}` } : {})
            },
            body: JSON.stringify(request),
            method: "POST"
        });
        if ((response.status / 400) === 1) return {
            type: "error",
            message: await response.text()
        };
        if (!response.ok) return {
            type: "error",
            message: `HTTP error! status: ${response.status}; content: ${await response.text()};`
        };
        return { type: "ok", message: await response.text() };
    } catch (error) {
        return {
            type: "error",
            message: (error as any).toString() + "\n" + (error as any).stack
        };
    }
}

export async function updateProfile(request: ProfileRequest & { id: number; }): Promise<ProfileResponse> {
    try {
        const token = getAccessToken();
        const response: Response = await fetch(apiServer + "profile/" + request.id, {
            headers: {
                "Content-Type": "application/json",
                ...(token ? { "Authorization": `Bearer ${token}` } : {})
            },
            body: JSON.stringify(request),
            method: "PUT"
        });
        if ((response.status / 400) === 1) return {
            type: "error",
            message: await response.text()
        };
        if (!response.ok) return {
            type: "error",
            message: `HTTP error! status: ${response.status}; content: ${await response.text()};`
        };
        return { type: "ok", message: await response.text() };
    } catch (error) {
        return {
            type: "error",
            message: (error as any).toString()
        };
    }
}

export async function getProfile(id: number): Promise<ReadProfileResponse> {
    try {
        const token = getAccessToken();
        const response: Response = await fetch(apiServer + "profile/" + id, {
            headers: (token ? { "Authorization": `Bearer ${token}` } : {}),
            method: "GET"
        });
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