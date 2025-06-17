namespace CSharpCodeReorganizer.Core.UnitTests;

public class MockMemberInfoExtensionsTest
{
    [Fact]
    public void GetMemberName_ShouldReturnCorrectName()
    {
        // Arrange
        var fieldDeclarationText = "private const int PrivateConstField = 42;";
        var memberDeclaration = SyntaxFactory.ParseMemberDeclaration(fieldDeclarationText);

        // Act
        var result = memberDeclaration.GetMemberName();

        // Assert
        Assert.Equal("PrivateConstField", result);
    }

    [Fact]
    public void GetMemberType_ShouldReturnCorrectType()
    {
        // Arrange
        var fieldDeclarationText = "public const string PublicConstant = \"ConstantValue\";";
        var memberDeclaration = SyntaxFactory.ParseMemberDeclaration(fieldDeclarationText);

        // Act
        var result = memberDeclaration.GetMemberType();

        // Assert
        Assert.Equal(MemberType.Field, result);
    }

    [Fact]
    public void GetMemberAccessModifier_ShouldReturnCorrectAccessModifier()
    {
        // Arrange
        var fieldDeclarationText = "private const int PrivateConstField = 42;";
        var memberDeclaration = SyntaxFactory.ParseMemberDeclaration(fieldDeclarationText);

        // Act
        var result = memberDeclaration.GetMemberInfo().AccessModifier;

        // Assert
        Assert.Equal(AccessModifier.Private, result);
    }

    [Fact]
    public void GetMemberAdditionalModifier_ShouldReturnCorrectAdditionalModifier()
    {
        // Arrange
        var fieldDeclarationText = "private const int PrivateConstField = 42;";
        var memberDeclaration = SyntaxFactory.ParseMemberDeclaration(fieldDeclarationText);

        // Act
        var result = memberDeclaration.GetMemberInfo().AdditionalModifier;

        // Assert
        Assert.Equal(AdditionalModifier.Const, result);
    }

    [Fact]
    public void GetMemberInfo_ShouldReturnCorrectMemberInfo()
    {
        // Arrange
        var fieldDeclarationText = "private const int PrivateConstField = 42;";
        var memberDeclaration = SyntaxFactory.ParseMemberDeclaration(fieldDeclarationText);

        // Act
        var result = memberDeclaration.GetMemberInfo();

        // Assert
        Assert.Equal(AccessModifier.Private, result.AccessModifier);
        Assert.Equal(AdditionalModifier.Const, result.AdditionalModifier);
        Assert.Equal("PrivateConstField", result.Identifier);
        Assert.Equal(MemberType.Field, result.MemberType);
    }

    [Fact]
    public void GetMemberInfo_ShouldReturnCorrectReadonlyField()
    {
        // Arrange
        var fieldDeclarationText = "readonly int PrivateReadonlyField = 42;";
        var memberDeclaration = SyntaxFactory.ParseMemberDeclaration(fieldDeclarationText);

        // Act
        var result = memberDeclaration.GetMemberInfo();

        // Assert
        Assert.Equal(AccessModifier.None, result.AccessModifier);
        Assert.Equal(AdditionalModifier.Readonly, result.AdditionalModifier);
        Assert.Equal("PrivateReadonlyField", result.Identifier);
        Assert.Equal(MemberType.Field, result.MemberType);
    }

    [Fact]
    public void GetMemberInfo_ShouldReturnCorrectStaticReadonlyField()
    {
        // Arrange
        var fieldDeclarationText = "static readonly int StaticPrivateReadonlyField = 42;";
        var memberDeclaration = SyntaxFactory.ParseMemberDeclaration(fieldDeclarationText);

        // Act
        var result = memberDeclaration.GetMemberInfo();

        // Assert
        Assert.Equal(AccessModifier.None, result.AccessModifier);
        Assert.Equal(AdditionalModifier.StaticReadonly, result.AdditionalModifier);
        Assert.Equal("StaticPrivateReadonlyField", result.Identifier);
        Assert.Equal(MemberType.Field, result.MemberType);
    }

    [Fact]
    public void GetMemberInfo_ShouldReturnCorrectProtectedField()
    {
        // Arrange
        var fieldDeclarationText = "protected int ProtectedField = 42;";
        var memberDeclaration = SyntaxFactory.ParseMemberDeclaration(fieldDeclarationText);

        // Act
        var result = memberDeclaration.GetMemberInfo();

        // Assert
        Assert.Equal(AccessModifier.Protected, result.AccessModifier);
        Assert.Equal(AdditionalModifier.None, result.AdditionalModifier);
        Assert.Equal("ProtectedField", result.Identifier);
        Assert.Equal(MemberType.Field, result.MemberType);
    }

