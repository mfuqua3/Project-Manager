using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Common;
using ProjectManager.Common.Contracts;
using ProjectManager.Common.Extensions;
using ProjectManager.Data;
using ProjectManager.Data.Entities;
using ProjectManager.Features.Projects.Abstractions;
using ProjectManager.Features.Projects.Domain.Commands;
using ProjectManager.Features.Projects.Domain.Common;
using ProjectManager.Features.Projects.Domain.Results;

namespace ProjectManager.Features.Projects.Managers;

public class ProjectsManager : IProjectsManager
{
    private readonly ProjectManagerDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfigurationProvider _configurationProvider;

    public ProjectsManager(ProjectManagerDbContext dbContext,
        IMapper mapper,
        IConfigurationProvider configurationProvider)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configurationProvider = configurationProvider;
    }

    async Task<PagedList<ProjectListItemDto>> IProjectsManager.GetProjectListAsync(GetProjectListCommand command)
        => await _dbContext.Projects
            .Page(command, out var count)
            .ProjectTo<ProjectListItemDto>(_configurationProvider)
            .ToPagedListAsync(command, count);


    async Task<CreateProjectResult> IProjectsManager.CreateProjectAsync(CreateProjectCommand command)
    {
        await _dbContext.Projects.AssertNameIsAvailableAsync(command.Name);
        var created = _mapper.Map<CreateProjectCommand, Project>(command);
        _dbContext.Projects.Add(created);
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<Project, CreateProjectResult>(created);
    }

    async Task<DeleteProjectResult> IProjectsManager.DeleteProjectAsync(DeleteProjectCommand command)
    {
        await _dbContext.Projects.Persist(_mapper).RemoveAsync(command);
        return Result<DeleteProjectResult>.Success();
    }

    async Task<GetProjectResult> IProjectsManager.GetProjectAsync(GetProjectCommand command) =>
        await _dbContext.Projects
            .ProjectTo<GetProjectResult>(_configurationProvider)
            .Where(x => x.Id == command.Id)
            .SingleOrNotFoundAsync();
}