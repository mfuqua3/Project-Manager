namespace ProjectManager.Common.Contracts;

public interface INamed
{
    public string Name { get; set; }
    public string NameNormalized { get; set; }
}