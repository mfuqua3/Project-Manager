import React, {useCallback, useLayoutEffect, useRef, useState} from "react";
import {Fade, List, ListItemButton, Typography} from "@mui/material";

interface Project {
    name: string;
    active: boolean;
}

interface ProjectListProps {
    isVisible: boolean;
}
export function ProjectList({isVisible}: ProjectListProps) {
    const initialProjectNames = ["MIRT", "BrokerVault", "Vanderbilt IAT", "Congenius", "Delta Dental", "MedeAnalytics", "Tivity RCM"];
    const initialProjects = initialProjectNames.map(name => ({name, active: false}));
    const [projectList, setProjectList] = useState<Project[]>(initialProjects);
    const [firstOpenComplete, setFirstOpenComplete] = useState<boolean>(false);  // New state variable
    const [loadedProjects, setLoadedProjects] = useState<number[]>([]);
    const timers = useRef<NodeJS.Timeout[]>([]);

    useLayoutEffect(() => {
        if (isVisible && !firstOpenComplete) {
            projectList.forEach((_, index) => {
                timers.current[index] = setTimeout(() => setLoadedProjects((previousLoadedProjects) => [...previousLoadedProjects, index]), index * 250);
            });
            setFirstOpenComplete(true);  // Set first open complete true
        } else if (isVisible && firstOpenComplete) {
            setLoadedProjects(projectList.map((_, index) => index));
        } else {
            setLoadedProjects([]);
            timers.current.forEach((time) => clearTimeout(time));
            timers.current = [];
        }

        return () => {
            timers.current.forEach((time) => clearTimeout(time));
        };
    }, [isVisible]);


    const handleProjectSelection = useCallback((index: number) => {
        setProjectList(projectList.map((project, projectIndex) => ({
            ...project,
            active: projectIndex === index,
        })));
    }, [projectList]);
    return (
        <List>
            {projectList.map((project, index) => (
                <Fade in={loadedProjects.includes(index)} timeout={800} key={project.name}>
                    <ListItemButton onClick={() => handleProjectSelection(index)} disableRipple>
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
    );
}

export default React.memo(ProjectList);