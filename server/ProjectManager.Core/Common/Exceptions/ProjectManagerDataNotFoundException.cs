using System;
using System.Runtime.Serialization;

namespace ProjectManager.Common.Exceptions;

/// <summary>
/// Represents errors that occur when the system fails to find the expected data.
/// </summary>
/// <remarks>
/// The exception handling middleware interprets this exception as a 404 error.
/// </remarks>
public class ProjectManagerDataNotFoundException : ProjectManagerException
{
    /// <summary>
    /// Initializes a new instance of the ProjectManagerDataNotFoundException with serialized data.
    /// </summary>
    /// <param name="info">The object that holds the serialized object data.</param>
    /// <param name="context">An object that describes the source or destination of the serialized data.</param>
    protected ProjectManagerDataNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        
    }

    /// <summary>
    /// Initializes a new instance of the ProjectManagerDataNotFoundException class.
    /// </summary>
    public ProjectManagerDataNotFoundException()
    {
        
    }

    /// <summary>
    /// Initializes a new instance of the ProjectManagerDataNotFoundException class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ProjectManagerDataNotFoundException(string message) : base(message)
    {
        
    }

    /// <summary>
    /// Initializes a new instance of the ProjectManagerDataNotFoundException class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    public ProjectManagerDataNotFoundException(string message, Exception inner) : base(message, inner)
    {
        
    }
}