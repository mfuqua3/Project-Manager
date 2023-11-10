using ProjectManager.Core.Data;

namespace ProjectManager.Tests.Infrastructure;

/// <summary>
/// Represents a scope for test execution that provides methods and properties for managing test state, 
/// data access, setup, and service provisioning, and allows creation of child execution scopes.
/// </summary>
public interface ITestingExecutionScope : ITestState, ITestingDataAccess<ProjectManagerDbContext>, ITestingSetUpMethods, ITestingServiceProvider, IDisposable
{
    /// <summary>
    /// Creates a child execution scope based on the current scope. 
    /// This allows for hierarchical setup and teardown within the context of a broader test scenario.
    /// </summary>
    /// <returns>A new instance of <see cref="ITestingExecutionScope"/> representing the child execution scope.</returns>
    ITestingExecutionScope CreateChildExecutionScope();
    /// <summary>
    /// Activates this execution scope and informs any state listening processes to run
    /// </summary>
    void Activate();
    /// <summary>
    /// Deactivates this execution scope and stops all state listening processes
    /// </summary>
    void Deactivate();
}