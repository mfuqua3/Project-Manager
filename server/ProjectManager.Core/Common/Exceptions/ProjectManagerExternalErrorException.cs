using System;
using System.Runtime.Serialization;

namespace ProjectManager.Common.Exceptions;

/// <summary>
/// Represents an error when external service or resource used by the application is not available or fails.
/// </summary>
/// <remarks>
/// Exception handling middleware will interpret it as BadGateway error (Status Code: 502).
/// </remarks>
public class ProjectManagerExternalErrorException : ProjectManagerException
{
    /// <summary>
    /// Creates a new instance of ProjectManagerExternalErrorException with serialized data.
    /// </summary>
    /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
    protected ProjectManagerExternalErrorException(SerializationInfo info, StreamingContext context): base(info, context)
    {
        
    }

    /// <summary>
    /// Creates a new instance of ProjectManagerExternalErrorException.
    /// </summary>
    public ProjectManagerExternalErrorException()
    {
    }

    /// <summary>
    /// Creates a new instance of ProjectManagerExternalErrorException with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ProjectManagerExternalErrorException(string message):base(message)
    {
        
    }

    /// <summary>
    /// Creates a new instance of ProjectManagerExternalErrorException with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public ProjectManagerExternalErrorException(string message, Exception inner):base(message, inner)
    {
        
    }
}