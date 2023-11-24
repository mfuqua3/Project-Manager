using System;
using System.Runtime.Serialization;

namespace ProjectManager.Common.DependencyInjection.NamedServices;

[Serializable]
public class DuplicateNamedRegistrationException : Exception
{
    public DuplicateNamedRegistrationException(string serviceName, Type serviceType):
        base($@"Named registration conflict. A {serviceType.Name} service named {serviceName} has already been registered.")
    {
        
    }
    protected DuplicateNamedRegistrationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}