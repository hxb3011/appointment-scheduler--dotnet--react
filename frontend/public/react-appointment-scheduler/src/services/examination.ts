import { apiServer } from "../utils/api";
import { getAppointment } from "./appointment";
import { getAccessToken } from "./auth";

type BaseExaminationErrorResponse = {
    type: "error";
    message?: any;
}

export type FilteredExaminationsRequest = {
    profile?: number;
    start?: string;
    end?: string;
}

export type Examination = {
    id?: number;
    appointment?: number;
    at?: string;
    number?: number;
    state?: number;
    profile?: number;
    doctor?: number;
    diagnostic?: string;
    description?: string;
}

export type ExaminationsResponse = BaseExaminationErrorResponse | (Examination[] & {
    type: "ok";
})

export async function getExaminations(request: FilteredExaminationsRequest): Promise<ExaminationsResponse> {
    try {
        const token = getAccessToken();
        const params = new URLSearchParams();
        request.profile && params.set("profile", request.profile.toString());
        request.start && params.set("start", request.start);
        request.end && params.set("end", request.end);
        const response: Response = await fetch(apiServer + "examination/filter" + (params.size ? '?' + params.toString() : ''), {
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
        const result = await Promise.all((await response.json()).map(async function (v: Examination) {
            if (v.appointment) {
                const appointment = await getAppointment(v.appointment);
                if (appointment.type === "ok") return {
                    ...v,
                    at: appointment.at,
                    number: appointment.number,
                    doctor: appointment.doctor,
                    profile: appointment.profile
                };
            }
            return v;
        })) as any;
        result.type = "ok";
        return result;
    } catch (error) {
        return {
            type: "error",
            message: (error as any).toString()
        };
    }
}

export async function getExamination(id: number): Promise<ExaminationsResponse> {
    try {
        const token = getAccessToken();
        const response: Response = await fetch(apiServer + "examination/" + id, {
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