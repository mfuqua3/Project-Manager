using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using ProjectManager.WebApi.ExceptionHandling;

namespace ProjectManager.Tests.HttpProxy;

public static class HttpProxyServiceCollectionExtensions
{
    /// <summary>
    /// Adds a hosted test server to the specified service collection.
    /// </summary>
    /// <typeparam name="T">The type of the hosted test server. The type must derive from <see cref="HostedTestServer"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <returns>A <see cref="Uri"/> representing the address at which the test server is hosted.</returns>
    /// <exception cref="InvalidOperationException">Thrown if a server port is already specified or an invalid port number is provided.</exception>
    /// <remarks>
    /// This extension method is typically used during the setup phase of integration tests to host a test server 
    /// with custom configurations and services. The hosted test server allows tests to interact with the application's 
    /// HTTP APIs and services in an isolated environment, providing a means to validate behavior under various conditions.
    /// </remarks>
    public static Uri AddHostedTestServer<T>(this IServiceCollection services)
        where T : HostedTestServer, new()
    {
        var port = PortFinder.GetAvailablePort();
        services.AddSingleton(typeof(T).Name,
            provider => new WebHostProxy(new Lazy<IHost>(() =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var testServerBuilder = new HostedTestServerBuilder();
                var hostedTestServer = new T
                {
                    ApplicationServiceProvider = provider
                };
                testServerBuilder
                    .ListenAtPort(port)
                    .ConfigureServices(hostedTestServer.ConfigureServices)
                    .Configure(hostedTestServer.Configure);
                return testServerBuilder.BuildWebHostBuilder(configuration).Build();
            })));
        return new UriBuilder
        {
            Host = "localhost",
            Port = port,
            Scheme = Uri.UriSchemeHttp
        }.Uri;
    }

    private sealed class HostedTestServerBuilder : IHostedTestServerBuilder
    {
        private int _port;
        private readonly Queue<Action<IServiceCollection>> _configureServicesQueue = new();
        private Action<IApplicationBuilder> _configureApp = null;


        public IHostedTestServerBuilder ListenAtPort(int port)
        {
            if (_port != 0)
            {
                throw new InvalidOperationException("Test Server port has already been specified");
            }

            if (port is < 1 or > 65535)
            {
                throw new InvalidOperationException("Must specify a valid port number (1-65535)");
            }

            _port = port;
            return this;
        }

        public IHostedTestServerBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            _configureServicesQueue.Enqueue(configureServices);
            return this;
        }

        public IHostedTestServerBuilder Configure(Action<IApplicationBuilder> configureApp)
        {
            if (_configureApp != null)
            {
                throw new InvalidOperationException($"{nameof(Configure)} may only be called one time");
            }

            _configureApp = configureApp;
            return this;
        }


        public IHostBuilder BuildWebHostBuilder(IConfiguration configuration)
        {
            if (_port == 0)
            {
                _port = PortFinder.GetAvailablePort();
            }

            var builder = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(cfg=>cfg.AddConfiguration(configuration))
                .ConfigureWebHost(webHost =>
                {
                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ??
                                      Environments.Development;
                    webHost
                        .UseKestrel(x => x.ListenLocalhost(_port))
                        .UseEnvironment(environment)
                        .ConfigureServices(
                            services => { services.AddProblemDetails(x => x.IncludeExceptionDetails()); })
                        .Configure(app =>
                        {
                            app.UseExceptionHandler();
                            _configureApp?.Invoke(app);
                        });
                    while (_configureServicesQueue.Count > 0)
                    {
                        var configureServices = _configureServicesQueue.Dequeue();
                        webHost.ConfigureServices(services => configureServices?.Invoke(services));
                    }
                });
            return builder;
        }
    }
}

public static class PortFinder
{
    public static int GetAvailablePort()
    {
        using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Bind(new IPEndPoint(IPAddress.Loopback, 0));
        var availablePort = ((IPEndPoint)socket.LocalEndPoint)!.Port;
        socket.Close();

        return availablePort;
    }
}