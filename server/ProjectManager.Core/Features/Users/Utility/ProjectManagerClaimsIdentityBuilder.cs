using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using ProjectManager.Features.Users.Domain.Common;

namespace ProjectManager.Features.Users.Utility;

public class ProjectManagerClaimsIdentityBuilder
{
    private readonly string _issuer;
    private string _originalIssuer;
    private string _authenticationType;
    private string _nameClaimType = ProjectManagerClaimTypes.Name;
    private string _roleClaimType = ProjectManagerClaimTypes.Role;
    private readonly List<ClaimRecord> _claims = new();

    public ProjectManagerClaimsIdentityBuilder() : this(ProjectManagerAuthenticationDefaults.Issuer)
    {
    }

    public ProjectManagerClaimsIdentityBuilder(string issuer)
    {
        _issuer = issuer;
    }

    public ProjectManagerClaimsIdentityBuilder SpecifyNameType([NotNull] string nameType)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameType);
        _nameClaimType = nameType;
        return this;
    }

    public ProjectManagerClaimsIdentityBuilder SpecifyRoleType([NotNull] string roleType)
    {
        ArgumentException.ThrowIfNullOrEmpty(roleType);
        _roleClaimType = roleType;
        return this;
    }

    public ProjectManagerClaimsIdentityBuilder SpecifyOriginalIssuer([NotNull] string originalIssuer)
    {
        ArgumentException.ThrowIfNullOrEmpty(originalIssuer);
        _originalIssuer = originalIssuer;
        return this;
    }

    public ProjectManagerClaimsIdentityBuilder SpecifyAuthenticationType([NotNull] string authenticationType)
    {
        ArgumentException.ThrowIfNullOrEmpty(authenticationType);
        _authenticationType = authenticationType;
        return this;
    }

    public ProjectManagerClaimsIdentityBuilder AddClaim([NotNull] string name, [NotNull] string value,
        string valueType = ClaimValueTypes.String)
    {
        _claims.Add(new ClaimRecord(name, value, valueType));
        return this;
    }

    public ClaimsIdentity Build() =>
        new(_claims.Select(x => new Claim(x.Name, x.Value, x.ValueType, _issuer, _originalIssuer)),
            authenticationType: _authenticationType, nameType: _nameClaimType, roleType: _roleClaimType);

    private sealed record ClaimRecord(string Name, string Value, string ValueType);
}