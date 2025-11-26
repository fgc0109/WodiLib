// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : StringValueObjectGenerator.cs
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
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using WodiLib.SourceGenerator.Core.Templates.FromAttribute;
using WodiLib.SourceGenerator.ValueObject.Extensions;
using WodiLib.SourceGenerator.ValueObject.Generation.Helper;
using WodiLib.SourceGenerator.ValueObject.Generation.Main.SingleValues.Abstract;
using MyAttr = WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Attributes.StringValueObjectAttribute;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.ValueObject.Generation.Main.SingleValues
{
    /// <summary>
    ///     単一 <see cref="string"/> 値を持つ値オブジェクトジェネレータ
    /// </summary>
    internal class StringValueObjectGenerator : SingleValueObjectGeneratorTemplate
    {
        /// <inheritdoc/>
        public override InitializeAttributeSourceAddable TargetAttribute => MyAttr.Instance;

        /// <inheritdoc/>
        private protected sealed override Type WrapType => typeof(string);

        /// <inheritdoc/>
        private protected override bool IsImplementFormattable(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol typeSymbol,
            AttributeData selfAttributeData,
            ILogger logger
        )
            => false;

        /// <inheritdoc/>
        private protected override bool ParentIsImplementFormattable(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol typeSymbol,
            AttributeData selfAttributeData,
            ILogger logger
        )
            => false;

        private protected override SourceFormatTargetBlock SourceFormatTargetsPublicStaticProperties(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol typeSymbol,
            AttributeData selfAttributeData,
            ILogger logger
        )
        {
            var isValidatePattern = IsValidatePattern(selfAttributeData);
            var isValidateSafetyPattern = IsValidateSafetyPattern(selfAttributeData);
            var isValidateAnyPattern = isValidatePattern || isValidateSafetyPattern;
            var isValidateByteLength = IsValidateByteLength(selfAttributeData);
            var isValidateLength = IsValidateLength(selfAttributeData);

            if (new[]
                {
                    isValidateAnyPattern,
                    isValidateByteLength,
                    isValidateLength,
                }.All(b => !b)) return Array.Empty<SourceFormatTarget>();

            var isNewMaxMinLength =
                typeSymbol.IsParentImplementsPropertyRecursive(MyAttr.MaxLength, selfAttributeData)
                || typeSymbol.IsParentImplementsPropertyRecursive(MyAttr.MinLength, selfAttributeData);
            var isNewByteMaxMinLength =
                typeSymbol.IsParentImplementsPropertyRecursive(MyAttr.ByteMaxLength, selfAttributeData)
                || typeSymbol.IsParentImplementsPropertyRecursive(MyAttr.ByteMinLength, selfAttributeData);

            return SourceTextFormatter.Format(
                "",
                SourceTextFormatter.If(
                    isValidatePattern,
                    SourceFormatTargetHelper.SourceFormatTargetsClassConstant_String(
                        MyAttr.Pattern,
                        selfAttributeData,
                        isOverwrittenProperty: isNewMaxMinLength
                    )
                ),
                SourceTextFormatter.If(
                    isValidateSafetyPattern,
                    SourceFormatTargetHelper.SourceFormatTargetsClassConstant_String(
                        MyAttr.SafetyPattern,
                        selfAttributeData
                    )
                ),
                SourceTextFormatter.If(
                    isValidateAnyPattern,
                    SourceFormatTargetHelper.SourceFormatTargetsClassConstant_Enum(
                        MyAttr.PatternOption,
                        selfAttributeData
                    )
                ),
                SourceTextFormatter.If(
                    isValidateByteLength,
                    SourceFormatTargetHelper.SourceFormatTargetsClassConstant_Numeric(
                        MyAttr.ByteMaxLength,
                        selfAttributeData,
                        isOverwrittenProperty: isNewByteMaxMinLength
                    ),
                    SourceFormatTargetHelper.SourceFormatTargetsClassConstant_Numeric(
                        MyAttr.ByteMinLength,
                        selfAttributeData,
                        isOverwrittenProperty: isNewByteMaxMinLength
                    )
                ),
                SourceTextFormatter.If(
                    isValidateLength,
                    SourceFormatTargetHelper.SourceFormatTargetsClassConstant_Numeric(
                        MyAttr.MaxLength,
                        selfAttributeData,
                        isOverwrittenProperty: isNewMaxMinLength
                    ),
                    SourceFormatTargetHelper.SourceFormatTargetsClassConstant_Numeric(
                        MyAttr.MinLength,
                        selfAttributeData,
                        isOverwrittenProperty: isNewMaxMinLength
                    )
                )
            );
        }

        /// <inheritdoc/>
        private protected override SourceFormatTargetBlock SourceFormatTargetsConstructorException(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol typeSymbol,
            AttributeData selfAttributeData,
            ILogger logger
        )
        {
            return SourceTextFormatter.Format(
                "",
                SourceFormatTargetsConstructorArgumentNullException(),
                SourceFormatTargetsConstructorArgumentOutOfRangeException(selfAttributeData),
                SourceFormatTargetsConstructorArgumentException(selfAttributeData)
            );
        }

        /// <inheritdoc/>
        private protected override SourceFormatTargetBlock SourceFormatTargetsConstructorBody(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol typeSymbol,
            AttributeData selfAttributeData,
            ILogger logger
        )
        {
            return SourceTextFormatter.Format(
                "",
                SourceFormatTargetsValidateNotNull(),
                SourceFormatTargetsValidateNotEmpty(selfAttributeData),
                SourceFormatTargetsValidatePattern(selfAttributeData),
                SourceFormatTargetsValidateNewLine(selfAttributeData),
                SourceFormatTargetsValidateByteLength(selfAttributeData),
                SourceFormatTargetsValidateLength(selfAttributeData),
                SourceTextFormatter.If(
                    // Rootクラスのみ
                    !typeSymbol.IsExtended(),
                    new SourceFormatTarget[]
                    {
                        // DoConstructorExpansion メソッドで RawValue を更新する可能性があるので RawValue の初期化を先に行う
                        $"{selfAttributeData.GetPropertyDataRecursive<string>(MyAttr.PropertyName.Name)} = value;",
                        $"DoConstructorExpansion(value);",
                    }
                )
            );
        }

        /// <inheritdoc/>
        private protected override SourceFormatTargetBlock SourceFormatTargetsExtendBody(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol typeSymbol,
            AttributeData selfAttributeData,
            ILogger logger
        )
        {
            var operationOverloadCodeMaker = new OperationOverloadCodeMaker(
                typeSymbol.Name,
                typeSymbol.Namespace(),
                selfAttributeData.GetPropertyDataRecursive<string>(MyAttr.PropertyName.Name)!,
                WrapType.FullName!
            );

            return SourceTextFormatter.Format(
                "",
                operationOverloadCodeMaker.BinaryOperatorNewInstance(
                    "+",
                    new[] { typeSymbol.FullName() },
                    !typeSymbol.IsAbstract
                ),
                new SourceFormatTarget[]
                {
                    $"partial void DoConstructorExpansion(string value);",
                }
            );
        }

        /// <summary>インスタンス（シングルトン）</summary>
        public static StringValueObjectGenerator Instance { get; } = new();

        /// <returns>空文字チェックを行う場合<see langword="true"/></returns>
        private static bool IsValidateEmpty(
            AttributeData selfAttributeData
        )
        {
            return selfAttributeData.GetPropertyDataRecursive<bool?>(MyAttr.IsAllowEmpty.Name) == false;
        }

        /// <returns>改行チェックを行う場合<see langword="true"/></returns>
        private static bool IsValidateNewLine(
            AttributeData selfAttributeData
        )
        {
            return selfAttributeData.GetPropertyDataRecursive<bool?>(MyAttr.IsAllowNewLine.Name) == false;
        }

        /// <returns>正規表現による必須または安全パターチェックを行う場合<see langword="true"/></returns>
        private static bool IsValidateAnyPattern(
            AttributeData selfAttributeData
        )
        {
            return IsValidatePattern(selfAttributeData) || IsValidateSafetyPattern(selfAttributeData);
        }

        /// <returns>正規表現によるパターチェックを行う場合<see langword="true"/></returns>
        private static bool IsValidatePattern(
            AttributeData selfAttributeData
        )
        {
            return selfAttributeData.GetPropertyDataRecursive<string?>(MyAttr.Pattern.Name) is not null;
        }

        /// <returns>正規表現による安全パターチェックを行う場合<see langword="true"/></returns>
        private static bool IsValidateSafetyPattern(
            AttributeData selfAttributeData
        )
        {
            return selfAttributeData.GetPropertyDataRecursive<string?>(MyAttr.SafetyPattern.Name) is not null;
        }

        /// <returns>範囲チェックを行う場合 <see langword="true"/></returns>
        private static bool IsValidateOutOfRange(
            AttributeData selfAttributeData
        )
            => IsValidateByteLength(selfAttributeData) || IsValidateLength(selfAttributeData);

        /// <returns>バイトサイズの範囲チェックを行う場合 <see langword="true"/></returns>
        private static bool IsValidateByteLength(
            AttributeData selfAttributeData
        )
        {
            var isDefaultByteMax = ToInt32FromIntString((string)MyAttr.ByteMaxLength.DefaultValue!)
                .ToString()
                .Equals(
                    selfAttributeData.GetPropertyDataRecursive<int?>(MyAttr.ByteMaxLength.Name).ToString()
                );
            var isDefaultByteMin = ((int)MyAttr.ByteMinLength.DefaultValue!).ToString()
                .Equals(selfAttributeData.GetPropertyDataRecursive<int?>(MyAttr.ByteMinLength.Name).ToString());
            return !isDefaultByteMax || !isDefaultByteMin;
        }

        /// <returns>文字列長の範囲チェックを行う場合 <see langword="true"/></returns>
        private static bool IsValidateLength(
            AttributeData selfAttributeData
        )
        {
            var isDefaultMaxLength = ToInt32FromIntString((string)MyAttr.MaxLength.DefaultValue!)
                .ToString()
                .Equals(selfAttributeData.GetPropertyDataRecursive<int?>(MyAttr.MaxLength.Name).ToString());
            var isDefaultMinLength = ((int)MyAttr.MinLength.DefaultValue!).ToString()
                .Equals(selfAttributeData.GetPropertyDataRecursive<int?>(MyAttr.MinLength.Name).ToString());
            return !isDefaultMaxLength || !isDefaultMinLength;
        }

        /// <returns><see cref="ArgumentNullException"/> ドキュメントコメント文字列パート</returns>
        private static SourceFormatTargetBlock SourceFormatTargetsConstructorArgumentNullException()
        {
            return SourceTextFormatter.Format(
                "",
                new SourceFormatTarget[]
                {
                    $@"/// <exception cref=""{typeof(ArgumentNullException).FullName}"">",
                    $"///     {Sentence.ErrorDesc.Null(Tag.ParamRef("value"))}",
                    $"/// </exception>",
                }
            );
        }

        /// <returns><see cref="ArgumentOutOfRangeException"/> ドキュメントコメント文字列パート</returns>
        private static SourceFormatTargetBlock SourceFormatTargetsConstructorArgumentOutOfRangeException(
            AttributeData selfAttributeData
        )
        {
            return SourceTextFormatter.Format(
                    "",
                    SourceTextFormatter.ReduceMany(
                        $"/// {__}",
                        "、または",
                        SourceTextFormatter.If(
                            IsValidateByteLength(selfAttributeData),
                            new SourceFormatTarget[]
                            {
                                $"{Sentence.ErrorDesc.OutOfRange($"{Tag.ParamRef("value")} のデータサイズ", Tag.See.Cref(MyAttr.ByteMinLength.Name), Tag.See.Cref(MyAttr.ByteMaxLength.Name))}",
                            }
                        ),
                        SourceTextFormatter.If(
                            IsValidateOutOfRange(selfAttributeData),
                            new SourceFormatTarget[]
                            {
                                $"{Sentence.ErrorDesc.OutOfRange($"{Tag.ParamRef("value")} の文字列長", Tag.See.Cref(MyAttr.MinLength.Name), Tag.See.Cref(MyAttr.MaxLength.Name))}",
                            }
                        )
                    )
                )
                .AppendPrefixAndSuffixIfNotEmpty(
                    $@"/// <exception cref=""{typeof(ArgumentOutOfRangeException).FullName}"">",
                    $"/// </exception>"
                );
        }

        /// <returns><see cref="ArgumentException"/> ドキュメントコメント文字列パート</returns>
        private static SourceFormatTargetBlock SourceFormatTargetsConstructorArgumentException(
            AttributeData selfAttributeData
        )
        {
            return SourceTextFormatter.Format(
                    "",
                    SourceTextFormatter.ReduceMany(
                        $"/// {__}",
                        "、または",
                        SourceTextFormatter.If(
                            IsValidateEmpty(selfAttributeData),
                            new SourceFormatTarget[]
                            {
                                $"{Tag.ParamRef("value")} が 空文字 の場合",
                            }
                        ),
                        SourceTextFormatter.If(
                            IsValidateNewLine(selfAttributeData),
                            new SourceFormatTarget[]
                            {
                                $"{Tag.ParamRef("value")} に改行コードが含まれる場合",
                            }
                        ),
                        SourceTextFormatter.If(
                            IsValidatePattern(selfAttributeData),
                            new SourceFormatTarget[]
                            {
                                $"{Tag.ParamRef("value")} が {Tag.See.Cref(MyAttr.Pattern.Name)} を満たさない場合",
                            }
                        )
                    )
                )
                .AppendPrefixAndSuffixIfNotEmpty(
                    $@"/// <exception cref=""{typeof(ArgumentException).FullName}"">",
                    $"/// </exception>"
                );
        }

        /// <returns>非 <see langword="null"/> チェックソースコード文字列パート</returns>
        private static SourceFormatTargetBlock SourceFormatTargetsValidateNotNull()
            => SourceTextFormatter.Format(
                "",
                new SourceFormatTarget[]
                {
                    $"{{   // Validate NotNull",
                    $"    if (value is null) throw new System.ArgumentNullException(nameof(value));",
                    $"}}",
                }
            );

        /// <returns>非空文字チェックソースコード文字列パート</returns>
        private static SourceFormatTargetBlock SourceFormatTargetsValidateNotEmpty(
            AttributeData selfAttributeData
        )
            => SourceTextFormatter.If(
                IsValidateEmpty(selfAttributeData),
                new SourceFormatTarget[]
                {
                    $"{{   // Validate NotEmpty",
                    $@"    if (value.Equals("""")) throw new System.ArgumentException($""{{nameof(value)}} cannot empty string."");",
                    $"}}",
                }
            );

        /// <returns>文字列パターンチェックソースコード文字列パート</returns>
        private static SourceFormatTargetBlock SourceFormatTargetsValidatePattern(
            AttributeData selfAttributeData
        )
            => SourceTextFormatter.If(
                IsValidateAnyPattern(selfAttributeData),
                new SourceFormatTarget[]
                {
                    $"{{   // Validate for Pattern",
                    ($"    var requireRegex = new System.Text.RegularExpressions.Regex({MyAttr.Pattern.Name}, (System.Text.RegularExpressions.RegexOptions) {MyAttr.PatternOption.Name});",
                        IsValidatePattern(selfAttributeData)),
                    ($@"    if (!requireRegex.IsMatch(value)) throw new System.ArgumentException(""No match pattern."", nameof(value));",
                        IsValidatePattern(selfAttributeData)),
                    ($"    var safetyRegex = new System.Text.RegularExpressions.Regex({MyAttr.SafetyPattern.Name}, (System.Text.RegularExpressions.RegexOptions) {MyAttr.PatternOption.Name});",
                        IsValidateSafetyPattern(selfAttributeData)),
                    ($"    if (!safetyRegex.IsMatch(value)) WodiLib.Sys.Cmn.WodiLibLogger.GetInstance().Warning(WodiLib.Sys.WarningMessage.NotMatchRegex(value, safetyRegex));",
                        IsValidateSafetyPattern(selfAttributeData)),
                    $"}}",
                }
            );

        /// <returns>改行チェックソースコード文字列パート</returns>
        private static SourceFormatTargetBlock SourceFormatTargetsValidateNewLine(
            AttributeData selfAttributeData
        )
            => SourceTextFormatter.If(
                IsValidateNewLine(selfAttributeData),
                new SourceFormatTarget[]
                {
                    $"{{   // Validate NewLine",
                    $@"    if ( value.Contains(""\n"") || value.Contains(""\r\n"") ) {{ throw new System.ArgumentException($""Cannot use NewLine for {{nameof(value)}}. (value: {{value}})""); }}",
                    $"}}",
                }
            );

        /// <returns>バイトサイズチェックソースコード文字列パート</returns>
        private static SourceFormatTargetBlock SourceFormatTargetsValidateByteLength(
            AttributeData selfAttributeData
        )
            => SourceTextFormatter.If(
                IsValidateByteLength(selfAttributeData),
                new SourceFormatTarget[]
                {
                    $"{{   // Validate for ByteLength",
                    $@"    var encoding = System.Text.Encoding.GetEncoding(""{selfAttributeData.GetPropertyDataRecursive<string>(MyAttr.ByteLengthEncoding.Name)}"");",
                    $"    var byteLength = encoding.GetByteCount(value);",
                    $@"    if (byteLength < {MyAttr.ByteMinLength.Name} || {MyAttr.ByteMaxLength.Name} < byteLength) throw new System.ArgumentException($""string byteLength between {MyAttr.ByteMinLength.Name} and {MyAttr.ByteMaxLength.Name}"");",
                    $"}}",
                }
            );

        /// <returns>文字列長チェックソースコード文字列パート</returns>
        private static SourceFormatTargetBlock SourceFormatTargetsValidateLength(
            AttributeData selfAttributeData
        )
            => SourceTextFormatter.If(
                IsValidateLength(selfAttributeData),
                new SourceFormatTarget[]
                {
                    $"{{   // Validate for Length",
                    $"    var length = value.Length;",
                    $@"    if (length < {MyAttr.MinLength.Name} || {MyAttr.MaxLength.Name} < length) throw new System.ArgumentException($""string length between {MyAttr.MinLength.Name} and {MyAttr.MaxLength.Name}"");",
                    $"}}",
                }
            );

        /// <summary>
        ///     int 文字列を int に変換する。
        /// </summary>
        /// <param name="src">変換対象</param>
        /// <returns>変換結果</returns>
        private static int ToInt32FromIntString(string src)
        {
            return src switch
            {
                "int.MaxValue" => int.MaxValue,
                "int.MinValue" => int.MinValue,
                _ => int.Parse(src),
            };
        }
    }
}
