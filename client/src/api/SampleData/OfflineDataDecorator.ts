import {RawHttpResult} from "../RawHttpResult";

const offlineMode = process.env["REACT_APP_IS_OFFLINE_ENABLED"] === "on";
export function OfflineData<T>(sampleData: T) {
    return function (
        _target: unknown,
        _propertyKey: unknown,
        descriptor: TypedPropertyDescriptor<any>
    ) {
        const originalMethod = descriptor.value;

        descriptor.value = function (...args: any[]) {
            if (offlineMode) {
                const result: RawHttpResult<T> = {
                    isSuccessStatusCode: true,
                    data: sampleData,
                    status: 200,
                    statusText: "OK"
                }
                return Promise.resolve(result);
            } else {
                return originalMethod.apply(this, args);
            }
        };
    }
}

