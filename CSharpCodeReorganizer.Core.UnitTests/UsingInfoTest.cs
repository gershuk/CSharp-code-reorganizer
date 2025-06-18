namespace CSharpCodeReorganizer.Core.UnitTests;

public class UsingInfoTests
{
    [Theory]
    [InlineData("System.Collections.Generic", "GC", true, false, "using GC = static System.Collections.Generic;")]
    [InlineData("System.Linq", null, true, false, "using static System.Linq;")]
    [InlineData("System.Linq", null, false, true, "global using System.Linq;")]
    [InlineData("System.Text", "ST", true, false, "using ST = static System.Text;")]
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
    [InlineData("System.Collections.Generic", "GC", true, false, true)]
    [InlineData("System.Linq", null, true, false, true)]
    [InlineData("System.Linq", null, false, true, true)]
    [InlineData("System.Text", "ST", true, false, true)]
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
}