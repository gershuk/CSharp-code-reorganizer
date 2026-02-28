using CSharpCodeReorganizer.Core.Comparers.Parameters;

namespace CSharpCodeReorganizer.Core.UnitTests;

public class UsingInfoComparerTests
{
    private readonly UsingInfoComparerParameters _parameters;
    private readonly UsingInfoComparer _usingInfoComparer;

    private readonly UsingInfoComparerByName _usingComparerByNameSystemHigh;
    private readonly UsingInfoComparerByName _usingComparerByNameSystemLow;
    private readonly UsingInfoComparerByAlias _usingComparerByAlias;
    private readonly UsingInfoComparerByStatic _usingComparerByStatic;
    private readonly UsingInfoComparerByGlobal _usingComparerByGlobal;

    public UsingInfoComparerTests()
    {
        _parameters = new()
        {
            IsSystemHigherThanOtherNamespaces = true,
            GlobalComparerParameters = new SyntaxComparerParameters(1),
            StaticComparerParameters = new SyntaxComparerParameters(2),
            AliasComparerParameters = new SyntaxComparerParameters(3),
            NameComparerParameters = new SyntaxComparerParameters(4),
        };

        _usingInfoComparer = new UsingInfoComparer(_parameters);
        _usingComparerByNameSystemHigh = new UsingInfoComparerByName(_parameters.NameComparerParameters, _parameters.IsSystemHigherThanOtherNamespaces);
        _usingComparerByNameSystemLow = new UsingInfoComparerByName(_parameters.NameComparerParameters, !_parameters.IsSystemHigherThanOtherNamespaces);
        _usingComparerByAlias = new UsingInfoComparerByAlias(_parameters.AliasComparerParameters);
        _usingComparerByStatic = new UsingInfoComparerByStatic(_parameters.StaticComparerParameters);
        _usingComparerByGlobal = new UsingInfoComparerByGlobal(_parameters.GlobalComparerParameters);
    }

    [Fact]
    public void CompareBySystemNamespace_ShouldReturnNegativeWhenLeftIsNotSystemAndRightIsSystemForSystemHigher()
    {
        var left = new UsingInfo("A");
        var right = new UsingInfo("System.A");

        var result = _usingComparerByNameSystemHigh.Compare(left, right);

        Assert.Equal(1, result);

        result = _usingComparerByNameSystemLow.Compare(left, right);
        Assert.Equal(-1, result);
    }

    [Fact]
    public void CompareBySystemNamespace_ShouldReturnPositiveWhenLeftIsSystemAndRightIsNotSystemSystemHigher()
    {
        var left = new UsingInfo("System.A");
        var right = new UsingInfo("A");

        var result = _usingComparerByNameSystemHigh.Compare(left, right);

        Assert.Equal(-1, result);

        result = _usingComparerByNameSystemLow.Compare(left, right);
        Assert.Equal(1, result);
    }

    [Fact]
    public void CompareByName_ShouldReturnStringComparisonResultWhenBothAreSystem()
    {
        var left = new UsingInfo("A");
        var right = new UsingInfo("B");

        var result = _usingComparerByNameSystemHigh.Compare(left, right);

        Assert.Equal(string.CompareOrdinal("A", "B"), result);

        result = _usingComparerByNameSystemLow.Compare(left, right);
        Assert.Equal(string.CompareOrdinal("A", "B"), result);
    }

    [Fact]
    public void CompareByNameWithSystem_ShouldReturnStringComparisonResultWhenBothAreSystem()
    {
        var left = new UsingInfo("System.A");
        var right = new UsingInfo("System.B");

        var result = _usingComparerByNameSystemHigh.Compare(left, right);

        Assert.Equal(string.CompareOrdinal("A", "B"), result);
    }

    [Fact]
    public void CompareByStatic_ShouldReturnNegativeWhenLeftIsNotStaticAndRightIsStatic()
    {
        var left = new UsingInfo("A") { IsStatic = false };
        var right = new UsingInfo("A") { IsStatic = true };

        var result = _usingComparerByStatic.Compare(left, right);

        Assert.Equal(-1, result);
    }

    [Fact]
    public void CompareByStatic_ShouldReturnPositiveWhenLeftIsStaticAndRightIsNotStatic()
    {
        var left = new UsingInfo("A") { IsStatic = true };
        var right = new UsingInfo("A") { IsStatic = false };

        var result = _usingComparerByStatic.Compare(left, right);

        Assert.Equal(1, result);
    }

