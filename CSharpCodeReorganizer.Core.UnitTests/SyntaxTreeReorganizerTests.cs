namespace CSharpCodeReorganizer.Core.UnitTests;

public class SyntaxTreeReorganizerTests
{
    private readonly SyntaxTreeReorganizer _reorganizer;

    private readonly IComparer<MemberInfo> _memberInfoComparer = new DefaultMemberInfoComparer();
    private readonly IComparer<UsingInfo> _usingInfoComparer = new DefaultUsingInfoComparer();

    public SyntaxTreeReorganizerTests()
    {
        _reorganizer = new SyntaxTreeReorganizer(_memberInfoComparer, _usingInfoComparer);
    }

    // Mock Comparers (For simplicity, using default comparers)
    private class DefaultMemberInfoComparer : IComparer<MemberInfo>
    {
        public int Compare(MemberInfo x, MemberInfo y) => string.CompareOrdinal(x.Identifier, y.Identifier);
    }

    private class DefaultUsingInfoComparer : IComparer<UsingInfo>
    {
        public int Compare(UsingInfo x, UsingInfo y) => string.CompareOrdinal(x.Name, y.Name);
    }

    private static List<MemberDeclarationSyntax> OrderMembersByIdentifier(SyntaxList<MemberDeclarationSyntax> list) =>
        [.. list.OrderBy(m => m.GetMemberInfo().Identifier?.ToString()
                        ?? throw new InvalidOperationException("Member has no identifier"),
                        StringComparer.Ordinal)];
    [Fact]
    public void VisitRegionDirectiveTrivia_ShouldReturnNull()
    {
        var code = @"#region 
                    #endregion
                    ";

        var tree = CSharpSyntaxTree.ParseText(code);

        //get trivia node
        var node = tree.GetRoot().FindTrivia(0).GetStructure() as RegionDirectiveTriviaSyntax;
        Assert.NotNull(node);

        var newRoot = _reorganizer.VisitRegionDirectiveTrivia(node);
        Assert.Null(newRoot);
    }

    [Fact]
    public void VisitClassDeclaration_WithMultipleMembers_ShouldReorderMembers()
    {
        var code = @"
                    class MyClass
                    {
                        private string field2;
                        public int Field1;
                    }
                }";

        var tree = CSharpSyntaxTree.ParseText(code);
        var root = (CompilationUnitSyntax)tree.GetRoot();

        var classDeclaration = root.Members.OfType<ClassDeclarationSyntax>().First();

        var newRoot = _reorganizer.VisitClassDeclaration(classDeclaration) as ClassDeclarationSyntax;

        Assert.NotNull(newRoot);

        var expectedFieldMembers = classDeclaration.Members.OrderBy(m => m.GetMemberInfo().Identifier?.ToString()
                                                                    ?? throw new InvalidOperationException("Member has no identifier"),
                                                                    StringComparer.Ordinal).ToList();

