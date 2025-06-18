namespace CSharpCodeReorganizer.Core.UnitTests;

public class MemberInfoComparerTests
{
    private readonly MemberInfoComparer _comparer;

    public MemberInfoComparerTests()
    {
        var parameters = new MemberInfoComparerParameters
        {
            AccessModifierPriorityTable = new Dictionary<AccessModifier, int>
                                          {
                                              { AccessModifier.None, 0 },
                                              { AccessModifier.Public, 1 },
                                              { AccessModifier.Private, 2 },
                                              { AccessModifier.Protected, 3 }
                                          }.ToFrozenDictionary(),

            AdditionalModifierPriorityTable = new Dictionary<AdditionalModifier, int>
                                              {
                                                  { AdditionalModifier.Static, 1 },
                                                  { AdditionalModifier.None, 2 }
                                              }.ToFrozenDictionary(),

            MemberTypePriorityTable = new Dictionary<MemberType, int>
                                          {
                                              { MemberType.Method, 1 },
                                              { MemberType.Property, 2 }
                                          }.ToFrozenDictionary(),

            IdentifierComparerPriority = new(3, OrderType.ASC)
        };

        _comparer = new MemberInfoComparer(parameters);
    }

    [Fact]
    public void CompareByAccessModifier_ShouldReturnCorrectOrder()
    {
        var member1 = new MemberInfo { AccessModifier = AccessModifier.Public, MemberType = MemberType.Method };
        var member2 = new MemberInfo { AccessModifier = AccessModifier.Private, MemberType = MemberType.Method };

        int result = _comparer.Compare(member1, member2);

        Assert.Equal(-1, result);
    }

    [Fact]
    public void CompareByAdditionalModifier_ShouldReturnCorrectOrder()
    {
        var member1 = new MemberInfo { AdditionalModifier = AdditionalModifier.Static, MemberType = MemberType.Method };
        var member2 = new MemberInfo { AdditionalModifier = AdditionalModifier.None, MemberType = MemberType.Method };

        int result = _comparer.Compare(member1, member2);

        Assert.Equal(-1, result);
    }

    [Fact]
    public void CompareByIdentifier_ShouldReturnCorrectOrder()
    {
        var member1 = new MemberInfo { Identifier = "A", MemberType = MemberType.Method };
        var member2 = new MemberInfo { Identifier = "B", MemberType = MemberType.Method };

        int result = _comparer.Compare(member1, member2);

        Assert.Equal(-1, result);
    }

    [Fact]
    public void CompareByMemberType_ShouldReturnCorrectOrder()
    {
        var member1 = new MemberInfo { MemberType = MemberType.Method };
        var member2 = new MemberInfo { MemberType = MemberType.Property };

        int result = _comparer.Compare(member1, member2);

        Assert.Equal(-1, result);
    }

    [Fact]
    public void Compare_ShouldReturnCorrectOrderBasedOnPriority()
    {
        var member1 = new MemberInfo
        {
            AccessModifier = AccessModifier.Public,
            AdditionalModifier = AdditionalModifier.None,
            Identifier = "A",
            MemberType = MemberType.Method
        };

        var member2 = new MemberInfo
        {
            AccessModifier = AccessModifier.Private,
            AdditionalModifier = AdditionalModifier.Static,
            Identifier = "B",
            MemberType = MemberType.Property
        };

        int result = _comparer.Compare(member1, member2);

        Assert.Equal(-1, result);
    }
}
