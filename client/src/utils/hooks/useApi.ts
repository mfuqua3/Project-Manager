import {useSnackbar} from "../snackbar";
import {isAxiosError} from "../guards/isAxiosError";
import {isProblemDetails} from "../guards";
import {ProblemDetails} from "../../domain/models";
import {useLogger} from "../logging";

export interface ProvidedErrorMessagingMethods {
    invoke: <T>(p: Promise<T>, success?: string) => Promise<T>;
}

export const useApi = (): ProvidedErrorMessagingMethods => {
    const showMessage = useSnackbar();
    const logger = useLogger(useApi);
    const invoke = <T>(promise: Promise<T>, success?: string) => {
        return promise
            .then((result: T) => {
                if (success !== null && success !== undefined) {
                    showMessage({
                        position: "BottomCenter",
                        type: "Success",
                        message: `${success}`,
                    });
                }
                return result;
            })
            .catch((error) => {
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
                    logger.error(error, message);
                }
                showMessage({
                    position: "BottomCenter",
                    type: "Error",
                    message: message,
                });
                throw error;
            });
    };

    return {invoke};
};
