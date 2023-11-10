using System.Runtime.Serialization;

namespace ProjectManager.Core.Utility.Exceptions;

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