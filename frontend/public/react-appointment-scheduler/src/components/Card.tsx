import * as React from "react";

import { Icon } from "../assets/icons/mdi";

import { FilledButton, OutlinedButton } from "./Button";

import "./Card.css"
import { Link, NavLink } from "react-router-dom";

type CardProps = React.Attributes & {
    heading?: string;
    description?: string;
    attributes?: React.HTMLAttributes<HTMLElement>
}

export function Card(props: CardProps) {
    const attributes = props.attributes || {};
    return (
        <div {...attributes} className="Card">
            <h1 className="heading">{props.heading}</h1>
            <div className="description">{props.description || "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Voluptas expedita voluptatibus molestiae nihil? Sapiente ab, hic accusamus dolor fuga laborum voluptates omnis, maiores aliquid explicabo adipisci iusto dolore corrupti facilis!"}</div>
        </div>
    );
}

type AccountCardProps = React.Attributes & {
    attributes?: React.HTMLAttributes<HTMLElement>
    imageURL?: string;
    name?: string;
    username?: string;
}

export function AccountCard(props: AccountCardProps) {
    const attributes = props.attributes || {};
    return (
        <div {...attributes} className="Account Card">
            <div className="avatar">
                {props.imageURL ? (
                    <img className="img" src={props.imageURL} alt="Ảnh đại diện" />
                ) : (
                    <span className="img mdi">{Icon.account_circle_outline}</span>
                )}
                <FilledButton content="Đăng nhập" />
            </div>
            <div className="info">
                <h1 className="name">{props.name}</h1>
                <div className="username">@{props.username || "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Voluptas expedita voluptatibus molestiae nihil? Sapiente ab, hic accusamus dolor fuga laborum voluptates omnis, maiores aliquid explicabo adipisci iusto dolore corrupti facilis!"}</div>
            </div>
        </div>
    );
}

type NavCardProps = React.Attributes & {
    attributes?: React.HTMLAttributes<HTMLElement>
    navigations?: {
        excludeSubs?: true;
        activeIcon?: string;
        icon?: string;
        name?: string;
        url?: string
    }[];
}

export function NavCard(props: NavCardProps) {
    const attributes = props.attributes || {};
    return (
        <div {...attributes} className="Nav Card" children={props.navigations?.map(nav => (
            <NavLink className={({ isActive }) => isActive ? "Item active" : "Item"} to={nav.url ?? "#"} end={nav.excludeSubs}>
                {({ isActive }) => (<><span className="mdi">{isActive ? nav.activeIcon : nav.icon}</span>{nav.name}</>)}
            </NavLink>
        ))} />
    );
}