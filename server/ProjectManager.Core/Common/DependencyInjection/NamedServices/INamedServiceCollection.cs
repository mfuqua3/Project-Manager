namespace ProjectManager.Common.DependencyInjection.NamedServices;

public interface INamedServiceCollection<T> : IReadOnlyDictionary<string, T>
{
}