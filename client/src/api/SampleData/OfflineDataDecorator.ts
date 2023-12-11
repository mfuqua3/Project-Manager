import {RawHttpResult} from "../RawHttpResult";
import {isOfflineMode} from "../../utils/helpers";

export function OfflineData<T>(sampleData: T | (() => T)) {
    return function (
        _target: unknown,
        _propertyKey: unknown,
        descriptor: TypedPropertyDescriptor<any>
    ) {
        const originalMethod = descriptor.value;

        descriptor.value = function (...args: any[]) {
            if (isOfflineMode) {
                let resolvedData: T;
                if(typeof sampleData === "function"){
                    resolvedData = (sampleData as () => T)();
                } else {
                    resolvedData = sampleData as T;
                }
                const result: RawHttpResult<T> = {
                    isSuccessStatusCode: true,
                    data: resolvedData,
                    status: 200,
                    statusText: "OK"
                }
                return Promise.resolve(result);
            } else {
                return originalMethod.apply(this, args);
            }
        };
        return descriptor;
    }
}