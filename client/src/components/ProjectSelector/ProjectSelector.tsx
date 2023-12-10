import React, {useCallback, useEffect, useState} from "react";
import {Box} from "@mui/material";
import {useSideMenu} from "./SideMenuProvider";

function ProjectSelector() {
    const {isOpen} = useSideMenu();
    const [translate, setTranslate] = useState(isOpen ? '0%' : '100%');

    const calculateTranslation = useCallback(() => {
        setTranslate(isOpen ? '0%' : '100%');
    }, [isOpen]);

    useEffect(() => {
        calculateTranslation();
    }, [calculateTranslation]);

    return (
        <>
            <Box sx={{
                position: "absolute",
                marginTop: "64px",
                right: 0,
                width: ["30%", "300px"], // Minimum width 30%, fixed width 300px
                minWidth: "30%", // minWidth is 30%
                height: "calc(100% - 64px)",
                backgroundColor: (theme) => theme.palette.primary.main,
                zIndex: 1, // Higher z-index to give 'lifted' appearance
                boxShadow: "0px 10px 5px 5px rgba(0,0,0,0.2)", // Added shadow to give 'lifted' appearance
                transition: '0.7s', // Added transition for smooth slide-in/slide-out effect
                transform: `translateX(${translate})`, // Slide the box based on isOpen
            }}/>
        </>
    )
}

export default React.memo(ProjectSelector);