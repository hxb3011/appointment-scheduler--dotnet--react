import React, { FormEvent, useEffect, useRef, useState } from "react";

import "./Page.css";
import "./SchedulerPage.css";
import { ComboBox } from "../components/ComboBoxField";
import { SubmitButton } from "../components/Button";
import { Calendar } from "../components/CalendarField";
import { Doctor, getDoctors } from "../services/doctor";
import { getParts, Part } from "../services/scheduler";
import { getProfiles, Profile } from "../services/profile";
import { createAppointment } from "../services/appointment";
import { useLocation, useNavigate } from "react-router-dom";
import { Patient } from "../services/patient";
import { getAccessToken, setAccessToken } from "../services/auth";

type SchedulerProps = {
    user?: Patient;
};

export function Scheduler(props: SchedulerProps) {
    const location = useLocation();
    const navigate = useNavigate();

    const [doctors, setDoctors] = useState<Doctor[]>([]);
    useEffect(() => {
        getDoctors().then((value) => {
            if (value.type === "ok") {
                setDoctors(value);
            } else if (value.type === "error") {
                if (value.message === "unauth") {
                    setAccessToken();
                    navigate("/login?redirect=" + encodeURIComponent("/schedule"))
                }
            }
        })
    }, [navigate])

    const [parts, setParts] = useState<Part[]>([]);
    useEffect(() => {
        getParts().then(value => {
            if (value.type === "ok") {
                setParts(value);
            } else if (value.type === "error") {
                if (value.message === "unauth") {
                    setAccessToken();
                    navigate("/login?redirect=" + encodeURIComponent("/schedule"))
                }
            }
        });
    }, [navigate])

    const [profiles, setProfiles] = useState<Profile[]>([]);
    useEffect(() => {
        getProfiles({}).then(value => {
            if (value.type === "ok") {
                setProfiles(value);
            } else if (value.type === "error") {
                if (value.message === "unauth") {
                    setAccessToken();
                    navigate("/login?redirect=" + encodeURIComponent("/schedule"))
                }
            }
        });
    }, [navigate])

    async function onSchedulerSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        var form = event.currentTarget;
        const [start, end] = form.timerange.value.split("@");
        const response = await createAppointment({
            doctor: Number(form.doctor.value),
            profile: Number(form.profile.value),
            date: form.date.value,
            begin_time: start,
            end_time: end
        });
        console.warn(response);
        if (response.type === "ok") {
            // if (response.payment_url)
            //     window.location.href = response.payment_url;
            alert("Đặt lịch thành công.");
            navigate("/appointment");
        } else alert("Lịch đã kín.");
    }

    return (
        <form className="Scheduler Page" onSubmit={onSchedulerSubmit}>
            <h2 className="title">Đặt lịch</h2>
            <ComboBox label="Hồ sơ" attributes={{ name: "profile" }}>
                {profiles.map(function (value) {
                    return (<option value={value.id}>{value.full_name}</option>);
                })}
                <option value="new">&lt;Thêm hồ sơ&gt;</option>
            </ComboBox>
            <ComboBox label="Chuyên khoa" attributes={{ name: "doctor" }}>
                <option value="none">(Chọn chuyên khoa)</option>
                {doctors.map(function (value) {
                    return (<option value={value.id}>{`Chuyên khoa ${value.position || "không xác định"} - BS.${value.full_name}`}</option>);
                })}
            </ComboBox>
            <Calendar label="Ngày khám" formName="date" children={
                (function () {
                    const dates = [];
                    const today = new Date();
                    const dayOfWeek = today.getDay(); // 0 (Chủ nhật) đến 6 (Thứ bảy) // Tính số ngày để lùi về thứ Hai của tuần hiện tại
                    const firstMonday = new Date();
                    firstMonday.setDate(today.getDate() - ((dayOfWeek === 0) ? 6 : dayOfWeek - 1));
                    for (let i = 0; i < 28; i++) {
                        const newDate = new Date(firstMonday);
                        newDate.setDate(firstMonday.getDate() + i); // Kiểm tra nếu ngày mới lớn hơn ngày hiện tại
                        dates.push({ date: newDate, isPast: newDate <= today });
                    }
                    return dates;
                })().map(function (value, index) {
                    return {
                        key: value.date.toISOString().split("T")[0],
                        label: `${value.date.getDate()}/${value.date.getMonth()+1}`,
                        disabled: value.isPast
                    };
                })
            } />
            <ComboBox label="Khung giờ" attributes={{ name: "timerange" }}>
                {parts.map(function (value) {
                    function simplify(time?: string) {
                        return !time ? '' : time.slice(0, time.indexOf(':', time.indexOf(':') + 1));
                    }
                    return (<option value={`${value.start}@${value.end}`}>{simplify(value.start)} - {simplify(value.end)}</option>);
                })}
            </ComboBox>
            <SubmitButton attributes={{
                style: {
                    borderRadius: "8px",
                    fontSize: "18px",
                    lineHeight: "24px",
                    padding: "16px"
                }
            }}>Đặt lịch</SubmitButton>
        </form>
    );
}