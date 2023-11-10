// src/useLogger.ts
import {useContext} from "react";
import {LoggerContext} from "./LoggerContext";
import {ErrorTypes, ILogger, Logger} from "./Logger";

interface Type<T = any> extends Function {
    new(...args: any[]): T;
}

export type LoggerType = string | symbol | Type | ((...args: any[]) => any);

export function useLogger(type: LoggerType): ILogger {
    const loggingAdapters = useContext(LoggerContext);
    const loggers = loggingAdapters.map(x => new Logger(x, type));
    return {
        debug: (messageTemplate, properties) =>
            loggers.forEach(x => x.debug(messageTemplate, properties ?? {})),
        error: (error: ErrorTypes, messageTemplate: string, properties: { [p: string]: string }) =>
            loggers.forEach(x => x.error(error, messageTemplate ?? "", properties ?? {})),
        info: (messageTemplate, properties) =>
            loggers.forEach(x => x.info(messageTemplate, properties ?? {})),
        warn: (messageTemplate, properties) =>
            loggers.forEach(x => x.warn(messageTemplate, properties ?? {})),
    };
}
