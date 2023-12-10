import React from "react";
import {Stack} from "@mui/material";
import MenuProvider from "../../utils/menu/MenuProvider";
import TopNavMd from "../TopNav/TopNav.Md";
import ScrollWrapper from "../UtilityWrappers/ScrollWrapper";
import {Outlet} from "react-router-dom";
import ProjectSelector from "../ProjectSelector/ProjectSelector";
import {SideMenuProvider} from "../ProjectSelector";

function MainLayout() {
    return (
        <SideMenuProvider>
            <Stack height={"100vh"} position={"relative"}>
                <MenuProvider>
                    <TopNavMd/>
                </MenuProvider>
                <ProjectSelector/>
                <ScrollWrapper>
                    <Outlet/>
                </ScrollWrapper>
            </Stack>
        </SideMenuProvider>
    );
}

export default React.memo(MainLayout);