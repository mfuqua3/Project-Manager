using Microsoft.Extensions.DependencyInjection;

namespace ProjectManager.Common.DependencyInjection;

/// <summary>
/// Provides an interface to manage the initialization and configuration of services in the project.
/// </summary>
public interface IModule
{
    /// <summary>
    /// Initializes services in the implementation of the IModule interface.
    /// </summary>
    /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
    void Initialize(IServiceCollection services);

    /// <summary>
    /// Configure services in the implementation of the IModule interface.
    /// </summary>
    /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
    void ConfigureServices(IServiceCollection services);
}