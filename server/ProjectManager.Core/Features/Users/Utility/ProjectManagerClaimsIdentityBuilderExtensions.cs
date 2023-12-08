using System.Security.Claims;
using Google.Apis.Auth;
using ProjectManager.Features.Users.Domain.Common;

namespace ProjectManager.Features.Users.Utility;

public static class ProjectManagerClaimsIdentityBuilderExtensions
{
    public static ProjectManagerClaimsIdentityBuilder AddNameClaim(this ProjectManagerClaimsIdentityBuilder builder,
        string name)
        => builder.SpecifyNameType(ProjectManagerClaimTypes.Name).AddClaim(ProjectManagerClaimTypes.Name, name);
    
    public static ProjectManagerClaimsIdentityBuilder AddRoleClaim(this ProjectManagerClaimsIdentityBuilder builder,
        string role)
        => builder.SpecifyRoleType(ProjectManagerClaimTypes.Role).AddClaim(ProjectManagerClaimTypes.Role, role);

    public static ProjectManagerClaimsIdentityBuilder AddEmailClaim(this ProjectManagerClaimsIdentityBuilder builder,
        string email)
        => builder.AddClaim(ProjectManagerClaimTypes.Email, email, ClaimValueTypes.Email);

    public static ProjectManagerClaimsIdentityBuilder AddSubjectClaim(this ProjectManagerClaimsIdentityBuilder builder,
        string subject)
        => builder.AddClaim(ProjectManagerClaimTypes.Id, subject);

    public static ClaimsIdentity BuildGoogleClaimsIdentity(this ProjectManagerClaimsIdentityBuilder builder,
        GoogleJsonWebSignature.Payload payload)
        => builder
            .SpecifyOriginalIssuer(payload.Issuer)
            .SpecifyAuthenticationType(ProjectManagerAuthenticationDefaults.GoogleAuthenticationScheme)
            .AddEmailClaim(payload.Email)
            .AddNameClaim(payload.Name)
            .AddClaim("google-id", payload.Subject)
            .Build();


}