using System.Runtime.Serialization;

namespace ProjectManager.Core.Utility.DependencyInjection;

[Serializable]
public class ModuleUsageException : Exception
{
    protected ModuleUsageException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ModuleUsageException(string message) : base(message)
    {
    }

    public ModuleUsageException(string message, Exception inner) : base(message, inner)
    {
        
    }
}