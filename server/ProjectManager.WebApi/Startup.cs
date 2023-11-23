using Microsoft.Extensions.Options;
using ProjectManager.Common.DependencyInjection;
using ProjectManager.Core;
using ProjectManager.WebApi.Auth;
using ProjectManager.WebApi.ExceptionHandling;
using ProjectManager.WebApi.OpenApi;
using ProjectManager.WebApi.OpenTelemetry;
using Serilog;

namespace ProjectManager.WebApi;

public class Startup
{
    private readonly IHostEnvironment _environment;

    public Startup(IHostEnvironment environment)
    {
        _environment = environment;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddNewtonsoftJson();
        services.AddHealthChecks();
        services.AddCors();
        
        services.AddModule<AuthModule>();
        services.AddModule<ExceptionHandlingModule>();
        services.AddModule<OpenTelemetryModule>();
        services.AddModule<OpenApiModule>();

        services.AddModule<ProjectManagerModule>();
    }

    public void Configure(IApplicationBuilder app, IOptions<CorsHostingOptions> corsOptions)
    {
        app.UseExceptionHandler();
        if (!_environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseSerilogRequestLogging();
        app.UseCors(x =>
        {
            x.WithOrigins(corsOptions.Value.AllowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/health");
            endpoints.MapControllers();
        });
    }
}