    [Fact]
    public void GetMemberInfo_ShouldReturnCorrectProtectedInternalField()
    {
        // Arrange
        var fieldDeclarationText = "protected internal int ProtectedInternalField = 42;";
        var memberDeclaration = SyntaxFactory.ParseMemberDeclaration(fieldDeclarationText);

        // Act
        var result = memberDeclaration.GetMemberInfo();

        // Assert
        Assert.Equal(AccessModifier.ProtectedInternal, result.AccessModifier);
        Assert.Equal(AdditionalModifier.None, result.AdditionalModifier);
        Assert.Equal("ProtectedInternalField", result.Identifier);
        Assert.Equal(MemberType.Field, result.MemberType);
    }

    [Fact]
    public void GetMemberInfo_ShouldReturnCorrectPrivateProtectedField()
    {
        // Arrange
        var fieldDeclarationText = "protected private int PrivateProtectedField = 42;";
        var memberDeclaration = SyntaxFactory.ParseMemberDeclaration(fieldDeclarationText);

        // Act
        var result = memberDeclaration.GetMemberInfo();

        // Assert
        Assert.Equal(AccessModifier.PrivateProtected, result.AccessModifier);
        Assert.Equal(AdditionalModifier.None, result.AdditionalModifier);
        Assert.Equal("PrivateProtectedField", result.Identifier);
        Assert.Equal(MemberType.Field, result.MemberType);
    }

    [Fact]
    public void GetMemberInfo_ShouldReturnCorrectForConstructor()
    {
        // Arrange
        var constructorDeclarationText = "public MyClass() { }";
        var memberDeclaration = SyntaxFactory.ParseMemberDeclaration(constructorDeclarationText);

        // Act
        var result = memberDeclaration.GetMemberInfo();

        // Assert
        Assert.Equal(AccessModifier.Public, result.AccessModifier);
        Assert.Equal(AdditionalModifier.None, result.AdditionalModifier);
        Assert.Equal("MyClass", result.Identifier);
        Assert.Equal(MemberType.Constructor, result.MemberType);
    }

    [Fact]
    public void GetMemberInfo_ShouldReturnCorrectForDestructor()
    {
        // Arrange
        var destructorDeclarationText = "~MyClass() { }";
        var memberDeclaration = SyntaxFactory.ParseMemberDeclaration(destructorDeclarationText);

        // Act
        var result = memberDeclaration.GetMemberInfo();

        // Assert
        Assert.Equal(AccessModifier.None, result.AccessModifier);
        Assert.Equal(AdditionalModifier.None, result.AdditionalModifier);
        Assert.Equal("MyClass", result.Identifier);
        Assert.Equal(MemberType.Destructor, result.MemberType);
    }

    [Fact]
    public void GetMemberInfo_ShouldReturnCorrectForDelegate()
    {
        // Arrange
        var delegateDeclarationText = "public delegate void MyDelegate(int param);";
        var memberDeclaration = SyntaxFactory.ParseMemberDeclaration(delegateDeclarationText);

        // Act
        var result = memberDeclaration.GetMemberInfo();

        // Assert
        Assert.Equal(AccessModifier.Public, result.AccessModifier);
        Assert.Equal(AdditionalModifier.None, result.AdditionalModifier);
        Assert.Equal("MyDelegate", result.Identifier);
        Assert.Equal(MemberType.Delegate, result.MemberType);
    }

    [Fact]
    public void GetMemberInfo_ShouldReturnCorrectForEvent()
    {
        // Arrange
        var eventDeclarationText = "public event EventHandler MyEvent;";
        var memberDeclaration = SyntaxFactory.ParseMemberDeclaration(eventDeclarationText);

        // Act
        var result = memberDeclaration.GetMemberInfo();

        // Assert
        Assert.Equal(AccessModifier.Public, result.AccessModifier);
        Assert.Equal(AdditionalModifier.None, result.AdditionalModifier);
        Assert.Equal("MyEvent", result.Identifier);
        Assert.Equal(MemberType.Event, result.MemberType);
    }

    [Fact]
    public void GetMemberInfo_ShouldReturnCorrectForEnum()
    {
        // Arrange
        var enumDeclarationText = "public enum MyEnum { Value1, Value2 }";
        var memberDeclaration = SyntaxFactory.ParseMemberDeclaration(enumDeclarationText);

        // Act
        var result = memberDeclaration.GetMemberInfo();

        // Assert
        Assert.Equal(AccessModifier.Public, result.AccessModifier);
        Assert.Equal(AdditionalModifier.None, result.AdditionalModifier);
        Assert.Equal("MyEnum", result.Identifier);
        Assert.Equal(MemberType.Enum, result.MemberType);
    }

    [Fact]
    public void GetMemberInfo_ShouldReturnCorrectForInterface()
    {
        // Arrange
        var interfaceDeclarationText = "public interface IMyInterface { }";
        var memberDeclaration = SyntaxFactory.ParseMemberDeclaration(interfaceDeclarationText);

        // Act
        var result = memberDeclaration.GetMemberInfo();

        // Assert
        Assert.Equal(AccessModifier.Public, result.AccessModifier);
        Assert.Equal(AdditionalModifier.None, result.AdditionalModifier);
        Assert.Equal("IMyInterface", result.Identifier);
        Assert.Equal(MemberType.Interface, result.MemberType);
    }

