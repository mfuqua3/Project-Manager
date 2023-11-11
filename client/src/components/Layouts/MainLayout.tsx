import React from "react";
import {Stack} from "@mui/material";
import MenuProvider from "../../utils/menu/MenuProvider";
import ResponsiveContainer from "../ResponsiveContainer/ResponsiveContainer";
import TopNavXS from "../TopNav/TopNav.XS";
import TopNavMd from "../TopNav/TopNav.Md";
import ScrollWrapper from "../UtilityWrappers/ScrollWrapper";
import {Outlet} from "react-router-dom";

function MainLayout() {
    return (
        <Stack height={"100vh"}>
            <MenuProvider>
                <ResponsiveContainer lower={<TopNavXS/>} upper={<TopNavMd/>}/>
            </MenuProvider>
            <ScrollWrapper>
                <Outlet/>
            </ScrollWrapper>
        </Stack>
    );
}

export default React.memo(MainLayout);
