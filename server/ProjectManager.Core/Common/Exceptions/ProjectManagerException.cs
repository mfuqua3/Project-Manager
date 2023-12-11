using System;
using System.Runtime.Serialization;

namespace ProjectManager.Common.Exceptions;

/// <summary>
/// This is the base exception type for all exceptions in the ProjectManager application domain.
/// </summary>
[Serializable]
public class
ProjectManagerException : Exception
{
    /// <summary>
    /// Initializes a new instance of the ProjectManagerException class with serialized data.
    /// </summary>
    /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
    protected ProjectManagerException(SerializationInfo info, StreamingContext context): base(info, context)
    {
        
    }

    /// <summary>
    /// Initializes a new instance of the ProjectManagerException class.
    /// </summary>
    public ProjectManagerException()
    {
        
    }

    /// <summary>
    /// Initializes a new instance of the ProjectManagerException class with a specific error message.
    /// </summary>
    /// <param name="message">A message that describes the error.</param>
    public ProjectManagerException(string message):base(message)
    {
        
    }

    /// <summary>
    /// Initializes a new instance of the ProjectManagerException class with a specific error message and a reference to 
    /// the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">A message that describes the error.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    public ProjectManagerException(string message, Exception inner):base(message, inner)
    {
        
    }
}