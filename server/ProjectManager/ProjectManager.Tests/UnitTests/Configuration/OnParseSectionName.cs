using NUnit.Framework;
using ProjectManager.Core.Utility.Configuration;
using Shouldly;

namespace ProjectManager.Tests.UnitTests.Configuration;

[TestFixture]
public class OnParseSectionName
{
    [Test]
    public void ReturnsSectionName_IfTypeFollowsConvention()
    {
        // Arrange
        var expectedSectionName = "Test";

        // Act
        var sectionName = ConfigurationUtility.ParseSectionName<TestOptions>();

        // Assert
        sectionName.ShouldBe(expectedSectionName);
    }

    [Test]
    public void ThrowsInvalidOperationException_IfTypeBreaksConvention()
    {
        // Act
        Should.Throw<InvalidOperationException>(ConfigurationUtility.ParseSectionName<SomeFeatureConfiguration>);
    }

    [Test]
    public void ThrowsInvalidOperationException_IfTypeIsInsufficientCharacters()
    {
        // Act
        Should.Throw<InvalidOperationException>(ConfigurationUtility.ParseSectionName<Options>);
    }

    private class TestOptions { }
    private class SomeFeatureConfiguration { }
    private class Options { }
}