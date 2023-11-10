// src/SeqLogProvider.tsx
import {Logger as SeqLoggingLogger, SeqLogLevel} from "seq-logging";
import {LogLevel} from "../LogLevel";
import {LoggingAdapter} from "../LoggingAdapter";

interface SeqLoggerConfiguration {
    serverUrl: string;
    apiKey?: string;
    logLevel?: LogLevel;
    onError?: (e: Error) => void;
}

export default class SeqLogger implements LoggingAdapter {
    name = "Seq";
    private readonly seqLogger: SeqLoggingLogger;
    private readonly currentLogLevel: LogLevel;

    constructor(private readonly configuration: SeqLoggerConfiguration) {
        this.seqLogger = new SeqLoggingLogger({
            serverUrl: configuration.serverUrl,
            apiKey: configuration.apiKey,
            onError: configuration.onError ?? ((e) => {
                console.error("Seq logging error:", e);
            })
        });
        this.currentLogLevel = configuration.logLevel ?? LogLevel.Debug;
    }

    log(level: LogLevel, type: string, messageTemplateArray: string[], templateVals: { [p: string]: string }): void {
        let messageTemplate = "";
        let templateItem = false;
        for (const messageTemplateElement of messageTemplateArray) {
            if (templateItem) {
                messageTemplate += '{';
            }
            messageTemplate += messageTemplateElement;
            if (templateItem) {
                messageTemplate += '}';
            }
            templateItem = !templateItem;
        }
        templateVals['callSource'] = type.toString();
        let logLevel: SeqLogLevel;
        switch (level) {
            case LogLevel.Trace:
                logLevel = "Verbose";
                break;
            case LogLevel.Debug:
                logLevel = "Debug";
                break;
            case LogLevel.Information:
                logLevel = "Information";
                break;
            case LogLevel.Warning:
                logLevel = "Warning";
                break;
            case LogLevel.Error:
                logLevel = "Error"
                break;
            case LogLevel.Critical:
                logLevel = "Fatal";
                break;
            case LogLevel.None:
                return;

        }
        this.seqLogger.emit({
            timestamp: new Date(),
            level: logLevel,
            messageTemplate,
            properties: templateVals
        })
    }
}
