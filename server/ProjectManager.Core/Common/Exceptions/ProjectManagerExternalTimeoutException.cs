using System.Runtime.Serialization;

namespace ProjectManager.Common.Exceptions;

public class ProjectManagerExternalTimeoutException : ProjectManagerException
{
    protected ProjectManagerExternalTimeoutException(SerializationInfo info, StreamingContext context): base(info, context)
    {
        
    }

    public ProjectManagerExternalTimeoutException()
    {
        
    }
    public ProjectManagerExternalTimeoutException(string message):base(message)
    {
        
    }
    public ProjectManagerExternalTimeoutException(string message, Exception inner):base(message, inner)
    {
        
    }
}