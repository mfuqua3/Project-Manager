import {useApiArea} from "../utils/env";
import {ProblemDetails, TokenModel} from "../domain/models";
import axios from "axios";
import {Result} from "./Result";

export type RawHttpResult<T = undefined> = { statusText: string, status: number } &
    ({ isSuccessStatusCode: true, data: T } | {
        isSuccessStatusCode: false,
        data: ProblemDetails
    });

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



