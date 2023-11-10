using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ProjectManager.Core.Utility.DependencyInjection.NamedServices;

public class NamedServiceDescriptor<TService, TImplementation> : NamedServiceDescriptor<TService>
    where TImplementation : class, TService
{
    public NamedServiceDescriptor([NotNull] string name, ServiceLifetime lifetime)
    :base(name, provider=> provider.GetRequiredService<TImplementation>(name), lifetime)
    {
        
    }
}
public class NamedServiceDescriptor<T> : NamedServiceDescriptor
{
    public NamedServiceDescriptor([NotNull] string name,
        [NotNull] Func<IServiceProvider, T> factory, ServiceLifetime lifetime) : base(name,
        typeof(NamedService<T>), provider => new NamedService<T>(name, () => factory(provider)),
        lifetime)
    {
    }
    public NamedServiceDescriptor([NotNull] string name, ServiceLifetime lifetime) : base(name, typeof(NamedService<T>), 
        provider => new NamedService<T>(name, provider.GetRequiredService<T>(name)), lifetime)
    {
    }
    public NamedServiceDescriptor([NotNull] string name,
        [NotNull] T instance) : base(name, typeof(NamedService<T>), new NamedService<T>(name, instance))
    {
    }
}

public class NamedServiceDescriptor : ServiceDescriptor
{
    [NotNull] public string Name { get; }

    public Type NamedServiceType =>
        (ServiceType.IsGenericType && ServiceType.GetGenericTypeDefinition() == typeof(NamedService<>))
            ? ServiceType.GenericTypeArguments.Single()
            : null;

    public NamedServiceDescriptor([NotNull] string name, [NotNull] Type serviceType,
        [NotNull] Func<IServiceProvider, object> factory, ServiceLifetime lifetime) : base(serviceType, factory,
        lifetime)
    {
        Name = name;
    }

    public NamedServiceDescriptor([NotNull] string name, [NotNull] Type serviceType,
        [NotNull] Type implementationType, ServiceLifetime lifetime) : base(
        serviceType, implementationType, lifetime)
    {
        Name = name;
    }

    public NamedServiceDescriptor([NotNull] string name, [NotNull] Type serviceType, [NotNull] object instance) : base(
        serviceType, instance)
    {
        Name = name;
    }
}