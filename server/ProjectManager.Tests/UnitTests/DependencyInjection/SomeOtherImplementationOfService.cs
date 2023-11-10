namespace ProjectManager.Tests.UnitTests.DependencyInjection;

public class SomeOtherImplementationOfService : ISomeService
{
    private readonly SomeDependency _dependency;

    public SomeOtherImplementationOfService(SomeDependency dependency)
    {
        _dependency = dependency;
    }

    public string StringProperty
    {
        get => _dependency.DependencyString;
        set => _dependency.DependencyString = value;
    }

    public void SomeMethod()
    {
        Console.WriteLine(StringProperty);
    }
}