using System.Diagnostics.CodeAnalysis;
using CSharpCodeReorganizer.Core.Comparers.Parameters;
using CSharpCodeReorganizer.Core.MemberData;

namespace CSharpCodeReorganizer.Core.Comparers;

public sealed class UsingInfoComparerByStatic(in SyntaxComparerParameters parameters) : SyntaxInfoComparer<UsingInfo>(parameters)
{
    protected override int CompareCore(UsingInfo left, UsingInfo right) =>
        (left.IsStatic, right.IsStatic) switch
        {
            (false, true) => -1,
            (true, false) => 1,
            (_, _) => 0,
        };
}

public sealed class UsingInfoComparerByGlobal(in SyntaxComparerParameters parameters) : SyntaxInfoComparer<UsingInfo>(parameters)
{
    protected override int CompareCore(UsingInfo left, UsingInfo right) =>
        (left.IsGlobal, right.IsGlobal) switch
        {
            (true, false) => -1,
            (false, true) => 1,
            (_, _) => 0,
        };
}

public sealed class UsingInfoComparerByName(in SyntaxComparerParameters parameters, bool IsSystemHigherThanOtherNamespaces) : SyntaxInfoComparer<UsingInfo>(parameters)
{
    protected override int CompareCore(UsingInfo left, UsingInfo right) =>
        (left.IsSystemUsing, right.IsSystemUsing) switch
        {
            (false, true) => IsSystemHigherThanOtherNamespaces ? 1 : -1,
            (true, false) => IsSystemHigherThanOtherNamespaces ? -1 : 1,
            (_, _) => string.CompareOrdinal(left.Name, right.Name),
        };
}

public sealed class UsingInfoComparerByAlias(in SyntaxComparerParameters parameters) : SyntaxInfoComparer<UsingInfo>(parameters)
{
    protected override int CompareCore(UsingInfo left, UsingInfo right) =>
        string.CompareOrdinal(left.Alias, right.Alias);
}

[method: SetsRequiredMembers]
public sealed class UsingInfoComparer(in UsingInfoComparerParameters parameters) : CompositeSyntaxInfoComparer<UsingInfo, UsingInfoComparerParameters>(in parameters)
{
    protected override SyntaxInfoComparer<UsingInfo>[] GetOrderedMethodsMatching(in UsingInfoComparerParameters parameters)
    {

        SyntaxInfoComparer<UsingInfo>[] comparers =
        [
            new UsingInfoComparerByGlobal(parameters.GlobalComparerParameters),
            new UsingInfoComparerByStatic(parameters.StaticComparerParameters),
            new UsingInfoComparerByAlias(parameters.AliasComparerParameters),
            new UsingInfoComparerByName(parameters.NameComparerParameters, parameters.IsSystemHigherThanOtherNamespaces)
        ];

        Array.Sort(comparers, static (x, y) => x.Parameters.Priority - y.Parameters.Priority);

        return comparers;
    }
}