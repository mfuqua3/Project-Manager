using ProjectManager.Common.Contracts;

namespace ProjectManager.Common.Extensions;

public static class RefreshTokenExtensions
{
    public static bool ValidateRefreshAgainst(this IRefreshToken token, string toCheck)
    {
        return token.HasValidRefreshToken() &&
            string.Equals(toCheck, token.RefreshToken, StringComparison.Ordinal);
    }

    public static bool HasValidRefreshToken(this IRefreshToken token)
    {
        return !string.IsNullOrEmpty(token.RefreshToken) &&
               token.RefreshTokenExpiryTime > DateTime.UtcNow;
    }
}