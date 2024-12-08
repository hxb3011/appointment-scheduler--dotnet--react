import React, { useEffect, useState } from "react";

import "./Page.css";
import "./AppointmentPage.css";
import { Link, useNavigate } from "react-router-dom";
import { FutureAppointment, getFutureAppointments } from "../services/appointment";
import { Patient } from "../services/patient";
import { setAccessToken } from "../services/auth";

type AppointmentProps = {
    user?: Patient;
};

export function Appointment(props: AppointmentProps) {
    const navigate = useNavigate();
    const [appointments, setAppointments] = useState<FutureAppointment[]>();
    useEffect(() => {
        getFutureAppointments().then(value => {
            if (value.type === "ok") {
                setAppointments(value);
            } else if (value.type === "error") {
                if (value.message === "unauth") {
                    setAccessToken();
                    navigate("/login?redirect=" + encodeURIComponent("/appointment"))
                }
            }
        });
    }, [])

    return (
        <div className="Appointment Page">
            <h2 className="title">Lịch đã đặt</h2>
            {appointments && appointments.map(function (appointment) {
                return (<Link key={appointment.id} to="#" className="Item">
                    <span className="number">{appointment.number}</span>
                    <div className="info">
                        <span className="name">{appointment.profile_fullname}</span>
                        <span className="at">{appointment.at && new Date(appointment.at).toLocaleString()}</span>
                    </div>
                </Link>)
            })}
        </div>
    );
}