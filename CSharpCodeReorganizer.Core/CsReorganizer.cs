using System.Diagnostics.CodeAnalysis;

using CSharpCodeReorganizer.Core.Comparers;
using CSharpCodeReorganizer.Core.Comparers.Parameters;
using CSharpCodeReorganizer.Core.MemberData;

namespace CSharpCodeReorganizer.Core;

public readonly struct CsReorganizerParameters
{
    public required MemberInfoComparerParameters MemberInfoComparerParams { get; init; }
    public required UsingInfoComparerParameters UsingInfoComparerParams { get; init; }

    [SetsRequiredMembers]
    public CsReorganizerParameters(in MemberInfoComparerParameters memberInfoComparerParams = default,
                                   in UsingInfoComparerParameters usingInfoComparerParams = default) =>
        (MemberInfoComparerParams, UsingInfoComparerParams) = (memberInfoComparerParams, usingInfoComparerParams);
}

public sealed class CsReorganizer
{
    public static CsReorganizer Default { get; } = new();

    public required IComparer<MemberInfo> MemberInfoComparer { private get; init; }
    public required IComparer<UsingInfo> UsingInfoComparer { private get; init; }

    private readonly SyntaxTreeReorganizer _syntaxReorganizerRewriter;

    [SetsRequiredMembers]
    public CsReorganizer(IComparer<MemberInfo>? memberInfoComparer = default, IComparer<UsingInfo>? usingInfoComparer = default)
    {
        MemberInfoComparer = memberInfoComparer ?? new MemberInfoComparer(new MemberInfoComparerParameters());
        UsingInfoComparer = usingInfoComparer ?? new UsingInfoComparer(new UsingInfoComparerParameters());
        _syntaxReorganizerRewriter = new SyntaxTreeReorganizer(MemberInfoComparer, UsingInfoComparer);
    }

    [SetsRequiredMembers]
    public CsReorganizer(in CsReorganizerParameters parameters)
    {
        MemberInfoComparer = new MemberInfoComparer(parameters.MemberInfoComparerParams);
        UsingInfoComparer = new UsingInfoComparer(parameters.UsingInfoComparerParams);
        _syntaxReorganizerRewriter = new SyntaxTreeReorganizer(MemberInfoComparer, UsingInfoComparer);
    }

    public string Reorganize(string code) => Reorganize(Parse(code)).ToString();

    private CompilationUnitSyntax Parse(string input)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(input);
        var root = syntaxTree.GetCompilationUnitRoot();
        return root;
    }

    private CompilationUnitSyntax Reorganize(CompilationUnitSyntax compilationUnit) =>
        compilationUnit.Accept(_syntaxReorganizerRewriter) as CompilationUnitSyntax
        ?? throw new Exception($"Reorganized root is null or not of type {typeof(CompilationUnitSyntax)}.");
}