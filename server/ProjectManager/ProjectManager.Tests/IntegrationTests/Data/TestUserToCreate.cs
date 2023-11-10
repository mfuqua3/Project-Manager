using System.Security.Claims;
using Bogus;

namespace ProjectManager.Tests.IntegrationTests.Data;

public class TestUserToCreate
{
    public const string Email = "ValidButNotInDatabase@gmail.com";

    public static ClaimsIdentity GenerateGoogleClaims()
    {
        var faker = new Faker();
        var firstName = faker.Name.FirstName();
        var lastName = faker.Name.LastName();
        return new ClaimsIdentity(new[]
        {
            new Claim("sub", faker.Random.ReplaceNumbers("#####################")),
            new Claim("email", Email),
            new Claim("email_verified", bool.TrueString, nameof(Boolean)),
            new Claim("name", $"{firstName} {lastName}"),
            new Claim("picture", $"https://lh3.googleusercontent.com/a/{faker.Random.AlphaNumeric(22)}=s96-c"),
            new Claim("given_name", firstName),
            new Claim("last_name", lastName)
        });
    }
}