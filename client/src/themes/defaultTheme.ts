import {createTheme} from "@mui/material";

const primary = {
    0: "#000022",
    5: "#001f3f",
    10: "#003355",
    20: "#004677",
    25: "#005999",
    30: "#006BBB",
    35: "#007FDD",
    40: "#0093FF",
    50: "#33A2FF",
    60: "#66B1FF",
    70: "#99C0FF",
    80: "#CCD0FF",
    90: "#EEEBFF",
    95: "#F5F5FF",
    98: "#FAFBFF",
    99: "#FDFFFF",
    100: "#FFFFFF"
}

// Define color keys
type ColorKeys = keyof typeof primary
type Colors = {[K in ColorKeys]: string}
const secondary: Colors = {
    0: "#000000",
    5: "#06161E",
    10: "#0F2E3A",
    20: "#1E5B75",
    25: "#28789D",
    30: "#3295C5",
    35: "#3AA4ED",
    40: "#45B3FF",
    50: "#68C1FF",
    60: "#90CFFF",
    70: "#B8DEFF",
    80: "#DFF2FF",
    90: "#F1F8FF",
    95: "#FCFEFF",
    98: "#FEFFFF",
    99: "#FFFEFF",
    100: "#FFFFFF"
};


const colorValues = {
    primary,
    secondary,
    tertiary: primary,
    neutral: secondary
}
const getColorAssignment = (color: Colors) => ({
    main: color["20"],
    contrastText: color["100"],
    fixed: color["90"],
    dim: color["80"],
    container: color["90"],
});

const theme = createTheme({
    palette: {
        background: {
            default: colorValues.tertiary["95"]
        },
        primary: getColorAssignment(colorValues.primary),
        onPrimary: getColorAssignment(colorValues.primary),
        secondary: getColorAssignment(colorValues.secondary),
        onSecondary: getColorAssignment(colorValues.secondary),
        tertiary: getColorAssignment(colorValues.tertiary),
        onTertiary: getColorAssignment(colorValues.tertiary),
        neutral: {
            main: colorValues.neutral["95"],
            dark: colorValues.neutral["80"],
            light: colorValues.neutral["99"],
            contrastText: colorValues.neutral["0"]
        },
        error: {
            main: "#f44336",
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