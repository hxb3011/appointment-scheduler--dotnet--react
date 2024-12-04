import React from "react";

import "./Page.css";
import "./ProfilePage.css";
import { Link } from "react-router-dom";
import { FilledButton } from "../components/Button";
import { Icon } from "../assets/icons/mdi";

export function Profile() {
    return (
        <div className="Profile Page">
            <h2 className="title">Hồ sơ</h2>
            <FilledButton url="/profile/create"><span className="mdi">{Icon.plus_circle}</span>Tạo hồ sơ</FilledButton>
            <Link to="#" className="Item">
                <div className="title">
                    <span className="name">Người khám:</span>
                    <span className="birthdate">Ngày sinh:</span>
                    <span className="gender">Giới tính:</span>
                </div>
                <div className="info">
                    <span className="name">Huynh Xuan Bach</span>
                    <span className="birthdate">30/11/2003</span>
                    <span className="gender">Nam</span>
                </div>
            </Link>
            <Link to="#" className="Item">
                <div className="title">
                    <span className="name">Người khám:</span>
                    <span className="birthdate">Ngày sinh:</span>
                    <span className="gender">Giới tính:</span>
                </div>
                <div className="info">
                    <span className="name">Huynh Xuan Bach</span>
                    <span className="birthdate">30/11/2003</span>
                    <span className="gender">Nam</span>
                </div>
            </Link>
        </div>
    );
}