    [Fact]
    public void GetMemberInfo_ShouldReturnCorrectForProperty()
    {
        // Arrange
        var propertyDeclarationText = "public int MyProperty { get; set; }";
        var memberDeclaration = SyntaxFactory.ParseMemberDeclaration(propertyDeclarationText);

        // Act
        var result = memberDeclaration.GetMemberInfo();

        // Assert
        Assert.Equal(AccessModifier.Public, result.AccessModifier);
        Assert.Equal(AdditionalModifier.None, result.AdditionalModifier);
        Assert.Equal("MyProperty", result.Identifier);
        Assert.Equal(MemberType.Property, result.MemberType);
    }

    [Fact]
    public void GetMemberInfo_ShouldReturnCorrectForIndexer()
    {
        // Arrange
        var indexerDeclarationText = "public int this[int index] { get; set; }";
        var memberDeclaration = SyntaxFactory.ParseMemberDeclaration(indexerDeclarationText);

        // Act
        var result = memberDeclaration.GetMemberInfo();

        // Assert
        Assert.Equal(AccessModifier.Public, result.AccessModifier);
        Assert.Equal(AdditionalModifier.None, result.AdditionalModifier);
        Assert.Equal("this", result.Identifier);
        Assert.Equal(MemberType.Indexer, result.MemberType);
    }

    [Fact]
    public void GetMemberInfo_ShouldReturnCorrectForOperator()
    {
        // Arrange
        var operatorDeclarationText = "public static MyType operator +(MyType left, MyType right) { }";
        var memberDeclaration = SyntaxFactory.ParseMemberDeclaration(operatorDeclarationText);

        // Act
        var result = memberDeclaration.GetMemberInfo();

        // Assert
        Assert.Equal(AccessModifier.Public, result.AccessModifier);
        Assert.Equal(AdditionalModifier.Static, result.AdditionalModifier);
        Assert.Equal("+", result.Identifier);
        Assert.Equal(MemberType.Operator, result.MemberType);
    }

    [Fact]
    public void GetMemberInfo_ShouldReturnCorrectForMethod()
    {
        // Arrange
        var methodDeclarationText = "public int MyMethod(int param) { }";
        var memberDeclaration = SyntaxFactory.ParseMemberDeclaration(methodDeclarationText);

        // Act
        var result = memberDeclaration.GetMemberInfo();

        // Assert
        Assert.Equal(AccessModifier.Public, result.AccessModifier);
        Assert.Equal(AdditionalModifier.None, result.AdditionalModifier);
        Assert.Equal("MyMethod", result.Identifier);
        Assert.Equal(MemberType.Method, result.MemberType);
    }

    [Fact]
    public void GetMemberInfo_ShouldReturnCorrectForStruct()
    {
        // Arrange
        var structDeclarationText = "public struct MyStruct { }";
        var memberDeclaration = SyntaxFactory.ParseMemberDeclaration(structDeclarationText);

        // Act
        var result = memberDeclaration.GetMemberInfo();

        // Assert
        Assert.Equal(AccessModifier.Public, result.AccessModifier);
        Assert.Equal(AdditionalModifier.None, result.AdditionalModifier);
        Assert.Equal("MyStruct", result.Identifier);
        Assert.Equal(MemberType.Struct, result.MemberType);
    }

    [Fact]
    public void GetMemberInfo_ShouldReturnCorrectForClass()
    {
        // Arrange
        var classDeclarationText = "public class MyClass { }";
        var memberDeclaration = SyntaxFactory.ParseMemberDeclaration(classDeclarationText);

        // Act
        var result = memberDeclaration.GetMemberInfo();

        // Assert
        Assert.Equal(AccessModifier.Public, result.AccessModifier);
        Assert.Equal(AdditionalModifier.None, result.AdditionalModifier);
        Assert.Equal("MyClass", result.Identifier);
        Assert.Equal(MemberType.Class, result.MemberType);
    }

    [Fact]
    public void GetMemberInfo_ShouldReturnCorrectForNamespace()
    {
        // Arrange
        var classDeclarationText = "namespace MyNamespace { }";
        // get namespace declaration syntax from text
        var memberDeclaration = SyntaxFactory.ParseCompilationUnit(classDeclarationText).Members[0];

        // Act
        var result = memberDeclaration.GetMemberInfo();

        // Assert
        Assert.Equal(AccessModifier.None, result.AccessModifier);
        Assert.Equal(AdditionalModifier.None, result.AdditionalModifier);
        Assert.Equal("MyNamespace", result.Identifier);
        Assert.Equal(MemberType.Namespace, result.MemberType);
    }
}