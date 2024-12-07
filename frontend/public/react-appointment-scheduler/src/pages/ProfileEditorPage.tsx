import React, { FormEvent, useEffect, useRef, useState } from "react";

import $ from "jquery";
import "jquery-validation"

import "./Page.css";
import "./ProfilePage.css";
import { SubmitButton } from "../components/Button";
import { ComboBox } from "../components/ComboBoxField";
import { FormField } from "../components/FormField";
import { Patient } from "../services/patient";
import { useLocation, useNavigate } from "react-router-dom";
import { createProfile, getProfile, Profile, ProfileRequest, updateProfile } from "../services/profile";

type ProfileEditorProps = {
    user?: Patient;
};

export function ProfileEditor(props: ProfileEditorProps) {
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
            }
        });
    }, [profile_id])
    const formRef = useRef<HTMLFormElement>(null);
    useEffect(() => {
        ($ as any).validator.addMethod("birthdateValidation", function (this: any, value: string, element: any) {
            const methods = ($ as any).validator.methods;
            if (!methods.required.call(this, value, element)) return true;
            return new Date(value) < new Date();
        }, "Ngày sinh phải là ngày trong quá khứ.");
        formRef.current && ($(formRef.current) as any).validate({
            rules: {
                full_name: {
                    required: true,
                    maxlength: 50
                },
                birthdate: {
                    required: true,
                    birthdateValidation: true
                }
            },
            messages: {
                full_name: {
                    required: "Vui lòng nhập họ và tên.",
                    maxlength: "Họ và tên không dài quá 50 ký tự."
                },
                birthdate: {
                    required: "Vui lòng nhập ngày sinh."
                }
            },
            submitHandler: async function (form: HTMLFormElement, e: FormEvent) {
                e.preventDefault();
                const full_name = form.full_name.value,
                    birthdate = form.birthdate.value,
                    gender = form.gender.value,
                    patient = props.user?.id;
                console.log(full_name, birthdate, gender, patient);
                if (full_name && birthdate && gender && patient) {
                    const response = profile_id
                        ? await updateProfile({ id: profile_id, full_name, birthdate, gender, patient })
                        : await createProfile({ full_name, birthdate, gender, patient });
                    console.log(response);
                    if (response.type === "ok") {
                        alert((profile_id ? "Sửa" : "Tạo") + " hồ sơ thành công.");
                        await navigate("/profile");
                        return;
                    }
                }
                console.log(full_name, birthdate, gender, patient);
                alert((profile_id ? "Sửa" : "Tạo") + " hồ sơ không thành công.");
            }
        });
    }, [])
    return (
        <form key={profile_id || "_"} ref={formRef} className="Profile Editor Page">
            <h2 className="title">Hồ sơ</h2>
            <FormField label="Người khám" attributes={{ name: "full_name", type: "text", value: profile?.full_name }} />
            <FormField label="Ngày sinh" attributes={{ name: "birthdate", type: "date", value: profile?.birthdate }} />
            <ComboBox label="Giới tính" attributes={{ name: "gender", value: profile?.gender }}>
                <option value="none">(Không xác định)</option>
                <option value="M">Nam</option>
                <option value="F">Nữ</option>
            </ComboBox>
            <SubmitButton attributes={{
                style: {
                    borderRadius: "8px",
                    fontSize: "18px",
                    lineHeight: "24px",
                    padding: "16px"
                }
            }}>{profile_id ? "Sửa hồ sơ" : "Tạo hồ sơ"}</SubmitButton>
        </form>
    );
}