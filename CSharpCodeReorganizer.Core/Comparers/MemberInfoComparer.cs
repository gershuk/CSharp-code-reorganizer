using System.Collections.Frozen;
using CSharpCodeReorganizer.Core.Comparers.Parameters;
using CSharpCodeReorganizer.Core.MemberData;


namespace CSharpCodeReorganizer.Core.Comparers;

public sealed class MemberInfoComparerByAccessModifier(in SyntaxComparerParameters parameters,
                                                       FrozenDictionary<AccessModifier, int> AccessModifierPriorityTable) : SyntaxInfoComparer<MemberInfo>(parameters)
{
    protected override int CompareCore(MemberInfo left, MemberInfo right) =>
        AccessModifierPriorityTable[left.AccessModifier] - AccessModifierPriorityTable[right.AccessModifier];
}

public sealed class MemberInfoComparerByAdditionalModifier(in SyntaxComparerParameters parameters,
                                                           FrozenDictionary<AdditionalModifier, int> AdditionalModifierPriorityTable) : SyntaxInfoComparer<MemberInfo>(parameters)
{
    protected override int CompareCore(MemberInfo left, MemberInfo right) =>
        AdditionalModifierPriorityTable[left.AdditionalModifier] - AdditionalModifierPriorityTable[right.AdditionalModifier];
}

public sealed class MemberInfoComparerByIdentifier(in SyntaxComparerParameters parameters) : SyntaxInfoComparer<MemberInfo>(parameters)
{
    protected override int CompareCore(MemberInfo left, MemberInfo right) => string.CompareOrdinal(left.Identifier, right.Identifier);
}

public sealed class MemberInfoComparerByMemberType(in SyntaxComparerParameters parameters,
                                                   FrozenDictionary<MemberType, int> MemberTypePriorityTable) : SyntaxInfoComparer<MemberInfo>(parameters)
{
    protected override int CompareCore(MemberInfo left, MemberInfo right) =>
        MemberTypePriorityTable[left.MemberType] - MemberTypePriorityTable[right.MemberType];
}

public sealed class MemberInfoComparer(in MemberInfoComparerParameters parameters) : CompositeSyntaxInfoComparer<MemberInfo, MemberInfoComparerParameters>(in parameters)
{
    protected override SyntaxInfoComparer<MemberInfo>[] GetOrderedMethodsMatching(in MemberInfoComparerParameters parameters)
    {
        SyntaxInfoComparer<MemberInfo>[] comparers =
        [
            new MemberInfoComparerByAccessModifier(parameters.AccessModifierComparerParameters, parameters.AccessModifierPriorityTable),
            new MemberInfoComparerByAdditionalModifier(parameters.AdditionalModifierComparerParameters, parameters.AdditionalModifierPriorityTable),
            new MemberInfoComparerByIdentifier(parameters.IdentifierComparerParameters),
            new MemberInfoComparerByMemberType(parameters.MemberTypeComparerParameters, parameters.MemberTypePriorityTable),
        ];

        Array.Sort(comparers, static (x, y) => x.Parameters.Priority - y.Parameters.Priority);

        return comparers;
    }
}
