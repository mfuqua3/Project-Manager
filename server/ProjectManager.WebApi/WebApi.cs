using System.Reflection;

namespace ProjectManager.WebApi;

public static class WebApi
{
    public static readonly Assembly Assembly = typeof(WebApi).Assembly;
    public static readonly string AssemblyName = Assembly.GetName().Name;
    public const string HostAddress = @"https://projects.mattfuqua.dev";
}