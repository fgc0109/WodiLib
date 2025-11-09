// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : SingleValueObjectGeneratorTemplate.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WodiLib.SourceGenerator.Core;
using WodiLib.SourceGenerator.Core.Extensions;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using WodiLib.SourceGenerator.Core.Templates.FromAttribute;
using WodiLib.SourceGenerator.ValueObject.Extensions;
using WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Enums;
using MyAttr =
    WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Attributes.Abstract.SingleValueObjectAttribute;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.ValueObject.Generation.Main.SingleValues.Abstract
{
    /// <summary>
    ///     単一値オブジェクトジェネレータのテンプレートクラス
    /// </summary>
    internal abstract partial class SingleValueObjectGeneratorTemplate : MainSourceAddableTemplate
    {
        /// <summary>
        ///     内部に保持する値の型
        /// </summary>
        private protected abstract Type WrapType { get; }

        /// <inheritdoc/>
        private protected sealed override SourceFormatTargetBlock GenerateTypeDefinitionSource(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol source,
            AttributeData selfAttributeData,
            ILogger logger
        )
        {
            var thisTypeName = source.ClassName();
            var accessibility = AccessibilityConverter.ConvertSourceText(source.DeclaredAccessibility);
            var isExtended = source.IsExtended();

            var propertyName = selfAttributeData.GetPropertyDataRecursive<string>(MyAttr.PropertyName.Name);
            var castType = selfAttributeData.GetPropertyDataRecursive<int?>(MyAttr.CastType.Name).ToString();

            var implInterfaces = GetImplementInterfaceSentence(
                thisTypeName,
                semanticModel,
                typeDecl,
                source,
                selfAttributeData,
                logger
            );
            var canOperation = CastType.CanOperation(castType);
            var castOperation = CastType.ToSourceText(castType);

            var classOrRecordOrStruct = source.IsRecord
                ? "record"
                : source.TypeKind == TypeKind.Class
                    ? "class"
                    : "struct";

            var sourceCustomizer = GetSourceCustomizer(source);

            var parentPropertyName =
                selfAttributeData.GetParentPropertyDataRecursive<string?>(MyAttr.PropertyName.Name);
            var newModifierRawValue = isExtended
                ? "new "
                : "";
            var rawValueBody = isExtended
                ? $"=> base.{parentPropertyName};"
                : "{ get; }";

            var wrapTypeIsClass = WrapType.IsClass;

            return SourceTextFormatter.Format(
                "",
                new SourceFormatTarget[]
                {
                    $"{accessibility} partial {classOrRecordOrStruct} {thisTypeName} {implInterfaces.PrefixIfNotEmpty(" : ")}",
                    $"{{",
                },
                SourceTextFormatter.Format(
                        IndentSpace,
                        SourceFormatTargetsPublicStaticProperties(
                            semanticModel,
                            typeDecl,
                            source,
                            selfAttributeData,
                            logger
                        ),
                        new[]
                        {
                            $"/// <summary>{WrapType}値</summary>",
                            $"public {newModifierRawValue}{WrapType} {propertyName} {rawValueBody}",
                            SourceFormatTarget.Empty,
                            $"/// <summary>",
                            $"/// {__}コンストラクタ",
                            $"/// </summary>",
                            $"/// {Tag.Param("value", "設定値")}",
                        },
                        SourceFormatTargetsConstructorException(
                            semanticModel,
                            typeDecl,
                            source,
                            selfAttributeData,
                            logger
                        ),
                        new SourceFormatTarget[]
                        {
                            ($"public {thisTypeName}({WrapType} value)", !isExtended),
                            ($"public {thisTypeName}({WrapType} value) : base(value)", isExtended),
                            $"{{",
                        },
                        SourceTextFormatter.Format(
                            IndentSpace,
                            SourceFormatTargetsConstructorBody(
                                semanticModel,
                                typeDecl,
                                source,
                                selfAttributeData,
                                logger
                            )
                        ),
                        new[]
                        {
                            $"}}",
                            SourceFormatTarget.Empty,
                        },
                        SourceTextFormatter.If(
                            IsOverrideBasicMethods(semanticModel, typeDecl, source, selfAttributeData, logger),
                            new[]
                            {
                                $"/// {Tag.InheritDoc()}",
                                $"public override string ToString() => {propertyName}.ToString();",
                                SourceFormatTarget.Empty,
                                $"/// {Tag.InheritDoc()}",
                                sourceCustomizer.SourceFormatTargetEqualsObject(
                                    semanticModel,
                                    typeDecl,
                                    source,
                                    selfAttributeData,
                                    logger
                                ),
                                SourceFormatTarget.Empty,
                                $"/// {Tag.InheritDoc()}",
                                $"public override int GetHashCode() => {propertyName}.GetHashCode();",
                                SourceFormatTarget.Empty,
                            }
                        ),
                        SourceTextFormatter.If(
                            IsImplementEquatable(semanticModel, typeDecl, source, selfAttributeData, logger),
                            new[]
                            {
                                $"/// {Tag.InheritDoc("System.IEquatable{T}.Equals(T)")}",
                                sourceCustomizer.SourceFormatTargetEqualsOther(
                                    semanticModel,
                                    typeDecl,
                                    source,
                                    selfAttributeData,
                                    logger
                                ),
                                SourceFormatTarget.Empty,
                                $"/// {Tag.Summary("== 演算子")}",
                                $"/// {Tag.Param("left", "左項")}",
                                $"/// {Tag.Param("right", "右項")}",
                                $"/// {Tag.Returns($"{Tag.ParamRef("left")} と {Tag.ParamRef("right")} が同一要素である場合 {Tag.See.Langword_True}")}",
                                $"public static bool operator ==({thisTypeName}? left, {thisTypeName}? right) => Equals(left, right);",
                                $"/// {Tag.Summary("!= 演算子")}",
                                $"/// {Tag.Param("left", "左項")}",
                                $"/// {Tag.Param("right", "右項")}",
                                $"/// {Tag.Returns($"{Tag.ParamRef("left")} と {Tag.ParamRef("right")} が同一要素ではない場合 {Tag.See.Langword_True}")}",
                                $"public static bool operator !=({thisTypeName}? left, {thisTypeName}? right) => !Equals(left, right);",
                                SourceFormatTarget.Empty,
                            }
                        ),
                        SourceTextFormatter.If(
                            IsImplementFormattable(semanticModel, typeDecl, source, selfAttributeData, logger),
                            () =>
                            {
                                var needNewModifier = ParentIsImplementFormattable(
                                    semanticModel,
                                    typeDecl,
                                    source,
                                    selfAttributeData,
                                    logger
                                );
                                var newModifierStr = needNewModifier
                                    ? "new "
                                    : "";

                                return new[]
                                {
                                    $"/// {Tag.InheritDoc("int.ToString(string)")}",
                                    $"public {newModifierStr}string ToString(string format) => {propertyName}.ToString(format);",
                                    SourceFormatTarget.Empty,
                                    $"/// {Tag.InheritDoc("int.ToString(System.IFormatProvider)")}",
                                    $"public {newModifierStr}string ToString(System.IFormatProvider formatProvider) => {propertyName}.ToString(formatProvider);",
                                    SourceFormatTarget.Empty,
                                    $"/// {Tag.InheritDoc("System.IFormattable.ToString(string?, System.IFormatProvider?)")}",
                                    $"public {newModifierStr}string ToString(string? format, System.IFormatProvider? formatProvider) => {propertyName}.ToString(format, formatProvider);",
                                    SourceFormatTarget.Empty,
                                };
                            }
                        ),
                        SourceTextFormatter.If(
                            IsImplementComparable(semanticModel, typeDecl, source, selfAttributeData, logger),
                            () =>
                            {
                                return new[]
                                {
                                    $"/// {Tag.InheritDoc($"System.IComparable{{T}}.CompareTo")}",
                                    $"public int CompareTo({thisTypeName}? other) => {propertyName}.CompareTo(other?.{propertyName});",
                                    SourceFormatTarget.Empty,
                                };
                            }
                        ),
                        SourceTextFormatter.If(
                            canOperation,
                            new[]
                            {
                                ($"/// {Tag.Summary($"{WrapType} から {thisTypeName} への {CastType.ToDocumentText(castType)}な型変換")}",
                                    !source.IsAbstract),
                                ($"/// {Tag.Param("value", "変換対象")}", !source.IsAbstract),
                                ($"/// {Tag.Returns("変換結果")}", !source.IsAbstract),
                                ($@"[return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(""value"")]",
                                    !source.IsAbstract),
                                ($"public static {castOperation} operator {thisTypeName}?({WrapType}? value) => value is null ? null : new {thisTypeName}(({WrapType}) value);",
                                    !source.IsAbstract),
                                ($"", !source.IsAbstract),
                                $"/// {Tag.Summary($"{thisTypeName} から {WrapType} への {CastType.ToDocumentText(castType)}な型変換")}",
                                $"/// {Tag.Param("value", "変換対象")}",
                                $"/// {Tag.Returns("変換結果")}",
                                $@"[return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(""value"")]",
                                $"public static {castOperation} operator {WrapType}?({thisTypeName}? value) => value?.{propertyName};",
                                SourceFormatTarget.Empty,
                            }
                        ),
                        SourceTextFormatter.If(
                            canOperation && !wrapTypeIsClass,
                            // 構造体の場合のみ、nullable ではいない場合のキャストを別途定義
                            new[]
                            {
                                ($"/// {Tag.Summary($"{WrapType} から {thisTypeName} への {CastType.ToDocumentText(castType)}な型変換")}",
                                    !source.IsAbstract),
                                ($"/// {Tag.Param("value", "変換対象")}", !source.IsAbstract),
                                ($"/// {Tag.Returns("変換結果")}", !source.IsAbstract),
                                ($"public static {castOperation} operator {thisTypeName}({WrapType} value) => new {thisTypeName}(value);",
                                    !source.IsAbstract),
                                ($"", !source.IsAbstract),
                                $"/// {Tag.Summary($"{thisTypeName} から {WrapType} への {CastType.ToDocumentText(castType)}な型変換")}",
                                $"/// {Tag.Param("value", "変換対象")}",
                                $"/// {Tag.Returns("変換結果")}",
                                $"public static {castOperation} operator {WrapType}({thisTypeName} value) => value.{propertyName};",
                                SourceFormatTarget.Empty,
                            }
                        ),
                        SourceFormatTargetsExtendBody(semanticModel, typeDecl, source, selfAttributeData, logger)
                    )
                    .TrimLastEmptyLine(),
                new SourceFormatTarget[]
                {
                    $"}}",
                }
            );
        }

        /// <summary>
        ///     <see cref="IEquatable{T}"/> 実装可否
        /// </summary>
        /// <returns><see cref="IEquatable{T}"/> を実装する場合 <see langword="true"/></returns>
        private protected virtual bool IsImplementEquatable(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol source,
            AttributeData selfAttributeData,
            ILogger logger
        )
            => !source.IsRecord
               && (selfAttributeData.GetPropertyDataRecursive<bool?>(MyAttr.ImplementEquatable.Name)! ?? false);

        /// <summary>
        ///     <see cref="IFormattable"/> 実装可否
        /// </summary>
        /// <returns><see cref="IFormattable"/> を実装する場合 <see langword="true"/></returns>
        private protected abstract bool IsImplementFormattable(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol source,
            AttributeData selfAttributeData,
            ILogger logger
        );

        /// <summary>
        ///     親クラスが <see cref="IFormattable"/> を継承しているかどうかを返す。
        /// </summary>
        private protected abstract bool ParentIsImplementFormattable(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol source,
            AttributeData selfAttributeData,
            ILogger logger
        );

        /// <summary>
        ///     <see cref="IComparable{T}"/> 実装可否
        /// </summary>
        /// <returns><see cref="IComparable{T}"/> を実装する場合 <see langword="true"/></returns>
        private protected virtual bool IsImplementComparable(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol source,
            AttributeData selfAttributeData,
            ILogger logger
        )
            => selfAttributeData.GetPropertyDataRecursive<bool?>(MyAttr.IsComparable.Name) ?? false;

        /// <summary>
        ///     親クラスが <see cref="IComparable{T}"/> を継承しているかどうかを返す。
        /// </summary>
        private protected virtual bool ParentIsImplementComparable(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol source,
            AttributeData selfAttributeData,
            ILogger logger
        )
        {
            if (!source.IsExtended())
            {
                return false;
            }

            // 親クラスが IComparable を継承しているかどうかを調べる。
            var baseType = source.BaseType!;
            return baseType.AllInterfaces.Any(x =>
                x.ToDisplayString() == "System.IComparable"
                || x.OriginalDefinition.ToDisplayString() == "System.IComparable<T>"
            );
        }

        /// <summary>
        ///     コンストラクタ XML ドキュメント例外説明
        /// </summary>
        /// <returns>コード文字列情報</returns>
        private protected abstract SourceFormatTargetBlock SourceFormatTargetsConstructorException(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol source,
            AttributeData selfAttributeData,
            ILogger logger
        );

        /// <summary>
        ///     コンストラクタ本体ソースコード
        /// </summary>
        /// <returns>コード文字列情報</returns>
        private protected abstract SourceFormatTargetBlock SourceFormatTargetsConstructorBody(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol source,
            AttributeData selfAttributeData,
            ILogger logger
        );

        /// <summary>
        ///     Object 基本メソッドオーバーライド要否
        /// </summary>
        /// <returns><see cref="object"/> の基本メソッドを継承する場合 <see langword="true"/></returns>
        private protected virtual bool IsOverrideBasicMethods(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol source,
            AttributeData selfAttributeData,
            ILogger logger
        )
            => !source.IsRecord
               && (selfAttributeData.GetPropertyDataRecursive<bool?>(MyAttr.OverrideBasicMethods.Name) ?? false);

        /// <summary>
        ///     public static property 定義コード
        /// </summary>
        /// <returns>ソースコード文字列情報</returns>
        private protected virtual SourceFormatTargetBlock SourceFormatTargetsPublicStaticProperties(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol source,
            AttributeData selfAttributeData,
            ILogger logger
        )
            => Array.Empty<SourceFormatTarget>();

        /// <summary>
        ///     クラス定義本体拡張コード
        /// </summary>
        /// <returns>ソースコード文字列情報</returns>
        private protected virtual SourceFormatTargetBlock SourceFormatTargetsExtendBody(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol source,
            AttributeData selfAttributeData,
            ILogger logger
        )
            => Array.Empty<SourceFormatTarget>();

        /// <summary>
        ///     ソースコードカスタマイズ用処理を取得する。
        /// </summary>
        /// <returns><see cref="ISourceCustomizer"/> インスタンス</returns>
        private static ISourceCustomizer GetSourceCustomizer(
            INamedTypeSymbol source
        )
        {
            if (source.TypeKind == TypeKind.Struct)
            {
                return StructCustomize.Instance;
            }

            return ClassCustomize.Instance;
        }

        /// <summary>
        ///     実装インタフェース宣言ソース文字列を取得する。
        /// </summary>
        /// <returns>ソースコード文字列</returns>
        private string GetImplementInterfaceSentence(
            string className,
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol source,
            AttributeData selfAttributeData,
            ILogger logger
        )
        {
            return new[]
            {
                IsImplementEquatable(semanticModel, typeDecl, source, selfAttributeData, logger)
                    ? $"System.IEquatable<{className}>"
                    : null,
                IsImplementFormattable(semanticModel, typeDecl, source, selfAttributeData, logger)
                    ? "System.IFormattable"
                    : null,
                IsImplementComparable(semanticModel, typeDecl, source, selfAttributeData, logger)
                    ? $"System.IComparable<{className}?>"
                    : null,
            }.JoinWithoutEmpty(",");
        }
    }
}
