using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectManager.Tests.HttpProxy;

/// <summary>
/// Represents a base class for hosted test servers.
/// </summary>
/// <remarks>
/// Derived classes should provide implementations for configuring services and the application pipeline to 
/// create a customized environment for hosting and testing the application. The <see cref="HostedTestServer"/> 
/// can be used to simulate various application states, behaviors, and configurations without affecting the 
/// production environment, making it an essential tool for thorough and reliable integration testing.
/// </remarks>
public abstract class HostedTestServer
{
    /// <summary>
    /// Gets the application service provider.
    /// </summary>
    public IServiceProvider ApplicationServiceProvider { get; init; }

    /// <summary>
    /// Configures the services of the test server.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure.</param>
    public abstract void ConfigureServices(IServiceCollection services);

    /// <summary>
    /// Configures the application pipeline of the test server.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> to configure.</param>
    public abstract void Configure(IApplicationBuilder app);
}
