using System;
using System.Runtime.Serialization;

namespace ProjectManager.Common.DependencyInjection;

[Serializable]
public class ModuleInitializationException : Exception
{
    protected ModuleInitializationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ModuleInitializationException(string message) : base(message)
    {
    }

    public ModuleInitializationException(string message, Exception inner) : base(message, inner)
    {
    }
}