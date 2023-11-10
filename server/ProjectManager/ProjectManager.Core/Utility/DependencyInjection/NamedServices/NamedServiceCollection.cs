namespace ProjectManager.Core.Utility.DependencyInjection.NamedServices;

internal class NamedServiceCollection<T> : Dictionary<string, T>, INamedServiceCollection<T>
{
    public NamedServiceCollection(IEnumerable<NamedService<T>> services) :
        base(services.ToDictionary(x => x.Name, x => x.Instance))
    {
    }

    public static INamedServiceCollection<T> Empty => 
        new NamedServiceCollection<T>(Array.Empty<NamedService<T>>());
}