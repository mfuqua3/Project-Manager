using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework;
using ProjectManager.Data.Entities;
using ProjectManager.Features.Users.Domain.Commands;
using ProjectManager.Tests.Infrastructure;
using ProjectManager.Tests.IntegrationTests.Data;
using ProjectManager.Tests.IntegrationTests.Stubs;
using ProjectManager.Tests.Utility.Extensions;
using Shouldly;

namespace ProjectManager.Tests.IntegrationTests.Features.Users;

[TestFixture]
public class OnPostGoogle : IntegrationTest
{
    private HttpResponseMessage _response;
    private UserManager<AppUser> _userManager;

    [OneTimeSetUp]
    public async Task ArrangeAndInvokeEndpointAsync()
    {
        var idTokenFactory = GetService<GoogleIdTokenFactory>();
        var idToken = idTokenFactory.GenerateFakeToken(TestUserToCreate.GenerateGoogleClaims());
        var client = CreateClient(withAuthorizations: false);
        _response = await client.PostAsJsonAsync("authorizations/google", new AuthenticateGoogleUserCommand { IdToken = idToken });
        _userManager = GetService<UserManager<AppUser>>();
    }
    [Test]
    public async Task ShouldSucceed()
    {
        await _response.ShouldBeSuccessAsync();
    }
    [Test]
    public async Task ShouldCreateUser()
    {
        var user = await _userManager.FindByEmailAsync(TestUserToCreate.Email);
        user.ShouldNotBeNull();
    }
    [Test]
    public async Task ShouldInitializeUserClaims()
    {
        var user = await _userManager.FindByEmailAsync(TestUserToCreate.Email);
        var claims = await _userManager.GetClaimsAsync(user!);
        claims.ShouldNotBeEmpty();
    }
}