import React, {useCallback, useContext, useLayoutEffect, useRef, useState} from "react";
import {Fade, List, ListItemButton, Typography} from "@mui/material";
import {ProjectListContext, useProjectList} from "./ProjectListProvider";

// Create the Context

interface ProjectListProps {
    isVisible: boolean;
}
export function ProjectList({isVisible}: ProjectListProps) {

    const {projects, selectProject} = useProjectList();

    const [firstOpenComplete, setFirstOpenComplete] = useState<boolean>(false);  // New state variable
    const [loadedProjects, setLoadedProjects] = useState<number[]>([]);
    const timers = useRef<NodeJS.Timeout[]>([]);

    useLayoutEffect(() => {
        if (isVisible && !firstOpenComplete) {
            projects.forEach((_, index) => {
                timers.current[index] = setTimeout(() => setLoadedProjects((previousLoadedProjects) => [...previousLoadedProjects, index]), index * 250);
            });
            setFirstOpenComplete(true);  // Set first open complete true
        } else if (isVisible && firstOpenComplete) {
            setLoadedProjects(projects.map((_, index) => index));
        } else {
            setLoadedProjects([]);
            timers.current.forEach((time) => clearTimeout(time));
            timers.current = [];
        }

        return () => {
            timers.current.forEach((time) => clearTimeout(time));
        };
    }, [isVisible]);




    return (
        <List>
            {projects.map((project, index) => (
                <Fade in={loadedProjects.includes(index)} timeout={800} key={project.name}>
                    <ListItemButton onClick={async () => await selectProject(index)} disableRipple>
                        <Typography
                            sx={{
                                fontSize: '1.25rem',
                                color: theme => project.active ? 'yellow' : theme.palette.secondary.contrastText
                            }}
                        >{project.name}</Typography>
                    </ListItemButton>
                </Fade>
            ))}
        </List>
    );
}

export default React.memo(ProjectList);