    [Fact]
    public void CompareByStatic_ShouldReturnZeroWhenBothAreStatic()
    {
        var left = new UsingInfo("A") { IsStatic = true };
        var right = new UsingInfo("A") { IsStatic = true };

        var result = _usingComparerByStatic.Compare(left, right);

        Assert.Equal(0, result);
    }

    [Fact]
    public void CompareByGlobal_ShouldReturnNegativeWhenLeftIsGlobalAndRightIsNotGlobal()
    {
        var left = new UsingInfo("A") { IsGlobal = true };
        var right = new UsingInfo("A") { IsGlobal = false };

        var result = _usingComparerByGlobal.Compare(left, right);

        Assert.Equal(-1, result);
    }

    [Fact]
    public void CompareByGlobal_ShouldReturnPositiveWhenLeftIsNotGlobalAndRightIsGlobal()
    {
        var left = new UsingInfo("A") { IsGlobal = false };
        var right = new UsingInfo("A") { IsGlobal = true };

        var result = _usingComparerByGlobal.Compare(left, right);

        Assert.Equal(1, result);
    }

    [Fact]
    public void CompareByGlobal_ShouldReturnZeroWhenBothAreGlobal()
    {
        var left = new UsingInfo("A") { IsGlobal = true };
        var right = new UsingInfo("A") { IsGlobal = true };

        var result = _usingComparerByGlobal.Compare(left, right);

        Assert.Equal(0, result);
    }

    [Fact]
    public void CompareByAlias_ShouldReturnStringComparisonResult()
    {
        var left = new UsingInfo("System.A") { Alias = "X" };
        var right = new UsingInfo("System.B") { Alias = "Y" };

        var result = _usingComparerByAlias.Compare(left, right);

        Assert.Equal(string.CompareOrdinal("X", "Y"), result);
    }

    [Fact]
    public void UsingInfoComparer_ShouldSortUsingInfosCorrectly()
    {
        // using NMethod1;
        // using System;
        // using System.IO;
        // using alias1 = System.Runtime.CompilerServices;
        // using alias2 = System.Runtime.CompilerServices;
        // using alias3 = System.Web;
        // using alias4 = System.Web;
        // using alias5 = System.Math;
        // using alias6 = System.Math;
        // using alias7 = NMethod1.NMethod2;
        // using alias8 = NMethod1.NMethod2.PublicClass1;
        // using static NMethod1.NMethod2.PublicClass1;
        // using static System.Console;
        // using static System.Math;
        //init list with this 
        UsingInfo[] usings =
        [
            new ("NMethod1"),
            new ("NMethod1.NMethod2"),
            new ("NMethod1.NMethod2.PublicClass1"),
            new ("System"),
            new ("System.IO"),
            new ("System.Math"),
            new ("System.Runtime.CompilerServices"),
            new ("System.Runtime.CompilerServices", "alias1"),
            new ("System.Runtime.CompilerServices", "alias2"),
            new ("System.Web", "alias3"),
            new ("System.Web", "alias4"),
            new ("System.Math", "alias5"),
            new ("System.Math", "alias6"),
            new ("NMethod1.NMethod2", "alias7"),
            new ("NMethod1.NMethod2.PublicClass1", "alias8"),
            new ("NMethod1.NMethod2.PublicClass1", isStatic:true),
            new ("System.Console", isStatic:true),
            new ("System.Math", isStatic:true)
        ];

        Array.Sort(usings, _usingInfoComparer);

        UsingInfo[] expected =
        [
            new ("System"),
            new ("System.IO"),
            new ("System.Math"),
            new ("System.Runtime.CompilerServices"),
            new ("NMethod1"),
            new ("NMethod1.NMethod2"),
            new ("NMethod1.NMethod2.PublicClass1"),
            new ("System.Runtime.CompilerServices", "alias1"),
            new ("System.Runtime.CompilerServices", "alias2"),
            new ("System.Web", "alias3"),
            new ("System.Web", "alias4"),
            new ("System.Math", "alias5"),
            new ("System.Math", "alias6"),
            new ("NMethod1.NMethod2", "alias7"),
            new ("NMethod1.NMethod2.PublicClass1", "alias8"),
            new ("System.Console", isStatic:true),
            new ("System.Math", isStatic:true),
            new ("NMethod1.NMethod2.PublicClass1", isStatic:true)
        ];

        for (var i = 0; i < expected.Length; i++)
        {
            Assert.Equal(expected[i], usings[i]);
        }
    }
}