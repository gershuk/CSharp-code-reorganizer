using System.Collections.Frozen;
using CSharpCodeReorganizer.Core.MemberData;

namespace CSharpCodeReorganizer.Core.Comparers.Parameters;

public static class DefaultPriorityTables
{
    public static readonly FrozenDictionary<MemberType, int> MemberTypePriorities = new Dictionary<MemberType, int>()
    {
        [MemberType.None] = 00,
        [MemberType.Namespace] = 10,
        [MemberType.Interface] = 20,
        [MemberType.Enum] = 30,
        [MemberType.RecordStruct] = 40,
        [MemberType.Struct] = 50,
        [MemberType.Record] = 60,
        [MemberType.Class] = 70,
        [MemberType.Delegate] = 80,
        [MemberType.Event] = 90,
        [MemberType.Field] = 100,
        [MemberType.Indexer] = 110,
        [MemberType.Property] = 120,
        [MemberType.Operator] = 130,
        [MemberType.Method] = 140,
        [MemberType.Constructor] = 150,
        [MemberType.Destructor] = 160,
    }.ToFrozenDictionary();

    public static readonly FrozenDictionary<AccessModifier, int> AccessModifiersPriorities = new Dictionary<AccessModifier, int>()
    {
        [AccessModifier.None] = 00,
        [AccessModifier.Public] = 10,
        [AccessModifier.ExplicitInterfaceImplementation] = 20,
        [AccessModifier.Internal] = 30,
        [AccessModifier.File] = 40,
        [AccessModifier.Protected] = 50,
        [AccessModifier.ProtectedInternal] = 60,
        [AccessModifier.PrivateProtected] = 70,
        [AccessModifier.Private] = 80,
    }.ToFrozenDictionary();

    public static readonly FrozenDictionary<AdditionalModifier, int> AdditionalModifiersPriorities = new Dictionary<AdditionalModifier, int>()
    {
        [AdditionalModifier.Const] = 00,
        [AdditionalModifier.StaticReadonly] = 10,
        [AdditionalModifier.Static] = 20,
        [AdditionalModifier.Readonly] = 30,
        [AdditionalModifier.None] = 40,
    }.ToFrozenDictionary();
}
