namespace ProjectManager.Core.Contracts;

public interface IUnique<T> where T : IEquatable<T>
{
    public T Id { get; set; }
}