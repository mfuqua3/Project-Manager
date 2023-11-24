using System;

namespace ProjectManager.Common.Contracts;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }

    public DateTime? Deleted { get; set; }
}