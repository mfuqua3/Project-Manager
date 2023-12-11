using System;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ProjectManager.Common.Encryption;

/// <summary>
/// This class is responsible for post-configuring encrypted options.
/// It implements the <see cref="IPostConfigureOptions{TOptions}"/> interface.
/// </summary>
/// <typeparam name="TOptions">The type of options to post-configure.</typeparam>
public class PostConfigureEncryptedOptions<TOptions> : IPostConfigureOptions<TOptions> where TOptions : class
{
    private readonly TypedEncryptedOptionsSettings _settings;

    private readonly IEncryptionReader _encryptionReader;

    private readonly IOptions<EncryptionOptions> _encryptionOptions;

    private readonly ILogger<PostConfigureEncryptedOptions<TOptions>> _logger;

    public PostConfigureEncryptedOptions(TypedEncryptedOptionsSettings settings, IEncryptionReader encryptionReader,
        IOptions<EncryptionOptions> encryptionOptions, ILogger<PostConfigureEncryptedOptions<TOptions>> logger)
    {
        _settings = settings;
        _encryptionReader = encryptionReader;
        _encryptionOptions = encryptionOptions;
        _logger = logger;
    }

    /// <summary>
    /// This method is responsible for post-configuration logic. It performs decryption of the encrypted property value
    /// and sets the decrypted value to the specified options object.
    /// </summary>
    /// <typeparam name="TOptions">The type of options object.</typeparam>
    /// <param name="name">The name of the specific configuration. Only perform post-configuration logic when the specified name matches the configured name.</param>
    /// <param name="options">The options object to be post-configured.</param>
    /// <exception cref="EncryptionException">
    /// Thrown when there is an error decrypting the value or no value was provided for decryption.
    /// </exception>
    /// <remarks>
    /// This method checks if the specified name matches the configured name for the instance.
    /// If encryption is disabled, a warning log will be emitted.
    /// It retrieves the value of the specified property from the options object and decrypts it using the configured encryption reader.
    /// The decrypted value is then set back to the property of the options object.
    /// Any exceptions during decryption or setting the decrypted value will result in an EncryptionException being thrown.
    /// </remarks>
    public void PostConfigure(string name, TOptions options)
    {
        if (_settings.Name != null && !string.Equals(name, _settings.Name))
        {
            return;
        }
        if (!_encryptionOptions.Value.Enabled)
        {
            _logger.LogWarning(
                "{Options} has been configured with an encrypted property but the value has not been decrypted " +
                "as encryption has been disabled by configuration", typeof(TOptions).Name);
            return;
        }

        var pi = _settings.PropertyInfo;
        var value = pi.GetValue(options)?.ToString() ??
                    throw new EncryptionException(
                        $"Unable to decrypt value of {pi.Name} as no value was provided.");

        try
        {
            var decrypted = _settings.Pattern == null
                ? _encryptionReader.GetEncryptedSetting(value)
                : GetEncryptedSettingByPattern(value, _settings.Pattern);

            pi.SetValue(options, decrypted);
        }
        catch (Exception ex)
        {
            throw new EncryptionException(
                $"Failed to decrypt property '{pi.Name}' of type '{pi.DeclaringType?.Name ?? "Unknown"}',", ex);
        }
    }

    private string GetEncryptedSettingByPattern(string input, Regex pattern)
    {
        var match = pattern.Match(input);
        if (!match.Success)
        {
            throw new EncryptionException(
                @$"Unable to decrypt a field using the provided pattern, as no matches were found.
                Configuration Type => {typeof(TOptions).Name}
                Property Name => {_settings.PropertyInfo.Name}
                Provided Value => {input}
                Provided Pattern => {pattern}");
        }

        var toDecrypt = match.Value;
        var decrypted = _encryptionReader.GetEncryptedSetting(toDecrypt);
        return pattern.Replace(input, decrypted);
    }
}