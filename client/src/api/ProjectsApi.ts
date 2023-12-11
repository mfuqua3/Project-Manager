import {useApiArea} from "../utils/env";
import axios from "axios";
import {RawHttpResult} from "./AuthorizationsApi";
import {CreateProjectRequest, DeleteProjectRequest, GetProjectRequest, GetProjectListRequest} from "../domain/requests";
import {CreateProjectResult, GetProjectResult, PagedList, ProjectListItemDto} from "../domain/models";
import {Result} from "./Result";

export class ProjectsApi {
    static apiArea = useApiArea("projects");

    static async getProjectList(command: GetProjectListRequest): Promise<RawHttpResult<PagedList<ProjectListItemDto>>> {
        const url = this.apiArea.urlForEndpoint("");
        const response = await axios.get(url, {params: command, validateStatus: null});
        return Result(response);
    }

    static async createProject(command: CreateProjectRequest): Promise<RawHttpResult<CreateProjectResult>> {
        const url = this.apiArea.urlForEndpoint("");
        const response = await axios.post(url, command, {validateStatus: null});
        return Result(response);
    }

    static async getProject(command: GetProjectRequest): Promise<RawHttpResult<GetProjectResult>> {
        const url = this.apiArea.urlForEndpoint("", {id: command.id})
        const response = await axios.get(url, {validateStatus: null});
        return Result(response);
    }

    static async deleteProject(command: DeleteProjectRequest): Promise<RawHttpResult> {
        const url = this.apiArea.urlForEndpoint("", {id: command.id})
        const response = await axios.delete(url, {validateStatus: null});
        return Result(response);
    }
}