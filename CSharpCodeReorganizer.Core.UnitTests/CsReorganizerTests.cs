namespace CSharpCodeReorganizer.Core.UnitTests;

public class CsReorganizerTests
{
    [Fact]
    public void Reorganize_ShouldReturnExpectedSortedUsingsList()
    {
        var usings = """
                namespace NMethod1
                {
                    namespace NMethod2
                    {
                        using System;
                        using System.IO;
                        using NMethod12;
                        using static System.Console;
                        using static System.Math;
                        using static NMethod1.NMethod2.PublicClass1;
                        using alias1 = System.Runtime.CompilerServices;
                        using alias2 = System.Runtime.CompilerServices;
                        using alias3 = System.Web;
                        using alias4 = System.Web;
                        using alias5 = System.Math;
                        using alias6 = System.Math;
                        using alias7 = NMethod1.NMethod2;
                        using alias8 = NMethod1.NMethod2.PublicClass1;
                    }
                }
                """;

        var reorganizer = CsReorganizer.Default;
        var result = reorganizer.Reorganize(usings).Trim();

        var expected = """
                    namespace NMethod1
                    {
                        namespace NMethod2
                        {
                            using System;
                            using System.IO;
                            using NMethod12;
                            using alias1 = System.Runtime.CompilerServices;
                            using alias2 = System.Runtime.CompilerServices;
                            using alias3 = System.Web;
                            using alias4 = System.Web;
                            using alias5 = System.Math;
                            using alias6 = System.Math;
                            using alias7 = NMethod1.NMethod2;
                            using alias8 = NMethod1.NMethod2.PublicClass1;
                            using static System.Console;
                            using static System.Math;
                            using static NMethod1.NMethod2.PublicClass1;
                        }
                    }
                    """;

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Reorganize_ShouldReturnExpectedSortedFieldsList()
    {
        var usings = """
                class Test
                {
                    protected static readonly int _protectedStaticReadOnly;
                    protected static int _protectedStatic;
                    protected readonly int _protectedReadOnly;
                    protected int _protected;
                    private static int _privateStatic;
                    public static readonly int PublicStaticReadOnly;
                    public readonly int PublicReadonly;
                    private int _private;
                    private readonly int _privateReadOnly;
                    public int Public;
                    private static readonly int _privateStaticReadOnly;
                    public static int PublicStatic;
                }
                """;

        var reorganizer = CsReorganizer.Default;
        var result = reorganizer.Reorganize(usings).Trim();

        var expected = """
                    class Test
                    {
                        public static readonly int PublicStaticReadOnly;
                        public static int PublicStatic;
                        public readonly int PublicReadonly;
                        public int Public;
                        protected static readonly int _protectedStaticReadOnly;
                        protected static int _protectedStatic;
                        protected readonly int _protectedReadOnly;
                        protected int _protected;
                        private static readonly int _privateStaticReadOnly;
                        private static int _privateStatic;
                        private readonly int _privateReadOnly;
                        private int _private;
                    }
                    """;

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Reorganize_ShouldReturnExpectedSortedTypeDefinitionList()
    {
        var usings = """
                class NM
                {
                    public record struct RecordStruct1;
                    internal record struct RecordStruct2;
                    private record struct RecordStruct3;
                    protected record struct RecordStruct4;
                    public struct Struct1;
                    internal struct Struct2;
                    private struct Struct3;
                    protected struct Struct4;
                    public record class RecordClass1;
                    internal record class RecordClass2;
                    private record class RecordClass3;
                    protected record class RecordClass4;
                    public class Class1;
                    internal class Class2;
                    private class Class3;
                    protected class Class4;
                }
                """;

        var reorganizer = CsReorganizer.Default;
        var result = reorganizer.Reorganize(usings).Trim();

        var expected = """
                    class NM
                    {
                        public record struct RecordStruct1;
                        public struct Struct1;
                        public record class RecordClass1;
                        public class Class1;
                        internal record struct RecordStruct2;
                        internal struct Struct2;
                        internal record class RecordClass2;
                        internal class Class2;
                        protected record struct RecordStruct4;
                        protected struct Struct4;
                        protected record class RecordClass4;
                        protected class Class4;
                        private record struct RecordStruct3;
                        private struct Struct3;
                        private record class RecordClass3;
                        private class Class3;
                    }
                    """;
        Console.WriteLine(result);
        Console.WriteLine(expected);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Reorganize_ShouldReturnExpectedSortedClassMembersListForInterfaceRealization()
    {
        var usings = """
                public class Test : IEnumerable<string>, IInterfaceWithName
                {
                    public string Name { get; set; }
                    private string PrivateName { get; set; }
                    string IInterfaceWithName.Name { get; set; }
                    public IEnumerator<string> GetEnumerator() {}
                    private void Print() {}
                    IEnumerator IEnumerable.GetEnumerator() {}
                }
                """;

        var reorganizer = CsReorganizer.Default;
        var result = reorganizer.Reorganize(usings).Trim();
        Console.WriteLine(result);
        var expected = """
                    public class Test : IEnumerable<string>, IInterfaceWithName
                    {
                        public string Name { get; set; }
                        public IEnumerator<string> GetEnumerator() {}
                        string IInterfaceWithName.Name { get; set; }
                        IEnumerator IEnumerable.GetEnumerator() {}
                        private string PrivateName { get; set; }
                        private void Print() {}
                    }
                    """;

        Assert.Equal(expected, result);
    }
}