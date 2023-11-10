using System.Collections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NUnit.Framework;
using ProjectManager.Core.Utility.DependencyInjection.NamedServices;
using Shouldly;

namespace ProjectManager.Tests.UnitTests.DependencyInjection;

[TestFixture]
public class OnAddNamedService
{
    private ServiceCollection _services;
    private const string SomeNamedService = "Some Named Service";
    private const string SomeOtherNamedService = "Some Other Named Service";

    private static IEnumerable ServiceLifetimes => new[]
    {
        ServiceLifetime.Transient,
        ServiceLifetime.Scoped,
        ServiceLifetime.Singleton
    };

    [SetUp]
    public void CreateServices()
    {
        _services = new ServiceCollection();
    }

    [Test]
    public void Can_Register_Scoped_Type()
    {
        _services.AddScoped<ISomeService, SomeImplementationOfService>(SomeNamedService);
        var registration = _services.Single(x => x.ServiceType == typeof(NamedService<ISomeService>));
        registration.Lifetime.ShouldBe(ServiceLifetime.Scoped);
    }
    
    [Test]
    public void Can_Register_Scoped__Factory_Without_Interface()
    {
        _services.AddScoped(SomeNamedService,
            _ => new SomeImplementationOfService { StringProperty = "Some property" });
        var registration = _services.Single(x => x.ServiceType == typeof(NamedService<SomeImplementationOfService>));
        registration.Lifetime.ShouldBe(ServiceLifetime.Scoped);
    }
    [Test]
    public void Can_Register_Scoped_Factory()
    {
        _services.AddScoped<ISomeService, SomeImplementationOfService>(SomeNamedService,
            (_) => new SomeImplementationOfService { StringProperty = "Some property" });
        var registration = _services.Single(x => x.ServiceType == typeof(NamedService<ISomeService>));
        registration.Lifetime.ShouldBe(ServiceLifetime.Scoped);
    }

    [Test]
    public void Can_Register_Transient_Type()
    {
        _services.AddTransient<ISomeService, SomeImplementationOfService>(SomeNamedService);
        var registration = _services.Single(x => x.ServiceType == typeof(NamedService<ISomeService>));
        registration.Lifetime.ShouldBe(ServiceLifetime.Transient);
    }
    
    [Test]
    public void Can_Register_Transient__Factory_Without_Interface()
    {
        _services.AddTransient(SomeNamedService,
            _ => new SomeImplementationOfService { StringProperty = "Some property" });
        var registration = _services.Single(x => x.ServiceType == typeof(NamedService<SomeImplementationOfService>));
        registration.Lifetime.ShouldBe(ServiceLifetime.Transient);
    }
    [Test]
    public void Can_Register_Transient_Factory()
    {
        _services.AddTransient<ISomeService, SomeImplementationOfService>(SomeNamedService,
            (_) => new SomeImplementationOfService { StringProperty = "Some property" });
        var registration = _services.Single(x => x.ServiceType == typeof(NamedService<ISomeService>));
        registration.Lifetime.ShouldBe(ServiceLifetime.Transient);
    }

    [Test]
    public void Can_Register_Singleton_Type()
    {
        _services.AddSingleton<ISomeService, SomeImplementationOfService>(SomeNamedService);
        var registration = _services.Single(x => x.ServiceType == typeof(NamedService<ISomeService>));
        registration.Lifetime.ShouldBe(ServiceLifetime.Singleton);
    }

    [Test]
    public void Can_Register_Singleton_Instances()
    {
        _services.AddSingleton(SomeNamedService,
            new SomeImplementationOfService { StringProperty = "Some property" });
        var registration = _services.Single(x => x.ServiceType == typeof(NamedService<SomeImplementationOfService>));
        registration.Lifetime.ShouldBe(ServiceLifetime.Singleton);
    }

    [Test]
    public void Can_Register_Singleton__Factory_Without_Interface()
    {
        _services.AddSingleton(SomeNamedService,
        _ => new SomeImplementationOfService { StringProperty = "Some property" });
        var registration = _services.Single(x => x.ServiceType == typeof(NamedService<SomeImplementationOfService>));
        registration.Lifetime.ShouldBe(ServiceLifetime.Singleton);
    }
    
    [Test]
    public void Can_Register_Singleton_Factory()
    {
        _services.AddSingleton<ISomeService, SomeImplementationOfService>(SomeNamedService,
            (_) => new SomeImplementationOfService { StringProperty = "Some property" });
        var registration = _services.Single(x => x.ServiceType == typeof(NamedService<ISomeService>));
        registration.Lifetime.ShouldBe(ServiceLifetime.Singleton);
    }

