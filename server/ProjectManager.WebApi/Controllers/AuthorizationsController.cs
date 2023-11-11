﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Core.Features.Authorization.Domain;
using ProjectManager.Core.Utility.Exceptions;
using ProjectManager.WebApi.Auth;
using IAuthorizationService = ProjectManager.Core.Features.Authorization.Services.IAuthorizationService;

namespace ProjectManager.WebApi.Controllers;

/// <summary>
/// Controller responsible for managing user authorizations.
/// </summary>
[ApiController]
public class AuthorizationsController : ApiController
{
    private const string RefreshCookieKey = "project-manager_auth";
    private readonly IAuthorizationService _service;
    /// <summary>
    /// Initializes an instance of the controller
    /// </summary>
    /// <param name="service"></param>
    public AuthorizationsController(IAuthorizationService service)
    {
        _service = service;
    }
    /// <summary>
    /// Authorizes a Google user and provides a token response.
    /// </summary>
    /// <param name="request">The token request details.</param>
    /// <returns>A new token response for authorized Google user.</returns>
    /// <response code="201">Returns the newly created token response for the user.</response>
    /// <exception cref="InvalidOperationException">Thrown if the user cannot be authorized through Google SSO.</exception>
    /// <exception cref="ProjectManagerDataNotFoundException">Thrown if the user does not exist in the system.</exception>
    /// <exception cref="ProjectManagerForbiddenAccessException">Thrown if the user is not whitelisted for project access.</exception>
    [HttpPost("google")]
    [AllowAnonymous]
    public async Task<ActionResult<TokenResponse>> AuthorizeGoogleUserAsync(TokenRequest request)
    {
        var response = await _service.AuthorizeGoogleSsoAsync(request);
        var cookieBuilder = new CookieBuilder
        {
            HttpOnly = true,
            Name = RefreshCookieKey,
            IsEssential = true,
            SecurePolicy = CookieSecurePolicy.None,
            SameSite = SameSiteMode.Lax,
            Expiration = TimeSpan.FromDays(7)
        };
        cookieBuilder.Extensions.Add(response.RefreshToken);
        var cookieOptions = cookieBuilder.Build(HttpContext);
        HttpContext.Response.Cookies.Append(RefreshCookieKey, response.RefreshToken, cookieOptions);
        return Created("", response);
    }
    /// <summary>
    /// Refreshes the authentication token for an already authenticated user.
    /// </summary>
    /// <returns>A new token response with refreshed credentials.</returns>
    /// <response code="201">Returns the refreshed token response for the user.</response>
    /// <exception cref="InvalidOperationException">Thrown if no refresh token is present or if the refresh token is invalid.</exception>
    /// <exception cref="ProjectManagerDataNotFoundException">Thrown if no user with the provided ID could be found.</exception>
    [HttpGet("refresh")]
    [Authorize(AuthenticationSchemes = ProjectManagerAuthenticationDefaults.RefreshScheme)]
    public async Task<ActionResult<TokenResponse>> RefreshAuthenticatedUserAsync()
    {
        var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        if (!HttpContext.Request.Cookies.TryGetValue(RefreshCookieKey, out var refreshToken))
        {
            throw new InvalidOperationException("No refresh token was present with the request");
        }

        TokenResponse response;
        try
        {
            response = await _service.RefreshAuthenticatedUserAsync(new RefreshRequest
            {
                UserId = userId,
                RefreshToken = refreshToken
            });
        }
        catch (Exception)
        {
            Response.Cookies.Delete(RefreshCookieKey);
            throw;
        }

        var cookieBuilder = new CookieBuilder
        {
            HttpOnly = true,
            Name = RefreshCookieKey,
            IsEssential = true,
            SecurePolicy = CookieSecurePolicy.SameAsRequest,
            Expiration = TimeSpan.FromDays(7)
        };
        cookieBuilder.Extensions.Add(response.RefreshToken);
        var cookieOptions = cookieBuilder.Build(HttpContext);
        HttpContext.Response.Cookies.Append(RefreshCookieKey, response.RefreshToken, cookieOptions);
        return Created("", response);
    }
    /// <summary>
    /// Signs out the current user.
    /// </summary>
    /// <returns>A response indicating that the user has been signed out.</returns>
    /// <response code="204">Indicates that the user was successfully signed out.</response>
    /// <exception cref="ProjectManagerDataNotFoundException">Thrown if no user with the provided ID could be found for sign out.</exception>
    [HttpGet("signout")]
    [AllowAnonymous]
    public async Task<IActionResult> SignoutAsync()
    {
        HttpContext.Response.Cookies.Delete(RefreshCookieKey);
        var result = await HttpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
        if (result.Succeeded)
        {
            var userId = result.Principal.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            await _service.SignOutUserAsync(new SignoutRequest { UserId = userId });
        }

        return NoContent();
    }
}