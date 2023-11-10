import React, {useEffect, useState} from "react";
import axios, {AxiosError} from "axios";
import {useAuth} from "./useAuth";
import {useLogger} from "../logging";
import {useToken} from "./TokenProvider";
import {SignInResult} from "./AuthProvider";

function SilentRefresh() {
    const {events, actions: {silentRefresh}} = useAuth();
    const {token} = useToken();
    const [refreshing, setRefreshing] = useState(false);
    const logger = useLogger(SilentRefresh);
    useEffect(() => {
        events.addAccessTokenExpiring(onAccessTokenExpiring);
        return () => {
            events.removeAccessTokenExpiring(onAccessTokenExpiring);
        };
    }, []);
    useEffect(() => {
        const interceptorId = axios.interceptors.response.use(
            (response) => {
                return response;
            },
            (error: Error | AxiosError) => {
                if (axios.isAxiosError(error) && error.response?.status === 401) {
                    logger.debug('401 response returned. Attempting auth refresh and reattempt.');
                    return TryRefreshToken()
                        .then((result) => {
                            if (result.succeeded) {
                                const config = {
                                    ...error.config,
                                    headers: {
                                        Authorization: `Bearer ${token}`,
                                    },
                                };
                                return axios
                                    .request(config)
                                    .then((resp) => {
                                        return resp.data;
                                    })
                                    .catch(() => {
                                        return error;
                                    });
                            }
                            return error;
                        })
                        .catch(() => {
                            return error;
                        });
                }
                return error;
            },
        );
        return () => {
            axios.interceptors.response.eject(interceptorId);
        };
    }, []);

    async function onAccessTokenExpiring(): Promise<void> {
        logger.debug("Access token expiring event dispatched. Attempting silent refresh.");
        await TryRefreshToken();
    }

    async function TryRefreshToken(): Promise<SignInResult | { succeeded: false }> {
        if (refreshing) return {succeeded: false};
        setRefreshing(true);
        try {
            return await silentRefresh();
        } finally {
            setRefreshing(false);
        }
    }

    return null;
}

export default React.memo(SilentRefresh);
