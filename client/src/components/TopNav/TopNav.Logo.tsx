import React from "react";
import {TopNavMenuItem} from "./TopNav.MenuItem";
import {Box} from "@mui/material";
import {useNavigate} from "react-router-dom";

function TopNavLogo() {
    const navigate = useNavigate();
    return (
        <>
            <TopNavMenuItem title={""}
                            icon={<Box component={"img"}
                                       sx={{
                                           height: "45px"
                                       }}
                                       src={"/logo.png"} alt={"logo"}/>}
                            onClick={() => navigate("")}/>
        </>
    );
}

export default React.memo(TopNavLogo);