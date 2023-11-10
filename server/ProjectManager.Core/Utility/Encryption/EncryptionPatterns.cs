using System.Text.RegularExpressions;

namespace ProjectManager.Core.Utility.Encryption;

/// <summary>
/// Provides Regex Patterns for extracting encrypted fields within partially encrypted configuration strings
/// </summary>
public static partial class EncryptionPatterns
{
    public static readonly Regex ConnectionStringPassword = ConnectionStringPasswordRegex();

    [GeneratedRegex("(?<=assword=)[^;]+")]
    private static partial Regex ConnectionStringPasswordRegex();
}
