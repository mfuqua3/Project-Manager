import React, {useEffect, useLayoutEffect, useRef, useState} from "react";
import {Box, Button, Divider, Fade, List, ListItemButton, Typography} from "@mui/material";
import {useSideMenu} from "./SideMenuProvider";

interface Project {
    name: string;
    active: boolean;
}

function ProjectSelector() {
    const initialProjects = ["MIRT", "BrokerVault", "Vanderbilt IAT", "Congenius", "Delta Dental", "MedeAnalytics", "Tivity RCM"];
    const projects = initialProjects.map(name => ({name, active: false}));
    const {isOpen} = useSideMenu();
    const [translate, setTranslate] = useState(isOpen ? '0%' : '100%');
    const [loaded, setLoaded] = useState<number[]>([]);
    const [projectList, setProjectList] = useState<Project[]>(projects);

    const calculateTranslation = () => {
        setTranslate(isOpen ? '0%' : '100%');
    };

    useEffect(() => {
        calculateTranslation();
    }, [isOpen]);

    const timer = useRef<NodeJS.Timeout[]>([]);
    useLayoutEffect(() => {
        if (isOpen) {
            projectList.forEach((_, index) => {
                timer.current[index] = setTimeout(() => setLoaded(oldLoaded => [...oldLoaded, index]), index * 250);
            });
        } else {
            setLoaded([]);
            timer.current.forEach((time) => clearTimeout(time));
            timer.current = [];
        }
        return () => {
            timer.current.forEach((time) => clearTimeout(time));
        };
    }, [isOpen]);

    const handleProjectClick = (index: number) => {
        setProjectList(projectList.map((project, i) => ({
            ...project,
            active: i === index,
        })));
    };

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
                transform: `translateX(${translate})`,
            }}>
                <Box sx={{textAlign: 'center', p: 1}}>
                    <Button variant={"contained"} color={"primary"}>
                        Launch New Project
                    </Button>
                </Box>
                <Divider/>
                <List>
                    {projectList.map((project, index) => (
                        <Fade in={loaded.includes(index)} timeout={800} key={project.name}>
                            <ListItemButton onClick={() => handleProjectClick(index)} disableRipple>
                                <Typography
                                    sx={{
                                        fontSize: '1.25rem',
                                        color: theme => project.active ? '#000000' : theme.palette.secondary.contrastText
                                    }}
                                >{project.name}</Typography>
                            </ListItemButton>
                        </Fade>
                    ))}
                </List>
            </Box>
        </>
    )
}

export default React.memo(ProjectSelector);