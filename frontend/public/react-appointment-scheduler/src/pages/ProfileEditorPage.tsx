import React, { FormEvent, useEffect, useRef } from "react";

import $ from "jquery";
import "jquery-validation"

import "./Page.css";
import "./ProfilePage.css";
import { Link } from "react-router-dom";
import { FilledButton, SubmitButton } from "../components/Button";
import { Icon } from "../assets/icons/mdi";
import { ComboBox } from "../components/ComboBoxField";
import { FormField } from "../components/FormField";

export function ProfileEditor() {
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
                    required: "Vui lòng nhập họ và tên."
                }
            },
            submitHandler: function (form: HTMLFormElement, e: FormEvent) {
                e.preventDefault();
                alert("submitHandler");
            }
        });
    }, [])
    return (
        <form ref={formRef} className="Profile Editor Page">
            <h2 className="title">Hồ sơ</h2>
            <FormField label="Người khám" attributes={{ name: "full_name", type: "text" }} />
            <FormField label="Ngày sinh" attributes={{ name: "birthdate", type: "date" }} />
            <ComboBox label="Giới tính" attributes={{ name: "gender" }}>
                <option value="m">Nam</option>
                <option value="f">Nữ</option>
            </ComboBox>
            <SubmitButton attributes={{
                style: {
                    borderRadius: "8px",
                    fontSize: "18px",
                    lineHeight: "24px",
                    padding: "16px"
                }
            }}>Tạo hồ sơ</SubmitButton>
        </form>
    );
}