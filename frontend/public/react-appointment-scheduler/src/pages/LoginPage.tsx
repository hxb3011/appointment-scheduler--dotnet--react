
import React, { Dispatch, FormEvent, SetStateAction, useEffect, useRef, useState } from 'react';

import $ from 'jquery';
import 'jquery-validation';

import "./Page.css";
import "./LoginPage.css";
import { FormField } from "../components/FormField";
import { SubmitButton } from "../components/Button";
import { useLocation, useNavigate } from 'react-router-dom';
import { login, setAccessToken } from '../services/auth'

type LoginPageProps = {
}

export function Login(props: LoginPageProps) {
    const location = useLocation();
    const queryParams = new URLSearchParams(location.search);
    const redirect = queryParams.get('redirect');
    const navigate = useNavigate();
    const formRef = useRef<HTMLFormElement>(null);

    useEffect(() => {
        ($ as any).validator.addMethod("usernameValidation", function (this: any, value: string, element: any) {
            const phonePattern = /^[0-9]{10,15}$/; // Adjust according to your phone number format
            const usernamePattern = /^[a-zA-Z][0-9a-zA-Z_-]{5,49}$/;
            const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/

            const methods = ($ as any).validator.methods;
            if (!methods.required.call(this, value, element)
                && !methods.minlength.call(this, value, element)
                && !methods.maxlength.call(this, value, element)
            ) return true;

            return phonePattern.test(value) || usernamePattern.test(value) || emailPattern.test(value);
        }, "Tên đăng nhập, Email, Số điện thoại không hợp lệ.");

        ($ as any).validator.addMethod("passwordValidation", function (this: any, value: string, element: any) {
            const methods = ($ as any).validator.methods;
            if (!methods.required.call(this, value, element)
                && !methods.minlength.call(this, value, element)
                && !methods.maxlength.call(this, value, element)
            ) return true;

            let l = false, u = false, d = false;
            for (const c of value) {
                const v = c.toString();
                if (/^[0-9]$/.test(v)) {
                    if (l && u) return true;
                    d = true;
                } else if (v === v.toLowerCase()) {
                    if (u && d) return true;
                    l = true;
                } else if (v === v.toUpperCase()) {
                    if (l && d) return true;
                    u = true;
                }
            }
            return false;
        }, "Mật khẩu phải có ít nhất một ký tự hoa, thường, số.");

        formRef.current && ($(formRef.current) as any).validate({
            rules: {
                username: {
                    required: true,
                    minlength: 6,
                    maxlength: 50,
                    usernameValidation: true
                },
                password: {
                    required: true,
                    // minlength: 8,
                    // maxlength: 50,
                    // passwordValidation: true
                }
            },
            messages: {
                username: {
                    required: "Vui lòng nhập tên đăng nhập, email hoặc số điện thoại.",
                    minlength: "Tên đăng nhập phải có tối thiểu 6 ký tự.",
                    maxlength: "Tên đăng nhập hoặc email không dài quá 50 ký tự."
                },
                password: {
                    required: "Vui lòng nhập mật khẩu",
                    // minlength: "Mật khẩu phải có tối thiểu 8 ký tự.",
                    // maxlength: "Mật khẩu phải không dài quá 50 ký tự."
                }
            },
            submitHandler: async function (form: HTMLFormElement, e: FormEvent) {
                e.preventDefault();
                const response = await login(new FormData(form));
                if (response.type == "ok") {
                    setAccessToken(response.access_token);
                    await navigate(redirect && redirect.length ? redirect : '/profile');
                } else if (response.message == "user not found.") {
                    alert("Người dùng không tồn tại.")
                } else {
                    alert("Thông tin đăng nhập không đúng.");
                }
            }
        });
    }, []);

    return (
        <form ref={formRef} className="Login Page">
            <h2 className="title">Đăng nhập</h2>
            <FormField label="Tên đăng nhập" attributes={{ name: "username", type: "text" }} />
            <FormField label="Mật khẩu" attributes={{ name: "password", type: "password" }} />
            <a className="note end" href="#">Quên mật khẩu?</a>
            <SubmitButton attributes={{
                style: {
                    borderRadius: "8px",
                    fontSize: "18px",
                    lineHeight: "24px",
                    padding: "16px"
                }
            }}>Đăng nhập</SubmitButton>
            <span className="note">Bạn chưa có tài khoản? <a href={"/register" + location.search}>Đăng ký</a> ngay</span>
        </form>
    );
}