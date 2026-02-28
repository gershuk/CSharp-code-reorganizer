namespace CSharpCodeReorganizer.Core.MemberData;

// TODO : Think of replacing it with struct of bool
[Flags]
public enum AccessModifier
{
    None = 0,
    Public = 1,
    Internal = 2,
    Protected = 4,
    Private = 8,
    ExplicitInterfaceImplementation = 16,
    File = 32,
    ProtectedInternal = Internal | Protected,
    PrivateProtected = Private | Protected,
}