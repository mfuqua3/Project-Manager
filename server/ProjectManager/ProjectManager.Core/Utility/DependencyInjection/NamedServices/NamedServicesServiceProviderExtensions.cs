using ProjectManager.Core.Utility.DependencyInjection.NamedServices;

// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection.Extensions;

public static class NamedServicesServiceProviderExtensions
{
    public static T GetRequiredService<T>(this IServiceProvider provider, string name)
    {
        var namedServices = provider.GetService<INamedServiceCollection<T>>() ??
                            NamedServiceCollection<T>.Empty;
        return namedServices.GetValueOrDefault(name) ?? 
               throw new InvalidOperationException("No Service has been registered with that name");
    }
    
    public static T GetService<T>(this IServiceProvider provider, string name)
    {
        var namedServices = provider.GetService<INamedServiceCollection<T>>() ??
                            NamedServiceCollection<T>.Empty;
        return namedServices.GetValueOrDefault(name); 
    }
}