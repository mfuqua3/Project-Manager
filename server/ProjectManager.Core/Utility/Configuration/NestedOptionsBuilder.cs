using Microsoft.Extensions.Options;

namespace ProjectManager.Core.Utility.Configuration;

public class NestedOptionsBuilder
{
    private readonly string _configSectionRoot;
    private readonly IServiceCollection _services;

    public NestedOptionsBuilder(string configSectionRoot, IServiceCollection services)
    {
        _configSectionRoot = configSectionRoot;
        _services = services;
    }
    public OptionsBuilder<TOptions> BindNestedOptions<TOptions>(string configSectionName,
        Action<NestedOptionsBuilder> configureNestedOptions = null)
        where TOptions : class
        => _services.BindOptions<TOptions>($"{_configSectionRoot}:{configSectionName}", configureNestedOptions);

    public OptionsBuilder<TOptions> BindNestedOptions<TOptions>(
        Action<NestedOptionsBuilder> configureNestedOptions = null)
        where TOptions : class
        => _services.BindOptions<TOptions>($"{_configSectionRoot}:{ConfigurationUtility.ParseSectionName<TOptions>()}",
            configureNestedOptions);
}