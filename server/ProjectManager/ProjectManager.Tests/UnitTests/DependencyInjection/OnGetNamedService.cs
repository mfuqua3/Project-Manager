using System.Collections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NUnit.Framework;
using ProjectManager.Core.Utility.DependencyInjection.NamedServices;
using Shouldly;

namespace ProjectManager.Tests.UnitTests.DependencyInjection;

[TestFixture]
public class OnGetNamedService
{
    private ServiceProvider _provider;
    private StringPropertyProvider _stringPropertyProvider;
    private const string ProvidedString = "This string is provided by a dependency";
    private const string SomeNamedService = "Some Named Service";
    private const string SomeOtherNamedService = "Some Other Named Service";
    private const string SomeNamedInstance = "Some Named Service";
    private const string SomeOtherNamedInstance = "Some Other Named Service";
    private const string InstanceStringProperty = "Instantiated String Property";
    private const string DoesntExistNamedService = "This Named Service Doesnt Exist";

    private class StringPropertyProvider
    {
        private readonly string _propertyValue = ProvidedString;

        public string GetPropertyValue() => _propertyValue;
    }
    [SetUp]
    public void BuildProvider()
    {
        var services = new ServiceCollection();
        _stringPropertyProvider = new StringPropertyProvider();
        services
            .AddSingleton(_stringPropertyProvider)
            .AddSingleton(new SomeDependency {DependencyString = InstanceStringProperty})
            .AddTransient<ISomeService, SomeImplementationOfService>(SomeNamedService, (provider) =>
            {
                var stringPropertyProvider = provider.GetRequiredService<StringPropertyProvider>();
                return new SomeImplementationOfService { StringProperty = stringPropertyProvider.GetPropertyValue() };
            })
            .AddTransient<ISomeService, SomeOtherImplementationOfService>(SomeOtherNamedService)
            .AddSingleton(SomeNamedInstance, (provider) =>
            {
                var stringPropertyProvider = provider.GetRequiredService<StringPropertyProvider>();
                return new SomeImplementationOfService { StringProperty = stringPropertyProvider.GetPropertyValue() };
            })
            .AddSingleton(SomeOtherNamedInstance, new SomeImplementationOfService{StringProperty = InstanceStringProperty});
        _provider = services.BuildServiceProvider();
    }

    private static IEnumerable ResolutionTypeTestCases
    {
        get
        {
            yield return new TestCaseData(SomeNamedService).Returns(typeof(SomeImplementationOfService));
            yield return new TestCaseData(SomeOtherNamedService).Returns(typeof(SomeOtherImplementationOfService));
        }
    }
    private static IEnumerable ResolutionValueTestCases
    {
        get
        {
            yield return new TestCaseData(SomeNamedService).Returns(ProvidedString);
            yield return new TestCaseData(SomeOtherNamedService).Returns(InstanceStringProperty);
        }
    }

    [TestCaseSource(nameof(ResolutionTypeTestCases))]
    public Type CanResolveNamedService(string serviceName) =>
        _provider.GetService<ISomeService>(serviceName).GetType();

    [TestCaseSource(nameof(ResolutionValueTestCases))]
    public string CanResolveExpectedService_ForInterfaceRegistration(string serviceName)
        => _provider.GetService<ISomeService>(serviceName).StringProperty;
    [TestCaseSource(nameof(ResolutionValueTestCases))]
    public string CanResolveExpectedService_ForClassRegistration(string serviceName)
        => _provider.GetService<SomeImplementationOfService>(serviceName).StringProperty;

    [Test]
    public void CanResolveNamedServiceCollection()
    {
        var serviceCollection = _provider.GetService<INamedServiceCollection<ISomeService>>();
        serviceCollection.ShouldSatisfyAllConditions(
            x => x.ShouldNotBeNull(),
            x => x.ContainsKey(SomeNamedService).ShouldBeTrue(),
            x => x.ContainsKey(SomeOtherNamedService).ShouldBeTrue(),
            x => x[SomeNamedService].ShouldBeOfType<SomeImplementationOfService>(),
            x => x[SomeOtherNamedService].ShouldBeOfType<SomeOtherImplementationOfService>());
    }



    [Test]
    public void GetService_ReturnsNull_ForNoRegistration()
    {
        _provider.GetService<ISomeService>(DoesntExistNamedService).ShouldBeNull();
    }
    [Test]
    public void GetRequiredService_Throws_ForNoRegistration()
    {
        Should.Throw<InvalidOperationException>(() =>
            _provider.GetRequiredService<ISomeService>(DoesntExistNamedService));
    }
}
