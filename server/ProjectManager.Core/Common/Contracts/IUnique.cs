using System;

namespace ProjectManager.Common.Contracts;

public interface IUnique<T> where T : IEquatable<T>
{
    public T Id { get; set; }
}