import React, {createContext, useCallback, useContext, useEffect, useState} from "react";
import {Project} from "./Project";
import {useLogger} from "../../utils/logging";
import {useApi} from "../../utils/hooks";
import {ProjectsApi} from "../../api/ProjectsApi";
import {useAuth} from "../../utils/auth";

export interface ProjectListContextState {
    projects: Project[];

    selectProject(index: number): Promise<void>
}

export const ProjectListContext = createContext<ProjectListContextState | null>(null)

// Create Provider
function ProjectListProvider({children}: { children: React.ReactNode }) {
    const apiInvoker = useApi();
    const {isAuthenticated} = useAuth();
    const [projectList, setProjectList] = useState<Project[]>([]);
    const logger = useLogger(ProjectListProvider);

    // Replace static project list with API call in future
    useEffect(() => {
        if(!isAuthenticated){
            setProjectList([]);
            return;
        }
        apiInvoker.invoke(ProjectsApi.getProjectList({page: 1, pageSize: 100}))
            .then(resp=>{
                    logger.debug('Project List Provider mounted and initialized with {projectCount} projects.',
                        {projectCount: resp.itemCount.toString()});
                    setProjectList(resp.items.map((proj)=>({
                        name: proj.name,
                        active: false
                    })));
            })

        // fetch project list from API
        // and update projectList state
    }, [isAuthenticated]);

    async function selectProject(index: number): Promise<void> {
        //API stuff here later
        handleProjectSelection(index);
    }

    const handleProjectSelection = useCallback((index: number) => {
        setProjectList(projectList.map((project, projectIndex) => {
            const isActive = projectIndex === index;
            if(isActive){
                logger.debug('Setting {projectName} as {projectActiveValue}',
                    {
                        projectName: project.name,
                        projectActiveValue: "active"
                    });
            }
            return {
                ...project,
                active: isActive,
            }
        }));
    }, [projectList]);

    return <ProjectListContext.Provider value={{
        projects: projectList,
        selectProject
    }}>{children}</ProjectListContext.Provider>
}

export function useProjectList(): ProjectListContextState {
    const context = useContext(ProjectListContext);

    if (context === null) {
        throw new Error('useProjectList must be used within a ProjectListProvider');
    }

    return context;
}

export default React.memo(ProjectListProvider);