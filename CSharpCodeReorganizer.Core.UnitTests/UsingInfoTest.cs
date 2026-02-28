namespace CSharpCodeReorganizer.Core.UnitTests;

// TODO: add case with name = null
public class UsingInfoTests
{
    [Theory]
    [InlineData("System.Collections.Generic", "GC", false, false, "using GC = System.Collections.Generic;")]
    [InlineData("System.Math", null, true, false, "using static System.Math;")]
    [InlineData("System.Linq", null, false, true, "global using System.Linq;")]
    [InlineData("System.Text", "ST", false, false, "using ST = System.Text;")]
    [InlineData("System.IO", null, true, true, "global using static System.IO;")]
    [InlineData("System.Math", null, false, false, "using System.Math;")]
    [InlineData("CSharpCodeReorganizer.Core.UnitTests", null, false, false, "using CSharpCodeReorganizer.Core.UnitTests;")]
    public void ToString_ReturnsCorrectStringRepresentation(string name,
                                                            string? alias,
                                                            bool isStatic,
                                                            bool isGlobal,
                                                            string expectedResult)
    {
        var usingInfo = new UsingInfo(name, alias, isStatic, isGlobal);
        var result = usingInfo.ToString();
        Assert.Equal(expectedResult, result);

        result = UsingInfo.ToString(name, alias, isStatic, isGlobal);
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("System.Collections.Generic", "GC", false, false, true)]
    [InlineData("System.Linq", null, true, false, true)]
    [InlineData("System.Linq", null, false, true, true)]
    [InlineData("System.Text", "ST", false, false, true)]
    [InlineData("System.IO", null, true, true, true)]
    [InlineData("System.Math", null, false, false, true)]
    [InlineData("CSharpCodeReorganizer.Core.UnitTests", null, false, false, false)]
    public void IsSystemUsing_ReturnsCorrectValue(string name,
                                                  string? alias,
                                                  bool isStatic,
                                                  bool isGlobal,
                                                  bool isSystemUsing)
    {
        var usingInfo = new UsingInfo(name, alias, isStatic, isGlobal);
        Assert.Equal(isSystemUsing, usingInfo.IsSystemUsing);
    }

    [Theory]
    [InlineData("using MySpace.Class;", "MySpace.Class", null, false, false, false)]
    [InlineData("using System.Collections.Generic;", "System.Collections.Generic", null, false, false, true)]
    [InlineData("global using MySpace.Class;", "MySpace.Class", null, false, true, false)]
    [InlineData("global using System.Collections.Generic;", "System.Collections.Generic", null, false, true, true)]
    [InlineData("using static MySpace.Class;", "MySpace.Class", null, true, false, false)]
    [InlineData("using static System.Math;", "System.Math", null, true, false, true)]
    [InlineData("global using static MySpace.Class;", "MySpace.Class", null, true, true, false)]
    [InlineData("global using static System.Math;", "System.Math", null, true, true, true)]
    [InlineData("using Alias = MySpace.Class;", "MySpace.Class", "Alias", false, false, false)]
    [InlineData("using Alias = System.Collections.Generic;", "System.Collections.Generic", "Alias", false, false, true)]
    [InlineData("global using Alias = MySpace.Class;", "MySpace.Class", "Alias", false, true, false)]
    [InlineData("global using Alias = System.Collections.Generic;", "System.Collections.Generic", "Alias", false, true, true)]
    [InlineData("using Alias = (int a, int b);", null, "Alias", false, false, false)]
    [InlineData("global using Alias = (int a, int b);", null, "Alias", false, true, false)]
    public void GetUsingInfo_ReturnsCorrectValue(string declarationText,
                                                 string? expectedName,
                                                 string? expectedAlias,
                                                 bool expectedIsStatic,
                                                 bool expectedIsGlobal,
                                                 bool expectedIsSystemUsing)
    {
        var syntaxFactory = SyntaxFactory.ParseCompilationUnit(declarationText);
        var usingDirective = syntaxFactory.Usings[0];
        var usingInfo = usingDirective.GetUsingInfo();
        Assert.Equal(expectedName, usingInfo.Name);
        Assert.Equal(expectedAlias, usingInfo.Alias);
        Assert.Equal(expectedIsStatic, usingInfo.IsStatic);
        Assert.Equal(expectedIsGlobal, usingInfo.IsGlobal);
        Assert.Equal(expectedIsSystemUsing, usingInfo.IsSystemUsing);
    }
}