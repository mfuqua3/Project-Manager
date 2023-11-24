using System;
using ProjectManager.Common.Contracts;

namespace ProjectManager.Data.Entities;

public class Project : IUnique<Guid>, INamed, ISoftDelete, ITracked
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string NameNormalized { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? Deleted { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
}