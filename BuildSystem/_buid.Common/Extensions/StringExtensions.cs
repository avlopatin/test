using System.Text.RegularExpressions;

namespace _buid.Common.Extensions;

internal static class StringExtensions
{
    private static readonly Regex PascalToKebabCaseRegex = new("(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z0-9])", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public static string ToKebabCaseLower(this string name)
        => PascalToKebabCaseRegex.Replace(name, "-$1")
            .Trim()
            .ToLower();
}
