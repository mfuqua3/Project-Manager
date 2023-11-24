using System;
using System.Runtime.Serialization;

namespace ProjectManager.Common.Exceptions;

public class ProjectManagerDataNotFoundException : ProjectManagerException
{
    protected ProjectManagerDataNotFoundException(SerializationInfo info, StreamingContext context): base(info, context)
    {
        
    }

    public ProjectManagerDataNotFoundException()
    {
        
    }
    public ProjectManagerDataNotFoundException(string message):base(message)
    {
        
    }
    public ProjectManagerDataNotFoundException(string message, Exception inner):base(message, inner)
    {
        
    }
}