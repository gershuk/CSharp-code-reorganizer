namespace CSharpCodeReorganizer.Core.MemberData;

// TODO : Think of replacing it with struct of bool
[Flags]
public enum AdditionalModifier
{
    None = 0,
    Const = 1,
    Static = 2,
    Readonly = 4,
    StaticReadonly = Static | Readonly,
}
