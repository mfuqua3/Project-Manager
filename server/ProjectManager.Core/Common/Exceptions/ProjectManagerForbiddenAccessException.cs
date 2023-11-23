using System.Runtime.Serialization;

namespace ProjectManager.Common.Exceptions;

public class ProjectManagerForbiddenAccessException : ProjectManagerException
{
    protected ProjectManagerForbiddenAccessException(SerializationInfo info, StreamingContext context): base(info, context)
    {
        
    }

    public ProjectManagerForbiddenAccessException()
    {
    }
    public ProjectManagerForbiddenAccessException(string message):base(message)
    {
        
    }
    public ProjectManagerForbiddenAccessException(string message, Exception inner):base(message, inner)
    {
        
    }
}