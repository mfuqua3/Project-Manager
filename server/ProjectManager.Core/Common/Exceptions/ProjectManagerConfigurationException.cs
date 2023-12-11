using System;
using System.Runtime.Serialization;

namespace ProjectManager.Common.Exceptions;

/// <summary>
/// This exception is thrown primarily when an expected configuration section does not exist or is invalid.
/// </summary>
public class ProjectManagerConfigurationException : ProjectManagerException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectManagerConfigurationException"/> class with serialized data.
    /// Used in serialization.
    /// </summary>
    /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
    protected ProjectManagerConfigurationException(SerializationInfo info, StreamingContext context): base(info, context)
    {
        
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectManagerConfigurationException"/> class.
    /// </summary>
    public ProjectManagerConfigurationException()
    {
        
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectManagerConfigurationException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ProjectManagerConfigurationException(string message):base(message)
    {
        
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectManagerConfigurationException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
    public ProjectManagerConfigurationException(string message, Exception inner):base(message, inner)
    {
        
    }
}