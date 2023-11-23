using System.Runtime.Serialization;

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
}