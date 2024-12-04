import * as React from "react";

import "./CalendarField.css"

type CalendarItem = {
    label: string;
    key: string;
    disabled?: boolean;
}

type CalendarProps = React.Attributes & {
    attributes?: React.HTMLAttributes<HTMLElement>
    label?: string;
    formName?: string;
    children?: Iterable<CalendarItem>;
}

export function Calendar(props: CalendarProps) {
    const attributes = props.attributes || {};
    return (
        <div className="Container">
            {props.label}
            <div {...attributes} className="Calendar Field">
                {["T2", "T3", "T4", "T5", "T6", "T7", "CN"]
                    .map(function (value) {
                        return (<label className="Header Item">{value}</label>);
                    })}
                {props.children && Array.from(props.children, function (value, k) {
                    console.log(`children[${k}]: `, value);
                    return (<label className="Item">
                        {value.label}
                        <input disabled={value.disabled} hidden={true} name={props.formName} value={value.key} type="radio" />
                    </label>);
                })}
            </div>
        </div>
    );
}