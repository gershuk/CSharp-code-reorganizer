using System.Diagnostics.CodeAnalysis;
using CSharpCodeReorganizer.Core.Comparers;
using CSharpCodeReorganizer.Core.Comparers.Parameters;
using CSharpCodeReorganizer.Core.MemberData;

namespace CSharpCodeReorganizer.Core;

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
