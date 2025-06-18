namespace CSharpCodeReorganizer.Core.UnitTests;

public class MemberInfoExtensionsTests
{
    [Theory]
    [InlineData("private const int PrivateConstField = 42;", "PrivateConstField", MemberType.Field, AccessModifier.Private, AdditionalModifier.Const)]
    [InlineData("public const string PublicConstant = \"ConstantValue\";", "PublicConstant", MemberType.Field, AccessModifier.Public, AdditionalModifier.Const)]
    [InlineData("readonly int PrivateReadonlyField = 42;", "PrivateReadonlyField", MemberType.Field, AccessModifier.None, AdditionalModifier.Readonly)]
    [InlineData("static readonly int StaticPrivateReadonlyField = 42;", "StaticPrivateReadonlyField", MemberType.Field, AccessModifier.None, AdditionalModifier.StaticReadonly)]
    [InlineData("protected int ProtectedField = 42;", "ProtectedField", MemberType.Field, AccessModifier.Protected, AdditionalModifier.None)]
    [InlineData("protected internal int ProtectedInternalField = 42;", "ProtectedInternalField", MemberType.Field, AccessModifier.ProtectedInternal, AdditionalModifier.None)]
    [InlineData("internal protected int InternalProtectedReversField = 42;", "InternalProtectedReversField", MemberType.Field, AccessModifier.ProtectedInternal, AdditionalModifier.None)]
    [InlineData("private protected int PrivateProtectedField = 42;", "PrivateProtectedField", MemberType.Field, AccessModifier.PrivateProtected, AdditionalModifier.None)]
    [InlineData("public MyClass() { }", "MyClass", MemberType.Constructor, AccessModifier.Public, AdditionalModifier.None)]
    [InlineData("~MyClass() { }", "MyClass", MemberType.Destructor, AccessModifier.None, AdditionalModifier.None)]
    [InlineData("public delegate void MyDelegate(int param);", "MyDelegate", MemberType.Delegate, AccessModifier.Public, AdditionalModifier.None)]
    [InlineData("public event EventHandler MyEvent;", "MyEvent", MemberType.Event, AccessModifier.Public, AdditionalModifier.None)]
    [InlineData("public enum MyEnum { Value1, Value2 }", "MyEnum", MemberType.Enum, AccessModifier.Public, AdditionalModifier.None)]
    [InlineData("public interface IMyInterface { }", "IMyInterface", MemberType.Interface, AccessModifier.Public, AdditionalModifier.None)]
    [InlineData("public int MyProperty { get; set; }", "MyProperty", MemberType.Property, AccessModifier.Public, AdditionalModifier.None)]
    [InlineData("public int this[int index] { get; set; }", "this", MemberType.Indexer, AccessModifier.Public, AdditionalModifier.None)]
    [InlineData("public static MyType operator +(MyType left, MyType right) { }", "+", MemberType.Operator, AccessModifier.Public, AdditionalModifier.Static)]
    [InlineData("public int MyMethod(int param) { }", "MyMethod", MemberType.Method, AccessModifier.Public, AdditionalModifier.None)]
    [InlineData("int IInterface.MyMethod(int param) { }", "MyMethod", MemberType.Method, AccessModifier.ExplicitInterfaceImplementation, AdditionalModifier.None)]
    [InlineData("public struct MyStruct { }", "MyStruct", MemberType.Struct, AccessModifier.Public, AdditionalModifier.None)]
    [InlineData("public class MyClass { }", "MyClass", MemberType.Class, AccessModifier.Public, AdditionalModifier.None)]
    [InlineData("namespace MyNamespace { }", "MyNamespace", MemberType.Namespace, AccessModifier.None, AdditionalModifier.None)]
    public void GetMemberInfo_ShouldReturnCorrectInfo(string declarationText,
                                                      string expectedIdentifier,
                                                      MemberType expectedMemberType,
                                                      AccessModifier expectedAccessModifier,
                                                      AdditionalModifier expectedAdditionalModifier)
    {
        var memberDeclaration = expectedMemberType is not MemberType.Namespace
                                ? SyntaxFactory.ParseMemberDeclaration(declarationText)
                                : SyntaxFactory.ParseCompilationUnit(declarationText).Members[0];

        Assert.NotNull(memberDeclaration);

        var result = memberDeclaration.GetMemberInfo();

        Assert.Equal(expectedIdentifier, result.Identifier);
        Assert.Equal(expectedMemberType, result.MemberType);
        Assert.Equal(expectedAccessModifier, result.AccessModifier);
        Assert.Equal(expectedAdditionalModifier, result.AdditionalModifier);
    }
}
