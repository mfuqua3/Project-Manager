namespace ProjectManager.Tests.Infrastructure;

/// <summary>
/// Provides methods to assist with setup activities specific to testing scenarios.
/// </summary>
public interface ITestingSetUpMethods
{
    /// <summary>
    /// Enables tracking of log events for the current test scope.
    /// </summary>
    void EnableLogTracking();
}