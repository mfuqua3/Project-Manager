using System;
using System.Runtime.Serialization;

namespace ProjectManager.Common.DependencyInjection;

/// <summary>
/// Represents an exception that is thrown when there is an error in using a module.
/// </summary>
/// <remarks>
/// This exception is typically thrown when there is an issue while using a module, such as passing incorrect parameters, encountering unexpected behavior, or any other usage-related
/// errors.
/// </remarks>
[Serializable]
public class ModuleUsageException : Exception
{
    /// <summary>
    /// Initializes a new instance of the ModuleUsageException class with serialized data.
    /// </summary>
    /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
    protected ModuleUsageException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    /// <summary>
    /// Represents an exception that is thrown when there is an error in using a module.
    /// </summary>
    /// <remarks>
    /// This exception is typically thrown when there is an issue while using a module, such as passing incorrect parameters, encountering unexpected behavior, or any other usage-related
    /// errors.
    /// </remarks>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public ModuleUsageException(string message) : base(message)
    {
    }

    /// <summary>
    /// Represents an exception that is thrown when there is an error in using a module.
    /// </summary>
    /// <remarks>
    /// This exception is typically thrown when there is an issue while using a module, such as passing incorrect parameters, encountering unexpected behavior, or any other usage-related
    /// errors.
    /// </remarks>
    /// <param name="message">A string that describes the error.</param>
    /// <param name="inner">The exception that caused the current exception.</param>
    public ModuleUsageException(string message, Exception inner) : base(message, inner)
    {
        
    }
}