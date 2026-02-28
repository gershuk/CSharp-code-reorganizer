using CSharpCodeReorganizer.Core.Comparers.Parameters;

namespace CSharpCodeReorganizer.Core.Comparers;

public abstract class SyntaxInfoComparer<TSyntax>(in SyntaxComparerParameters parameters) : IComparer<TSyntax> where TSyntax : struct
{
    protected int IsAscOrder => Parameters.Order switch
    {
        OrderType.ASC => 1,
        OrderType.DESC => -1,
        _ => throw new ArgumentOutOfRangeException()
    };

    public SyntaxComparerParameters Parameters { get; set; } = parameters;
    public int Compare(TSyntax left, TSyntax right) => IsAscOrder * CompareCore(left, right);
    protected abstract int CompareCore(TSyntax left, TSyntax right);
}

public abstract class CompositeSyntaxInfoComparer<TSyntax, TParams> : IComparer<TSyntax>
                                                           where TSyntax : struct
                                                           where TParams : struct
{
    private readonly SyntaxInfoComparer<TSyntax>[] _comparers;

    protected abstract SyntaxInfoComparer<TSyntax>[] GetOrderedMethodsMatching(in TParams parameters);

    public int Compare(TSyntax left, TSyntax right)
    {
        var verdicts = _comparers.Select(comparer => (Name: comparer.GetType().Name, Value: comparer.Compare(left, right)));
        var result = verdicts.FirstOrDefault(result => result.Value != 0).Value;

        Console.WriteLine($"Comparing {typeof(TSyntax).Name}: left: {left}, right: {right}");

        foreach (var (index, res) in verdicts.Index())
        {
            Console.WriteLine($"{index} Comparing result from {res.Name}: {res.Value}");
        }

        Console.WriteLine($"Result of comparison: {result}");

        return result;
    }

    public CompositeSyntaxInfoComparer(in TParams parameters)
    {
        _comparers = GetOrderedMethodsMatching(parameters);
    }
}