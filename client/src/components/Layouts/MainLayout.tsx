import React from "react";
import {Stack, Typography} from "@mui/material";
import MenuProvider from "../../utils/menu/MenuProvider";
import TopNavMd from "../TopNav/TopNav.Md";
import ScrollWrapper from "../UtilityWrappers/ScrollWrapper";
import {Outlet} from "react-router-dom";
import ProjectSelector from "../ProjectSelector/ProjectSelector";
import {SideMenuProvider} from "../ProjectSelector";
import {isOfflineMode} from "../../utils/helpers";

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
                {isOfflineMode &&
                    <Typography variant="subtitle1" style={{ position: "sticky", bottom: "0", color: "Information", textAlign: "center" }}>
                        Offline Mode is Enabled
                    </Typography>
                }
            </Stack>
        </SideMenuProvider>
    );
}

export default React.memo(MainLayout);