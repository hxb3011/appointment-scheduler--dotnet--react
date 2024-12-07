import { useState } from "react";
import { apiServer } from "../utils/api";
import { getAccessToken } from "./auth";
import { json } from "stream/consumers";
import { count } from "console";
import { getProfile, getProfiles } from "./profile";
import { escape } from "querystring";

type BaseAppointmentErrorResponse = {
    type: "error";
    message?: string;
}

export type AppointmentRequest = {
    date?: string;
    begin_time?: string;
    end_time?: string;
    profile?: number;
    doctor?: number;
}

export type Appointment = {
    id?: number;
    at?: string;
    number?: number;
    state?: number;
    profile?: number;
    doctor?: number;
    payment_url?: string;
}

export type FutureAppointment = {
    id?: number;
    at?: string;
    number?: number;
    profile_fullname?: string;
}

export type AppointmentResponse = BaseAppointmentErrorResponse | (Appointment & {
    type: "ok";
})

export type CreateAppointmentResponse = BaseAppointmentErrorResponse | (Appointment & {
    type: "ok";
})

export type AppointmentsResponse = BaseAppointmentErrorResponse | (Appointment[] & {
    type: "ok";
})

export type FutureAppointmentsResponse = BaseAppointmentErrorResponse | (FutureAppointment[] & {
    type: "ok";
})

export async function getAppointments(): Promise<AppointmentsResponse> {
    try {
        const token = getAccessToken();
        const response: Response = await fetch(apiServer + "appointment/user/current", {
            headers: (token ? { "Authorization": `Bearer ${token}` } : {}),
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
        result.type = "ok";
        return result
    } catch (error) {
        return {
            type: "error",
            message: (error as any).toString()
        };
    }
}

export async function getFutureAppointments(): Promise<FutureAppointmentsResponse> {
    const appointments = await getAppointments();
    if (appointments.type == "ok") {
        const result = await Promise.all(appointments.map(async function (v) {
            if (v.profile) {
                const profile = await getProfile(v.profile);
                if (profile.type == "ok") {
                    return {
                        id: v.id,
                        at: v.at,
                        number: v.number,
                        profile_fullname: profile.full_name
                    };
                }
            }
            return {
                id: v.id,
                at: v.at,
                number: v.number
            };
        })) as any;
        result.type = "ok";
        return result;
    } else return appointments;
}

export async function createAppointment(request: AppointmentRequest): Promise<CreateAppointmentResponse> {
    try {
        const token = getAccessToken();
        const response: Response = await fetch(apiServer + `appointment?date=${encodeURIComponent(request.date || '')}&start=${encodeURIComponent(request.begin_time || '')}&end=${encodeURIComponent(request.end_time || '')}&doctor=${encodeURIComponent(request.doctor || '')}&profile=${encodeURIComponent(request.profile || '')}`, {
            headers: {
                "Content-Type": "application/json",
                ...(token ? { "Authorization": `Bearer ${token}` } : {})
            },
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
        result.type = "ok";
        return result;
    } catch (error) {
        return {
            type: "error",
            message: (error as any).toString()
        };
    }
}

export async function getAppointment(id: number): Promise<AppointmentsResponse> {
    try {
        const token = getAccessToken();
        const response: Response = await fetch(apiServer + "appointment/" + id, {
            headers: (token ? { "Authorization": `Bearer ${token}` } : {}),
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