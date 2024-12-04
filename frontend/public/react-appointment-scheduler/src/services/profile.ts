import { useState } from "react";
import { apiServer } from "../utils/api";
import { getAccessToken } from "./auth";
import { json } from "stream/consumers";
import { count } from "console";

type BaseProfileErrorResponse = {
    type: "error";
    message?: string;
}

export type ProfileRequest = {
    patient?: number;
    full_name?: string;
    birthdate?: string;
    gender?: 'M' | 'F';
}

export type Profile = {
    id?: number;
    patient?: number;
    full_name?: string;
    birthdate?: string;
    gender?: 'M' | 'F';
}

export type ProfileResponse = BaseProfileErrorResponse | (Profile & {
    type: "ok";
})

export type CreateProfileResponse = BaseProfileErrorResponse | {
    type: "ok";
    message?: string;
}

export type ProfilesResponse = BaseProfileErrorResponse | (Profile[] & {
    type: "ok";
})

export async function getProfiles(offset: number, count: number): Promise<ProfilesResponse> {
    try {
        const token = getAccessToken();
        const response: Response = await fetch(apiServer + "profile/current", {
            headers: {
                "Content-Type": "application/json",
                ...(token ? { "Authorization": `Bearer ${token}` } : {})
            },
            body: JSON.stringify({ offset, count }),
            method: "GET"
        });
        if ((response.status / 400) == 1) return {
            type: "error",
            message: await response.json()
        };
        if (!response.ok) return {
            type: "error",
            message: `HTTP error! status: ${response.status}; content: ${JSON.stringify(response)};`
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

export async function createProfile(request: ProfileRequest): Promise<CreateProfileResponse> {
    try {
        const token = getAccessToken();
        const response: Response = await fetch(apiServer + "profile/current", {
            headers: {
                "Content-Type": "application/json",
                ...(token ? { "Authorization": `Bearer ${token}` } : {})
            },
            body: JSON.stringify(request),
            method: "POST"
        });
        if ((response.status / 400) == 1) return {
            type: "error",
            message: await response.json()
        };
        if (!response.ok) return {
            type: "error",
            message: `HTTP error! status: ${response.status}; content: ${JSON.stringify(response)};`
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

export async function getProfile(id: number): Promise<ProfilesResponse> {
    try {
        const token = getAccessToken();
        const response: Response = await fetch(apiServer + "profile/current/" + id, {
            headers: {
                ...(token ? { "Authorization": `Bearer ${token}` } : {})
            },
            method: "GET"
        });
        if ((response.status / 400) == 1) return {
            type: "error",
            message: await response.json()
        };
        if (!response.ok) return {
            type: "error",
            message: `HTTP error! status: ${response.status}; content: ${JSON.stringify(response)};`
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