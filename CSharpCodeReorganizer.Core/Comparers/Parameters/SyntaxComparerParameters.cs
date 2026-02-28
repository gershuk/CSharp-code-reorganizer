using System.Diagnostics.CodeAnalysis;

namespace CSharpCodeReorganizer.Core.Comparers.Parameters;

public enum OrderType : int
{
    DESC = -1,
    ASC = 1,
}

[method: SetsRequiredMembers]
public readonly struct SyntaxComparerParameters(int Priority, OrderType Order = OrderType.ASC)
{
    public required int Priority { get; init; } = Priority;
    public required OrderType Order { get; init; } = Order;
}
