import React from "react";

import "./Page.css";
import "./AppointmentPage.css";
import { Link } from "react-router-dom";

export function Appointment() {
    return (
        <div className="Appointment Page">
            <h2 className="title">Lịch đã đặt</h2>
            <Link to="#" className="Item">
                <span className="number">20</span>
                <div className="info">
                    <span className="name">Huynh Xuan Bach</span>
                    <span className="at">7:00 - 20/04/2022</span>
                </div>
            </Link>
            <Link to="#" className="Item">
                <span className="number">20</span>
                <div className="info">
                    <span className="name">Huynh Xuan Bach</span>
                    <span className="at">7:00 - 20/04/2022</span>
                </div>
            </Link>
        </div>
    );
}