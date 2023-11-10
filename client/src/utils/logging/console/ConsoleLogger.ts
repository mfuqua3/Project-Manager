import {LogLevel} from "../LogLevel";
import {LoggingAdapter} from "../LoggingAdapter";

export interface ConsoleLoggerConfiguration {
    logLevel?: LogLevel;
}

// Utility function to format the timestamp
function formatTimestamp(date: Date): string {
    return date.toISOString().substr(11, 8);
}

// Utility function to format the log level
function formatLogLevel(level: LogLevel): string {
    switch (level) {
        case LogLevel.Trace:
            return "TRC";
        case LogLevel.Debug:
            return "DBG";
        case LogLevel.Information:
            return "INF";
        case LogLevel.Warning:
            return "WRN";
        case LogLevel.Error:
            return "ERR";
        case LogLevel.Critical:
            return "CRT";
        case LogLevel.None:
            return "N/A";

    }
}

function templatedMessage(messageTemplateArray: string[], templateProperties: {
    [key: string]: string | undefined
}): string[] {
    return messageTemplateArray.map((templateItem, idx) => {
        let value: string;
        if (idx % 2 === 0) {
            value = templateItem;
        } else {
            value = templateProperties[templateItem] ?? "undefined";
        }
        if (idx < messageTemplateArray.length - 1) {
            value += "%c";
        }
        return value;
    });
}

const baseStyle = "font-weight: bold;";
const defaultStyle = `${baseStyle} background: inherit; color: inherit;`;

// Utility function to style the console log output
function consoleLogLevelStyle(level: LogLevel): string {

    if (level === LogLevel.Error || level === LogLevel.Critical) {
        return `${baseStyle} background: red;`;
    }
    if (level == LogLevel.Warning) {
        return `${baseStyle} color: yellow;`;
    }
    if (level == LogLevel.Debug) {
        return `${baseStyle} color: #D3D3D3;`;
    }

    return `${baseStyle} color: inherit;`;
}

function argumentStyle(argument: string): string {
    const withoutStyle = argument.endsWith('%c') ? argument.substring(0, argument.length - 2) : argument;
    if (!isNaN(Number(withoutStyle))) {
        return `${baseStyle} color: #CBC3E3;`;
    }
    if (withoutStyle === 'undefined') {
        return `${baseStyle} color: #FFCCCB;`;

    }
    return `${baseStyle} color: teal;`;
}

export default class ConsoleLogger implements LoggingAdapter {
    name = "Console";
    private readonly currentLogLevel: LogLevel;

    constructor(private readonly configuration: ConsoleLoggerConfiguration) {
        this.currentLogLevel = configuration.logLevel ?? LogLevel.Debug;
    }

    log(level: LogLevel, loggerType: string, messageTemplateArray: string[], templateVals: {
        [p: string]: string | undefined
    }): void {
        if (level < this.currentLogLevel) {
            return;
        }
        const timestamp = formatTimestamp(new Date());
        const levelString = formatLogLevel(level);
        const message = templatedMessage(messageTemplateArray, templateVals);
        console.log(
            `[${timestamp} %c${levelString}%c] <%c${loggerType.toString()}%c> ${message.join('')}`,
            ...[
                consoleLogLevelStyle(level),
                defaultStyle,
                `${baseStyle} color: orange;`,
                defaultStyle,
                ...message.flatMap((_, idx) => {
                    if (idx >= message.length - 1) {
                        return [];
                    }
                    const returned = idx % 2 === 0 ? [argumentStyle(message[idx + 1])] : [defaultStyle];
                    return returned;
                })
            ]
        );
    }
}