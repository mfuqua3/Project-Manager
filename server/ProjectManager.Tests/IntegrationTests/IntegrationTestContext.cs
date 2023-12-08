using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using ProjectManager.Data;

namespace ProjectManager.Tests.IntegrationTests;

[SetUpFixture]
public class IntegrationTestContext 
{
    public static IServiceProvider Services { get; private set; }
    public static TestServer TestServer { get; private set; }
    private readonly bool _tearDownDatabase = false;
    private string _databaseName;

    [OneTimeSetUp]
    public async Task SetUpIntegrationTests()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        _databaseName = "project_manager_tests_db";
        if (_databaseName == "") throw new NotImplementedException("Provide a database name");
        var application = new IntegrationTestsApplication(_databaseName);
        
        Services = application.Services;
        TestServer = application.Server;
        await using var scope = Services.CreateAsyncScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<ProjectManagerDbContext>();
        await dbContext.Database.MigrateAsync();
    }


    [OneTimeTearDown]
    public async Task TearDownIntegrationTests()
    {
        await using var teardownScope = Services.CreateAsyncScope();
        await using var dbContext = teardownScope.ServiceProvider.GetRequiredService<ProjectManagerDbContext>();
        try
        {
            if (_tearDownDatabase)
            {
                await dbContext.Database.EnsureDeletedAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(@"An exception occurred during teardown. " + e.Message);
        }
    }
}
