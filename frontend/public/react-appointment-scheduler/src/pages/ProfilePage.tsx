import React, { useEffect, useState } from "react";

import "./Page.css";
import "./ProfilePage.css";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { FilledButton } from "../components/Button";
import { Icon } from "../assets/icons/mdi";
import { setAccessToken, getAccessToken } from "../services/auth";
import { currentUser } from "../services/patient";
import { getProfile, getProfiles, Profile as Profile_t } from "../services/profile";

export function Profile() {
    const [profiles, setProfiles] = useState<Profile_t[]>();
    useEffect(() => {
        getProfiles().then(value => {
            if (value.type == "ok") {
                setProfiles(value);
                return;
            }
        });
    }, [])

    return (
        <div className="Profile Page">
            <h2 className="title">Hồ sơ</h2>
            <FilledButton url="/profile/create"><span className="mdi">{Icon.plus_circle}</span>Tạo hồ sơ</FilledButton>
            {profiles && profiles.map(function (profile) {
                return (<Link to="#" className="Item">
                    <div className="title">
                        <span className="name">Người khám:</span>
                        <span className="birthdate">Ngày sinh:</span>
                        <span className="gender">Giới tính:</span>
                    </div>
                    <div className="info">
                        <span className="name">{profile.full_name}</span>
                        <span className="birthdate">{profile.date_of_birth && new Date(profile.date_of_birth).toDateString()}</span>
                        <span className="gender">{profile.gender === "M" ? "Nam" : profile.gender === "F" ? "Nam" : "(Khác)"}</span>
                    </div>
                </Link>);
            }) as React.ReactNode}
        </div>
    );
}