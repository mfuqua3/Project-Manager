using AutoMapper;
using ProjectManager.Data.Entities;
using ProjectManager.Features.Projects.Domain.Commands;
using ProjectManager.Features.Projects.Domain.Common;
using ProjectManager.Features.Projects.Domain.Results;

namespace ProjectManager.Features.Projects.Mapping;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<CreateProjectCommand, Project>();
        CreateMap<Project, ProjectListItemDto>();
        CreateMap<Project, CreateProjectResult>();
        CreateMap<Project, GetProjectResult>();
    }
}