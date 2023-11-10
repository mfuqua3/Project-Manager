using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;

namespace ProjectManager.Core.Utility.Encryption;

public static class EncryptionServiceCollectionExtensions
{
    /// <summary>
    /// Adds encryption services to the specified service collection.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="configureOptions">An optional action delegate to configure the provided <see cref="EncryptionOptions"/>.</param>
    public static void AddEncryption(this IServiceCollection services,
        Action<EncryptionOptions> configureOptions = null)
    {
        if (configureOptions is not null)
        {
            services.Configure(configureOptions);
        }

        services.AddTransient<IEncryptionReader, EncryptionReader>();
    }

    /// <summary>
    /// Configures encrypted options for a named options instance.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being requested.</typeparam>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="name">The name of the options instance.</param>
    /// <param name="memberAccessExpression">An expression that identifies the encrypted option.</param>
    /// <param name="pattern">An optional regular expression pattern to match against the encrypted string.</param>
    /// <exception cref="ArgumentException">Thrown when the expression is not a direct property access expression or the property is not of type <see cref="string"/>.</exception>
    public static void ConfigureEncrypted<TOptions>(this IServiceCollection services, string name,
        Expression<Func<TOptions, string>> memberAccessExpression, Regex pattern = null)
        where TOptions : class
    {
        if (memberAccessExpression.Body is not MemberExpression { Member: PropertyInfo propertyInfo } ||
            propertyInfo.PropertyType != typeof(string))
        {
            throw new ArgumentException($"Invalid use of {nameof(ConfigureEncrypted)}. " +
                                        $"Expression must be a direct property access expression to a " +
                                        $"string property on the options object.");
        }

        var settings = new TypedEncryptedOptionsSettings
        {
            PropertyInfo = propertyInfo,
            Name = name,
            Pattern = pattern
        };
        services.AddTransient<IPostConfigureOptions<TOptions>>(provider =>
        {
            var reader = provider.GetRequiredService<IEncryptionReader>();
            var encryptionOptions = provider.GetRequiredService<IOptions<EncryptionOptions>>();
            var logger = provider.GetRequiredService<ILogger<PostConfigureEncryptedOptions<TOptions>>>();
            return new PostConfigureEncryptedOptions<TOptions>(settings, reader, encryptionOptions, logger);
        });
    }

    /// <summary>
    /// Configures encrypted options for the default options instance.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being requested.</typeparam>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="memberAccessExpression">An expression that identifies the encrypted option.</param>
    /// <param name="pattern">An optional regular expression pattern to match against the encrypted string.</param>
    public static void ConfigureEncrypted<TOptions>(this IServiceCollection services,
        Expression<Func<TOptions, string>> memberAccessExpression, Regex pattern = null)
        where TOptions : class
        => services.ConfigureEncrypted(name: Options.DefaultName, memberAccessExpression, pattern);
}