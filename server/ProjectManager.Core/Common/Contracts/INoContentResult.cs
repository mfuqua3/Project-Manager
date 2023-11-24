namespace ProjectManager.Common.Contracts;

public interface IResult
{
    bool IsSuccess { get; init; }
    public string Message { get; init; }
}

public static class Result<T> where T : IResult, new()
{
    public static T Success() => new T { IsSuccess = true };
    public static T Failed(string message) => new T { IsSuccess = false, Message = message };
}