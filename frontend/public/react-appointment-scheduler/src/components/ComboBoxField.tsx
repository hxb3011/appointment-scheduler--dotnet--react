import * as React from "react";

import "./ComboBoxField.css"

type ComboBoxProps = React.Attributes & {
    attributes?: React.SelectHTMLAttributes<HTMLSelectElement>
    label?: string;
    children?: Iterable<React.ReactNode>;
}

export function ComboBox(props: ComboBoxProps) {
    const attributes = props.attributes || {};
    return (
        <label className="Container">
            {props.label}
            <select className="Combo Box Field" {...attributes}>
                {props.children || (<option value="none">Không có {props.label?.toLowerCase()}</option>)}
            </select>
        </label>
    );
}