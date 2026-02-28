namespace CSharpCodeReorganizer.Core;

public static class SyntaxListExtensions
{
    public static SyntaxList<TNode> ToSyntaxList<TNode>(this IEnumerable<TNode> nodes) where TNode : SyntaxNode => [.. nodes];
}
