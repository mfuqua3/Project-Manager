using System;
using System.Runtime.Serialization;

namespace ProjectManager.Common.Exceptions;

/// <summary>
/// Represents errors that occur when an operation within the ProjectManager encounters forbidden or prohibitted scenario.
/// </summary>
/// <remarks>
/// This exception is interpreted by the exception handling middleware as a Forbidden (403) error.
/// </remarks>
public class ProjectManagerForbiddenAccessException : ProjectManagerException
{
    /// <summary>
    /// Initializes a new instance of the ProjectManagerForbiddenAccessException class with serialized data.
    /// </summary>
    /// <param name="info">The object that holds the serialized object data.</param>
    /// <param name="context">The contextual information about the source or destination.</param>
    protected ProjectManagerForbiddenAccessException(SerializationInfo info, StreamingContext context): base(info, context)
    {
        
    }

    /// <summary>
    /// Initializes a new instance of the ProjectManagerForbiddenAccessException class.
    /// </summary>
    public ProjectManagerForbiddenAccessException()
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the ProjectManagerForbiddenAccessException class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ProjectManagerForbiddenAccessException(string message):base(message)
    {
        
    }
    
    /// <summary>
    /// Initializes a new instance of the ProjectManagerForbiddenAccessException class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
    public ProjectManagerForbiddenAccessException(string message, Exception inner):base(message, inner)
    {
        
    }
}