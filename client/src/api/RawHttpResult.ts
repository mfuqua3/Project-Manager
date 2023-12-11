import {ProblemDetails} from "../domain/models";

export type RawHttpResult<T = undefined> = { statusText: string, status: number } &
    ({ isSuccessStatusCode: true, data: T } | {
        isSuccessStatusCode: false,
        data: ProblemDetails
    });