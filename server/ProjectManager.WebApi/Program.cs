using ProjectManager.Common.Configuration;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Events;

namespace ProjectManager.WebApi;

public class Program
{
    public static Task Main(string[] args)
        => CreateHostBuilder().Build().RunAsync();

    public static IHostBuilder CreateHostBuilder(string[] args = null) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog(ConfigureLogging)
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
            .ConfigureServices(services =>
            {
                services.BindOptions<HostingOptions>(x =>
                {
                    x.BindNestedOptions<CorsHostingOptions>("Cors");
                });
            });
    public static void ConfigureLogging(
        HostBuilderContext ctx,
        IServiceProvider serviceProvider,
        LoggerConfiguration lc)
        => lc.ReadFrom.Configuration(ctx.Configuration)
            .ReadFrom.Services(serviceProvider)
            //.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .Enrich.WithSpan()
            .Enrich.FromLogContext();
}