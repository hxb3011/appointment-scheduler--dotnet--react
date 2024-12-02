import log from "../utils/log"

type MaterialColorThemeMetadata = {
    label: string;
    description?: string;
};

type MaterialColorThemeKey = "primary" | "surface-tint" | "on-primary"
    | "primary-container" | "on-primary-container" | "secondary"
    | "on-secondary" | "secondary-container" | "on-secondary-container"
    | "tertiary" | "on-tertiary" | "tertiary-container"
    | "on-tertiary-container" | "error" | "on-error" | "error-container"
    | "on-error-container" | "background" | "on-background" | "surface"
    | "on-surface" | "surface-variant" | "on-surface-variant" | "outline"
    | "outline-variant" | "shadow" | "scrim" | "inverse-surface"
    | "inverse-on-surface" | "inverse-primary" | "primary-fixed"
    | "on-primary-fixed" | "primary-fixed-dim" | "on-primary-fixed-variant"
    | "secondary-fixed" | "on-secondary-fixed" | "secondary-fixed-dim"
    | "on-secondary-fixed-variant" | "tertiary-fixed" | "on-tertiary-fixed"
    | "tertiary-fixed-dim" | "on-tertiary-fixed-variant" | "surface-dim"
    | "surface-bright" | "surface-container-lowest" | "surface-container-low"
    | "surface-container" | "surface-container-high" | "surface-container-highest"

type MaterialColorThemeSchema = {
    [key in MaterialColorThemeKey]: string
};

type MaterialColorTheme = {
    metadata: MaterialColorThemeMetadata;
    schemas: {
        dark: MaterialColorThemeSchema;
        light: MaterialColorThemeSchema;
        dark_medium_contrast?: MaterialColorThemeSchema;
        light_medium_contrast?: MaterialColorThemeSchema;
        dark_high_contrast?: MaterialColorThemeSchema;
        light_high_contrast?: MaterialColorThemeSchema;
    };
    stylesheet: CSSStyleSheet;
}

type MaterialColorThemeRegistry = {
    [key: string]: MaterialColorTheme
}

function makeCSSRuleString(variant: string, schema: MaterialColorThemeSchema) {
    return `[mdc-theme="${variant}"] { ${Object.entries(schema).map(function ([key, value]) {
        return ` --mdc-${key}: ${value}; `;
    }).join("")} }`;
}

function defineTheme(
    args: {
        metadata: MaterialColorThemeMetadata
    } & MaterialColorTheme["schemas"]
): MaterialColorTheme {
    const sheet = new CSSStyleSheet();
    sheet.disabled = true;

    var rules = sheet.cssRules;
    sheet.insertRule(`[mdc-theme] { /* <element mdc-theme="<[dark|light|system]>"> */ ${Object.entries(
        args.metadata
    ).map(function ([key, value]) {
        return value ? ` --theme-${key}: ${value}; ` : ``;
    }).join("")} ${Object.entries({
        "--theme-preview--primary-dark": args.dark,
        "--theme-preview--primary-light": args.light,
        "--theme-preview--primary-dark__high-contrast": args.dark_high_contrast,
        "--theme-preview--primary-light__high-contrast": args.light_high_contrast,
        "--theme-preview--primary-dark__medium-contrast": args.dark_medium_contrast,
        "--theme-preview--primary-light__medium-contrast": args.light_medium_contrast
    }).map(function ([key, value]) {
        return value ? ` ${key}: ${value.primary}; ` : ``;
    }).join("")} }`, rules.length);

    sheet.insertRule(makeCSSRuleString("dark", args.dark), rules.length);
    sheet.insertRule(makeCSSRuleString("light", args.light), rules.length);

    let schema: MaterialColorThemeSchema | undefined;
    if (schema = args.dark_medium_contrast) sheet.insertRule(makeCSSRuleString("dark mediumContrast", schema), rules.length);
    if (schema = args.light_medium_contrast) sheet.insertRule(makeCSSRuleString("light mediumContrast", schema), rules.length);
    if (schema = args.dark_high_contrast) sheet.insertRule(makeCSSRuleString("dark highContrast", schema), rules.length);
    if (schema = args.light_high_contrast) sheet.insertRule(makeCSSRuleString("light highContrast", schema), rules.length);

    sheet.insertRule(`@media (prefers-color-scheme: dark) { ${makeCSSRuleString(
        "system", args.dark
    )} ${(schema = args.dark_medium_contrast) && args.light_medium_contrast ? makeCSSRuleString(
        "system mediumContrast", schema
    ) : ``} ${(schema = args.dark_high_contrast) && args.light_high_contrast ? makeCSSRuleString(
        "system highContrast", schema
    ) : ``} }`, rules.length);

    sheet.insertRule(`@media (prefers-color-scheme: light) { ${makeCSSRuleString(
        "system", args.light
    )} ${args.dark_medium_contrast && (schema = args.light_medium_contrast) ? makeCSSRuleString(
        "system mediumContrast", schema
    ) : ``} ${args.dark_high_contrast && (schema = args.light_high_contrast) ? makeCSSRuleString(
        "system highContrast", schema
    ) : ``} }`, rules.length);

    return {
        metadata: args.metadata,
        schemas: {
            dark: args.dark,
            light: args.light,
            dark_medium_contrast: args.dark_medium_contrast,
            light_medium_contrast: args.light_medium_contrast,
            dark_high_contrast: args.dark_high_contrast,
            light_high_contrast: args.light_high_contrast
        },
        stylesheet: sheet
    }
}

