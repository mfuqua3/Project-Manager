namespace ProjectManager.Tests.UnitTests.DependencyInjection;

public class SomeImplementationOfService : ISomeService
{
    public string StringProperty { get; set; }

    public void SomeMethod()
    {
        Console.WriteLine(@"I write to console or something");
    }
}