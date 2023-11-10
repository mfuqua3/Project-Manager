namespace ProjectManager.Tests.Infrastructure;

/// <summary>
/// Provides methods for creating data access sessions and contexts, tailored for testing purposes.
/// </summary>
public interface ITestingDataAccess<out TRepo>
{
    /// <summary>
    /// Creates a new session for managing and tracking changes to stored entities.
    /// </summary>
    /// <returns>An open data access repository.</returns>
    TRepo OpenSession();
}