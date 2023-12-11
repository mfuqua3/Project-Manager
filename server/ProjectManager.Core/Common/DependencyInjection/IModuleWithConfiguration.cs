using Microsoft.Extensions.Configuration;

namespace ProjectManager.Common.DependencyInjection
{
    /// <summary>
    /// Represents a module with configuration.
    /// </summary>
    public interface IModuleWithConfiguration : IModule
    {
        /// <summary>
        /// Provides the configuration for the module.
        /// </summary>
        /// <param name="configuration">The configuration for the module.</param>
        void ProvideConfiguration(IConfiguration configuration);
    }
}