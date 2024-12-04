import React from "react";

import "./Page.css";
import "./ProfilePage.css";
import { Link } from "react-router-dom";
import { FilledButton, SubmitButton } from "../components/Button";
import { Icon } from "../assets/icons/mdi";
import { ComboBox } from "../components/ComboBoxField";
import { FormField } from "../components/FormField";

export function ProfileEditor() {
    return (
        <form className="Profile Editor Page">
            <h2 className="title">Hồ sơ</h2>
            <FormField label="Người khám" attributes={{ type: "text" }} />
            <FormField label="Ngày sinh" attributes={{ type: "date" }} />
            <ComboBox label="Giới tính">
                <option key="none">(Khác)</option>
                <option key="m">Nam</option>
                <option key="f">Nữ</option>
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