using System;
using System.Reflection;


namespace ProjectManager;

public static class ProjectManagerApplication
{
    public static readonly Assembly Assembly = typeof(ProjectManagerApplication).Assembly;
    public static readonly string AssemblyName = Assembly.GetName().Name;
    public static readonly Version Version = Assembly.GetName().Version;
    public static readonly string ProductName = Assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
}