using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ProjectManager.WebApi.Auth;

public static class ProjectManagerAuthenticationDefaults
{
    public const string DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    public const string RefreshScheme = "Refresh";
}