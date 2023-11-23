using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectManager.Data;
using ProjectManager.Tests.Utility;
using Serilog.Events;

namespace ProjectManager.Tests.Infrastructure;

/// <inheritdoc />
public class TestingExecutionScope : ITestingExecutionScope
{
    private IServiceScope _executionScope;
    private readonly Queue<IDisposable> _managedDisposables = new();
    private LogEventTracker _logEventTracker;
    private bool _disposed;
    private bool _active;

    public TestingExecutionScope(IServiceProvider serviceProvider)
    {
        _executionScope = serviceProvider.CreateScope();
    }

    private TestingExecutionScope(IServiceProvider serviceProvider, ITestState initialState) : this(serviceProvider)
    {
        if (initialState.LogTrackingEnabled)
        {
            EnableLogTracking();
        }
    }

    /// <inheritdoc />
    public bool LogTrackingEnabled => _logEventTracker != null;

    /// <inheritdoc />
    public IEnumerable<LogEvent> LogEvents => LogTrackingEnabled
        ? _logEventTracker.Events
        : throw new InvalidOperationException(
            "Log tracking must be enabled to audit log events from a tracked session.");


    /// <inheritdoc />
    public void EnableLogTracking()
    {
        var sink = GetService<LogEventPublishingSink>();
        _logEventTracker = new LogEventTracker(sink);
        _managedDisposables.Enqueue(_logEventTracker);
    }

    /// <inheritdoc />
    public T GetService<T>() => _executionScope.ServiceProvider.GetRequiredService<T>();

    public T GetService<T>(string name)
    {
        throw new NotImplementedException();
    }


    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public ITestingExecutionScope CreateChildExecutionScope()
        => new TestingExecutionScope(_executionScope.ServiceProvider, this);

    /// <inheritdoc />
    public void Activate()
    {
        if (_active)
        {
            return;
        }

        _active = true;
        _logEventTracker?.Listen();
    }

    /// <inheritdoc />
    public void Deactivate()
    {
        if (!_active)
        {
            return;
        }

        _active = false;
        _logEventTracker?.StopListening();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            while (_managedDisposables.TryDequeue(out var disposable))
            {
                try
                {
                    disposable?.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            _executionScope?.Dispose();
        }

        _executionScope = null;
        _disposed = true;
    }

    private static Lazy<T> InitializeLazyService<T>(IServiceScope scope) =>
        InitializeLazyService<T>(scope.ServiceProvider);

    private static Lazy<T> InitializeLazyService<T>(IServiceProvider serviceProvider)
        => new(serviceProvider.GetRequiredService<T>);

    public ProjectManagerDbContext OpenSession()
        => _executionScope.ServiceProvider
            .GetRequiredService<IDbContextFactory<ProjectManagerDbContext>>()
            .CreateDbContext();
}