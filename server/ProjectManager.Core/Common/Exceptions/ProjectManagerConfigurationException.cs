using System;
using System.Runtime.Serialization;

namespace ProjectManager.Common.Exceptions;

public class ProjectManagerConfigurationException : ProjectManagerException
{
    protected ProjectManagerConfigurationException(SerializationInfo info, StreamingContext context): base(info, context)
    {
        
    }

    public ProjectManagerConfigurationException()
    {
        
    }
    public ProjectManagerConfigurationException(string message):base(message)
    {
        
    }
    public ProjectManagerConfigurationException(string message, Exception inner):base(message, inner)
    {
        
    }
}