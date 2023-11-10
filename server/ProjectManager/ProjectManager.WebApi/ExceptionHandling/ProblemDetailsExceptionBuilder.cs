using System.Net;

namespace ProjectManager.WebApi.ExceptionHandling;

public class ProblemDetailsExceptionBuilder : IProblemDetailsExceptionBuilder, IProblemDetailsExceptionMapper
{
    private readonly Dictionary<Type, ExceptionMapping> _exceptionMappings = new();

    public IProblemDetailsExceptionBuilder Map<T>(HttpStatusCode code)
        where T : Exception
        => Map<T>((int)code, code.ToString());

    public IProblemDetailsExceptionBuilder Map<T>(int statusCode, string statusType)
        where T : Exception
    {
        _exceptionMappings[typeof(T)] = new ExceptionMapping { StatusCode = statusCode, StatusDetail = statusType };
        return this;
    }

    public IProblemDetailsExceptionMapper Build() => this;

    public bool TryGetMapping(Type exceptionType, out ExceptionMapping mapping)
    {
        mapping = null;
        var currentExceptionType = exceptionType;
        while (currentExceptionType != null)
        {
            if (_exceptionMappings.TryGetValue(currentExceptionType, out mapping))
            {
                return true;
            }

            currentExceptionType = currentExceptionType.BaseType;
        }

        return false;
    }
}