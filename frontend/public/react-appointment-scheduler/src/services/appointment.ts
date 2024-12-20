import { apiServer } from "../utils/api";
import { getAccessToken } from "./auth";
import { getProfile } from "./profile";

type BaseAppointmentErrorResponse = {
    type: "error";
    message?: any;
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
    message?: any;
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
        const response: Response = await fetch(apiServer + "appointment/patient/current", {
            headers: (token ? { "Authorization": `Bearer ${token}` } : {}),
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
    if (appointments.type === "ok") {
        const result = await Promise.all(appointments.map(async function (v) {
            if (v.profile) {
                const profile = await getProfile(v.profile);
                if (profile.type === "ok") {
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
        const response: Response = await fetch(apiServer + "appointment", {
            headers: {
                "Content-Type": "application/json",
                ...(token ? { "Authorization": `Bearer ${token}` } : {})
            },
            body: JSON.stringify(request),
            method: "POST"
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
        return { type: "ok", message: await response.text() };
    } catch (error) {
        return {
            type: "error",
            message: (error as any).toString()
        };
    }
}

export async function getAppointment(id: number): Promise<AppointmentResponse> {
    try {
        const token = getAccessToken();
        const response: Response = await fetch(apiServer + "appointment/" + id, {
            headers: (token ? { "Authorization": `Bearer ${token}` } : {}),
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