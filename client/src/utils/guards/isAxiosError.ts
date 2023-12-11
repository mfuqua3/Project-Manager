import axios, {AxiosError} from "axios";

export function isAxiosError(payload: any): payload is AxiosError {
    return axios.isAxiosError(payload);
}
