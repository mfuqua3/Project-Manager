using NSubstitute;
using NUnit.Framework;
using ProjectManager.Common.Contracts;
using ProjectManager.Common.Extensions;
using Shouldly;

namespace ProjectManager.Tests.UnitTests.Extensions
{
    [TestFixture]
    [TestOf(typeof(RefreshTokenExtensions))]
    public class RefreshTokenExtensionsTest
    {
        private IRefreshToken _tokenMock;

        [SetUp]
        public void Setup()
        {
            _tokenMock = Substitute.For<IRefreshToken>();
        }

        #region ValidateRefreshAgainst

        [Test]
        public void ValidateRefreshAgainst_GivenInvalidTokenAndCorrectString_ReturnsFalse()
        {
            string toCheck = "valid";

            _tokenMock.RefreshToken = "invalid";
            _tokenMock.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);

            bool result = _tokenMock.ValidateRefreshAgainst(toCheck);

            result.ShouldBeFalse();
        }

        [Test]
        public void ValidateRefreshAgainst_GivenValidTokenAndWrongString_ReturnsFalse()
        {
            string toCheck = "valid";

            _tokenMock.RefreshToken = "valid";
            _tokenMock.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(-1);

            bool result = _tokenMock.ValidateRefreshAgainst(toCheck);

            result.ShouldBeFalse();
        }

        [Test]
        public void ValidateRefreshAgainst_GivenValidTokenAndCorrectString_ReturnsTrue()
        {
            string toCheck = "valid";

            _tokenMock.RefreshToken = "valid";
            _tokenMock.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);

            bool result = _tokenMock.ValidateRefreshAgainst(toCheck);

            result.ShouldBeTrue();
        }

        #endregion

        #region HasValidRefreshToken

        [Test]
        public void HasValidRefreshToken_GivenEmptyRefreshToken_ReturnsFalse()
        {
            _tokenMock.RefreshToken = string.Empty;
            _tokenMock.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);

            bool result = _tokenMock.HasValidRefreshToken();

            result.ShouldBeFalse();
        }

        [Test]
        public void HasValidRefreshToken_GivenPastExpiry_ReturnsFalse()
        {
            _tokenMock.RefreshToken = "valid";
            _tokenMock.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(-1);

            bool result = _tokenMock.HasValidRefreshToken();

            result.ShouldBeFalse();
        }

        [Test]
        public void HasValidRefreshToken_GivenFutureExpiry_ReturnsTrue()
        {
            _tokenMock.RefreshToken = "valid";
            _tokenMock.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);

            bool result = _tokenMock.HasValidRefreshToken();

            result.ShouldBeTrue();
        }

        #endregion
    }
}