import * as React from "react";
import AppBar from "@mui/material/AppBar";
import Box from "@mui/material/Box";
import Toolbar from "@mui/material/Toolbar";
import {useNavigate} from "react-router-dom";
import LockPersonIcon from '@mui/icons-material/LockPerson';
import LockOpenIcon from '@mui/icons-material/LockOpen';
import TopNavLogo from "./TopNav.Logo";
import "./TopNav.css";
import {TopNavMenuItem} from "./TopNav.MenuItem";
import {ProjectManagerRoutes} from "../../App.Routes";
import {useAuth} from "../../utils/auth";
import {isOfflineMode} from "../../utils/helpers";

function TopNavMd() {
    const navigate = useNavigate();
    const {isAuthenticated} = useAuth();
    return (
        <Box sx={{flexGrow: 1}}>
            <AppBar position={"static"}>
                <Toolbar>
                    <Box display={"flex"} flexDirection={"row"} alignItems={"center"} justifyContent={"space-between"}
                         width={"100%"}>
                        <div />
                        <Box component={"img"} className={"nav-image"} sx={(theme) => ({
                            boxShadow: `0 0 5px ${theme.palette.primary.dim}, 0 0 10px ${theme.palette.primary.dim}, 0 0 15px ${theme.palette.primary.dim}`,
                            backgroundColor: `${theme.palette.primary.dim}`
                        })} height={"28px"}/>
                        {
                            (isAuthenticated || isOfflineMode) ?
                                <TopNavLogo /> :
                                <TopNavMenuItem title={""} tooltip={isAuthenticated ? "Sign Out" : "Sign In"}
                                                                               icon={isAuthenticated ? <LockOpenIcon/> : <LockPersonIcon/>}
                                                                               onClick={() => navigate(ProjectManagerRoutes.login)}/>
                        }
                    </Box>
                </Toolbar>
            </AppBar>
        </Box>
    );
}

export default React.memo(TopNavMd);
