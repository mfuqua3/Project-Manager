import {LoggerContext} from "./LoggerContext";
import React, {ReactNode, useEffect} from "react";
import {isDevelopment} from "../env";
import {Logger} from "./Logger";
import {LoggingAdapter} from "./LoggingAdapter";

export interface LoggingProviderLogger {
    logger: LoggingAdapter;
    devOnly?: boolean;
}

export type LoggerTypes = LoggingAdapter | LoggingProviderLogger;

function isProviderLogger(candidate: LoggerTypes): candidate is LoggingProviderLogger {
    return "logger" in candidate;
}

export interface LoggingProviderProps {
    loggers: LoggerTypes[];
    children: ReactNode | ReactNode[];
}


function LoggingProvider({loggers, children}: LoggingProviderProps) {
    function getLoggingAdapters(): LoggingAdapter[] {
        const pureLoggers = loggers.filter(l => !isProviderLogger(l)) as LoggingAdapter[];
        const providerLoggers = loggers.filter(l => isProviderLogger(l)) as LoggingProviderLogger[];
        return [...pureLoggers, ...providerLoggers
            .filter(pl => isDevelopment() || pl.devOnly === undefined || !pl.devOnly)
            .map(pl => pl.logger)];
    }

    useEffect(() => {
        const adapters = getLoggingAdapters();
        const names = adapters.map(x => x.name);
        for (const loadedLogger of adapters) {
            const logger = new Logger(loadedLogger, LoggingProvider);
            logger.debug(`Initializing Logging Providers...`, {})
            for (const loggerName of names) {
                logger.debug('{loggerName} loaded', {loggerName});
            }
        }
    }, []);
    return (
        <LoggerContext.Provider value={getLoggingAdapters()}>
            {children}
        </LoggerContext.Provider>
    );
}

export default React.memo(LoggingProvider);