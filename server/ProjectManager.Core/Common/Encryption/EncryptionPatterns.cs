using System.Text.RegularExpressions;

namespace ProjectManager.Common.Encryption;

/// <summary>
/// Provides Regex Patterns for extracting encrypted fields within partially encrypted configuration strings
/// </summary>
public static partial class EncryptionPatterns
{
    /// <summary>
    /// Regular expression pattern for validating connection string passwords.
    /// </summary>
    public static readonly Regex ConnectionStringPassword = ConnectionStringPasswordRegex();

    /// <summary>
    /// Retrieves the regular expression pattern for extracting the password from a connection string. </summary>
    /// <returns>
    /// Returns a regular expression pattern to match and extract the password from a connection string.
    /// </returns>
    [GeneratedRegex("(?<=assword=)[^;]+")]
    private static partial Regex ConnectionStringPasswordRegex();
}
