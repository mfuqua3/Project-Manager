using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ProjectManager.Common.Exceptions;

public class ProjectManagerBadRequestException : ProjectManagerException
{
    protected ProjectManagerBadRequestException(SerializationInfo info, StreamingContext context): base(info, context)
    {
        
    }

    public ProjectManagerBadRequestException()
    {
        
    }
    public ProjectManagerBadRequestException(string message):base(message)
    {
        
    }
    public ProjectManagerBadRequestException(string message, Exception inner):base(message, inner)
    {
        
    }

    public static void ThrowIfInvalid(ModelStateDictionary modelState)
    {
        if (modelState.IsValid)
        {
            return;
        }
        var exceptionMessageSb = new StringBuilder("Invalid Request. ");
        exceptionMessageSb.Append(string.Join(". ",
            modelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage)));
        var exceptions = modelState.Values
            .SelectMany(x => x.Errors)
            .Select(x => x.Exception)
            .Where(x => x != null)
            .ToList();
        throw exceptions.Count switch
        {
            0 => new ProjectManagerBadRequestException(exceptionMessageSb.ToString()),
            1 => new ProjectManagerBadRequestException(exceptionMessageSb.ToString(), exceptions[0]),
            _ => new ProjectManagerBadRequestException(exceptionMessageSb.ToString(),
                new AggregateException(exceptions))
        };
    }
}