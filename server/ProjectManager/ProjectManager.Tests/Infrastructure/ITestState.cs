using Serilog.Events;

namespace ProjectManager.Tests.Infrastructure;

/// <summary>
/// Represents the state of the current test, providing access to various aspects such as tenant information and log tracking details.
/// </summary>
public interface ITestState
{

    /// <summary>
    /// Gets a value indicating whether log tracking is enabled for the current test scope.
    /// </summary>
    bool LogTrackingEnabled { get; }

    /// <summary>
    /// Gets the collection of log events captured during the test, if log tracking is enabled.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when attempting to access the log events when log tracking is disabled.
    /// </exception>
    IEnumerable<LogEvent> LogEvents { get; }
}