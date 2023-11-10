namespace ProjectManager.Core.Utility.DependencyInjection.NamedServices;

public class NamedService<T>
{
    private readonly Func<T> _instanceFactory;

    public NamedService(string name, T instance) : this(name, () => instance)
    {
    }

    public NamedService(string name, Func<T> instanceFactory)
    {
        _instanceFactory = instanceFactory;
        Name = name;
    }

    public string Name { get; }
    public T Instance => _instanceFactory();
}