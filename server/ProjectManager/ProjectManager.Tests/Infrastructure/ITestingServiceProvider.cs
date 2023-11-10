namespace ProjectManager.Tests.Infrastructure;

/// <summary>
/// Provides methods for retrieving services tailored for testing purposes.
/// </summary>
public interface ITestingServiceProvider
{
    /// <summary>
    /// Retrieves the service of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the service object to retrieve.</typeparam>
    /// <returns>A service object of type <typeparamref name="T"/>.</returns>
    /// <exception cref="InvalidOperationException">There is no service of type <typeparamref name="T"/>.</exception>
    T GetService<T>();

    /// <summary>
    /// Retrieves the named service of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the service object to retrieve.</typeparam>
    /// <param name="name">The name of the service registration to retrieve.</param>
    /// <returns>A service object of type <typeparamref name="T"/> that matches the given <paramref name="name"/>.</returns>
    /// <exception cref="InvalidOperationException">There is no service of type <typeparamref name="T"/> with the given name.</exception>
    T GetService<T>(string name);
}