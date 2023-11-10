using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectManager.Core.Features.Authorization;
using ProjectManager.Core.Features.Authorization.Engines;
using ProjectManager.Tests.IntegrationTests.Data;
using ProjectManager.Tests.IntegrationTests.Stubs;
using ProjectManager.Tests.Utility;
using ProjectManager.WebApi;
using Serilog;

namespace ProjectManager.Tests.IntegrationTests;

internal class IntegrationTestsApplication : WebApplicationFactory<Program>
{
    private readonly string _databaseName;


    public IntegrationTestsApplication(string databaseName)
    {
        _databaseName = databaseName;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var jwtSecret = SecretGenerator.GenerateSecret();
        builder.ConfigureTestServices(services =>
        {
            services.Configure<JwtOptions>(opt => opt.Secret = jwtSecret);
            services.Configure<GoogleOptions>(opt => opt.ValidEmails = new List<string>
            {
                TestAdmin.Email,
                TestUserToCreate.Email
            });
            services.AddScoped<IntegrationTestDataUtility>();
            services.AddTransient<GoogleIdTokenFactory>();
            services.AddSingleton<IGoogleAuthenticationEngine, GoogleAuthenticationEngineStub>();
        });
        base.ConfigureWebHost(builder);
        builder.ConfigureServices((ctx, services) =>
        {
            
        });
    }

    protected override IHostBuilder CreateHostBuilder()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environments.Development;

        var sink = new LogEventPublishingSink();

        return Program.CreateHostBuilder()
            .UseEnvironment(environment)
            .ConfigureServices(services => services.AddSingleton(sink))
            .UseSerilog((ctx, services, lc) =>
            {
                Program.ConfigureLogging(ctx, services, lc);
                lc.WriteTo.Sink(sink);
            });
    }
}