using System.Collections.Concurrent;
using Serilog.Events;

namespace ProjectManager.Tests.Utility;

public class LogEventTracker : IDisposable
{
    private readonly LogEventPublishingSink _sink;

    private readonly ConcurrentQueue<LogEvent> _events = new();
    private bool _listening = false;

    public LogEventTracker(LogEventPublishingSink sink)
    {
        _sink = sink;
        sink.EventEmitted += Sink_EventEmitted;
    }

    public void Listen()
    {
        if (_listening) return;
        _listening = true;
        _sink.EventEmitted += Sink_EventEmitted;
    }

    public void StopListening()
    {
        if (!_listening) return;
        _listening = false;
        _sink.EventEmitted -= Sink_EventEmitted;
    }
    private void Sink_EventEmitted(object sender, LogEventPublishingSink.EventArgs e)
        => _events.Enqueue(e.Event);

    public IEnumerable<LogEvent> Events => _events.ToArray();


    

    private void ReleaseUnmanagedResources()
    {
        _sink.EventEmitted -= Sink_EventEmitted;
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~LogEventTracker()
    {
        ReleaseUnmanagedResources();
    }
}