namespace CSharpCodeReorganizer.Core.UnitTests;

public class SyntaxListExtensionsTests
{
    [Fact]
    public void ToSyntaxList_WithEmptyEnumerable_ReturnsEmptySyntaxList()
    {
        var nodes = Enumerable.Empty<SyntaxNode>();
        var result = nodes.ToSyntaxList();
        Assert.Empty(result);
    }

    [Fact]
    public void ToSyntaxList_WithSingleNode_ReturnsSyntaxListWithSingleNode()
    {
        var node = SyntaxFactory.ParseExpression("1 + 2");
        var result = new[] { node }.ToSyntaxList();
        Assert.Single(result);
        Assert.Equal(node.ToString(), result[0].ToString());
    }

    [Fact]
    public void ToSyntaxList_WithMultipleNodes_ReturnsSyntaxListWithAllNodes()
    {
        var node1 = SyntaxFactory.ParseExpression("1 + 2");
        var node2 = SyntaxFactory.ParseExpression("3 * 4");
        var result = new[] { node1, node2 }.ToSyntaxList();
        Assert.Equal(2, result.Count);
        Assert.Equal(node1.ToString(), result[0].ToString());
        Assert.Equal(node2.ToString(), result[1].ToString());
    }
}