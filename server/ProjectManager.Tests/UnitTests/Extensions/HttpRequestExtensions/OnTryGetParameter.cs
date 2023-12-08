using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using NUnit.Framework;
using ProjectManager.Common.Extensions;
using Shouldly;

namespace ProjectManager.Tests.UnitTests.Extensions.HttpRequestExtensions;

[TestFixture]
public class OnTryGetParameter
{
    private HttpRequest _request;
    private const string Key = "id";

    private readonly Dictionary<string, StringValues> _queryValues = new()
    {
        { Key, "QueryValue" },
    };

    private readonly Dictionary<string, object> _routeValues = new()
    {
        { Key, "RouteValue" }
    };

    [SetUp]
    public void SetUpFixture()
    {
        _request = Substitute.For<HttpRequest>();
    }

    [Test]
    public void ReturnsTrue_IfQueryExists()
    {
        _request.Query.Returns(new QueryCollection(_queryValues));
        _request.RouteValues.Returns(new RouteValueDictionary());
        _request.TryGetParameter(Key, out _).ShouldBe(true);
    }
    [Test]
    public void ReturnsTrue_IfRouteExists()
    {
        _request.Query.Returns(new QueryCollection());
        _request.RouteValues.Returns(new RouteValueDictionary(_routeValues.ToArray()));
        _request.TryGetParameter(Key, out _).ShouldBe(true);
    }
    [Test]
    public void ReturnsTrue_IfQueryAndRouteExists()
    {
        _request.Query.Returns(new QueryCollection(_queryValues));
        _request.RouteValues.Returns(new RouteValueDictionary(_routeValues.ToArray()));
        _request.TryGetParameter(Key, out _).ShouldBe(true);
    }
    [Test]
    public void ReturnsFalse_IfQueryAndRouteBothDoNotExist()
    {
        _request.Query.Returns(new QueryCollection());
        _request.RouteValues.Returns(new RouteValueDictionary());
        _request.TryGetParameter(Key, out _).ShouldBe(false);
    }
    [Test]
    public void ReturnsQueryParameter()
    {
        _request.Query.Returns(new QueryCollection(_queryValues));
        _request.RouteValues.Returns(new RouteValueDictionary());
        _request.TryGetParameter(Key, out var value);
        value.ShouldBe(_queryValues[Key]);
    }

    [Test]
    public void ReturnsRouteParameter()
    {
        _request.Query.Returns(new QueryCollection());
        _request.RouteValues.Returns(new RouteValueDictionary(_routeValues));
        _request.TryGetParameter(Key, out var value);
        value.ShouldBe(_routeValues[Key]);
    }

    [Test]
    public void FavorsQueryParameter_IfRouteParameterIsAlsoDefined()
    {
        _request.Query.Returns(new QueryCollection(_queryValues));
        _request.RouteValues.Returns(new RouteValueDictionary(_routeValues.ToArray()));
        _request.TryGetParameter(Key, out var value);
        value.ShouldBe(_queryValues[Key]);
    }

    [Test]
    public void ValueIsDefault_IfQueryAndRouteBothDoNotExist()
    {
        _request.Query.Returns(new QueryCollection());
        _request.RouteValues.Returns(new RouteValueDictionary());
        _request.TryGetParameter(Key, out var value);
        value.ShouldBe(default);
    }
}