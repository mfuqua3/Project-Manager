import {createTheme} from "@mui/material";

const primary = {
    0: "#000000",
    5: "#00150C",
    10: "#002116",
    20: "#003827",
    25: "#004431",
    30: "#00513B",
    35: "#005E45",
    40: "#006C4F",
    50: "#008864",
    60: "#26A37C",
    70: "#4ABF95",
    80: "#68DBAF",
    90: "#86F8CA",
    95: "#BCFFE0",
    98: "#E7FFF1",
    99: "#F4FFF7",
    100: "#FFFFFF"
}
const secondary = {
    0: "#000000",
    5: "#01150D",
    10: "#092017",
    20: "#1F352B",
    25: "#2A4036",
    30: "#354B41",
    35: "#40574D",
    40: "#4C6358",
    50: "#657C71",
    60: "#7E968A",
    70: "#98B1A4",
    80: "#B3CCBF",
    90: "#CFE9DA",
    95: "#DDF7E8",
    98: "#E7FFF1",
    99: "#F4FFF7",
    100: "#FFFFFF"
};
const tertiary = {
    0: "#000000",
    5: "#250700",
    10: "#360F00",
    20: "#591D00",
    25: "#6B2400",
    30: "#7B2F08",
    35: "#8A3A14",
    40: "#9A461F",
    50: "#B95D34",
    60: "#D9764B",
    70: "#F98F62",
    80: "#FFB598",
    90: "#FFDBCE",
    95: "#FFEDE7",
    98: "#FFF8F6",
    99: "#FFFBFF",
    100: "#FFFFFF"
}
const neutral = {
    0: "#000000",
    5: "#0E1210",
    10: "#191C1A",
    20: "#2E312F",
    25: "#393C3A",
    30: "#444845",
    35: "#505351",
    40: "#5C5F5C",
    50: "#757875",
    60: "#8E918E",
    70: "#A9ACA9",
    80: "#C5C7C4",
    90: "#E1E3DF",
    95: "#EFF1EE",
    98: "#F8FAF6",
    99: "#FBFDF9",
    100: "#FFFFFF"
};
const theme = createTheme({
    palette: {
        background: {
            default: tertiary["95"]
        },
        primary: {
            main: primary["40"],
            contrastText: primary["100"],
            fixed: primary["90"],
            dim: primary["80"],
            container: primary["90"],
        },
        onPrimary: {
            main: primary["100"],
            contrastText: primary["40"],
            fixed: primary["10"],
            dim: primary["30"],
            container: primary["10"],
        },
        secondary: {
            main: secondary["40"],
            contrastText: secondary["100"],
            fixed: secondary["90"],
            dim: secondary["80"],
            container: secondary["90"],
        },
        onSecondary: {
            main: secondary["100"],
            contrastText: secondary["40"],
            fixed: secondary["10"],
            dim: secondary["30"],
            container: secondary["10"],
        },
        tertiary: {
            main: tertiary["40"],
            contrastText: tertiary["100"],
            fixed: tertiary["90"],
            dim: tertiary["80"],
            container: tertiary["90"],
        },
        onTertiary: {
            main: tertiary["100"],
            contrastText: tertiary["40"],
            fixed: tertiary["10"],
            dim: tertiary["30"],
            container: tertiary["10"],
        },
        neutral: {
            main: neutral["95"],
            dark: neutral["80"],
            light: neutral["99"],
            contrastText: neutral["0"]
        },
        error: {
            main: "#BA1A1A",
        },
        info: {
            main: "#2196f3",
        },
        success: {
            main: "#4caf50",
        },
        warning: {
            main: "#ff9800",
        }
    },
    typography: {
        fontSize: 12,
        fontFamily: "DM Sans",
        body1: {
            fontWeight: 300,
            fontSize: "14px",
            lineHeight: "16px",
        },
    }

});

declare module '@mui/material/styles' {

    interface Palette {
        neutral: Palette['primary'];
        tertiary: Palette['primary'];
        onPrimary: Palette['primary'];
        onSecondary: Palette['primary'];
        onTertiary: Palette['primary'];
    }

    interface PaletteOptions {
        neutral: PaletteOptions['primary'];
        tertiary: PaletteOptions['primary'];
        onPrimary: PaletteOptions['primary'];
        onSecondary: PaletteOptions['primary'];
        onTertiary: PaletteOptions['primary'];
    }

    interface PaletteColor {
        container?: string;
        fixed?: string;
        dim?: string;
    }

    interface SimplePaletteColorOptions {
        container?: string;
        fixed?: string;
        dim?: string;
    }
}

export const defaultTheme = theme;