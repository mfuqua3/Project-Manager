namespace ProjectManager.Core.Contracts;

public interface IPaginated
{
    public int Page { get; set; }

    public int PageSize { get; set; }
}