    [Test]
    public void Throws_On_Name_Duplicate([ValueSource(nameof(ServiceLifetimes))] ServiceLifetime serviceLifetime)
    {
        _services.Add<ISomeService, SomeImplementationOfService>(SomeNamedService, serviceLifetime);
        Should.Throw<Exception>(() =>
            _services.Add<ISomeService, SomeOtherImplementationOfService>(SomeNamedService, serviceLifetime));
    }
    
    [Test]
    public void Throws_On_Name_Duplicate_For_No_Interface([ValueSource(nameof(ServiceLifetimes))] ServiceLifetime serviceLifetime)
    {
        _services.Add<SomeImplementationOfService>(SomeNamedService, serviceLifetime);
        Should.Throw<Exception>(() =>
            _services.Add<SomeImplementationOfService>(SomeNamedService, serviceLifetime));
    }
    [Test]
    public void Throws_If_Service_And_Implementation_Type_Are_Same(
        [ValueSource(nameof(ServiceLifetimes))] ServiceLifetime serviceLifetime)
    {
        Should.Throw<Exception>(() =>
            _services.Add<SomeImplementationOfService, SomeImplementationOfService>(SomeNamedService, serviceLifetime));
    }

    [Test]
    public void Can_Register_Multiple([ValueSource(nameof(ServiceLifetimes))] ServiceLifetime serviceLifetime)
    {
        _services.Add<ISomeService, SomeImplementationOfService>(SomeNamedService, serviceLifetime);
        Should.NotThrow(() =>
            _services.Add<ISomeService, SomeOtherImplementationOfService>(SomeOtherNamedService, serviceLifetime));
    }
    [Test]
    public void Can_Register_Multiple_Instances()
    {
        _services.AddSingleton(SomeNamedService, new SomeImplementationOfService{StringProperty = "First"});
        Should.NotThrow(() =>
            _services.AddSingleton(SomeOtherNamedService, new SomeImplementationOfService{StringProperty = "Second"}));
    }
    [Test]
    public void Can_Register_Multiple_For_No_Interface([ValueSource(nameof(ServiceLifetimes))] ServiceLifetime serviceLifetime)
    {
        _services.Add<SomeImplementationOfService>(SomeNamedService, serviceLifetime);
        Should.NotThrow(() =>
            _services.Add<SomeImplementationOfService>(SomeOtherNamedService, serviceLifetime));
    }
    [Test]
    public void Registers_Named_Service_Without_Interface([ValueSource(nameof(ServiceLifetimes))] ServiceLifetime serviceLifetime)
    {
        _services.Add<SomeImplementationOfService>(SomeNamedService, serviceLifetime);
        var registration = _services.SingleOrDefault(x => x.ServiceType == typeof(NamedService<SomeImplementationOfService>));
        registration.ShouldSatisfyAllConditions(
            x => x.ShouldNotBeNull(),
            x => x.ShouldBeAssignableTo<NamedServiceDescriptor>(),
            x => (x as NamedServiceDescriptor)?.Name.ShouldBe(SomeNamedService),
            x => x.ServiceType.ShouldBe(typeof(NamedService<SomeImplementationOfService>)),
            x => x.Lifetime.ShouldBe(serviceLifetime)
        );
    }
    [Test]
    public void Registers_Named_Service([ValueSource(nameof(ServiceLifetimes))] ServiceLifetime serviceLifetime)
    {
        _services.Add<ISomeService, SomeImplementationOfService>(SomeNamedService, serviceLifetime);
        var registration = _services.SingleOrDefault(x => x.ServiceType == typeof(NamedService<ISomeService>));
        registration.ShouldSatisfyAllConditions(
            x => x.ShouldNotBeNull(),
            x => x.ShouldBeAssignableTo<NamedServiceDescriptor>(),
            x => (x as NamedServiceDescriptor)?.Name.ShouldBe(SomeNamedService),
            x => x.ServiceType.ShouldBe(typeof(NamedService<ISomeService>)),
            x => x.Lifetime.ShouldBe(serviceLifetime)
        );
    }

    [Test]
    public void Registers_Named_Service_Collection(
        [ValueSource(nameof(ServiceLifetimes))]
        ServiceLifetime serviceLifetime)
    {
        _services.Add<ISomeService, SomeImplementationOfService>(SomeNamedService, serviceLifetime);
        var collectionRegistration =
            _services.SingleOrDefault(x => x.ServiceType == typeof(INamedServiceCollection<ISomeService>));
        collectionRegistration.ShouldNotBeNull();
    }
    [Test]
    public void Registers_Named_Service_Collection_For_No_Interface(
        [ValueSource(nameof(ServiceLifetimes))]
        ServiceLifetime serviceLifetime)
    {
        _services.Add<SomeImplementationOfService>(SomeNamedService, serviceLifetime);
        var collectionRegistration =
            _services.SingleOrDefault(x => x.ServiceType == typeof(INamedServiceCollection<SomeImplementationOfService>));
        collectionRegistration.ShouldNotBeNull();
    }
}