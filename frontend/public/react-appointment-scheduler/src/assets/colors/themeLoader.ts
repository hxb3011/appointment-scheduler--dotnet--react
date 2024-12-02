import log from "../utils/log"
import themes from "./themeRegistry"

const _tagName = "assets.colors.themeRegistry.ts:MaterialColorThemeLoader"

const _themeLoader = document.createElement("style");
_themeLoader.id = "themeLoader";
document.head.appendChild(_themeLoader);

function theme(name?: string): string {
    log(log.VERBOSE, _tagName)
    const key = _tagName + ".dynamicThemeName";
    let old = localStorage.getItem(key);
    if (!old || !themes[old]) {
        old = Object.keys(themes)[0];
        if (!name) localStorage.setItem(key, old);
    }
    if (!name) {
        const oldTheme = themes[old];
        if (oldTheme) {
            let content = ``;
            for (const theme of oldTheme.stylesheet.cssRules)
                content += theme.cssText;
            _themeLoader.innerHTML = content;
        }
        return old;
    }
    const newTheme = themes[name];
    if (newTheme) {
        let content = ``;
        for (const theme of newTheme.stylesheet.cssRules)
            content += theme.cssText;
        _themeLoader.innerHTML = content;
        newTheme.stylesheet.disabled = false;
        localStorage.setItem(key, name);
    }
    return old;
};

function onLoadTheme(this: Window, e: Event): any {
    this.document.adoptedStyleSheets = [
        ...this.document.adoptedStyleSheets,
        ...Object.values(themes).map(function (value) {
            return value.stylesheet;
        })
    ]
    theme();
}

function onDispose(this: Window, e: Event): any {
    window.removeEventListener("load", onLoadTheme);
    window.removeEventListener("unload", onDispose)
}

window.addEventListener("load", onLoadTheme);
window.addEventListener("unload", onDispose)

type MaterialColorThemeLoader = {
    theme(name?: string): string;
}

const loader = {
    theme, [Symbol.toStringTag]() { return _tagName; }
} as MaterialColorThemeLoader

export default loader;