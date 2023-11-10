namespace ProjectManager.Core.Utility.Configuration;

public static class ConfigurationUtility
{
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