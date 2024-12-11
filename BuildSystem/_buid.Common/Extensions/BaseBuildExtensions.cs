using System.Linq.Expressions;

namespace _buid.Common.Extensions;

internal static class BaseBuildExtensions
{
    public static T GetValue<T>(this IBaseBuild build, Expression<Func<T>> parameterExpression)
        where T : class
        => build.TryGetValue(parameterExpression)
           ?? throw new InvalidOperationException($"Cannot get value for {parameterExpression}");

    public static ApplicationVersion ResolveVersion(this IBaseBuild build)
    {
        var major = build.Major;
        var minor = build.Minor;
        var patch = build.BuildCounter;
        var preReleaseSuffix = build.FormatPreReleaseSuffix();

        return new(
            AssemblyVersion: $"{major}.{minor}",
            Version: $"{major}.{minor}.{patch}{preReleaseSuffix}",
            FileVersion: $"{major}.{minor}.{patch}",
            InformationalVersion: $"{major}.{minor}.{patch}{preReleaseSuffix}"); // can include commit if needed
    }

    public static string FormatPreReleaseSuffix(this IBaseBuild build)
        => build.Branch == "master" || build.Branch == "main"
        ? string.Empty : "-pre";
}