const themes: MaterialColorThemeRegistry = {
    "Material 1": defineTheme({
        "metadata": {
            "label": "Material 1"
        },
        "dark": {
            "primary": "rgb(131 210 229)",
            "surface-tint": "rgb(131 210 229)",
            "on-primary": "rgb(0 54 63)",
            "primary-container": "rgb(0 78 91)",
            "on-primary-container": "rgb(167 238 255)",
            "secondary": "rgb(178 203 210)",
            "on-secondary": "rgb(28 52 57)",
            "secondary-container": "rgb(51 74 80)",
            "on-secondary-container": "rgb(205 231 238)",
            "tertiary": "rgb(189 197 235)",
            "on-tertiary": "rgb(39 47 77)",
            "tertiary-container": "rgb(62 69 101)",
            "on-tertiary-container": "rgb(220 225 255)",
            "error": "rgb(255 180 171)",
            "on-error": "rgb(105 0 5)",
            "error-container": "rgb(147 0 10)",
            "on-error-container": "rgb(255 218 214)",
            "background": "rgb(15 20 22)",
            "on-background": "rgb(222 227 229)",
            "surface": "rgb(15 20 22)",
            "on-surface": "rgb(222 227 229)",
            "surface-variant": "rgb(63 72 75)",
            "on-surface-variant": "rgb(191 200 203)",
            "outline": "rgb(137 146 149)",
            "outline-variant": "rgb(63 72 75)",
            "shadow": "rgb(0 0 0)",
            "scrim": "rgb(0 0 0)",
            "inverse-surface": "rgb(222 227 229)",
            "inverse-on-surface": "rgb(43 49 51)",
            "inverse-primary": "rgb(0 104 120)",
            "primary-fixed": "rgb(167 238 255)",
            "on-primary-fixed": "rgb(0 31 37)",
            "primary-fixed-dim": "rgb(131 210 229)",
            "on-primary-fixed-variant": "rgb(0 78 91)",
            "secondary-fixed": "rgb(205 231 238)",
            "on-secondary-fixed": "rgb(5 31 36)",
            "secondary-fixed-dim": "rgb(178 203 210)",
            "on-secondary-fixed-variant": "rgb(51 74 80)",
            "tertiary-fixed": "rgb(220 225 255)",
            "on-tertiary-fixed": "rgb(18 26 55)",
            "tertiary-fixed-dim": "rgb(189 197 235)",
            "on-tertiary-fixed-variant": "rgb(62 69 101)",
            "surface-dim": "rgb(15 20 22)",
            "surface-bright": "rgb(52 58 60)",
            "surface-container-lowest": "rgb(9 15 17)",
            "surface-container-low": "rgb(23 29 30)",
            "surface-container": "rgb(27 33 34)",
            "surface-container-high": "rgb(37 43 45)",
            "surface-container-highest": "rgb(48 54 55)"
        },
        "light": {
            "primary": "rgb(0 104 120)",
            "surface-tint": "rgb(0 104 120)",
            "on-primary": "rgb(255 255 255)",
            "primary-container": "rgb(167 238 255)",
            "on-primary-container": "rgb(0 31 37)",
            "secondary": "rgb(75 98 104)",
            "on-secondary": "rgb(255 255 255)",
            "secondary-container": "rgb(205 231 238)",
            "on-secondary-container": "rgb(5 31 36)",
            "tertiary": "rgb(85 93 126)",
            "on-tertiary": "rgb(255 255 255)",
            "tertiary-container": "rgb(220 225 255)",
            "on-tertiary-container": "rgb(18 26 55)",
            "error": "rgb(186 26 26)",
            "on-error": "rgb(255 255 255)",
            "error-container": "rgb(255 218 214)",
            "on-error-container": "rgb(65 0 2)",
            "background": "rgb(245 250 252)",
            "on-background": "rgb(23 29 30)",
            "surface": "rgb(245 250 252)",
            "on-surface": "rgb(23 29 30)",
            "surface-variant": "rgb(219 228 231)",
            "on-surface-variant": "rgb(63 72 75)",
            "outline": "rgb(111 121 123)",
            "outline-variant": "rgb(191 200 203)",
            "shadow": "rgb(0 0 0)",
            "scrim": "rgb(0 0 0)",
            "inverse-surface": "rgb(43 49 51)",
            "inverse-on-surface": "rgb(236 242 243)",
            "inverse-primary": "rgb(131 210 229)",
            "primary-fixed": "rgb(167 238 255)",
            "on-primary-fixed": "rgb(0 31 37)",
            "primary-fixed-dim": "rgb(131 210 229)",
            "on-primary-fixed-variant": "rgb(0 78 91)",
            "secondary-fixed": "rgb(205 231 238)",
            "on-secondary-fixed": "rgb(5 31 36)",
            "secondary-fixed-dim": "rgb(178 203 210)",
            "on-secondary-fixed-variant": "rgb(51 74 80)",
            "tertiary-fixed": "rgb(220 225 255)",
            "on-tertiary-fixed": "rgb(18 26 55)",
            "tertiary-fixed-dim": "rgb(189 197 235)",
            "on-tertiary-fixed-variant": "rgb(62 69 101)",
            "surface-dim": "rgb(213 219 221)",
            "surface-bright": "rgb(245 250 252)",
            "surface-container-lowest": "rgb(255 255 255)",
            "surface-container-low": "rgb(239 244 246)",
            "surface-container": "rgb(233 239 241)",
            "surface-container-high": "rgb(228 233 235)",
            "surface-container-highest": "rgb(222 227 229)"
        },
        "dark_medium_contrast": {
            "primary": "rgb(136 214 233)",
            "surface-tint": "rgb(131 210 229)",
            "on-primary": "rgb(0 25 31)",
            "primary-container": "rgb(75 156 173)",
            "on-primary-container": "rgb(0 0 0)",
            "secondary": "rgb(182 207 214)",
            "on-secondary": "rgb(1 25 31)",
            "secondary-container": "rgb(124 149 155)",
            "on-secondary-container": "rgb(0 0 0)",
            "tertiary": "rgb(194 201 239)",
            "on-tertiary": "rgb(12 20 49)",
            "tertiary-container": "rgb(136 143 179)",
            "on-tertiary-container": "rgb(0 0 0)",
            "error": "rgb(255 186 177)",
            "on-error": "rgb(55 0 1)",
            "error-container": "rgb(255 84 73)",
            "on-error-container": "rgb(0 0 0)",
            "background": "rgb(15 20 22)",
            "on-background": "rgb(222 227 229)",
            "surface": "rgb(15 20 22)",
            "on-surface": "rgb(246 252 253)",
            "surface-variant": "rgb(63 72 75)",
            "on-surface-variant": "rgb(195 204 207)",
            "outline": "rgb(155 164 167)",
            "outline-variant": "rgb(123 133 135)",
            "shadow": "rgb(0 0 0)",
            "scrim": "rgb(0 0 0)",
            "inverse-surface": "rgb(222 227 229)",
            "inverse-on-surface": "rgb(37 43 45)",
            "inverse-primary": "rgb(0 80 92)",
            "primary-fixed": "rgb(167 238 255)",
            "on-primary-fixed": "rgb(0 20 24)",
            "primary-fixed-dim": "rgb(131 210 229)",
            "on-primary-fixed-variant": "rgb(0 60 70)",
            "secondary-fixed": "rgb(205 231 238)",
            "on-secondary-fixed": "rgb(0 20 24)",
            "secondary-fixed-dim": "rgb(178 203 210)",
            "on-secondary-fixed-variant": "rgb(34 58 63)",
            "tertiary-fixed": "rgb(220 225 255)",
            "on-tertiary-fixed": "rgb(7 15 44)",
            "tertiary-fixed-dim": "rgb(189 197 235)",
            "on-tertiary-fixed-variant": "rgb(45 53 83)",
            "surface-dim": "rgb(15 20 22)",
            "surface-bright": "rgb(52 58 60)",
            "surface-container-lowest": "rgb(9 15 17)",
            "surface-container-low": "rgb(23 29 30)",
            "surface-container": "rgb(27 33 34)",
            "surface-container-high": "rgb(37 43 45)",
            "surface-container-highest": "rgb(48 54 55)"
        },
        "light_medium_contrast": {
            "primary": "rgb(0 74 86)",
            "surface-tint": "rgb(0 104 120)",
            "on-primary": "rgb(255 255 255)",
            "primary-container": "rgb(40 127 144)",
            "on-primary-container": "rgb(255 255 255)",
            "secondary": "rgb(47 70 76)",
            "on-secondary": "rgb(255 255 255)",
            "secondary-container": "rgb(97 120 127)",
            "on-secondary-container": "rgb(255 255 255)",
            "tertiary": "rgb(58 65 97)",
            "on-tertiary": "rgb(255 255 255)",
            "tertiary-container": "rgb(107 115 149)",
            "on-tertiary-container": "rgb(255 255 255)",
            "error": "rgb(140 0 9)",
            "on-error": "rgb(255 255 255)",
            "error-container": "rgb(218 52 46)",
            "on-error-container": "rgb(255 255 255)",
            "background": "rgb(245 250 252)",
            "on-background": "rgb(23 29 30)",
            "surface": "rgb(245 250 252)",
            "on-surface": "rgb(23 29 30)",
            "surface-variant": "rgb(219 228 231)",
            "on-surface-variant": "rgb(59 68 71)",
            "outline": "rgb(88 97 99)",
            "outline-variant": "rgb(115 124 127)",
            "shadow": "rgb(0 0 0)",
            "scrim": "rgb(0 0 0)",
            "inverse-surface": "rgb(43 49 51)",
            "inverse-on-surface": "rgb(236 242 243)",
            "inverse-primary": "rgb(131 210 229)",
            "primary-fixed": "rgb(40 127 144)",
            "on-primary-fixed": "rgb(255 255 255)",
            "primary-fixed-dim": "rgb(0 101 117)",
            "on-primary-fixed-variant": "rgb(255 255 255)",
            "secondary-fixed": "rgb(97 120 127)",
            "on-secondary-fixed": "rgb(255 255 255)",
            "secondary-fixed-dim": "rgb(72 96 102)",
            "on-secondary-fixed-variant": "rgb(255 255 255)",
            "tertiary-fixed": "rgb(107 115 149)",
            "on-tertiary-fixed": "rgb(255 255 255)",
            "tertiary-fixed-dim": "rgb(83 90 123)",
            "on-tertiary-fixed-variant": "rgb(255 255 255)",
            "surface-dim": "rgb(213 219 221)",
            "surface-bright": "rgb(245 250 252)",
            "surface-container-lowest": "rgb(255 255 255)",
            "surface-container-low": "rgb(239 244 246)",
            "surface-container": "rgb(233 239 241)",
            "surface-container-high": "rgb(228 233 235)",
            "surface-container-highest": "rgb(222 227 229)"
        },
        "dark_high_contrast": {
            "primary": "rgb(243 252 255)",
            "surface-tint": "rgb(131 210 229)",
            "on-primary": "rgb(0 0 0)",
            "primary-container": "rgb(136 214 233)",
            "on-primary-container": "rgb(0 0 0)",
            "secondary": "rgb(243 252 255)",
            "on-secondary": "rgb(0 0 0)",
            "secondary-container": "rgb(182 207 214)",
            "on-secondary-container": "rgb(0 0 0)",
            "tertiary": "rgb(252 250 255)",
            "on-tertiary": "rgb(0 0 0)",
            "tertiary-container": "rgb(194 201 239)",
            "on-tertiary-container": "rgb(0 0 0)",
            "error": "rgb(255 249 249)",
            "on-error": "rgb(0 0 0)",
            "error-container": "rgb(255 186 177)",
            "on-error-container": "rgb(0 0 0)",
            "background": "rgb(15 20 22)",
            "on-background": "rgb(222 227 229)",
            "surface": "rgb(15 20 22)",
            "on-surface": "rgb(255 255 255)",
            "surface-variant": "rgb(63 72 75)",
            "on-surface-variant": "rgb(243 252 255)",
            "outline": "rgb(195 204 207)",
            "outline-variant": "rgb(195 204 207)",
            "shadow": "rgb(0 0 0)",
            "scrim": "rgb(0 0 0)",
            "inverse-surface": "rgb(222 227 229)",
            "inverse-on-surface": "rgb(0 0 0)",
            "inverse-primary": "rgb(0 47 55)",
            "primary-fixed": "rgb(182 240 255)",
            "on-primary-fixed": "rgb(0 0 0)",
            "primary-fixed-dim": "rgb(136 214 233)",
            "on-primary-fixed-variant": "rgb(0 25 31)",
            "secondary-fixed": "rgb(210 235 242)",
            "on-secondary-fixed": "rgb(0 0 0)",
            "secondary-fixed-dim": "rgb(182 207 214)",
            "on-secondary-fixed-variant": "rgb(1 25 31)",
            "tertiary-fixed": "rgb(226 229 255)",
            "on-tertiary-fixed": "rgb(0 0 0)",
            "tertiary-fixed-dim": "rgb(194 201 239)",
            "on-tertiary-fixed-variant": "rgb(12 20 49)",
            "surface-dim": "rgb(15 20 22)",
            "surface-bright": "rgb(52 58 60)",
            "surface-container-lowest": "rgb(9 15 17)",
            "surface-container-low": "rgb(23 29 30)",
            "surface-container": "rgb(27 33 34)",
            "surface-container-high": "rgb(37 43 45)",
            "surface-container-highest": "rgb(48 54 55)"
        },
        "light_high_contrast": {
            "primary": "rgb(0 38 46)",
            "surface-tint": "rgb(0 104 120)",
            "on-primary": "rgb(255 255 255)",
            "primary-container": "rgb(0 74 86)",
            "on-primary-container": "rgb(255 255 255)",
            "secondary": "rgb(13 37 43)",
            "on-secondary": "rgb(255 255 255)",
            "secondary-container": "rgb(47 70 76)",
            "on-secondary-container": "rgb(255 255 255)",
            "tertiary": "rgb(25 33 62)",
            "on-tertiary": "rgb(255 255 255)",
            "tertiary-container": "rgb(58 65 97)",
            "on-tertiary-container": "rgb(255 255 255)",
            "error": "rgb(78 0 2)",
            "on-error": "rgb(255 255 255)",
            "error-container": "rgb(140 0 9)",
            "on-error-container": "rgb(255 255 255)",
            "background": "rgb(245 250 252)",
            "on-background": "rgb(23 29 30)",
            "surface": "rgb(245 250 252)",
            "on-surface": "rgb(0 0 0)",
            "surface-variant": "rgb(219 228 231)",
            "on-surface-variant": "rgb(29 37 40)",
            "outline": "rgb(59 68 71)",
            "outline-variant": "rgb(59 68 71)",
            "shadow": "rgb(0 0 0)",
            "scrim": "rgb(0 0 0)",
            "inverse-surface": "rgb(43 49 51)",
            "inverse-on-surface": "rgb(255 255 255)",
            "inverse-primary": "rgb(199 243 255)",
            "primary-fixed": "rgb(0 74 86)",
            "on-primary-fixed": "rgb(255 255 255)",
            "primary-fixed-dim": "rgb(0 50 59)",
            "on-primary-fixed-variant": "rgb(255 255 255)",
            "secondary-fixed": "rgb(47 70 76)",
            "on-secondary-fixed": "rgb(255 255 255)",
            "secondary-fixed-dim": "rgb(24 48 54)",
            "on-secondary-fixed-variant": "rgb(255 255 255)",
            "tertiary-fixed": "rgb(58 65 97)",
            "on-tertiary-fixed": "rgb(255 255 255)",
            "tertiary-fixed-dim": "rgb(35 43 73)",
            "on-tertiary-fixed-variant": "rgb(255 255 255)",
            "surface-dim": "rgb(213 219 221)",
            "surface-bright": "rgb(245 250 252)",
            "surface-container-lowest": "rgb(255 255 255)",
            "surface-container-low": "rgb(239 244 246)",
            "surface-container": "rgb(233 239 241)",
            "surface-container-high": "rgb(228 233 235)",
            "surface-container-highest": "rgb(222 227 229)"
        }
    }),
    [Symbol.toStringTag]() { return "assets.colors.themeRegistry.ts:MaterialColorThemeRegistry" }
}

export default themes;