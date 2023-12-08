using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NUnit.Framework;
using ProjectManager.Common.Extensions;
using ProjectManager.Data.Entities;
using Shouldly;

namespace ProjectManager.Tests.UnitTests.Extensions
{
    [TestFixture]
    [TestOf(typeof(UserManagerExtensions))]
    public class UserManagerExtensionsTest
    {
        private UserManager<AppUser> _mockUserManager;
        private AppUser _testUser;

        [SetUp]
        public void SetUp()
        {
            var userStore = Substitute.For<IUserStore<AppUser>>();
            _mockUserManager = Substitute.For<UserManager<AppUser>>(userStore, null, null, null, null, null, null, null, null);

            _testUser = new AppUser
            {
                RefreshToken = "oldToken",
                RefreshTokenExpiryTime = DateTime.Now.AddDays(-1),
                Name = "TestUser",
                NameNormalized = "TESTUSER"
            };
        }

        [Test]
        public async Task GenerateRefreshTokenAsync_ShouldRefreshTokenAndExpiry()
        {
            // Call the method in test
            var newToken = await _mockUserManager.GenerateRefreshTokenAsync(_testUser);

            // Check the returned token is not null
            newToken.ShouldNotBeNull();

            // Validate the new token is different from the old one
            newToken.ShouldNotBe("oldToken");

            // Validate the expiry time is extended
            _testUser.RefreshTokenExpiryTime.ShouldBeGreaterThan(DateTime.UtcNow);
        }

        [Test]
        public async Task RevokeRefreshTokenAsync_ShouldNullifyTokenAndNotChangeExpiry()
        {
            // Save the old expiry time
            var oldExpiry = _testUser.RefreshTokenExpiryTime;

            // Call the method in test
            await _mockUserManager.RevokeRefreshTokenAsync(_testUser);

            // Check the refresh token is null
            _testUser.RefreshToken.ShouldBeNull();

            // Validate the expiry time is not changed
            _testUser.RefreshTokenExpiryTime.ShouldBe(oldExpiry);
        }
    }
}