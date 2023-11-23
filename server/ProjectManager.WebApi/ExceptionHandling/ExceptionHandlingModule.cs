using System.Net;
using ProjectManager.Common.DependencyInjection;
using ProjectManager.Common.Exceptions;

namespace ProjectManager.WebApi.ExceptionHandling;

public class ExceptionHandlingModule : Module
{
    private readonly IHostEnvironment _environment;

    public ExceptionHandlingModule(IHostEnvironment environment)
    {
        _environment = environment;
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddProblemDetails(cfg=>
        {
            cfg.MapExceptions(x =>
            {
                x.Map<ProjectManagerBadRequestException>(HttpStatusCode.BadRequest);
                x.Map<ProjectManagerDataNotFoundException>(HttpStatusCode.NotFound);
                x.Map<ProjectManagerForbiddenAccessException>(HttpStatusCode.Forbidden);
                x.Map<ProjectManagerExternalTimeoutException>(HttpStatusCode.GatewayTimeout);
                x.Map<ProjectManagerExternalErrorException>(HttpStatusCode.BadGateway);
            });
            if (!_environment.IsProduction())
            {
                cfg.IncludeExceptionDetails();
            }
        });
    }
}