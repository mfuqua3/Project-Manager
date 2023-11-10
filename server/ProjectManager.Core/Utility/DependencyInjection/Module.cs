using System.Collections;
using ProjectManager.Core.Utility.Configuration;

namespace ProjectManager.Core.Utility.DependencyInjection;

public abstract class Module<T> : Module, IModuleWithConfiguration where T : class, new()
{
    protected IConfiguration Configuration;
    public virtual bool RequireConfigurationFromAppSettings => false;

    public void ProvideConfiguration(IConfiguration configuration)
        => Configuration = configuration;

    public override void ConfigureServices(IServiceCollection services)
    {
        var options = GetOptions() ?? new T();
        ConfigureServices(services, options);
    }

    public T GetOptions()
    {
        if (Configuration == null)
        {
            throw new InvalidOperationException(
                $"Configuration is null. Must call {nameof(ProvideConfiguration)} before {nameof(GetOptions)}");
        }
        string configSectionName;
        try
        {
            configSectionName = ConfigurationUtility.ParseSectionName<T>();
        }
        catch (InvalidOperationException parseEx)
        {
            if (!RequireConfigurationFromAppSettings)
            {
                return null;
            }
            throw new ModuleUsageException($"Failed to parse configuration section name for {typeof(T).Name}. " +
                                           "Ensure the options class name follows the convention (e.g., 'Smtp' for 'SmtpOptions'). " +
                                           "Details: " + parseEx.Message, parseEx);
        }

        try
        {
            var configurationSection = Configuration.GetRequiredSection(configSectionName);
            return configurationSection.Get<T>(x => x.ErrorOnUnknownConfiguration = true);
        }
        catch (InvalidOperationException configEx)
        {
            if (!RequireConfigurationFromAppSettings)
            {
                return null;
            }
            throw new ModuleUsageException(
                $"Unable to initialize {typeof(Module<T>).Name} with section '{configSectionName}'. " +
                "Ensure the configuration section exists and matches the expected structure. " +
                "Module options must map to top-level configuration sections following a 'PropertyName' to 'PropertyNameOptions' naming convention. " +
                "Details: " + configEx.Message, configEx);
        }
    }

    public abstract void ConfigureServices(IServiceCollection services, T options);
}


public abstract class Module : IServiceCollection, IModule
{
    private IServiceCollection _inner;
    public void Initialize(IServiceCollection services) => _inner = services;
    public abstract void ConfigureServices(IServiceCollection services);

    public IEnumerator<ServiceDescriptor> GetEnumerator()
        => _inner.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => _inner.GetEnumerator();

    public void Add(ServiceDescriptor item)
        => _inner.Add(item);

    public void Clear()
        => _inner.Clear();

    public bool Contains(ServiceDescriptor item)
        => _inner.Contains(item);

    public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
        => _inner.CopyTo(array, arrayIndex);

    public bool Remove(ServiceDescriptor item)
        => _inner.Remove(item);

    public int Count => _inner.Count;
    public bool IsReadOnly => _inner.IsReadOnly;

    public int IndexOf(ServiceDescriptor item)
        => _inner.IndexOf(item);

    public void Insert(int index, ServiceDescriptor item)
        => _inner.Insert(index, item);

    public void RemoveAt(int index)
        => _inner.RemoveAt(index);

    public ServiceDescriptor this[int index]
    {
        get => _inner[index];
        set => _inner[index] = value;
    }
}