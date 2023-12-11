import {ProblemDetails} from "../../domain/models";
import {RawHttpResult} from "../RawHttpResult";
import {isOfflineMode} from "../../utils/helpers";

export function NoOfflineMode() {
    return function (
        _target: unknown,
        _propertyKey: string,
        descriptor: TypedPropertyDescriptor<any>
    ) {
        const originalMethod = descriptor.value;

        descriptor.value = function (...args: any[]) {
            if (isOfflineMode) {
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