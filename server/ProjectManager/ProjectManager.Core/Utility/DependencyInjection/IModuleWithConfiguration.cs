namespace ProjectManager.Core.Utility.DependencyInjection;

public interface IModuleWithConfiguration : IModule
{
    void ProvideConfiguration(IConfiguration configuration);
}