﻿using System.Runtime.Serialization;

namespace ProjectManager.Core.Utility.Exceptions;

public class ProjectManagerExternalErrorException : ProjectManagerException
{
    protected ProjectManagerExternalErrorException(SerializationInfo info, StreamingContext context): base(info, context)
    {
        
    }

    public ProjectManagerExternalErrorException()
    {
        
    }
    public ProjectManagerExternalErrorException(string message):base(message)
    {
        
    }
    public ProjectManagerExternalErrorException(string message, Exception inner):base(message, inner)
    {
        
    }
}