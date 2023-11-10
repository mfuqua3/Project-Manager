using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;

namespace ProjectManager.Core.Utility.Encryption;

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