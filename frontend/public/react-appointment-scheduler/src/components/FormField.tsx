import * as React from "react";

import { Icon } from "../assets/icons/mdi";

import { FilledButton, OutlinedButton } from "./Button";

import "./FormField.css"
import { Link, NavLink } from "react-router-dom";

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