import {AxiosResponse} from "axios";
import {ProblemDetails} from "../domain/models";
import {isProblemDetails} from "../utils/guards";

import {RawHttpResult} from "./RawHttpResult";

export function Result<T>(response: AxiosResponse<T, unknown>): RawHttpResult<T> {
    const isSuccessStatusCode = response.status >= 200 && response.status < 400;
    if (isSuccessStatusCode) {
        return {
            isSuccessStatusCode: true,
            data: response.data,
            status: response.status,
            statusText: response.statusText
        };
    }
    let error: ProblemDetails;
    if (isProblemDetails(response.data as unknown as object)) {
        error = response.data as unknown as ProblemDetails;
    } else {
        throw new Error(`Unknown error during execution of HTTP Request`);
    }
    return {
        isSuccessStatusCode: false,
        data: error,
        status: response.status,
        statusText: response.statusText
    };
}