import React, {useState} from "react";
import {TopNavMenuItem} from "./TopNav.MenuItem";
import {Box} from "@mui/material";
import "./TopNav.css";
import {useProjectList, useSideMenu} from "../ProjectSelector";

function TopNavLogo() {
    const {isOpen, toggleOpen} = useSideMenu();
    const {projects} = useProjectList();
    const activeProject = projects.find(x => x.active);
    const [isDirty, setIsDirty] = useState(false);

    const handleClick = () => {
        setIsDirty(true);
        toggleOpen();
    }
    const getClasses = () => {
        if (!isDirty) return [];
        return isOpen ? ["icon-open"] : ["icon-close"];
    }
    return (
        <>
            <TopNavMenuItem title={activeProject?.name ?? ""} iconPositioning={"end"}
                            icon={<Box component={"img"}
                                       className={getClasses().join(' ')}
                                       sx={{
                                           height: "45px"
                                       }}
                                       src={"/logo.png"} alt={"logo"}/>}
                            onClick={handleClick}/>
        </>
    );
}

export default React.memo(TopNavLogo);