import React, { useEffect, useState } from "react";

import "./Page.css";
import "./ProfilePage.css";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { FilledButton } from "../components/Button";
import { Icon } from "../assets/icons/mdi";
import { Patient } from "../services/patient";
import { getProfiles, Profile as Profile_t } from "../services/profile";
import { setAccessToken } from "../services/auth";

type ProfilePageProps = {
    user?: Patient;
};

export function Profile(props: ProfilePageProps) {
    const location = useLocation();
    const navigate = useNavigate();
    const [profiles, setProfiles] = useState<Profile_t[]>();
    useEffect(() => {
        getProfiles({}).then(value => {
            if (value.type === "ok") {
                setProfiles(value);
                return;
            } else if (value.type === "error") {
                if (value.message === "unauth") {
                    setAccessToken();
                    navigate("/login?redirect=" + encodeURIComponent(location.pathname + location.search + location.hash));
                }
            }
        });
    }, [props.user?.id])

    return (<div className="Profile Page">
        <h2 className="title">Hồ sơ</h2>
        <FilledButton url="/profile/create"><span className="mdi">{Icon.plus_circle}</span>Tạo hồ sơ</FilledButton>
        {profiles?.map(function (profile) {
            return (<Link to={"/profile/info?id=" + profile.id} className="Item">
                <div className="title">
                    <span className="name">Người khám:</span>
                    <span className="birthdate">Ngày sinh:</span>
                    <span className="gender">Giới tính:</span>
                </div>
                <div className="info">
                    <span className="name">{profile.full_name}</span>
                    <span className="birthdate">{profile.birthdate && new Date(profile.birthdate).toLocaleDateString()}</span>
                    <span className="gender">{profile.gender === "M" ? "Nam" : profile.gender === "F" ? "Nam" : "(Khác)"}</span>
                </div>
            </Link>);
        }) as React.ReactNode}
    </div>);
}