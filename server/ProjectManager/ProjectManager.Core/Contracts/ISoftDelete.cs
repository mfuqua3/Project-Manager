namespace ProjectManager.Core.Contracts;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }

    public DateTime? Deleted { get; set; }
}