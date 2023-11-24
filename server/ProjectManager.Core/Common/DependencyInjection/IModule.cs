using Microsoft.Extensions.DependencyInjection;

namespace ProjectManager.Common.DependencyInjection;

public interface IModule
{
    void Initialize(IServiceCollection services);
    void ConfigureServices(IServiceCollection services);
}