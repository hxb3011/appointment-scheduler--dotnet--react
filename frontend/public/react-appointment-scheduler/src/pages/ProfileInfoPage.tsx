import React, { FormEvent, useEffect, useRef, useState } from "react";

import "./Page.css";
import "./ProfileInfoPage.css";
import { SubmitButton } from "../components/Button";
import { ComboBox } from "../components/ComboBoxField";
import { FormField } from "../components/FormField";
import { Patient } from "../services/patient";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { createProfile, getProfile, Profile, ProfileRequest, updateProfile } from "../services/profile";
import { setAccessToken } from "../services/auth";
import { Examination, getExaminations } from "../services/examination";

type ProfileInfoProps = {
    user?: Patient;
};

export function ProfileInfo(props: ProfileInfoProps) {
    const location = useLocation();
    const navigate = useNavigate();
    const query = new URLSearchParams(location.search);
    const sprofile_id = query.get("id");
    const profile_id = sprofile_id && Number(sprofile_id);
    const [profile, setProfile] = useState<Profile>();
    useEffect(() => {
        if (!profile_id) return;
        getProfile(profile_id).then(value => {
            if (value.type === "ok") {
                setProfile(value);
                return;
            } else if (value.type === "error") {
                if (value.message === "unauth") {
                    setAccessToken();
                    navigate("/login?redirect=" + encodeURIComponent(location.pathname + location.search + location.hash));
                }
            }
        });
    }, [profile_id]);

    const [examinations, setExaminations] = useState<Examination[]>();
    useEffect(() => {
    }, [props.user?.id])

    async function onFilterSubmit(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();
        const form = e.currentTarget;
        const start = form.start.value, end = form.end.value;
        console.log(form, start, end);
        if (start && end) {
            const dnow = new Date(new Date().toISOString().split("T")[0]), dstart = new Date(start), dend = new Date(end);
            if (dstart > dnow) {
                alert("Ngày bắt đầu không được sau ngày hiện tại .");
                return;
            }
            if (dstart > dend) {
                alert("Ngày bắt đầu không được sau ngày kết thúc.");
                return;
            }
            if (dend < dnow) {
                alert("Ngày kết thúc không được trước ngày hiện tại.");
                return;
            }
            getExaminations({ profile: profile?.id, start: dstart.toISOString().split("T")[0], end: dend.toISOString().split("T")[0] }).then(value => {
                if (value.type === "ok") {
                    setExaminations(value);
                    return;
                } else if (value.type === "error") {
                    if (value.message === "unauth") {
                        setAccessToken();
                        navigate("/login?redirect=" + encodeURIComponent(location.pathname + location.search + location.hash));
                    }
                }
            });
            return;
        }
        alert("Ngày lọc không hợp lệ.");
    }

    return (<div className="Profile Info Page">
        <h2 className="title">Hồ sơ</h2>
        <div className="Item">
            <div className="title">
                <span className="name">Người khám:</span>
                <span className="birthdate">Ngày sinh:</span>
                <span className="gender">Giới tính:</span>
            </div>
            <div className="info">
                <span className="name">{profile?.full_name}</span>
                <span className="birthdate">{profile?.birthdate && new Date(profile.birthdate).toLocaleDateString()}</span>
                <span className="gender">{profile?.gender === "M" ? "Nam" : profile?.gender === "F" ? "Nam" : "(Khác)"}</span>
            </div>
        </div>
        <form className="Filter" key={profile_id || "_"} onSubmit={onFilterSubmit}>
            <FormField label="Ngày bắt đầu" attributes={{ name: "start", type: "date" }} />
            <SubmitButton attributes={{
                style: {
                    borderRadius: "8px",
                    fontSize: "18px",
                    lineHeight: "24px",
                    margin: "0 0 8px",
                    padding: "16px"
                }
            }}>Lọc phiếu khám</SubmitButton>
            <FormField label="Ngày kết thúc" attributes={{ name: "end", type: "date" }} />
        </form>
        {(examinations && examinations.length) ? examinations.map((examination) => (
            <Link to="#" className="Item" style={{ alignSelf: "center", alignItems: "start", width: "calc(100% - 64px)" }}>
                <div className="title">
                    <span className="name">Chuẩn đoán:</span>
                    <span className="at">Khám lúc:</span>
                    <span className="description">Mô tả:</span>
                </div>
                <div className="info">
                    <span className="name">{examination.diagnostic}</span>
                    <span className="at">{examination.at && new Date(examination.at).toLocaleDateString()}</span>
                    <span className="description">{examination.description}</span>
                </div>
            </Link>
        )) : (<span style={{ alignSelf: "center" }}>Không tìm thấy phiếu khám.</span>)}
    </div>);
}