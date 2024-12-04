import * as React from "react";

import "./Button.css"

type ButtonProps = React.Attributes & {
    attributes?: React.HTMLAttributes<HTMLElement>
    url?: string;
    children?: React.ReactNode;
}

export function FilledButton(props: ButtonProps) {
    const attributes = props.attributes || {};
    return (
        <a {...attributes} className="Filled Button" href={props.url}>{props.children}</a>
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
        <a {...attributes} className="Outlined Button" href={props.url}>{props.children}</a>
    );
}