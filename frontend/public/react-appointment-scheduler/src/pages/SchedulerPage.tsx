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
                <span key="none">(Chọn chuyên khoa)</span>
                <span key="rhm">Răng hàm mặt</span>
                <span key="tmh">Tai mũi họng</span>
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
                <span key="hxb">Huynh Xuan Bach</span>
                <span key="new">&lt;Thêm hồ sơ&gt;</span>
            </ComboBox>
            <ComboBox label="Khung giờ">
                <span key="none">7:30 - 8:00</span>
                <span key="rhm">8:00 - 8:30</span>
                <span key="tmh">9:30 - 9:00</span>
            </ComboBox>
            <SubmitButton attributes={{
                style: {
                    borderRadius: "8px",
                    fontSize: "18px",
                    lineHeight: "24px",
                    padding: "16px"
                }
            }} content="Đặt lịch" />
        </form>
    );
}