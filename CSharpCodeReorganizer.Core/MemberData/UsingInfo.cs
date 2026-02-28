using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace CSharpCodeReorganizer.Core.MemberData;

public readonly struct UsingInfo
{
    public const string SYSTEM_PREFIX = "System";

    public required string? Name { get; init; }
    public string? Alias { get; init; }
    public bool IsStatic { get; init; }
    public bool IsGlobal { get; init; }

    public bool IsSystemUsing { get; }
    public string StringPresentation { get; }

    [SetsRequiredMembers]
    public UsingInfo(string? name, string? alias = default, bool isStatic = default, bool isGlobal = default)
    {
        Name = name;
        Alias = alias;
        IsStatic = isStatic;
        IsGlobal = isGlobal;
        IsSystemUsing = Name?.StartsWith(SYSTEM_PREFIX, StringComparison.Ordinal) ?? false;
        StringPresentation = ToString(this);
    }

    public static string ToString(string name, string? alias = default, bool isStatic = default, bool isGlobal = default) =>
        ToString(new(name, alias, isStatic, isGlobal));

    public static string ToString(in UsingInfo info)
    {
        var sb = new StringBuilder();

        if (info.IsGlobal)
            sb.Append("global ");

        sb.Append("using");

        if (!string.IsNullOrEmpty(info.Alias))
            sb.Append($" {info.Alias} =");

        if (info.IsStatic)
            sb.Append(" static");

        sb.Append($" {info.Name};");

        return sb.ToString();
    }

    public override string ToString() => StringPresentation;
}