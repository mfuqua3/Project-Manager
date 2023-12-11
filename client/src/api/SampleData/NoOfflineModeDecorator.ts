import {ProblemDetails} from "../../domain/models";
import {RawHttpResult} from "../RawHttpResult";
const offlineMode = process.env["REACT_APP_IS_OFFLINE_ENABLED"] === "on";

export function NoOfflineMode() {
    return function (
        _target: unknown,
        _propertyKey: string,
        descriptor: TypedPropertyDescriptor<any>
    ) {
        const originalMethod = descriptor.value;

        descriptor.value = function (...args: any[]) {
            if (offlineMode) {
                const problem: ProblemDetails = {
                    detail: `${_propertyKey} is not supported in offline mode.`,
                    status: 501,
                    title: "Not Implemented",
                    type: "https://tools.ietf.org/html/rfc7231#section-6.6.2"
                }
                const result: RawHttpResult<unknown> = {
                    data: problem,
                    isSuccessStatusCode: false,
                    status: problem.status,
                    statusText: problem.title
                }
                return Promise.resolve(result);
            } else {
                return originalMethod.apply(this, args);
            }
        };
    }
}