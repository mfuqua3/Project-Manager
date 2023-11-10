import {useApiArea} from "../utils/env";
import {ProblemDetails, TokenModel} from "../domain/models";
import axios, {AxiosResponse} from "axios";
import {isProblemDetails} from "../utils/guards";

export type RawHttpResult<T = undefined> = { statusText: string, status: number } &
    ({ isSuccessStatusCode: true, data: T } | {
        isSuccessStatusCode: false,
        data: ProblemDetails
    });

function Result<T>(response: AxiosResponse<T, unknown>): RawHttpResult<T> {
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

export class AuthorizationsApi {
    static apiArea = useApiArea('authorizations');

    static async authorizeGoogleSignin(idToken: string): Promise<RawHttpResult<TokenModel>> {
        const url = this.apiArea.urlForEndpoint('google');
        const response = await axios.post<TokenModel>(url, {idToken}, {withCredentials: true, validateStatus: null});
        return Result(response);
    }

    static async refreshToken(): Promise<RawHttpResult<TokenModel>> {
        const url = this.apiArea.urlForEndpoint('refresh');
        const response = await axios.get<TokenModel>(url, {withCredentials: true, validateStatus: null});
        return Result(response);
    }

    static async signout(): Promise<RawHttpResult> {
        const url = this.apiArea.urlForEndpoint("signout");
        const response = await axios.get(url, {withCredentials: true, validateStatus: null});
        return Result(response);
    }
}