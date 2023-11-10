using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ProjectManager.Core.Utility.DependencyInjection;

namespace ProjectManager.WebApi.OpenTelemetry;

public class OpenTelemetryModule : Module
{
    private const string ServiceName = "project-manager";
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .WithTracing(builder =>
            {
                builder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(ServiceName))
                    .AddAspNetCoreInstrumentation();
            });
    }
}