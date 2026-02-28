using CSharpCodeReorganizer.Core.MemberData;

namespace CSharpCodeReorganizer.Core;

public static class UsingInfoExtensions
{
    public static UsingInfo GetUsingInfo(this UsingDirectiveSyntax usingDirective)
    {
        ArgumentNullException.ThrowIfNull(usingDirective);
        return new UsingInfo(usingDirective.Name?.ToString(),
                             usingDirective.Alias?.Name.Identifier.Text,
                             !string.IsNullOrEmpty(usingDirective.StaticKeyword.ValueText),
                             !string.IsNullOrEmpty(usingDirective.GlobalKeyword.ValueText));
    }
}