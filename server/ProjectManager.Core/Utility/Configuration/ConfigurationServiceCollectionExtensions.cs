using Microsoft.Extensions.Options;

namespace ProjectManager.Core.Utility.Configuration;

public static class ConfigurationServiceCollectionExtensions
{
    public static OptionsBuilder<TOptions> BindOptions<TOptions>(this IServiceCollection services,
        string configSectionName, Action<NestedOptionsBuilder> configureNestedOptions = null) where TOptions : class
    {
        var optionsBuilder = services.AddOptions<TOptions>()
            .BindConfiguration(configSectionName);
        if (configureNestedOptions is not null)
        {
            configureNestedOptions(new NestedOptionsBuilder(configSectionName, services));
        }

        return optionsBuilder;
    }

    /// <summary>
    /// Invokes BindOptions by convention. Assumes that the config section shares the name with the options instance,
    /// without the 'Options' suffix.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configureNestedOptions"></param>
    /// <typeparam name="TOptions"></typeparam>
    /// <returns></returns>
    public static OptionsBuilder<TOptions> BindOptions<TOptions>(this IServiceCollection services,
        Action<NestedOptionsBuilder> configureNestedOptions = null)
        where TOptions : class =>
        services.BindOptions<TOptions>(ConfigurationUtility.ParseSectionName<TOptions>(), configureNestedOptions);
}