using System.Collections.Generic;

namespace ProjectManager.Common.DependencyInjection.NamedServices;

public interface INamedServiceCollection<T> : IReadOnlyDictionary<string, T>
{
}