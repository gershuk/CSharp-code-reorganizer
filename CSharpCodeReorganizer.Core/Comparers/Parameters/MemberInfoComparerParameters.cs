using System.Collections.Frozen;
using CSharpCodeReorganizer.Core.MemberData;

namespace CSharpCodeReorganizer.Core.Comparers.Parameters;

public readonly struct MemberInfoComparerParameters(SyntaxComparerParameters? memberTypeComparerParameters = default,
                                                    SyntaxComparerParameters? accessModifierComparerParameters = default,
                                                    SyntaxComparerParameters? additionalModifierComparerParameters = default,
                                                    SyntaxComparerParameters? identifierComparerParameters = default,
                                                    FrozenDictionary<MemberType, int>? memberTypePriorityTable = default,
                                                    FrozenDictionary<AccessModifier, int>? accessModifierPriorityTable = default,
                                                    FrozenDictionary<AdditionalModifier, int>? additionalModifierPriorityTable = default)
{
    public FrozenDictionary<MemberType, int> MemberTypePriorityTable { get; init; } = memberTypePriorityTable ?? DefaultPriorityTables.MemberTypePriorities;
    public FrozenDictionary<AccessModifier, int> AccessModifierPriorityTable { get; init; } = accessModifierPriorityTable ?? DefaultPriorityTables.AccessModifiersPriorities;
    public FrozenDictionary<AdditionalModifier, int> AdditionalModifierPriorityTable { get; init; } = additionalModifierPriorityTable ?? DefaultPriorityTables.AdditionalModifiersPriorities;
    public SyntaxComparerParameters AccessModifierComparerParameters { get; init; } = accessModifierComparerParameters ?? new(0);
    public SyntaxComparerParameters AdditionalModifierComparerParameters { get; init; } = additionalModifierComparerParameters ?? new(1);
    public SyntaxComparerParameters MemberTypeComparerParameters { get; init; } = memberTypeComparerParameters ?? new(2);
    public SyntaxComparerParameters IdentifierComparerParameters { get; init; } = identifierComparerParameters ?? new(3);

    public MemberInfoComparerParameters() : this(default, default, default, default, default) { }
}
