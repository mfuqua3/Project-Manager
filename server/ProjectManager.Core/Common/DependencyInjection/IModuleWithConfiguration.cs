using Microsoft.Extensions.Configuration;

namespace ProjectManager.Common.DependencyInjection;

public interface IModuleWithConfiguration : IModule
{
    void ProvideConfiguration(IConfiguration configuration);
}