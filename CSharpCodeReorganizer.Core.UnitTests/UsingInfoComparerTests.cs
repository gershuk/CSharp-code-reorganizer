namespace CSharpCodeReorganizer.Core.Comparers.Tests;

public class UsingInfoComparerTests
{
    private readonly UsingInfoComparer _comparer;

    public UsingInfoComparerTests()
    {
        var parameters = new UsingInfoComparerParameters
        {
            IsSystemHigherThanOtherNamespaces = true,
            GlobalGroup = new GroupSortingParameters { GroupPriority = 1 },
            StaticGroup = new GroupSortingParameters { GroupPriority = 2 },
            AliasGroup = new GroupSortingParameters { GroupPriority = 3 },
            NameGroup = new GroupSortingParameters { GroupPriority = 4 }
        };
        _comparer = new UsingInfoComparer(in parameters);
    }

    [Fact]
    public void CompareByStatic_ShouldReturnNegativeWhenLeftIsNotStaticAndRightIsStatic()
    {
        var left = new UsingInfo("A") { IsStatic = false };
        var right = new UsingInfo("A") { IsStatic = true };

        int result = UsingInfoComparer.CompareByStatic(left, right);

        Assert.Equal(-1, result);
    }

    [Fact]
    public void CompareByStatic_ShouldReturnPositiveWhenLeftIsStaticAndRightIsNotStatic()
    {
        var left = new UsingInfo("A") { IsStatic = true };
        var right = new UsingInfo("A") { IsStatic = false };

        int result = UsingInfoComparer.CompareByStatic(left, right);

        Assert.Equal(1, result);
    }

    [Fact]
    public void CompareByStatic_ShouldReturnZeroWhenBothAreStatic()
    {
        var left = new UsingInfo("A") { IsStatic = true };
        var right = new UsingInfo("A") { IsStatic = true };

        int result = UsingInfoComparer.CompareByStatic(left, right);

        Assert.Equal(0, result);
    }

    [Fact]
    public void CompareByGlobal_ShouldReturnNegativeWhenLeftIsGlobalAndRightIsNotGlobal()
    {
        var left = new UsingInfo("A") { IsGlobal = true };
        var right = new UsingInfo("A") { IsGlobal = false };

        int result = UsingInfoComparer.CompareByGlobal(left, right);

        Assert.Equal(-1, result);
    }

    [Fact]
    public void CompareByGlobal_ShouldReturnPositiveWhenLeftIsNotGlobalAndRightIsGlobal()
    {
        var left = new UsingInfo("A") { IsGlobal = false };
        var right = new UsingInfo("A") { IsGlobal = true };

        int result = UsingInfoComparer.CompareByGlobal(left, right);

        Assert.Equal(1, result);
    }

    [Fact]
    public void CompareByGlobal_ShouldReturnZeroWhenBothAreGlobal()
    {
        var left = new UsingInfo("A") { IsGlobal = true };
        var right = new UsingInfo("A") { IsGlobal = true };

        int result = UsingInfoComparer.CompareByGlobal(left, right);

        Assert.Equal(0, result);
    }

    [Fact]
    public void CompareByName_ShouldReturnNegativeWhenLeftIsSystemAndRightIsNotSystem()
    {
        var left = new UsingInfo("System.A");
        var right = new UsingInfo("A");

        int result = _comparer.CompareByName(left, right);

        Assert.Equal(-1, result);
    }

    [Fact]
    public void CompareByName_ShouldReturnPositiveWhenLeftIsNotSystemAndRightIsSystem()
    {
        var left = new UsingInfo("A");
        var right = new UsingInfo("System.A");

        int result = _comparer.CompareByName(left, right);

        Assert.Equal(1, result);
    }

    [Fact]
    public void CompareByName_ShouldReturnStringComparisonResultWhenBothAreSystem()
    {
        var left = new UsingInfo("System.A");
        var right = new UsingInfo("System.B");

        int result = _comparer.CompareByName(left, right);

        Assert.Equal(string.Compare("A", "B", StringComparison.Ordinal), result);
    }

    [Fact]
    public void CompareByAlias_ShouldReturnStringComparisonResult()
    {
        var left = new UsingInfo("A") { Alias = "X" };
        var right = new UsingInfo("B") { Alias = "Y" };

        int result = UsingInfoComparer.CompareByAlias(left, right);

        Assert.Equal(string.Compare("X", "Y", StringComparison.Ordinal), result);
    }
}
