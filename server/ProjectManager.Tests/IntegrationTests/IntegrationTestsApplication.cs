using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using ProjectManager.Data;
using ProjectManager.Features.Users;
using ProjectManager.Features.Users.Abstractions;
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
            services.PostConfigure<DataOptions>(opt =>
            {
                var connectionStringBuilder = new NpgsqlConnectionStringBuilder(opt.ConnectionString)
                {
                    Database = _databaseName
                };
                opt.ConnectionString = connectionStringBuilder.ConnectionString;
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