import log from "../utils/log"

type ThemeMode = `dark` | `light`;
type ThemeContrast = `high` | `medium` | `normal`;
type ThemeMetaProperties = `--theme-preview--primary-${ThemeMode}${`` | `__${`high-contrast` | `medium-contrast`}`}`;
type ThemeGroups = {
    [o in ThemeMode]: {
        [c in ThemeContrast]?: CSSStyleRule;
    };
} & {
    meta?: CSSStyleRule;
    metaToMigrated?: {
        [property in ThemeMetaProperties]?: CSSStyleValue;
    };
};
type ThemeMappingSelector = `[mdc-theme${`` | `="${ThemeMode}${`` | ` ${"highContrast" | "mediumContrast"}`}"`}]`;
type ThemeMapping = (element: CSSStyleRule) => void;

const _defaultThemeName = "material_1";

const _dynamicThemeCSSLoader = document.head.appendChild((() => {
    let style = document.createElement("style");
    style.id = "dynamicThemeCSSLoader";
    return style;
})());

const _tagName = "assets.colors.dynamicThemeCSSLoader.js:DynamicThemeCSSLoader"

const lg = log.bindTag(_tagName);
const lgv = lg.bindPriority("v");
const lge = lg.bindPriority("e");

function appendMediaRule(theme: ThemeGroups, mode: ThemeMode,
    contrast: ThemeContrast, mediaRule: CSSGroupingRule, selector: string): void {
    const rule = theme[mode][contrast];
    if (!rule) return;
    const rules = mediaRule.cssRules;
    (<CSSStyleRule>(rules[
        mediaRule.insertRule(rule.cssText, rules.length)
    ])).selectorText = selector;
}

function appendThemeMediaRule(theme: ThemeGroups, mode: ThemeMode, styleSheet: CSSStyleSheet): void {
    const rules = styleSheet.cssRules;
    const rule = <CSSMediaRule>(rules[styleSheet.insertRule(
        `@media (prefers-color-scheme: ${mode}) {}`, rules.length)]);
    appendMediaRule(theme, mode, "high", rule, `[mdc-theme="system highContrast"]`);
    appendMediaRule(theme, mode, "medium", rule, `[mdc-theme="system mediumContrast"]`);
    appendMediaRule(theme, mode, "normal", rule, `[mdc-theme="system"]`);
}

function storeThemeMeta(theme: ThemeGroups, rule: CSSStyleRule): void {
    theme.meta = rule;
    const metaToMigrated = theme.metaToMigrated;
    if (!metaToMigrated) return;
    for (const key in metaToMigrated) {
        const value = metaToMigrated[key];
        if (value) rule.styleMap.set(key, value);
    }
    delete theme.metaToMigrated;
}

function storeTheme(theme: ThemeGroups, mode: ThemeMode,
    contrast: ThemeContrast, prop: string, element: CSSStyleRule): void {
    theme[mode][contrast] = element;
    const value = element.styleMap.get("--mdc-primary");
    if (!value) return;
    const themeMeta = theme.meta;
    if (!themeMeta) {
        let metaToMigrated = theme.metaToMigrated;
        if (!metaToMigrated) theme.metaToMigrated =
            metaToMigrated = {};
        metaToMigrated[prop] = value;
    } else themeMeta.styleMap.set(prop, value);
}

function loadTheme(name: string): boolean {
    const loader = _dynamicThemeCSSLoader;
    const contentToRollback = loader.textContent;
    const sheet = loader.sheet;
    if (!sheet) {
        lge(`html>head>link[rel=stylesheet]#${loader.id} tag is required!`);
        loader.textContent = contentToRollback;
        return false;
    }

    const rules = sheet.cssRules;
    loader.textContent = ``;
    const importedSheet = (<CSSImportRule>(rules[
        sheet.insertRule(`@import url("/assets/colors/${name}.css");`, rules.length)
    ])).styleSheet;
    if (!importedSheet) {
        lge(`"${name}" theme not found!`);
        loader.textContent = contentToRollback;
        return false;
    }

    const importedRules = importedSheet.cssRules;
    lgv(`<begin function>`);
    const theme: ThemeGroups = {
        dark: {},
        light: {}
    };

    const themeMapping = <{ [selector in ThemeMappingSelector]: ThemeMapping; }>{
        '[mdc-theme]': storeThemeMeta.bind(this, theme),
        '[mdc-theme="dark"]': storeTheme.bind(this, theme, "dark", "normal", "--theme-preview--primary-dark"),
        '[mdc-theme="light"]': storeTheme.bind(this, theme, "light", "normal", "--theme-preview--primary-light"),
        '[mdc-theme="dark highContrast"]': storeTheme.bind(this, theme, "dark", "high", "--theme-preview--primary-dark__high-contrast"),
        '[mdc-theme="light highContrast"]': storeTheme.bind(this, theme, "light", "high", "--theme-preview--primary-light__high-contrast"),
        '[mdc-theme="dark mediumContrast"]': storeTheme.bind(this, theme, "dark", "medium", "--theme-preview--primary-dark__medium-contrast"),
        '[mdc-theme="light mediumContrast"]': storeTheme.bind(this, theme, "light", "medium", "--theme-preview--primary-light__medium-contrast")
    };

    for (let i = 0; i < importedRules.length; i++) {
        const rule = importedRules[i];
        if (!(rule instanceof CSSStyleRule)) continue;
        const mapping = <ThemeMapping | undefined>(themeMapping[rule.selectorText]);
        if (mapping) mapping(rule);
    }

    appendThemeMediaRule(theme, "dark", sheet);
    appendThemeMediaRule(theme, "light", sheet);
    lgv(`<end function>`);
    return true;
}

function theme(name?: string): string {
    const key = _tagName + ".dynamicThemeName";
    let old = localStorage.getItem(key);
    if (!old) {
        old = _defaultThemeName;
        name ||= old;
    }
    if (name && loadTheme(name))
        localStorage.setItem(key, name);
    return old;
};

function onLoadTheme(this: Window, e: Event): any { theme(theme()); }

function onDispose(this: Window, e: Event): any {
    window.removeEventListener("load", onLoadTheme);
    window.removeEventListener("unload", onDispose)
}

window.addEventListener("load", onLoadTheme);
window.addEventListener("unload", onDispose)

type DynamicThemeCSSLoader = {
    theme(name?: string): string;
    [Symbol.toStringTag](): string;
}

const DynamicThemeCSSLoader: DynamicThemeCSSLoader = { theme, [Symbol.toStringTag]() { return _tagName; } }

export default DynamicThemeCSSLoader;