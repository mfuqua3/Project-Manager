import {LogLevel} from "./LogLevel";

export interface LoggingAdapter {
    name: string;

    log(level: LogLevel, type: string, messageTemplateArray: string[], templateProperties: {
        [key: string]: string | undefined
    }): void;
}