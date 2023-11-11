import React from "react";
import {AppBar, Grid, Stack, Toolbar} from "@mui/material";
import "./TopNav.css";
import {TopNavMenuItem} from "./TopNav.MenuItem";
import MenuIcon from "@mui/icons-material/Menu";
import {useDrawer} from "../../utils/drawer";
import NavigationDrawer from "../NavigationDrawer/NavigationDrawer";
import TopNavLogo from "./TopNav.Logo";

function TopNavXS() {
    const navigationDrawer = useDrawer({content: <NavigationDrawer/>, anchor: "left"});
    return (
        <AppBar position="relative">
            <Toolbar disableGutters>
                <Grid container justifyContent={"space-between"} alignContent={"stretch"} alignItems={"center"}>
                    <Grid item>
                        <Stack direction={"row"}>
                            <TopNavMenuItem title={""}
                                            icon={<MenuIcon
                                                sx={(theme) => ({
                                                    height: "25px",
                                                    width: "25px",
                                                    border: `1px solid ${theme.palette.primary.contrastText}`,
                                                    borderRadius: "5px"
                                                })}/>}
                                            onClick={() => navigationDrawer.open()}/>
                        </Stack>
                    </Grid>
                    <Grid item>
                        <Stack direction={"row"} spacing={1}>
                            <TopNavLogo />
                        </Stack>
                    </Grid>
                </Grid>
            </Toolbar>
        </AppBar>
    );
}

export default React.memo(TopNavXS);
