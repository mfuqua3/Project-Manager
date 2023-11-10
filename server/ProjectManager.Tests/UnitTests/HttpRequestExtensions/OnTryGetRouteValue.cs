using System.Collections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using NUnit.Framework;
using ProjectManager.Core.Utility.Extensions;
using Shouldly;

namespace ProjectManager.Tests.UnitTests.HttpRequestExtensions;

[TestFixture]
public class OnTryGetRouteValue
{
    private HttpRequest _request;

    private static IEnumerable<TestCaseData> CanGetTestCases
    {
        get
        {
            yield return new TestCaseData(new RouteValueDictionary { { "id", "123" } }).Returns(true);
            yield return new TestCaseData(new RouteValueDictionary { { "not id", "123" } }).Returns(false);
        }
    }

    private static IEnumerable<TestCaseData> ParsingTestCases
    {
        get
        {
            yield return new TestCaseData(new RouteValueDictionary { { "id", "123" } }).Returns(123);
            yield return new TestCaseData(new RouteValueDictionary { { "not id", "123" } }).Returns(default(int));
        }
    }

    private class SomeClassWithToStringOverride
    {
        public override string ToString()
            => "ToString was overridden";
    }

    private static IEnumerable ConvertibleTestCases
    {
        get
        {
            yield return new TestCaseData(true).Returns(true);
            yield return new TestCaseData('c').Returns('c');
            yield return new TestCaseData((sbyte)5).Returns((sbyte)5);
            yield return new TestCaseData((byte)120).Returns((byte)120);
            yield return new TestCaseData((short)1111).Returns((short)1111);
            yield return new TestCaseData((ushort)1231).Returns((ushort)1231);
            yield return new TestCaseData((int)-234325).Returns((int)-234325);
            yield return new TestCaseData((uint)12511).Returns((uint)12511);
            yield return new TestCaseData((long)-12312315122).Returns((long)-12312315122);
            yield return new TestCaseData((ulong)12321125125122).Returns((ulong)12321125125122);
            yield return new TestCaseData((float)1251213.12).Returns((float)1251213.12);
            yield return new TestCaseData((double)126161.12).Returns((double)126161.12);
            yield return new TestCaseData((decimal)1231161.19).Returns((decimal)1231161.19);
            yield return new TestCaseData(new DateTime(1991,1,26)).Returns(new DateTime(1991,1,26));
            yield return new TestCaseData("a string").Returns("a string");
        }
    }

    private static object[] _toStringValueCases =
        { "a string", 1000, Guid.NewGuid(), new SomeClassWithToStringOverride() };

    [SetUp]
    public void SetUpFixture()
    {
        _request = Substitute.For<HttpRequest>();
    }

    [TestCaseSource(nameof(ConvertibleTestCases))]
    public T SimplestGenericOverload_CanHandleConvertiblesByDefault<T>(T convertible)
    {
        const string key = "key";
        _request.RouteValues.Returns(new RouteValueDictionary { { key, convertible } });
        _request.TryGetRouteValue<T>(key, out var value);
        return value;
    }

    [Test]
    public void SimplestOverload_CanParseStrings([ValueSource(nameof(_toStringValueCases))] object routeValue)
    {
        const string key = "key";
        _request.RouteValues.Returns(new RouteValueDictionary { { key, routeValue } });
        _request.TryGetRouteValue(key, out var value);
        value.ShouldBe(routeValue.ToString());
    }

    [TestCaseSource(nameof(CanGetTestCases))]
    public bool WithValidInputParameters_ReturnsParsingSuccessResult(RouteValueDictionary routeValues)
    {
        // Arrange
        _request.RouteValues.Returns(new RouteValueDictionary(routeValues));

        // Act
        return _request.TryGetRouteValue("id", obj => int.Parse(obj?.ToString() ?? string.Empty), out var value);
    }

    [TestCaseSource(nameof(ParsingTestCases))]
    public int WithValidInputParameters_ReturnsParsedResult(RouteValueDictionary routeValues)
    {
        // Arrange
        _request.RouteValues.Returns(new RouteValueDictionary(routeValues));

        // Act
        _request.TryGetRouteValue("id", obj => int.Parse(obj?.ToString() ?? string.Empty), out var value);
        return value;
    }

    [Test]
    public void WithInvalidValueConverter_ThrowsArgumentException()
    {
        // Arrange
        _request.RouteValues.Returns(new RouteValueDictionary { { "id", "abc" } });

        // Act
        Should.Throw<ArgumentException>(() => _request.TryGetRouteValue("id",
            obj => int.Parse(obj?.ToString() ?? string.Empty), out _));
    }
}