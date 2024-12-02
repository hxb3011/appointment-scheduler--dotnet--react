import * as React from "react";

import "./ComboBoxField.css"

type ComboBoxProps = React.Attributes & {
    attributes?: React.HTMLAttributes<HTMLSelectElement>
    label?: string;
    children?: Iterable<React.ReactNode>;
}

export function ComboBox(props: ComboBoxProps) {
    const attributes = props.attributes || {};
    return (
        <label className="Container">
            {props.label}
            <select className="ComboBox" {...attributes}>
                {props.children && Array.from(props.children, function(node, k) {
                    return (<option value={(node as React.ReactElement).key || k}>
                        {node}
                    </option>);
                })}
            </select>
        </label>
    );
}