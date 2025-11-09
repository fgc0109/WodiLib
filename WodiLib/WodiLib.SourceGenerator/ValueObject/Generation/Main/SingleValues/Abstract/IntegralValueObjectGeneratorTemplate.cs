// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : IntegralValueObjectGeneratorTemplate.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WodiLib.SourceGenerator.Core;
using WodiLib.SourceGenerator.Core.Extensions;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using WodiLib.SourceGenerator.Core.Templates.FromAttribute;
using WodiLib.SourceGenerator.ValueObject.Generation.Helper;
using WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Enums;
using MyAttr =
    WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Attributes.Abstract.
    IntegralNumericValueObjectAttribute;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.ValueObject.Generation.Main.SingleValues.Abstract
{
    /// <summary>
    ///     数値単一値オブジェクトジェネレータのテンプレートクラス
    /// </summary>
    internal abstract class IntegralValueObjectGeneratorTemplate : SingleValueObjectGeneratorTemplate
    {
        /// <inheritdoc/>
        private protected override bool IsImplementEquatable(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol typeSymbol,
            AttributeData selfAttributeData,
            ILogger logger
        )
            => !typeSymbol.IsRecord;

        /// <inheritdoc/>
        private protected sealed override bool IsImplementFormattable(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol typeSymbol,
            AttributeData selfAttributeData,
            ILogger logger
        )
            => selfAttributeData.GetPropertyDataRecursive<bool?>(MyAttr.IsUseBasicFormattable.Name) ?? false;

        private protected sealed override bool ParentIsImplementFormattable(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol typeSymbol,
            AttributeData selfAttributeData,
            ILogger logger
        )
            => typeSymbol.IsExtended();

        /// <inheritdoc/>
        private protected sealed override bool ParentIsImplementComparable(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol typeSymbol,
            AttributeData selfAttributeData,
            ILogger logger
        )
            => IntegralNumericOperation.CanCompare(
                selfAttributeData.GetParentPropertyDataRecursive<int?>(MyAttr.Operations.Name)?.ToString()
            );

        /// <inheritdoc/>
        private protected override SourceFormatTargetBlock SourceFormatTargetsPublicStaticProperties(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol typeSymbol,
            AttributeData selfAttributeData,
            ILogger logger
        )
        {
            var isValidateRange = IsValidateRange(selfAttributeData);
            var isValidateSafetyRange = IsValidateSafetyRange(
                selfAttributeData
            );

            if (!isValidateRange && !isValidateSafetyRange) return Array.Empty<SourceFormatTarget>();

            var isNewMaxMinValue = typeSymbol.IsParentImplementsPropertyRecursive(MyAttr.MaxValue, selfAttributeData)
                                   || typeSymbol.IsParentImplementsPropertyRecursive(
                                       MyAttr.MinValue,
                                       selfAttributeData
                                   );
            var isNewSafetyMaxMinValue =
                typeSymbol.IsParentImplementsPropertyRecursive(MyAttr.SafetyMaxValue, selfAttributeData)
                || typeSymbol.IsParentImplementsPropertyRecursive(MyAttr.SafetyMinValue, selfAttributeData);

            return SourceTextFormatter.Format(
                "",
                SourceTextFormatter.If(
                    isValidateRange,
                    SourceFormatTargetHelper.SourceFormatTargetsClassConstant_Numeric(
                        MyAttr.MaxValue,
                        selfAttributeData,
                        isOverwrittenProperty: isNewMaxMinValue
                    ),
                    SourceFormatTargetHelper.SourceFormatTargetsClassConstant_Numeric(
                        MyAttr.MinValue,
                        selfAttributeData,
                        isOverwrittenProperty: isNewMaxMinValue
                    )
                ),
                SourceTextFormatter.If(
                    isValidateSafetyRange,
                    SourceFormatTargetHelper.SourceFormatTargetsClassConstant_Numeric(
                        MyAttr.SafetyMaxValue,
                        selfAttributeData,
                        isOverwrittenProperty: isNewSafetyMaxMinValue
                    ),
                    SourceFormatTargetHelper.SourceFormatTargetsClassConstant_Numeric(
                        MyAttr.SafetyMinValue,
                        selfAttributeData,
                        isOverwrittenProperty: isNewSafetyMaxMinValue
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
            if (!IsValidateRange(selfAttributeData))
                return Array.Empty<SourceFormatTarget>();

            return SourceTextFormatter.Format(
                "",
                new SourceFormatTarget[]
                {
                    $@"/// <exception cref=""{typeof(ArgumentOutOfRangeException).FullName}"">",
                    $@"/// {__}<paramref name=""value""/> が <see cref=""{MyAttr.MinValue.Name}""/> 未満または <see cref=""{MyAttr.MaxValue.Name}""/> を超える場合。",
                    $"/// </exception>",
                }
            );
        }

        /// <inheritdoc/>
        private protected override SourceFormatTargetBlock SourceFormatTargetsConstructorBody(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol typeSymbol,
            AttributeData selfAttributeData,
            ILogger logger
        ) // => throw new Exception(typeSymbol.BaseType.SpecialType.ToString());
            => SourceTextFormatter.Format(
                "",
                SourceTextFormatter.If(
                    IsValidateRange(selfAttributeData),
                    new SourceFormatTarget[]
                    {
                        $"{{   // value range",
                        $@"{__}if (value < {MyAttr.MinValue.Name} || {MyAttr.MaxValue.Name} < value) throw new System.ArgumentOutOfRangeException(nameof(value), value, $""value between {{{MyAttr.MinValue.Name}}} and {{{MyAttr.MaxValue.Name}}}"");",
                        $"}}",
                    }
                ),
                SourceTextFormatter.If(
                    IsValidateSafetyRange(selfAttributeData),
                    new SourceFormatTarget[]
                    {
                        $"{{   // safety value range",
                        $"{__}if (value < {MyAttr.SafetyMinValue.Name} || {MyAttr.SafetyMaxValue.Name} < value) WodiLib.Sys.Cmn.WodiLibLogger.GetInstance().Warning(WodiLib.Sys.WarningMessage.OutOfRange(nameof(value), {MyAttr.SafetyMinValue.Name}, {MyAttr.SafetyMaxValue.Name}, value));",
                        $"}}",
                    }
                ),
                SourceTextFormatter.If(
                    // Rootクラスのみ
                    !typeSymbol.IsExtended(),
                    new SourceFormatTarget[]
                    {
                        // DoConstructorExpansion メソッドで RawValue を更新する可能性があるので RawValue の初期化を先に行う
                        $"{selfAttributeData.GetPropertyDataRecursive<string>(MyAttr.PropertyName.Name)} = value;",
                    }
                ),
                new[]
                {
                    $"DoConstructorExpansion(value);",
                }
            );

        /// <inheritdoc/>
        private protected sealed override SourceFormatTargetBlock SourceFormatTargetsExtendBody(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol typeSymbol,
            AttributeData selfAttributeData,
            ILogger logger
        )
        {
            var fullName = typeSymbol.FullName();

            var operations = selfAttributeData.GetPropertyDataRecursive<int?>(MyAttr.Operations.Name).ToString();
            var canIncreaseAndDecrease = IntegralNumericOperation.CanIncreaseAndDecrease(operations);
            var addAndSubtractableTypes = AddAndSubtractableTypes(
                    selfAttributeData,
                    fullName
                )
                .ToArray();
            var multipleAndDividableTypes = MultipleAndDividableTypes(
                    selfAttributeData,
                    fullName
                )
                .ToArray();
            var canModulo = IntegralNumericOperation.CanModulo(operations);
            var canComplement = IntegralNumericOperation.CanComplement(operations);
            var canShift = IntegralNumericOperation.CanShift(operations);
            var canAnd = IntegralNumericOperation.CanAnd(operations);
            var canOr = IntegralNumericOperation.CanOr(operations);
            var canXor = IntegralNumericOperation.CanXor(operations);
            var comparableTypes = ComparableTypes(
                    selfAttributeData,
                    fullName
                )
                .ToArray();

            var operationOverloadCodeMaker = new OperationOverloadCodeMaker(
                typeSymbol.Name,
                typeSymbol.Namespace(),
                selfAttributeData.GetPropertyDataRecursive<string>(MyAttr.PropertyName.Name)!,
                WrapType.FullName!
            );
            return SourceTextFormatter.Format(
                "",
                operationOverloadCodeMaker.UnaryOperatorIncrement(canIncreaseAndDecrease),
                operationOverloadCodeMaker.UnaryOperatorDecrement(canIncreaseAndDecrease),
                operationOverloadCodeMaker.BinaryOperatorNewInstance("+", addAndSubtractableTypes, true),
                operationOverloadCodeMaker.BinaryOperatorNewInstance("-", addAndSubtractableTypes, true),
                operationOverloadCodeMaker.BinaryOperatorNewInstance("*", multipleAndDividableTypes, true),
                operationOverloadCodeMaker.BinaryOperatorNewInstance("/", multipleAndDividableTypes, true),
                operationOverloadCodeMaker.BinaryOperatorNewInstance("%", new[] { "int" }, canModulo),
                operationOverloadCodeMaker.UnaryOperatorComplement(canComplement),
                operationOverloadCodeMaker.BinaryOperatorNewInstanceByInt("<<", canShift),
                operationOverloadCodeMaker.BinaryOperatorNewInstanceByInt(">>", canShift),
                operationOverloadCodeMaker.BinaryOperatorNewInstanceBySameClass("&", canAnd),
                operationOverloadCodeMaker.BinaryOperatorNewInstanceBySameClass("|", canOr),
                operationOverloadCodeMaker.BinaryOperatorNewInstanceBySameClass("^", canXor),
                operationOverloadCodeMaker.BinaryOperatorBool("<", comparableTypes, true),
                operationOverloadCodeMaker.BinaryOperatorBool("<=", comparableTypes, true),
                operationOverloadCodeMaker.BinaryOperatorBool(">=", comparableTypes, true),
                operationOverloadCodeMaker.BinaryOperatorBool(">", comparableTypes, true),
                new SourceFormatTarget[]
                {
                    $"partial void DoConstructorExpansion({WrapType} value);",
                }
            );
        }

        /// <returns>範囲チェックを行う場合 <see langword="true"/> </returns>
        private bool IsValidateRange(
            AttributeData selfAttributeData
        )
        {
            var settedMinValue = selfAttributeData.GetPropertyDataRecursive<int?>(MyAttr.MinValue.Name).ToString();
            var settedMaxValue = selfAttributeData.GetPropertyDataRecursive<int?>(MyAttr.MaxValue.Name).ToString();
            var defaultMinValue = ToInt32FromIntString(GetMinDefaultValue()).ToString();
            var defaultMaxValue = ToInt32FromIntString(GetMaxDefaultValue()).ToString();

            return settedMinValue != defaultMinValue
                   || settedMaxValue != defaultMaxValue;
        }

        private protected abstract string GetMinDefaultValue();
        private protected abstract string GetMaxDefaultValue();

        /// <returns>安全範囲チェックを行う場合 <see langword="true"/> </returns>
        private bool IsValidateSafetyRange(
            AttributeData selfAttributeData
        )
        {
            var settedMinValue =
                selfAttributeData.GetPropertyDataRecursive<int?>(MyAttr.SafetyMinValue.Name).ToString();
            var settedMaxValue =
                selfAttributeData.GetPropertyDataRecursive<int?>(MyAttr.SafetyMaxValue.Name).ToString();
            var defaultMinValue = ToInt32FromIntString(GetSafetyMinDefaultValue()).ToString();
            var defaultMaxValue = ToInt32FromIntString(GetSafetyMaxDefaultValue()).ToString();

            return settedMinValue != defaultMinValue
                   || settedMaxValue != defaultMaxValue;
        }

        private protected abstract string GetSafetyMinDefaultValue();
        private protected abstract string GetSafetyMaxDefaultValue();

        /// <returns>加減算可能な右項型の一覧</returns>
        private static IEnumerable<string> AddAndSubtractableTypes(
            AttributeData selfAttributeData,
            string fullName
        )
            => OperationRightTypes(
                selfAttributeData,
                fullName,
                IntegralNumericOperation.CanAddAndSubtract,
                MyAttr.AddAndSubtractTypes.Name
            );

        /// <returns>乗除算可能な右項型の一覧</returns>
        private static IEnumerable<string> MultipleAndDividableTypes(
            AttributeData selfAttributeData,
            string fullName
        )
            => OperationRightTypes(
                selfAttributeData,
                fullName,
                IntegralNumericOperation.CanMultipleAndDivide,
                MyAttr.MultipleAndDivideOtherTypes.Name
            );

        /// <returns>比較可能な右項型の一覧</returns>
        private static IEnumerable<string> ComparableTypes(
            AttributeData selfAttributeData,
            string fullName
        )
            => OperationRightTypes(
                selfAttributeData,
                fullName,
                IntegralNumericOperation.CanCompare,
                MyAttr.CompareOtherTypes.Name
            );

        /// <returns>演算可能な右項型の一覧</returns>
        private static IEnumerable<string> OperationRightTypes(
            AttributeData selfAttributeData,
            string fullName,
            Func<string, bool> funcOperateSameType,
            string getTypesPropertyName
        )
        {
            var operateSameType = funcOperateSameType(
                (selfAttributeData.GetPropertyDataRecursive<int?>(MyAttr.Operations.Name) ?? 0).ToString()
            );
            var typedConstantList = selfAttributeData.GetArrayPropertyDataRecursive(getTypesPropertyName);
            if (!operateSameType && (typedConstantList is null || typedConstantList.Length == 0))
                return Array.Empty<string>();

            var result = new List<string>();
            if (typedConstantList is not null)
            {
                result.AddRange(typedConstantList);
            }

            if (operateSameType && !result.Contains(fullName))
            {
                result = new[] { fullName }.Concat(result).ToList();
            }

            return result;
        }

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
                "byte.MaxValue" => byte.MaxValue,
                "byte.MinValue" => byte.MinValue,
                _ => int.Parse(src),
            };
        }
    }
}
