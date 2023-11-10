import React from "react";
import {Route, Routes} from "react-router-dom";
import MainLayout from "./components/Layouts/MainLayout";
import NotFound from "./components/Errors/NotFound";
import CardContentLayout from "./components/Layouts/CardContentLayout";
import {Typography} from "@mui/material";
import Login from "./components/Login/Login";

export const ProjectManagerRoutes = {
    home: "/",
    login: "login"
}

function AppRoutes() {
    return (
        <Routes>
            <Route element={<MainLayout/>}>
                <Route path={"/"}>
                    <Route index element={
                        <CardContentLayout>
                            <Typography>
                                Coming Soon
                            </Typography>
                        </CardContentLayout>}/>
                </Route>
                <Route path={ProjectManagerRoutes.login} element={<Login/>}/>
                <Route path={"*"} element={<NotFound/>}/>
            </Route>
        </Routes>
    );
}

export default React.memo(AppRoutes);
