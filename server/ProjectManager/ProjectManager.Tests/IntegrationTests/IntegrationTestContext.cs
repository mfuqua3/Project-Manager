using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ProjectManager.Tests.IntegrationTests;

[SetUpFixture]
public class IntegrationTestContext 
{
    public static IServiceProvider Services { get; private set; }
    public static TestServer TestServer { get; private set; }
    private readonly bool _tearDownDatabase = false; //TODO
    private string _databaseName;

    [OneTimeSetUp]
    public async Task SetUpIntegrationTests()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        _databaseName = "";//TODO
        if (_databaseName == "") throw new NotImplementedException("Provide a database name");
        var application = new IntegrationTestsApplication(_databaseName);
        
        Services = application.Services;
        TestServer = application.Server;
    }


    [OneTimeTearDown]
    public async Task TearDownIntegrationTests()
    {
        await using var teardownScope = Services.CreateAsyncScope();
        //TODO Example Teardown with EF DbContext
        //     await using var dbContext = teardownScope.ServiceProvider.GetRequiredService<DbContext <-- Replace Me>();
        //     try
        //     {
        //         if (_tearDownDatabase)
        //         {
        //             await dbContext.Database.EnsureDeletedAsync();
        //         }
        //     }
        //     catch (Exception e)
        //     {
        //         Console.WriteLine(@"An exception occurred during teardown. " + e.Message);
        //     }
    }
}
