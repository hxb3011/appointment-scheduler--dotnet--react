import * as React from "react";

import "./FormField.css"

type FormFieldProps = React.Attributes & {
    attributes?: React.InputHTMLAttributes<HTMLInputElement>
    label?: string;
}

export function FormField(props: FormFieldProps) {
    const attributes = props.attributes || {};
    return (
        <label className="Container">
            {props.label}
            <input className="Form Field" {...attributes} />
        </label>
    );
}