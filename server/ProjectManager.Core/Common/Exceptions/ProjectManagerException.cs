using System.Runtime.Serialization;

namespace ProjectManager.Common.Exceptions;

[Serializable]
public class ProjectManagerException : Exception
{
    protected ProjectManagerException(SerializationInfo info, StreamingContext context): base(info, context)
    {
        
    }

    public ProjectManagerException()
    {
        
    }
    public ProjectManagerException(string message):base(message)
    {
        
    }
    public ProjectManagerException(string message, Exception inner):base(message, inner)
    {
        
    }
}