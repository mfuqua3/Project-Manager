using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;
using ProjectManager.Common.Exceptions;
using ProjectManager.Features.Users.Abstractions;
using ProjectManager.Features.Users.Domain.Common;
using ProjectManager.Features.Users.Utility;

namespace ProjectManager.Features.Users.Engines;

class GoogleAuthenticationEngine : IGoogleAuthenticationEngine
{
    private readonly GoogleOptions _configuration;

    public GoogleAuthenticationEngine(IOptions<GoogleOptions> options)
    {
        _configuration = options.Value;
    }
    public async Task<GoogleLoginDto> AuthenticateGoogleSsoAsync(string idToken)
    {
        GoogleJsonWebSignature.Payload payload;
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                // Replace with your app's client ID
                Audience = new List<string> { $"{_configuration.ClientId}.apps.googleusercontent.com" }
            };
            payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
        }
        catch (Exception ex)
        {
            throw new AuthenticationException(
                "Google SSO credentials could not be validated. See inner exception for details", ex);
        }
        
        if (!_configuration.ValidEmails.Contains(payload.Email))
        {
            throw new ProjectManagerForbiddenAccessException("User is not authorized.");
        }

        var claimsIdentity = new ProjectManagerClaimsIdentityBuilder()
            .BuildGoogleClaimsIdentity(payload);
        return new GoogleLoginDto(payload.Subject, claimsIdentity.Claims);
    }
}