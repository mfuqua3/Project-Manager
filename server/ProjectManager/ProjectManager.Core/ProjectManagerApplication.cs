using System.Reflection;

namespace ProjectManager.Core;

public static class ProjectManagerApplication
{
    public static readonly Assembly Assembly = typeof(ProjectManagerApplication).Assembly;
    public static readonly string AssemblyName = Assembly.GetName().Name;
}