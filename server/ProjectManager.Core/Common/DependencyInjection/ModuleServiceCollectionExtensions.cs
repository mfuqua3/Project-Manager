using System.Reflection;
using Microsoft.Extensions.Logging.Abstractions;

namespace ProjectManager.Common.DependencyInjection;

public static class ModuleServiceCollectionExtensions
{
    public static IServiceCollection AddModule<TModule, TOptions>(this IServiceCollection services,
        Action<TOptions> configureOptions)
        where TModule : Module<TOptions> where TOptions : class, new()
    {
        var constructors = typeof(TModule).GetConstructors();
        var constructor = constructors.OrderBy(x => x.GetParameters().Length).First();
        var parameters = constructor.GetParameters();
        var moduleInstance = ConstructModuleInstance<TModule>(services, parameters);
        moduleInstance.Initialize(services);
        var options = moduleInstance.GetOptions() ?? new TOptions();
        configureOptions.Invoke(options);
        try
        {
            moduleInstance.ConfigureServices(services, options);
        }
        catch (Exception ex)
        {
            throw new ModuleInitializationException(
                $"An exception occurred while attempting to bootstrap {typeof(TModule).Name}. See inner exception for details.",
                ex);
        }

        return services;
    }

    public static IServiceCollection AddModule<T>(this IServiceCollection services)
        where T : Module
    {
        var constructors = typeof(T).GetConstructors();
        var constructor = constructors.OrderBy(x => x.GetParameters().Length).First();
        var parameters = constructor.GetParameters();
        var moduleInstance = ConstructModuleInstance<T>(services, parameters);
        moduleInstance.Initialize(services);
        try
        {
            moduleInstance.ConfigureServices(services);
        }
        catch (Exception ex)
        {
            throw new ModuleInitializationException(
                $"An exception occurred while attempting to bootstrap {typeof(T).Name}. See inner exception for details.",
                ex);
        }

        return services;
    }

    private static T ConstructModuleInstance<T>(IServiceCollection services, ParameterInfo[] parameters)
    {
        // Retrieve necessary services
        var configuration =
            GetLazyInstance(services, ThrowNoServiceInstanceException<T, IConfiguration>);
        if (parameters.Length == 0)
        {
            var defaultInstance = Activator.CreateInstance<T>();
            if (defaultInstance is IModuleWithConfiguration withConfiguration)
            {
                withConfiguration.ProvideConfiguration(configuration.Value);
            }

            return defaultInstance;
        }

        var hostEnvironment =
            GetLazyInstance(services, ThrowNoServiceInstanceException<T, IHostEnvironment>);
        var loggerFactory =
            GetLazyInstance<ILoggerFactory>(services, () => new NullLoggerFactory());
        var parameterTypes = parameters.Select(param => param.ParameterType).ToList();

        // Find disallowed constructor parameters
        var allowedParameterTypes = new[] { typeof(IConfiguration), typeof(ILogger), typeof(IHostEnvironment) };
        var allowedParameterGenericTypes = new[] { typeof(ILogger<>) };
        var invalidParameters = parameterTypes.Where(t =>
            !allowedParameterTypes.Contains(t) &&
            (!t.IsGenericType || !allowedParameterGenericTypes.Contains(t.GetGenericTypeDefinition()))).ToArray();
        if (invalidParameters.Any())
        {
            throw new ModuleUsageException(
                $"Module constructor may only inject {nameof(IConfiguration)}, {nameof(IHostEnvironment)}, or " +
                $"{nameof(ILogger<T>)}/{nameof(ILogger)}. " +
                $"Requested types are not available at container initialization: {string.Join(',', invalidParameters.Select(x => x.Name))}");
        }

        var args = new List<object>();

        foreach (var parameterType in parameterTypes)
        {
            if (parameterType == typeof(IConfiguration))
            {
                args.Add(configuration.Value);
                continue;
            }

            if (parameterType == typeof(IHostEnvironment))
            {
                args.Add(hostEnvironment.Value);
                continue;
            }

            if (parameterType == typeof(ILogger))
            {
                args.Add(loggerFactory.Value.CreateLogger(typeof(T)));
                continue;
            }

            if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(ILogger<>))
            {
                var typeArgument = parameterType.GetGenericArguments().First();
                args.Add(loggerFactory.Value.CreateLogger(typeArgument));
            }
        }

        var instance = (T)Activator.CreateInstance(typeof(T), args.ToArray());
        if (instance is IModuleWithConfiguration moduleWithConfiguration)
        {
            moduleWithConfiguration.ProvideConfiguration(configuration.Value);
        }

        return instance;
    }

    private static TService ThrowNoServiceInstanceException<TModule, TService>()
        => throw new ModuleInitializationException(
            $"Failed to initialize Module {typeof(TModule).Name}. " +
            $"Unable to resolve an instance of the requested {typeof(TService).Name} parameter");

    private static Lazy<T> GetLazyInstance<T>(IServiceCollection services, Func<T> defaultValueFactory)
        where T : class =>
        new(() =>
        {
            var serviceInstance = services.FirstOrDefault(x => x.ServiceType == typeof(T));
            if (serviceInstance == null)
            {
                return defaultValueFactory.Invoke();
            }

            if (serviceInstance.ImplementationInstance != null)
            {
                return serviceInstance.ImplementationInstance as T;
            }

            if (serviceInstance.ImplementationFactory != null)
            {
                var provider = services.BuildServiceProvider();
                return serviceInstance.ImplementationFactory(provider) as T;
            }

            return defaultValueFactory.Invoke();
        });
}