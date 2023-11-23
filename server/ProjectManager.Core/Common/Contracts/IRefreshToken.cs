namespace ProjectManager.Common.Contracts;

public interface IRefreshToken
{
    string RefreshToken { get; set; }
    DateTime RefreshTokenExpiryTime { get; set; }
}