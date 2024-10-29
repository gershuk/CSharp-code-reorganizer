namespace CSharpCodeReorganizer.Core;

public class Reorganizer
{
    public static Reorganizer Default { get; } = new();

    public string Reorganize(string code)
    {
        return code;
    }
}
