using CSharpCodeReorganizer.Core.MemberData;

namespace CSharpCodeReorganizer.Core;

public static class MemberInfoExtensions
{
    public static MemberInfo GetMemberInfo(this MemberDeclarationSyntax memberDeclaration)
    {
        ArgumentNullException.ThrowIfNull(memberDeclaration);

        return new MemberInfo()
        {
            Identifier = GetMemberName(memberDeclaration),
            AccessModifier = GetMemberAccessModifier(memberDeclaration),
            AdditionalModifier = GetMemberAdditionalModifier(memberDeclaration),
            MemberType = GetMemberType(memberDeclaration),
        };
    }

    public static string GetMemberName(this MemberDeclarationSyntax memberDeclaration) =>
        memberDeclaration switch
        {
            NamespaceDeclarationSyntax declaration => declaration.Name.ToString(),
            FieldDeclarationSyntax declaration => string.Join(", ",
                                                            declaration.Declaration.Variables.Select(static variableDeclaration => variableDeclaration.Identifier)),
            DelegateDeclarationSyntax declaration => declaration.Identifier.ToString(),
            EventFieldDeclarationSyntax declaration => string.Join(", ",
                                                            declaration.Declaration.Variables.Select(static variableDeclaration => variableDeclaration.Identifier)),
            EventDeclarationSyntax declaration => declaration.Identifier.ToString(),
            EnumDeclarationSyntax declaration => declaration.Identifier.ToString(),
            InterfaceDeclarationSyntax declaration => declaration.Identifier.ToString(),
            PropertyDeclarationSyntax declaration => declaration.Identifier.ToString(),
            MethodDeclarationSyntax declaration => declaration.Identifier.ToString(),
            StructDeclarationSyntax declaration => declaration.Identifier.ToString(),
            ClassDeclarationSyntax declaration => declaration.Identifier.ToString(),
            RecordDeclarationSyntax declaration => declaration.Identifier.ToString(),
            ConstructorDeclarationSyntax declaration => declaration.Identifier.ToString(),
            DestructorDeclarationSyntax declaration => declaration.Identifier.ToString(),
            IndexerDeclarationSyntax declaration => "this",
            OperatorDeclarationSyntax declaration => declaration.OperatorToken.Text,
            // TODO : Replace with full cover
            _ => string.Empty,
        };

    public static MemberType GetMemberType(this MemberDeclarationSyntax memberDeclaration) =>
        memberDeclaration.Kind() switch
        {
            SyntaxKind.FieldDeclaration => MemberType.Field,
            SyntaxKind.ConstructorDeclaration => MemberType.Constructor,
            SyntaxKind.DestructorDeclaration => MemberType.Destructor,
            SyntaxKind.DelegateDeclaration => MemberType.Delegate,
            SyntaxKind.EventDeclaration => MemberType.Event,
            SyntaxKind.EventFieldDeclaration => MemberType.Event,
            SyntaxKind.EnumDeclaration => MemberType.Enum,
            SyntaxKind.InterfaceDeclaration => MemberType.Interface,
            SyntaxKind.PropertyDeclaration => MemberType.Property,
            SyntaxKind.IndexerDeclaration => MemberType.Indexer,
            SyntaxKind.OperatorDeclaration => MemberType.Operator,
            SyntaxKind.MethodDeclaration => MemberType.Method,
            SyntaxKind.StructDeclaration => MemberType.Struct,
            SyntaxKind.ClassDeclaration => MemberType.Class,
            SyntaxKind.RecordDeclaration => MemberType.Record,
            SyntaxKind.RecordStructDeclaration => MemberType.RecordStruct,
            SyntaxKind.NamespaceDeclaration => MemberType.Namespace,
            // TODO : Replace with full cover
            _ => MemberType.None,
        };

    public static AccessModifier GetMemberAccessModifier(this MemberDeclarationSyntax memberDeclaration)
    {
        var modifierKinds = memberDeclaration.Modifiers.Select(token => token.Kind()).ToHashSet();

        var accessModifier = AccessModifier.None;

        if (modifierKinds.Count != 0
            || memberDeclaration is not MethodDeclarationSyntax { ExplicitInterfaceSpecifier: not null }
                                 and not BasePropertyDeclarationSyntax { ExplicitInterfaceSpecifier: not null })
        {

            if (modifierKinds.Contains(SyntaxKind.PublicKeyword))
                accessModifier |= AccessModifier.Public;

            if (modifierKinds.Contains(SyntaxKind.InternalKeyword))
                accessModifier |= AccessModifier.Internal;

            if (modifierKinds.Contains(SyntaxKind.ProtectedKeyword))
                accessModifier |= AccessModifier.Protected;

            if (modifierKinds.Contains(SyntaxKind.PrivateKeyword))
                accessModifier |= AccessModifier.Private;

            if (modifierKinds.Contains(SyntaxKind.FileKeyword))
                accessModifier |= AccessModifier.File;
        }
        else
        {
            accessModifier = AccessModifier.ExplicitInterfaceImplementation;
        }

        return accessModifier;
    }


    public static AdditionalModifier GetMemberAdditionalModifier(this MemberDeclarationSyntax memberDeclaration)
    {
        var modifierKinds = memberDeclaration.Modifiers.Select(token => token.Kind()).ToHashSet();
        var kindsCount = modifierKinds.Count;
        var isConst = modifierKinds.Contains(SyntaxKind.ConstKeyword);
        var isStatic = modifierKinds.Contains(SyntaxKind.StaticKeyword);
        var isReadOnly = modifierKinds.Contains(SyntaxKind.ReadOnlyKeyword);

        return (kindsCount, isConst, isStatic, isReadOnly) switch
        {
            (0, _, _, _) => AdditionalModifier.None,
            ( > 0, true, _, _) => AdditionalModifier.Const,
            ( > 0, _, true, true) => AdditionalModifier.StaticReadonly,
            ( > 0, _, _, true) => AdditionalModifier.Readonly,
            ( > 0, _, true, _) => AdditionalModifier.Static,
            _ => AdditionalModifier.None,
        };
    }
}
