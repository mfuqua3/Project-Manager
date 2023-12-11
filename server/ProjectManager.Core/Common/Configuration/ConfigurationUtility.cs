using System;

namespace ProjectManager.Common.Configuration;

/// <summary>
/// Utility class for configuration related operations.
/// </summary>
public static class ConfigurationUtility
{
    /// <summary>
    /// Parses the section name for a given options type.
    /// </summary>
    /// <typeparam name="TOptions">The type of the options.</typeparam>
    /// <returns>The parsed section name.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the options type does not conform to the naming convention of ending with 'Options' suffix,
    /// or if the section name cannot be parsed.
    /// </exception>
    public static string ParseSectionName<TOptions>()
    {
        const string suffix = "Options";
        var optionsName = typeof(TOptions).Name;
        if (!optionsName.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Invalid use of BindOptions. Can not bind by convention because " +
                                                $"{typeof(TOptions)} does not conform to the convention. Options types " +
                                                $"should end with 'Options' suffix.");
        }

        var optionsCharacterCount = suffix.Length;
        var configSection = optionsName.Remove(optionsName.Length - optionsCharacterCount, optionsCharacterCount);
        if (string.IsNullOrWhiteSpace(configSection))
        {
            throw new InvalidOperationException("Unable to parse config section. Name is insufficient characters.");
        }

        return configSection;
    }
}