        for (var i = 0; i < classDeclaration.Members.Count; ++i)
        {
            Assert.Equal(expectedFieldMembers[i].ToString(), newRoot.Members[i].ToString());
        }
    }

    [Fact]
    public void VisitRecordDeclaration_WithMultipleMembers_ShouldReorderMembers()
    {
        var code = @"
                    record MyRecord(int a)
                    {
                        private string field2;
                        public int Field1;
                    }";
        var tree = CSharpSyntaxTree.ParseText(code);
        var root = (CompilationUnitSyntax)tree.GetRoot();
        var recordDeclaration = root.Members.OfType<RecordDeclarationSyntax>().First();
        var newRoot = _reorganizer.VisitRecordDeclaration(recordDeclaration) as RecordDeclarationSyntax;

        Assert.NotNull(newRoot);

        var expectedFieldMembers = OrderMembersByIdentifier(recordDeclaration.Members);

        for (var i = 0; i < recordDeclaration.Members.Count; ++i)
        {
            Assert.Equal(expectedFieldMembers[i].ToString(), newRoot.Members[i].ToString());
        }
    }

    [Fact]
    public void VisitRecordStructDeclaration_WithMultipleMembers_ShouldReorderMembers()
    {
        var code = @"
                    record struct MyRecordStruct(int a)
                    {
                        private string field2;
                        public int Field1;
                    }";
        var tree = CSharpSyntaxTree.ParseText(code);
        var root = (CompilationUnitSyntax)tree.GetRoot();
        var recordDeclaration = root.Members.OfType<RecordDeclarationSyntax>().First();
        var newRoot = _reorganizer.VisitRecordDeclaration(recordDeclaration) as RecordDeclarationSyntax;

        Assert.NotNull(newRoot);

        var expectedFieldMembers = OrderMembersByIdentifier(recordDeclaration.Members);

        for (var i = 0; i < recordDeclaration.Members.Count; ++i)
        {
            Assert.Equal(expectedFieldMembers[i].ToString(), newRoot.Members[i].ToString());
        }
    }

    [Fact]
    public void VisitStructDeclaration_WithMultipleMembers_ShouldReorderMembers()
    {
        var code = @"
                    struct MyStruct
                    {
                        private string field2;
                        public int Field1;
                    }
                }";

        var tree = CSharpSyntaxTree.ParseText(code);
        var root = (CompilationUnitSyntax)tree.GetRoot();

        var structDeclaration = root.Members.OfType<StructDeclarationSyntax>().First();

        var newRoot = _reorganizer.VisitStructDeclaration(structDeclaration) as StructDeclarationSyntax;

        Assert.NotNull(newRoot);

        var expectedFieldMembers = OrderMembersByIdentifier(structDeclaration.Members);

        for (var i = 0; i < structDeclaration.Members.Count; ++i)
        {
            Assert.Equal(expectedFieldMembers[i].ToString(), newRoot.Members[i].ToString());
        }
    }

    [Fact]
    public void VisitInterfaceDeclaration_WithMultipleMembers_ShouldReorderMembers()
    {
        var code = @"
                    interface MyInterface
                    {
                        private string Method2();
                        public int Method1();
                    }
                }";
        var tree = CSharpSyntaxTree.ParseText(code);
        var root = (CompilationUnitSyntax)tree.GetRoot();
        var interfaceDeclaration = root.Members.OfType<InterfaceDeclarationSyntax>().First();
        var newRoot = _reorganizer.VisitInterfaceDeclaration(interfaceDeclaration) as InterfaceDeclarationSyntax;

        Assert.NotNull(newRoot);

        var expectedMethodMembers = OrderMembersByIdentifier(interfaceDeclaration.Members);

        for (var i = 0; i < newRoot.Members.Count; ++i)
        {
            Assert.Equal(expectedMethodMembers[i].ToString(), newRoot.Members[i].ToString());
        }
    }

    [Fact]
    public void VisitNamespaceDeclaration_WithMultipleMembers_ShouldReorderMembers()
    {
        var code = @"
                    namespace MyNamespace
                    {
                       class MyClass1 {}
                        struct MyStruct {}
                        interface MyInterface {}
                    }";
        var tree = CSharpSyntaxTree.ParseText(code);
        var root = (CompilationUnitSyntax)tree.GetRoot();
        var namespaceDeclaration = root.Members.OfType<NamespaceDeclarationSyntax>().First();
        var newRoot = _reorganizer.VisitNamespaceDeclaration(namespaceDeclaration) as NamespaceDeclarationSyntax;

        Assert.NotNull(newRoot);

        var expectedMembers = OrderMembersByIdentifier(namespaceDeclaration.Members);

        for (var i = 0; i < newRoot.Members.Count; ++i)
        {
            Assert.Equal(expectedMembers[i].ToString(), newRoot.Members[i].ToString());
        }
    }

    [Fact]
    public void VisitFileScopedNamespaceDeclaration_WithMultipleMembers_ShouldReorderMembers()
    {
        var code = @"
                    namespace MyNamespace;

                    class MyClass1 {}
                    struct MyStruct {}
                    interface MyInterface {}
                    ";
        var tree = CSharpSyntaxTree.ParseText(code);
        var root = (CompilationUnitSyntax)tree.GetRoot();
        var namespaceDeclaration = root.Members.OfType<FileScopedNamespaceDeclarationSyntax>().First();
        var newRoot = _reorganizer.VisitFileScopedNamespaceDeclaration(namespaceDeclaration) as FileScopedNamespaceDeclarationSyntax;

        Assert.NotNull(newRoot);

        var expectedMembers = OrderMembersByIdentifier(namespaceDeclaration.Members);

        for (var i = 0; i < newRoot.Members.Count; ++i)
        {
            Assert.Equal(expectedMembers[i].ToString(), newRoot.Members[i].ToString());
        }
    }

    [Fact]
    public void VisitCompilationUnit_WithMultipleUsings_ShouldReorderUsings()
    {
        var code = @"
                    using System.Collections.Generic;
                    using System;
                    namespace MyNamespace;
                    ";
        var tree = CSharpSyntaxTree.ParseText(code);
        var root = (CompilationUnitSyntax)tree.GetRoot();
        var compilationUnit = root;
        var newRoot = _reorganizer.VisitCompilationUnit(compilationUnit) as CompilationUnitSyntax;

        Assert.NotNull(newRoot);

        var expectedUsings = compilationUnit.Usings.OrderBy(UsingInfoExtensions.GetUsingInfo, _usingInfoComparer).ToList();
        for (var i = 0; i < newRoot.Usings.Count; ++i)
        {
            Assert.Equal(expectedUsings[i].ToString(), newRoot.Usings[i].ToString());
        }
    }

    [Fact]
    public void VisitCompilationUnit_WithMultipleMembers_ShouldReorderMembers()
    {
        var code = @"
                    namespace MyNamespace
                    {
                        class MyClass2 {}
                        class MyClass1 {}
                    }
                    ";

        var tree = CSharpSyntaxTree.ParseText(code);

        var root = tree.GetRoot() as CompilationUnitSyntax;

        Assert.NotNull(root);

        var newRoot = _reorganizer.VisitCompilationUnit(root) as CompilationUnitSyntax;

        Assert.NotNull(newRoot);

        var namespaceDeclaration = newRoot.Members.OfType<NamespaceDeclarationSyntax>().First();
        var classDeclarations = namespaceDeclaration.Members.OfType<ClassDeclarationSyntax>().ToList();
        var expectedClassDeclarations = classDeclarations.OrderBy(c => c.GetMemberInfo().Identifier, StringComparer.Ordinal).ToList();

        for (var i = 0; i < classDeclarations.Count; ++i)
        {
            Assert.Equal(expectedClassDeclarations[i].GetMemberInfo().Identifier, classDeclarations[i].GetMemberInfo().Identifier);
        }
    }

    [Fact]
    public void VisitCompilationUnit_ShouldReorderUsingsAndMembers()
    {
        // Arrange
        var code = @"
                using System.Collections.Generic;
                using System;

                namespace TestNamespace
                {
                    struct Struct1
                    {
                        private int field1;
                        private int field2;
                    }

                    struct Struct2
                    {
                        private int field1;
                        private int field2;
                    }

                    class MyClass
                    {
                        private string field2;
                        public int Field1;
                    }

                    class AnotherClass
                    {
                        private string field1;
                        public int Field2;
                    }

                    record Record1
                    {
                        public int Property1 { get; init; }
                        public string Property2 { get; init; }
                    }

                    record Record2
                    {
                        public int Property3 { get; init; }
                        public string Property4 { get; init; }
                    }

                    record struct RecordStruct1
                    {
                        public int Property1 { get; init; }
                        public string Property2 { get; init; }
                    }

                    record struct RecordStruct2
                    {
                        public int Property3 { get; init; }
                        public string Property4 { get; init; }
                    }
                }";

        var tree = CSharpSyntaxTree.ParseText(code);
        var root = (CompilationUnitSyntax)tree.GetRoot();

        var newRoot = (CompilationUnitSyntax)_reorganizer.Visit(root);

        var expectedUsings = root.Usings.OrderBy(UsingInfoExtensions.GetUsingInfo, _usingInfoComparer).ToList();

        for (var i = 0; i < newRoot.Usings.Count; ++i)
        {
            Assert.Equal(expectedUsings[i].Name?.ToString(), newRoot.Usings[i].Name?.ToString());
        }

        var namespaceDeclaration = newRoot.Members.OfType<NamespaceDeclarationSyntax>().First();
        var classDeclarations = namespaceDeclaration.Members.OfType<ClassDeclarationSyntax>().ToList();
        var expectedClassDeclarations = classDeclarations.OrderBy(c => c.GetMemberInfo().Identifier, StringComparer.Ordinal).ToList();

        for (var i = 0; i < classDeclarations.Count; ++i)
        {
            Assert.Equal(expectedClassDeclarations[i].GetMemberInfo().Identifier, classDeclarations[i].GetMemberInfo().Identifier);
        }

        var classDeclaration = classDeclarations.First();
        var expectedFieldMembers = OrderMembersByIdentifier(classDeclaration.Members);

        for (var i = 0; i < classDeclaration.Members.Count; ++i)
        {
            Assert.Equal(expectedFieldMembers[i].ToString(), classDeclaration.Members[i].ToString());
        }
    }
}