import React, { FormEvent, useEffect, useRef } from "react";

import $ from "jquery";
import "jquery-validation";

import "./Page.css";
import "./RegisterPage.css";
import { FormField } from "../components/FormField";
import { SubmitButton } from "../components/Button";
import { useLocation, useNavigate } from "react-router-dom";
import { register } from "../services/auth";

export function Register() {
    const location = useLocation();
    const queryParams = new URLSearchParams(location.search);
    const redirect = queryParams.get('redirect');
    const formRef = useRef<HTMLFormElement>(null);
    const navigate = useNavigate();

    useEffect(() => {
        ($ as any).validator.addMethod("phoneValidation", function (this: any, value: string, element: any) {
            const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
            const methods = ($ as any).validator.methods;
            if (!methods.required.call(this, value, element)) return true;

            if (!methods.minlength.call(this, value, element)
                && !methods.maxlength.call(this, value, element)
            ) return true;

            return emailPattern.test(value);
        }, "Số điện thoại không hợp lệ.");

        ($ as any).validator.addMethod("usernameValidation", function (this: any, value: string, element: any) {
            const usernamePattern = /^[a-zA-Z][0-9a-zA-Z_-]{5,49}$/;
            const phonePattern = /^[0-9]{10,15}$/;

            const methods = ($ as any).validator.methods;
            if (!methods.required.call(this, value, element)
                && !methods.minlength.call(this, value, element)
                && !methods.maxlength.call(this, value, element)
            ) return true;

            return usernamePattern.test(value) || phonePattern.test(value);
        }, "Tên đăng nhập hoặc số điện thoại không hợp lệ.");

        ($ as any).validator.addMethod("passwordValidation", function (this: any, value: string, element: any) {
            const methods = ($ as any).validator.methods;
            if (!methods.required.call(this, value, element)
                && !methods.minlength.call(this, value, element)
                && !methods.maxlength.call(this, value, element)
            ) return true;

            let l = false, u = false, d = false;
            console.error(this, value, element);
            for (const c of value) {
                const v = c.toString();
                if (/^[0-9]$/.test(v)) {
                    console.error(v, "Digit");
                    if (l && u) return true;
                    d = true;
                } else if (v === v.toLowerCase()) {
                    console.error(v, "LowerCase");
                    if (u && d) return true;
                    l = true;
                } else if (v === v.toUpperCase()) {
                    console.error(v, "UpperCase");
                    if (l && d) return true;
                    u = true;
                }
            }
            return false;
        }, "Mật khẩu phải có ít nhất một ký tự hoa, thường, số.");

        formRef.current && ($(formRef.current) as any).validate({
            rules: {
                full_name: {
                    required: true,
                    maxlength: 50
                },
                email: {
                    required: false,
                    minlength: 6,
                    maxlength: 50,
                    emailValidation: true
                },
                username: {
                    required: true,
                    minlength: 6,
                    maxlength: 50,
                    usernameValidation: true
                },
                password: {
                    required: true,
                    minlength: 8,
                    maxlength: 50,
                    passwordValidation: true
                }
            },
            messages: {
                full_name: {
                    required: "Vui lòng nhập họ và tên.",
                    maxlength: "Họ và tên không dài quá 50 ký tự."
                },
                email: {
                    required: "Vui lòng nhập email.",
                    minlength: "Email phải có tối thiểu 10 ký tự.",
                    maxlength: "Email không dài quá 15 ký tự."
                },
                username: {
                    required: "Vui lòng nhập tên đăng nhập hoặc số điện thoại.",
                    minlength: "Tên đăng nhập phải có tối thiểu 6 ký tự.",
                    maxlength: "Tên đăng nhập không dài quá 50 ký tự."
                },
                password: {
                    required: "Vui lòng nhập mật khẩu",
                    minlength: "Mật khẩu phải có tối thiểu 8 ký tự.",
                    maxlength: "Mật khẩu phải không dài quá 50 ký tự."
                }
            },
            submitHandler: async function (form: HTMLFormElement, e: FormEvent) {
                e.preventDefault();
                const response = await register(new FormData(form));
                if (response.type == "ok") {
                    await navigate(redirect && redirect.length ? '/login' + location.search : '/login');
                } else {
                    alert("Thông tin đăng ký không hợp lệ.");
                }
            }
        });
    }, []);

    return (
        <form ref={formRef} className="Register Page">
            <h2 className="title">Đăng ký</h2>
            <FormField label="Họ và tên *" attributes={{ name: "full_name", type: "text" }} />
            <FormField label="Email" attributes={{ name: "email", type: "text" }} />
            <FormField label="Tên đăng nhập *" attributes={{ name: "username", type: "text" }} />
            <FormField label="Mật khẩu *" attributes={{ name: "password", type: "password" }} />
            <SubmitButton attributes={{
                style: {
                    borderRadius: "8px",
                    fontSize: "18px",
                    lineHeight: "24px",
                    padding: "16px"
                }
            }}>Đăng ký</SubmitButton>
            <span className="note">Bạn đã có tài khoản? <a href={"/login" + location.search}>Đăng nhập</a></span>
        </form>
    );
}