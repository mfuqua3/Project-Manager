using Microsoft.Extensions.Hosting;

namespace ProjectManager.Tests.HttpProxy;

public class WebHostProxy : IDisposable
{
    private Lazy<IHost> _host;
    private bool _disposed = false;
    private bool _started = false;

    public WebHostProxy(Lazy<IHost> host)
    {
        _host = host;
    }

    public async Task StartAsync()
    {
        if (_started)
        {
            throw new InvalidOperationException("Web host proxy has already been started.");
        }
        await _host.Value.StartAsync();
        _started = true;
    }


    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing)
        {
            _host.Value.Dispose();
        }

        _host = null;
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~WebHostProxy()
    {
        Dispose(false);
    }
}
