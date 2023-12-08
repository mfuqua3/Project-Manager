using IResult = ProjectManager.Common.Contracts.IResult;

namespace ProjectManager.Features.Users.Domain.Results;

public class SignOutUserResult : IResult
{
    public bool IsSuccess { get; init; }
    public string Message { get; init; }
}