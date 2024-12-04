import React from "react";

import "./Page.css";
import "./SchedulerPage.css";
import { ComboBox } from "../components/ComboBoxField";
import { SubmitButton } from "../components/Button";
import { Calendar } from "../components/CalendarField";

export function Scheduler() {
    return (
        <form className="Scheduler Page">
            <h2 className="title">Đặt lịch</h2>
            <ComboBox label="Chuyên khoa">
                <option key="none">(Chọn chuyên khoa)</option>
                <option key="rhm">Răng hàm mặt</option>
                <option key="tmh">Tai mũi họng</option>
            </ComboBox>
            <Calendar label="Ngày khám" formName="date" children={
                Array.from({ length: 28 }, function (value, index) {
                    console.log(index, value);
                    return {
                        key: index.toString(),
                        label: "01/01",
                        disabled: false
                    };
                })
            } />
            <ComboBox label="Hồ sơ">
                <option value="hxb">Huynh Xuan Bach</option>
                <option value="new">&lt;Thêm hồ sơ&gt;</option>
            </ComboBox>
            <ComboBox label="Khung giờ">
                <option value="none">7:30 - 8:00</option>
                <option value="rhm">8:00 - 8:30</option>
                <option value="tmh">9:30 - 9:00</option>
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