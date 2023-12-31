import {useApiArea} from "../utils/env";
import {TokenModel} from "../domain/models";
import axios from "axios";
import {Result} from "./Result";
import {RawHttpResult} from "./RawHttpResult";
import {NoOfflineMode} from "./SampleData/NoOfflineModeDecorator";
import {OfflineData} from "./SampleData/OfflineDataDecorator";
import AccessTokenSampleData from "./SampleData/AccessTokenSampleData";

export class AuthorizationsApi {
    static apiArea = useApiArea('authorizations');

    @OfflineData(AccessTokenSampleData)
    static async authorizeGoogleSignin(idToken: string): Promise<RawHttpResult<TokenModel>> {
        const url = this.apiArea.urlForEndpoint('google');
        const response = await axios.post<TokenModel>(url, {idToken}, {withCredentials: true, validateStatus: null});
        return Result(response);
    }

    @OfflineData(AccessTokenSampleData)
    static async refreshToken(): Promise<RawHttpResult<TokenModel>> {
        const url = this.apiArea.urlForEndpoint('refresh');
        const response = await axios.get<TokenModel>(url, {withCredentials: true, validateStatus: null});
        return Result(response);
    }

    @NoOfflineMode()
    static async signout(): Promise<RawHttpResult> {
        const url = this.apiArea.urlForEndpoint("signout");
        const response = await axios.get(url, {withCredentials: true, validateStatus: null});
        return Result(response);
    }
}