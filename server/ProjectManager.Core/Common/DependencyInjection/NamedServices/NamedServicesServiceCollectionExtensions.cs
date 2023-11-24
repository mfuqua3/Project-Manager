using System;
using System.Linq;
using ProjectManager.Common.DependencyInjection.NamedServices;

// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection.Extensions;

public static class NamedServicesServiceCollectionExtensions
{
    public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection services,
        string name)
        where TImplementation : class, TService =>
        services.Add<TService, TImplementation>(name, ServiceLifetime.Transient);
    public static IServiceCollection AddTransient<TService>(this IServiceCollection services,
        string name, Func<IServiceProvider, TService> factory) where TService : class =>
        services.Add<TService>(name, factory, ServiceLifetime.Transient);
    public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection services,
        string name, Func<IServiceProvider, TImplementation> factory)
        where TImplementation : class, TService =>
        services.Add<TService, TImplementation>(name, factory, ServiceLifetime.Transient);

    public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection services,
        string name)
        where TImplementation : class, TService =>
        services.Add<TService, TImplementation>(name, ServiceLifetime.Scoped);
    public static IServiceCollection AddScoped<TService>(this IServiceCollection services,
        string name, Func<IServiceProvider, TService> factory) where TService : class =>
        services.Add<TService>(name, factory, ServiceLifetime.Scoped);
    public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection services,
        string name, Func<IServiceProvider, TImplementation> factory)
        where TImplementation : class, TService =>
        services.Add<TService, TImplementation>(name, factory, ServiceLifetime.Scoped);

    public static IServiceCollection AddSingleton<TService>(this IServiceCollection services,
        string name, Func<IServiceProvider, TService> factory) where TService : class =>
        services.Add<TService>(name, factory, ServiceLifetime.Singleton);

    public static IServiceCollection AddSingleton<TService>(this IServiceCollection services, string name,
        TService instance)
    {
        services.ValidateServiceNameAvailability<TService>(name);
        services.TryAdd(new ServiceDescriptor(typeof(INamedServiceCollection<TService>),
            typeof(NamedServiceCollection<TService>), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(TService), instance));
        services.Add(new NamedServiceDescriptor<TService>(name, instance));
        return services;
    }

    public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection services,
        string name)
        where TImplementation : class, TService =>
        services.Add<TService, TImplementation>(name, ServiceLifetime.Singleton);

    public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection services,
        string name, Func<IServiceProvider, TImplementation> factory)
        where TImplementation : class, TService =>
        services.Add<TService, TImplementation>(name, factory, ServiceLifetime.Singleton);

    public static IServiceCollection Add<TService, TImplementation>(this IServiceCollection services, string name,
        ServiceLifetime serviceLifetime)
        where TImplementation : class, TService
    {
        if (typeof(TService) == typeof(TImplementation))
        {
            throw new InvalidOperationException(
                $"Invalid registration. {nameof(TService)} is the same type as {nameof(TImplementation)}. " +
                $"Consider using the single-parameter registration overload.");
        }
        services.ValidateServiceNameAvailability<TService>(name);
        services.TryAdd(new ServiceDescriptor(typeof(INamedServiceCollection<TService>),
            typeof(NamedServiceCollection<TService>), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(TImplementation), typeof(TImplementation), serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), serviceLifetime));
        services.Add(new NamedServiceDescriptor<TService,TImplementation>(name, serviceLifetime));
        return services;
    }

    public static IServiceCollection Add<TService, TImplementation>(this IServiceCollection services, string name,
        Func<IServiceProvider, TImplementation> factory,
        ServiceLifetime serviceLifetime)
        where TImplementation : class, TService
    {
        if (typeof(TService) == typeof(TImplementation))
        {
            throw new InvalidOperationException(
                $"Invalid registration. {nameof(TService)} is the same type as {nameof(TImplementation)}. " +
                $"Consider using the single-parameter registration overload.");
        }
        services.ValidateServiceNameAvailability<TService>(name);
        services.TryAdd(new ServiceDescriptor(typeof(INamedServiceCollection<TService>),
            typeof(NamedServiceCollection<TService>), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(TImplementation), factory, serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(TService), factory, serviceLifetime));
        services.Add(new NamedServiceDescriptor<TService>(name, factory, serviceLifetime));
        return services;
    }

    public static IServiceCollection Add<TService>(this IServiceCollection services, string name,
        Func<IServiceProvider, TService> factory,
        ServiceLifetime serviceLifetime) where TService : class
    {
        services.ValidateServiceNameAvailability<TService>(name);
        services.TryAdd(new ServiceDescriptor(typeof(INamedServiceCollection<TService>),
            typeof(NamedServiceCollection<TService>), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(TService), factory, serviceLifetime));
        services.Add(new NamedServiceDescriptor<TService>(name, factory, serviceLifetime));
        return services;
    }

    public static IServiceCollection Add<TService>(this IServiceCollection services, string name,
        ServiceLifetime serviceLifetime)
    {
        services.ValidateServiceNameAvailability<TService>(name);
        services.TryAdd(new ServiceDescriptor(typeof(INamedServiceCollection<TService>),
            typeof(NamedServiceCollection<TService>), ServiceLifetime.Transient));
        services.Add(new ServiceDescriptor(typeof(TService), serviceLifetime));
        services.Add(new NamedServiceDescriptor<TService>(name, serviceLifetime));
        return services;
    }
    public static IServiceCollection Replace<TService>(this IServiceCollection services,
        Func<IServiceProvider, TService> implementationFactory, ServiceLifetime lifetime)
        => services.Replace(new ServiceDescriptor(typeof(TService), sp => implementationFactory(sp), lifetime));


    private static void ValidateServiceNameAvailability<TService>(this IServiceCollection services, string name)
    {
        var registrationExists = services.Any(x => x is NamedServiceDescriptor namedRegistration &&
                                                   namedRegistration.NamedServiceType == typeof(TService) &&
                                                   namedRegistration.Name == name);
        if (registrationExists)
        {
            throw new DuplicateNamedRegistrationException(name, typeof(TService));
        }
    }
}
