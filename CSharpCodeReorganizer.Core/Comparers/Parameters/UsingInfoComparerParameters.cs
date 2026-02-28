namespace CSharpCodeReorganizer.Core.Comparers.Parameters;

public readonly struct UsingInfoComparerParameters(SyntaxComparerParameters? globalComparerParameters = default,
                                                   SyntaxComparerParameters? staticComparerParameters = default,
                                                   SyntaxComparerParameters? aliasComparerParameters = default,
                                                   SyntaxComparerParameters? nameComparerParameters = default,
                                                   bool IsSystemHigherThanOtherNamespaces = true)
{
    public SyntaxComparerParameters GlobalComparerParameters { get; init; } = globalComparerParameters ?? new(0);
    public SyntaxComparerParameters StaticComparerParameters { get; init; } = staticComparerParameters ?? new(1);
    public SyntaxComparerParameters AliasComparerParameters { get; init; } = aliasComparerParameters ?? new(2);
    public SyntaxComparerParameters NameComparerParameters { get; init; } = nameComparerParameters ?? new(3);
    public bool IsSystemHigherThanOtherNamespaces { get; init; } = IsSystemHigherThanOtherNamespaces;

    public UsingInfoComparerParameters() : this(default, default, default, default, true) { }
}