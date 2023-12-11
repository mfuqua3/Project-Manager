using System;
using System.Runtime.Serialization;

namespace ProjectManager.Common.DependencyInjection;

/// <summary>
/// Represents an exception that is thrown when a module fails to initialize.
/// </summary>
[Serializable]
public class ModuleInitializationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ModuleInitializationException"/> class with serialized data.
    /// </summary>
    /// <param name="info">The <see cref="SerializationInfo"/> object that holds the serialized object data.</param>
    /// <param name="context">The <see cref="StreamingContext"/> object that specifies the source and destination of the serialized stream.</param>
    protected ModuleInitializationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    /// <summary>
    /// Represents an exception that is thrown when module initialization fails.
    /// </summary>
    /// <remarks>
    /// This exception is thrown when there are errors during the initialization process
    /// of a module.
    /// </remarks>
    public ModuleInitializationException(string message) : base(message)
    {
    }

    /// <summary>
    /// Represents an exception that is thrown when an error occurs during module initialization.
    /// </summary>
    /// <remarks>
    /// This exception is typically thrown when an exception has been caught during the initialization
    /// process of a module and needs to be propagated to the caller.
    /// </remarks>
    /// <seealso cref="Exception"/>
    public ModuleInitializationException(string message, Exception inner) : base(message, inner)
    {
    }
}