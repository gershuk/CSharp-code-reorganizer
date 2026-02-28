using CSharpCodeReorganizer.Core.MemberData;

namespace CSharpCodeReorganizer.Core;

public class SyntaxTreeReorganizer : CSharpSyntaxRewriter
{
    private readonly IComparer<MemberInfo> _memberInfoComparer;
    private readonly IComparer<UsingInfo> _usingInfoComparer;

    public override SyntaxNode? VisitRegionDirectiveTrivia(RegionDirectiveTriviaSyntax node)
    {
        return null;
    }

    public SyntaxTreeReorganizer(IComparer<MemberInfo> memberInfoComparer, IComparer<UsingInfo> usingInfoComparer)
    {
        _memberInfoComparer = memberInfoComparer;
        _usingInfoComparer = usingInfoComparer;
    }

    public override SyntaxNode? VisitCompilationUnit(CompilationUnitSyntax node)
    {
        var usings = OrganizeUsings(node.Usings);
        var members = OrganizeMembers(node.Members);
        return node.WithUsings(usings).WithMembers(members);
    }

    public override SyntaxNode? VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
    {
        var usings = OrganizeUsings(node.Usings);
        var members = OrganizeMembers(node.Members);
        return node.WithUsings(usings).WithMembers(members);
    }

    public override SyntaxNode? VisitFileScopedNamespaceDeclaration(FileScopedNamespaceDeclarationSyntax node)
    {
        var usings = OrganizeUsings(node.Usings);
        var members = OrganizeMembers(node.Members);
        return node.WithUsings(usings).WithMembers(members);
    }

    public override SyntaxNode? VisitRecordDeclaration(RecordDeclarationSyntax node)
    {
        var members = OrganizeMembers(node.Members);
        return node.WithMembers(members);
    }

    public override SyntaxNode? VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        var members = OrganizeMembers(node.Members);
        return node.WithMembers(members);
    }

    public override SyntaxNode? VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
    {
        var members = OrganizeMembers(node.Members);
        return node.WithMembers(members);
    }

    public override SyntaxNode? VisitStructDeclaration(StructDeclarationSyntax node)
    {
        var members = OrganizeMembers(node.Members);
        return node.WithMembers(members);
    }

    private SyntaxList<UsingDirectiveSyntax> OrganizeUsings(IReadOnlyCollection<UsingDirectiveSyntax> usingDirectives)
        => usingDirectives.OrderBy(UsingInfoExtensions.GetUsingInfo, _usingInfoComparer).ToSyntaxList();

    private SyntaxList<MemberDeclarationSyntax> OrganizeMembers(IReadOnlyCollection<MemberDeclarationSyntax> memberDeclarations) =>
        memberDeclarations.Select(member => member.Accept(this))
                          .OfType<MemberDeclarationSyntax>()
                          .OrderBy(MemberInfoExtensions.GetMemberInfo, _memberInfoComparer)
                          .ToSyntaxList();
}
