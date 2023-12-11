using System;
using System.Runtime.Serialization;

namespace ProjectManager.Common.Exceptions;

/// <summary>
/// Represents timeout errors that occur during communication with external services in the project management.
/// </summary>
/// <remarks>
/// This type of exception is interpreted by the middleware as a GatewayTimeout (Status Code: 504).
/// </remarks>
public class ProjectManagerExternalTimeoutException : ProjectManagerException
{
    /// <summary>
    /// Initializes a new instance of the ProjectManagerExternalTimeoutException with serialized data.
    /// </summary>
    /// <param name="info">The object that holds the serialized object data</param>
    /// <param name="context">The contextual information about the source or destination</param>
    protected ProjectManagerExternalTimeoutException(SerializationInfo info, StreamingContext context): base(info, context)
    {
            
    }

    /// <summary>
    /// Initializes a new instance of the ProjectManagerExternalTimeoutException class.
    /// </summary>
    public ProjectManagerExternalTimeoutException()
    {
            
    }

    /// <summary>
    /// Initializes a new instance of the ProjectManagerExternalTimeoutException class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error</param>
    public ProjectManagerExternalTimeoutException(string message):base(message)
    {
            
    }

    /// <summary>
    /// Initializes a new instance of the ProjectManagerExternalTimeoutException class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error</param>
    /// <param name="inner">The exception that is the cause of the current exception</param>
    public ProjectManagerExternalTimeoutException(string message, Exception inner):base(message, inner)
    {
            
    }
}