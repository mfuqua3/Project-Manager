import React, {useEffect} from "react";
import AppRoutes from "./App.Routes";
import {GoogleOAuthProvider} from "@react-oauth/google";
import TokenProvider from "./utils/auth/TokenProvider";
import AuthProvider from "./utils/auth/AuthProvider";
import DrawerRoot from "./utils/drawer/DrawerRoot";
import SnackbarRoot from "./utils/snackbar/SnackbarRoot";
import ModalRoot from "./utils/modal/ModalRoot";
import ProjectListProvider from "./components/ProjectSelector/ProjectListProvider";
import {useLogger} from "./utils/logging";

const offlineMode = process.env["OFFLINE_MODE"] === "on";

function App() {
    const logger = useLogger(App);
    useEffect(() => {
        logger.debug("Offline mode is {setting}",
            {
                setting: process.env["REACT_APP_IS_OFFLINE_ENABLED"] ?? "undefined"
            });
    }, []);
    const clientId = process.env["REACT_APP_GOOGLE_CLIENT_ID"] ?? "";
    return (
        <GoogleOAuthProvider clientId={clientId}>
            <TokenProvider expiryThreshold={5 * 60 * 1000}>
                <AuthProvider>
                    <>
                        <ProjectListProvider>
                            <AppRoutes/>
                        </ProjectListProvider>
                        <DrawerRoot/>
                        <SnackbarRoot/>
                        <ModalRoot/>
                    </>
                </AuthProvider>
            </TokenProvider>
        </GoogleOAuthProvider>
    );
}

export default App;
