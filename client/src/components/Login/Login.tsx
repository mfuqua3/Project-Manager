import React from 'react';
import {Box, Button, Typography} from '@mui/material';
import {CredentialResponse, GoogleLogin} from '@react-oauth/google';
import {useLogger} from "../../utils/logging";
import {useAuth} from "../../utils/auth";
import {isDevelopment} from "../../utils/env";
import LoadingWrapper from "../UtilityWrappers/LoadingWrapper";
import {useSnackbar} from "../../utils/snackbar";
import CardContentLayout from "../Layouts/CardContentLayout";


function Login() {
    const {actions, isReady, ...authState} = useAuth();
    const showMessage = useSnackbar();
    const logger = useLogger(Login);
    const devMode = isDevelopment();

    async function handleSuccess(credentialResponse: CredentialResponse) {
        if (credentialResponse.credential) {
            const result = await actions.handleGoogleSso(credentialResponse.credential);
            if (result.succeeded) {
                logger.debug('User Login Successful!', {});
            } else {
                if (result.details.status === 403) {
                    showMessage({
                        message: "Access Denied",
                        type: "Error"
                    });
                } else {
                    showMessage({
                        message: 'An unknown error has occurred',
                        type: "Error"
                    });
                }
            }
        }
    }

    async function attemptRefresh() {
        const result = await actions.silentRefresh();
        if (result.succeeded) {
            logger.debug("Refresh Success!")
        } else {
            logger.debug("Refresh failure!")
        }
    }

    return (
        !isReady ? <LoadingWrapper loading={true}><Box/></LoadingWrapper> :
            <>
                <CardContentLayout>
                    <Typography variant="h4" gutterBottom>
                        Sign in to Manage Content
                    </Typography>
                    {authState.isAuthenticated ?
                        <>
                            <Typography variant="body1" gutterBottom>
                                {`It looks like you're already signed in as ${authState.user.profile.name}. Sign out?`}
                            </Typography>
                            <Box sx={{marginTop: 2, justifyContent: 'center', display: 'flex'}}>
                                <Button variant={"contained"} onClick={async () => await actions.signout()}
                                        sx={{margin: '5px'}}>
                                    Yea, sign me out
                                </Button>
                                {devMode &&
                                    <Button variant={"contained"} onClick={attemptRefresh} sx={{margin: '5px'}}>
                                        Refresh Access (temporary)
                                    </Button>
                                }
                            </Box>
                        </> :
                        <>
                            <Typography variant="body1" gutterBottom>
                                {"If you are an authorized administrator of this site, log in with your Google account to access " +
                                    "management features."}
                            </Typography>
                            <Box sx={{marginTop: 2, justifyContent: 'center', display: 'flex'}}>
                                <GoogleLogin theme={"outline"} type={"standard"} auto_select={false}
                                             onSuccess={handleSuccess}
                                />
                            </Box>
                        </>}
                    {
                        !authState.isAuthenticated &&
                        <Typography variant="body2" sx={{marginTop: 3}}>
                            Only administrators will be able to log in. The app does not support basic users at this
                            time.
                        </Typography>
                    }
                </CardContentLayout>
            </>
    );
}

export default React.memo(Login);
