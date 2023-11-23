using System.Runtime.CompilerServices;
using Bogus;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using ProjectManager.Data;
using ProjectManager.Tests.IntegrationTests;
using Serilog.Events;

namespace ProjectManager.Tests.Infrastructure;

/// <summary>
/// Provides a base class for integration tests.
/// Manages test fixture and test scope initialization, teardown,
/// and offers utility methods to aid testing scenarios.
/// </summary>
public abstract class IntegrationTest
{
    private const string EarlyAccessException =
        "Disallowed call to {0}. Test fixture has not initialized. " +
        "Do not call into test methods from the constructor, " +
        "initialize fixture resources within a [OneTimeSetup] method.";
    private ITestingExecutionScope _testFixtureExecutionScope;
    private ITestingExecutionScope _testExecutionScope;
    private IHostEnvironment _hostEnvironment;
    private TestServer _testServer;
    private bool _testFixtureInitialized;

    [OneTimeSetUp]
    public void InitializeFixtureScope()
    {
        var applicationServiceProvider = IntegrationTestContext.Services;
        _hostEnvironment = applicationServiceProvider.GetRequiredService<IHostEnvironment>();
        _testServer = IntegrationTestContext.TestServer;
        //Initialize Test Fixture Scope
        _testFixtureExecutionScope = new TestingExecutionScope(applicationServiceProvider);
        _testFixtureExecutionScope.Activate();
    }

    [SetUp]
    public void InitializeTestScope()
    {
        if (!_testFixtureInitialized)
        {
            //The first time [SetUp] is called in a base class, it means the [OneTimeSetUp] in all derived classes
            //have concluded. This signals that the first test scope has begun
            ConcludeTestFixtureInitialization();
        }

        _testExecutionScope = _testFixtureExecutionScope.CreateChildExecutionScope();
        _testExecutionScope.Activate();
    }

    [TearDown]
    public void TearDownTestScope() => _testExecutionScope.Dispose();

    [OneTimeTearDown]
    public void TearDownFixtureScope() => _testFixtureExecutionScope.Dispose();

    /// <inheritdoc cref="IWorkingEnvironment.EnvironmentName"/>
    protected string EnvironmentName => _hostEnvironment.EnvironmentName;
    protected Faker Faker { get; } = new();

    private ITestingExecutionScope GetCurrentExecutionScope([CallerMemberName] string callerMemberName = null)
    {
        EnsureResourceAccessPermitted(callerMemberName);
        return _testFixtureInitialized ? _testExecutionScope : _testFixtureExecutionScope;
    }

    private IntegrationTest EnsureResourceAccessPermitted([CallerMemberName] string callerMemberName = null)
    {
        if (_testFixtureExecutionScope == null)
        {
            throw new TestResourceAccessException(string.Format(EarlyAccessException, callerMemberName));
        }

        return this;
    }

    private void ConcludeTestFixtureInitialization()
    {
        _testFixtureInitialized = true;
        _testFixtureExecutionScope.Deactivate();
    }

    /// <summary>
    /// Creates an HTTP client that can send requests to the test server.
    /// </summary>
    /// <returns>An instance of <see cref="HttpClient"/>.</returns>
    protected HttpClient CreateClient() => EnsureResourceAccessPermitted()._testServer.CreateClient();

    protected RequestBuilder CreateRequest(string path) => EnsureResourceAccessPermitted()._testServer.CreateRequest(path);

    /// <summary>
    /// Retrieves all log events in the order they were logged.
    /// If the log tracking is not enabled for the test fixture execution scope, it will retrieve log events
    /// from the test execution scope.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when attempting to retrieve log events
    /// from a scope when log tracking has not been enabled.</exception>
    /// <returns>An <see cref="IEnumerable{LogEvent}"/> containing all logged events.</returns>
    protected IEnumerable<LogEvent> GetAllLogEventsInOrder()
        => EnsureResourceAccessPermitted()._testFixtureExecutionScope.LogTrackingEnabled
            ? _testFixtureExecutionScope.LogEvents.Concat(_testExecutionScope?.LogEvents ?? Array.Empty<LogEvent>())
            : _testExecutionScope.LogEvents;


    /// <summary>
    /// Stores required test data within the appropriate data storage mechanism.
    /// </summary>
    /// <param name="testObjects">The data objects to be stored.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected async Task StoreRequiredTestData(params object[] testObjects)
    {
        throw new NotImplementedException();
    }

    protected ProjectManagerDbContext OpenSession() =>
        GetCurrentExecutionScope().OpenSession();

    /// <inheritdoc cref="ITestingSetUpMethods.EnableLogTracking()"/>
    protected void EnableLogTracking()
        => GetCurrentExecutionScope().EnableLogTracking();

    /// <inheritdoc cref="ITestingServiceProvider.GetService{T}()"/>
    protected T GetService<T>()
        => GetCurrentExecutionScope().GetService<T>();

    /// <inheritdoc cref="ITestingServiceProvider.GetService{T}(string)"/>
    protected T GetService<T>(string name)
        => GetCurrentExecutionScope().GetService<T>(name);
}
