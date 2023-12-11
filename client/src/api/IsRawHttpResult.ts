import {isProblemDetails} from "../utils/guards";

import {RawHttpResult} from "./RawHttpResult";

export function isRawHttpResult<T>(arg: any): arg is RawHttpResult<T> {
    return arg && arg.statusText && typeof (arg.statusText) === 'string' &&
        arg.status && typeof (arg.status) === 'number' &&
        ((arg.isSuccessStatusCode && arg.data && typeof (arg.isSuccessStatusCode) === 'boolean') ||
            (arg.isSuccessStatusCode === false && isProblemDetails(arg.data)));
}