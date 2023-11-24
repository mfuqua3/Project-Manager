using IResult = ProjectManager.Common.Contracts.IResult;

namespace ProjectManager.Features.Projects.Domain.Results;

public class DeleteProjectResult : IResult
{
    public bool IsSuccess { get; init; }
    public string Message { get; init; }
}