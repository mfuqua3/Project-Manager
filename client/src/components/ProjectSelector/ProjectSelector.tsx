import React, {useEffect, useState} from "react";
import {Box, Button, Divider} from "@mui/material";
import {useSideMenu} from "./SideMenuProvider";
import {ProjectList} from "./ProjectList";

function ProjectSelector() {
    const {isOpen} = useSideMenu();
    const [menuPosition, setMenuPosition] = useState(isOpen ? '0%' : '100%');

    const adjustMenuPosition = () => {
        setMenuPosition(isOpen ? '0%' : '100%');
    };

    useEffect(() => {
        adjustMenuPosition();
    }, [isOpen]);

    return (
        <>
            <Box sx={{
                position: "fixed",
                marginTop: "64px",
                right: 0,
                width: "250px",
                height: "calc(100% - 64px)",
                backgroundColor: (theme) => theme.palette.secondary.main,
                zIndex: 1,
                boxShadow: "0px 10px 5px 5px rgba(0,0,0,0.2)",
                transition: '0.7s',
                transform: `translateX(${menuPosition})`,
            }}>
                <Box sx={{textAlign: 'center', p: 1}}>
                    <Button variant={"contained"} color={"primary"}>
                        Launch New Project
                    </Button>
                </Box>
                <Divider/>
                <ProjectList isVisible={isOpen} />
            </Box>
        </>
    );
}

export default React.memo(ProjectSelector);