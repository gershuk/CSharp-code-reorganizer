namespace CSharpCodeReorganizer.Core.MemberData;

public readonly record struct MemberInfo(string? Identifier = default,
                                         AccessModifier AccessModifier = AccessModifier.None,
                                         AdditionalModifier AdditionalModifier = AdditionalModifier.None,
                                         MemberType MemberType = MemberType.None);