import {LoggingAdapter} from "./LoggingAdapter";
import {LogLevel} from "./LogLevel";
import {parseTemplateLikeString} from "./loggingHelpers";
import {ProblemDetails} from "../../domain/models";
import {isProblemDetails} from "../guards";
import {StringBuilder} from "../helpers";
import {isDevelopment} from "../env";
import {LoggerType} from "./useLogger";

export type ErrorTypes = Error | ProblemDetails;

export interface ILogger {
    debug(messageTemplate: string, properties?: { [key: string]: string }): void,

    info(messageTemplate: string, properties?: { [key: string]: string }): void,

    warn(messageTemplate: string, properties?: { [key: string]: string }): void,

    error(error: ErrorTypes, messageTemplate?: string, properties?: { [key: string]: string }): void,
}

export interface StructuredLog {
    messageTemplate: string,
    properties: { [key: string]: string | undefined },
}

export class Logger implements ILogger {
    private readonly loggerType: string;

    constructor(private readonly adapter: LoggingAdapter, private readonly type: LoggerType) {
        if (typeof type === 'function') {
            this.loggerType = type.name;
        } else {
            this.loggerType = type.toString();
        }
    }

    debug(messageTemplate: string, properties: { [key: string]: string } = {}): void {
        const templateLike = parseTemplateLikeString(messageTemplate);
        this.adapter.log(LogLevel.Debug, this.loggerType, templateLike, properties);
    }

    info(messageTemplate: string, properties: { [key: string]: string } = {}): void {
        const templateLike = parseTemplateLikeString(messageTemplate);
        this.adapter.log(LogLevel.Information, this.loggerType, templateLike, properties);
    }

    warn(messageTemplate: string, properties: { [key: string]: string } = {}): void {
        const templateLike = parseTemplateLikeString(messageTemplate);
        this.adapter.log(LogLevel.Warning, this.loggerType, templateLike, properties);
    }

    error(error: ErrorTypes, messageTemplate = "", properties: { [key: string]: string } = {}): void {
        const structuredErrorLog = this.formatErrorType(error);
        const templateLike = parseTemplateLikeString((messageTemplate ?? '') + '\n' + structuredErrorLog.messageTemplate);
        this.adapter.log(LogLevel.Error, this.loggerType, templateLike, {...properties, ...structuredErrorLog.properties});
    }

    private formatErrorType(error: ErrorTypes) {
        if (isProblemDetails(error)) {
            return this.formatProblemDetails(error);
        }
        return this.formatJavascriptError(error);
    }

    private formatJavascriptError(jsError: Error): StructuredLog {
        return {messageTemplate: jsError.toString(), properties: {}};
    }

    private formatProblemDetails(apiError: ProblemDetails): StructuredLog {
        const templateSb = new StringBuilder();
        templateSb.append("{status}: {title}");
        if (apiError.detail) {
            templateSb.append(' - {detail}');
        }
        if (isDevelopment()) {
            templateSb.appendLine('');
            templateSb.append("traceId: {traceId}");
        }
        if (apiError.error) {
            templateSb.appendLine('');
            templateSb.appendLine('exception: {exception}');
            templateSb.appendLine('message: {message}');
            templateSb.append('{stackTrace}');
        }
        return {
            messageTemplate: templateSb.toString(),
            properties: {
                title: apiError.title,
                status: apiError.status.toString(),
                traceId: apiError.traceId,
                detail: apiError.detail,
                exception: apiError.error?.name ?? undefined,
                message: apiError.error?.message ?? undefined,
                stackTrace: apiError.error?.stackTrace ?? undefined
            }
        }
    }
}