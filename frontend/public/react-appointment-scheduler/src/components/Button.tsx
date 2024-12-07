import * as React from "react";

import "./Button.css"
import { Link } from "react-router-dom";

type ButtonProps = React.Attributes & {
    attributes?: React.HTMLAttributes<HTMLElement>
    url?: string;
    children?: React.ReactNode;
}

export function FilledButton(props: ButtonProps) {
    const attributes = props.attributes || {};
    return (
        <Link {...attributes} className="Filled Button" to={props.url || "#"}>{props.children}</Link>
    );
}

export function SubmitButton(props: ButtonProps) {
    const attributes = props.attributes || {};
    return (
        <input {...attributes} className="Filled Button" type="submit" value={props.children ? props.children.toString() : ""} />
    );
}

export function OutlinedButton(props: ButtonProps) {
    const attributes = props.attributes || {};
    return (
        <Link {...attributes} className="Outlined Button" to={props.url || "#"}>{props.children}</Link>
    );
}