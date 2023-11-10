namespace ProjectManager.Core.Utility.DependencyInjection.NamedServices;

public interface INamedServiceCollection<T> : IReadOnlyDictionary<string, T>
{
}