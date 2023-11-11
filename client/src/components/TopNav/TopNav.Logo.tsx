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
                                           height: "28px",
                                           border: "1px solid rgba(255,255,255,0.6)",
                                           borderRadius: "14px"
                                       }}
                                       src={"/logo192.png"}/>}
                            onClick={() => navigate("")}/>
        </>
    );
}

export default React.memo(TopNavLogo);