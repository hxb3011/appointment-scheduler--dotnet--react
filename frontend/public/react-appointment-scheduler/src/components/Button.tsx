import * as React from "react";

import "./Button.css"

type ButtonProps = React.Attributes & {
    attributes?: React.HTMLAttributes<HTMLElement>
    content?: string;
    href?: string;
}

export function FilledButton(props: ButtonProps) {
    const attributes = props.attributes || {};
    return (
        <a {...attributes} className="Filled Button" href={props.href}>{props.content}</a>
    );
}

export function OutlinedButton(props: ButtonProps) {
    const attributes = props.attributes || {};
    return (
        <a {...attributes} className="Outlined Button" href={props.href}>{props.content}</a>
    );
}