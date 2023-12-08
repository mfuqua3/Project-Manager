using System;
using ProjectManager.Common.Contracts;

namespace ProjectManager.Common.Extensions
{
    /// <summary>
    /// A static class that provides extension methods for IRefreshToken.
    /// </summary>
    public static class RefreshTokenExtensions
    {
        /// <summary>
        /// Validates the refresh token against the provided string.
        /// </summary>
        /// <param name="token">The IRefreshToken instance.</param>
        /// <param name="toCheck">The string to compare to the IRefreshToken's RefreshToken.</param>
        /// <returns>True if the provided IRefreshToken has a valid RefreshToken that matches the provided string; False otherwise.</returns>
        public static bool ValidateRefreshAgainst(this IRefreshToken token, string toCheck)
        {
            return token.HasValidRefreshToken() &&
                   string.Equals(toCheck, token.RefreshToken, StringComparison.Ordinal);
        }

        /// <summary>
        /// Determines whether the IRefreshToken has a valid refresh token.
        /// </summary>
        /// <param name="token">The IRefreshToken instance.</param>
        /// <returns>True if the IRefreshToken's RefreshToken is not empty, and its RefreshTokenExpiryTime is in the future; False otherwise.</returns>
        public static bool HasValidRefreshToken(this IRefreshToken token)
        {
            return !string.IsNullOrEmpty(token.RefreshToken) &&
                   token.RefreshTokenExpiryTime > DateTime.UtcNow;
        }
    }
}