import {useSnackbar} from "../snackbar";
import {isAxiosError, isProblemDetails} from "../guards";
import {ProblemDetails} from "../../domain/models";
import {ErrorTypes, useLogger} from "../logging";
import {RawHttpResult} from "../../api/RawHttpResult";
import {isRawHttpResult} from "../../api/IsRawHttpResult";

export interface ProvidedErrorMessagingMethods {
    invoke: <T>(p: Promise<T | RawHttpResult<T>>, success?: string) => Promise<T>;
}

export const useApi = (): ProvidedErrorMessagingMethods => {
    const showMessage = useSnackbar();
    const logger = useLogger(useApi);
    const invoke = async <T>(promise: Promise<T | RawHttpResult<T>>, success?: string) => {
        let result: RawHttpResult<T> | Awaited<T>;
        try {
            result = await promise;
        } catch (error) {
            let message: string;
            if (isAxiosError(error)) {
                if (error.response?.data && isProblemDetails(error.response.data)) {
                    message = error.response.data.title;
                    logger.error(error.response.data as ProblemDetails, 'The API Server has reported an error');
                } else {
                    message = error.message;
                    logger.error(error, 'An error occurred while attempting to communicate with the API.')
                }
            } else {
                message = "An unknown error has occurred.";
                logger.error(error as ErrorTypes, message);
            }
            showMessage({
                position: "BottomCenter",
                type: "Error",
                message: message,
            });
            throw error;
        }
        if(isRawHttpResult(result) && !result.isSuccessStatusCode){
            logger.error(result.data as ProblemDetails);
            showMessage({
                position: "BottomCenter",
                type: "Error",
                message: result.data.detail ?? result.data.title,
            });
            throw result.data;
        }
        if (success !== null && success !== undefined) {
            showMessage({
                position: "BottomCenter",
                type: "Success",
                message: `${success}`,
            });
        }
        return isRawHttpResult(result) ? result.data : result;
    };

    return {invoke};
};
