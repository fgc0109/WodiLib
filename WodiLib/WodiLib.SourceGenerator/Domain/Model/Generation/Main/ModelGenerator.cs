// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ModelGenerator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WodiLib.SourceGenerator.Core.Extensions;
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using WodiLib.SourceGenerator.Core.Templates.FromAttribute;
using WodiLib.SourceGenerator.Domain.Model.Generation.PostInitAction.Attributes;

namespace WodiLib.SourceGenerator.Domain.Model.Generation.Main
{
    /// <summary>
    /// </summary>
    internal partial class ModelGenerator : MainSourceAddableTemplate
    {
        public override InitializeAttributeSourceAddable TargetAttribute =>
            ModelAttribute.Instance;

        private protected override SourceFormatTargetBlock GenerateTImportUsingSource(WorkState workState)
        {
            return new[]
            {
                "using System.Linq;", // IEnumerable<T> 拡張メソッド使用のため
                "using WodiLib.Sys;", // EqualityComparerFactory 使用のため
            };
        }

        private protected override SourceFormatTargetBlock GenerateTypeDefinitionSource(WorkState workState)
        {
            try
            {
                var propertyValues = workState.PropertyValues;
                var typeDefinitionInfo = workState.CurrentTypeDefinitionInfo;

                var currentSymbol = workState.CurrentSymbol;
                if (currentSymbol is null)
                {
                    return "";
                }

                var modelInfo = BuildModelInformation(
                    workState,
                    currentSymbol,
                    mutableModelClassName: workState.Name.Replace("ReadOnly", ""),
                    classTypeParameters: propertyValues.WorkResult.TargetSymbol?.TypeParameters,
                    description: propertyValues[ModelAttribute.Description.Name]!,
                    accessibility: AccessibilityConverter.ConvertSourceText(typeDefinitionInfo.Accessibility),
                    isAbstract: typeDefinitionInfo.IsAbstract,
                    baseModelClass: GetBaseModelClassName(propertyValues[ModelAttribute.BaseModelClass.Name]),
                    settingsParameterTypes: propertyValues.GetArrayValue(ModelAttribute.SettingsParameterTypes.Name)
                );
                CollectModelMembers(currentSymbol, modelInfo);

                return SourceTextFormatter.Format(
                    "",
                    // 設定インタフェース
                    BuildSettingsInterfaceSource(modelInfo),
                    SourceFormatTargetBlock.Empty,
                    // 設定DTO
                    BuildSettingsDtoSource(modelInfo),
                    SourceFormatTargetBlock.Empty,
                    // モデルクラス
                    BuildMutableClassSource(modelInfo),
                    SourceFormatTargetBlock.Empty,
                    // 読取専用モデルクラス
                    BuildImmutableClassSource(modelInfo)
                );
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        private static void CollectModelMembers(INamedTypeSymbol currentSymbol, ModelInformation modelInfo)
        {
            currentSymbol.GetMembers()
                .Aggregate(
                    modelInfo.Members,
                    (acc, member) =>
                    {
                        if (member.IsStatic) return acc;

                        switch (member)
                        {
                            case IMethodSymbol methodSymbol:
                            {
                                // MutableMethodAttribute 探索
                                var findMutableMethodAttrResult = methodSymbol.GetAttributes()
                                    .FirstOrDefault(attr => attr.AttributeClass?.FullName()
                                                            == MutableMethodAttribute.Instance.TypeFullName
                                    );
                                if (findMutableMethodAttrResult is not null)
                                {
                                    var accessibility =
                                        findMutableMethodAttrResult.GetPropertyData<string>(
                                            nameof(MutableMethodAttribute.Accessibility)
                                        )!;
                                    acc.Methods.Add(
                                        new MethodDefinition(
                                            methodSymbol,
                                            accessibility
                                        )
                                    );
                                }

                                // MutableConstructorAttribute 探索
                                var findMutableConstantAttrResult = methodSymbol.GetAttributes()
                                    .FirstOrDefault(attr => attr.AttributeClass?.FullName()
                                                            == MutableConstructorAttribute.Instance.TypeFullName
                                    );
                                if (findMutableConstantAttrResult is not null)
                                {
                                    acc.Constructors.Add(
                                        new ConstructorDefinition(
                                            methodSymbol,
                                            modelInfo.MutableInfo.MutableModelClassNameWithoutInOutKeyword
                                        )
                                    );
                                }

                                break;
                            }
                            case IPropertySymbol propertySymbol:
                            {
                                // MutablePropertyAttribute 探索
                                var findImmutablePropertyAttrResult = propertySymbol.GetAttributes()
                                    .FirstOrDefault(attr => attr.AttributeClass?.FullName()
                                                            == MutablePropertyAttribute.Instance.TypeFullName
                                    );
                                if (findImmutablePropertyAttrResult is not null)
                                {
                                    // 戻り値の型情報
                                    var returnType =
                                        findImmutablePropertyAttrResult.GetPropertyData<INamedTypeSymbol?>(
                                            nameof(MutablePropertyAttribute.ReturnType)
                                        );
                                    // setter アクセシビリティ
                                    var setterAccessibility =
                                        findImmutablePropertyAttrResult.GetPropertyData<string>(
                                            nameof(MutablePropertyAttribute.Accessibility)
                                        )!;
                                    acc.MutableModelProperties.Add(
                                        new MutableModelPropertyDefinition(
                                            propertySymbol,
                                            returnType,
                                            setterAccessibility
                                        )
                                    );
                                }

                                // SettingsPropertyAttribute 探索
                                var findModelCorePropertyAttrResult = propertySymbol.GetAttributes()
                                    .FirstOrDefault(attr => attr.AttributeClass?.FullName()
                                                            == SettingsPropertyAttribute.Instance.TypeFullName
                                    );
                                if (findModelCorePropertyAttrResult is not null)
                                {
                                    var defaultValue =
                                        findModelCorePropertyAttrResult.GetPropertyData<string?>(
                                            nameof(SettingsPropertyAttribute.DefaultValue)
                                        )
                                        ?? "default";
                                    var forceAbstract =
                                        findModelCorePropertyAttrResult.GetPropertyData<bool?>(
                                            nameof(SettingsPropertyAttribute.ForceAbstract)
                                        )
                                        ?? false;
                                    var forceVirtual =
                                        findModelCorePropertyAttrResult.GetPropertyData<bool?>(
                                            nameof(SettingsPropertyAttribute.ForceVirtual)
                                        )
                                        ?? false;
                                    var setterAccessibility =
                                        findModelCorePropertyAttrResult.GetPropertyData<string?>(
                                            nameof(SettingsPropertyAttribute.SetterAccessibility)
                                        )
                                        ?? "public";

                                    // 戻り値の型情報
                                    var returnType =
                                        findModelCorePropertyAttrResult.GetPropertyData<INamedTypeSymbol?>(
                                            nameof(SettingsPropertyAttribute.ReturnType)
                                        );
                                    acc.SettingsProperties.Add(
                                        new ModelSettingsPropertyDefinition(
                                            propertySymbol,
                                            returnType ?? propertySymbol.Type,
                                            modelInfo.ImmutableInfo.ImmutableModelClassNameWithoutInOutKeyword,
                                            modelInfo.SettingsInterfaceInfo.SettingsInterfaceName,
                                            defaultValue,
                                            forceAbstract,
                                            forceVirtual,
                                            setterAccessibility
                                        )
                                    );
                                }

                                break;
                            }
                        }

                        return acc;
                    }
                );
        }

        private static string? GetBaseModelClassName(string? originalValue)
        {
            if (originalValue is null)
            {
                return null;
            }

            if (originalValue.IndexOf('<') == -1)
            {
                // 総称型を使わない場合
                //      名前空間を除去
                return originalValue.Split('.').Last();
            }

            // 総称型を使う場合
            //      クラス名部分と総称型部分に分割
            //      クラス名部分は名前空間を除去
            //      総称型部分はそのまま
            var regex = new Regex("^([^<]*)(<.+)?");
            var matchGroups = regex.Matches(originalValue)[0].Groups;
            return $"{matchGroups[1].Value.Split('.').Last()}{matchGroups[2].Value}";
        }

        private static ModelInformation BuildModelInformation(
            WorkState workState,
            INamedTypeSymbol currentSymbol,
            string mutableModelClassName,
            ImmutableArray<ITypeParameterSymbol>? classTypeParameters,
            string description,
            string accessibility,
            bool isAbstract,
            string? baseModelClass,
            string[]? settingsParameterTypes
        )
        {
            var baseModelClassNoGeneric = baseModelClass?.Split('<')[0];

            var typeParamConstraints = classTypeParameters?.Select(t =>
                    {
                        var constraints = t.ConstraintTypes.Select(constraint => constraint.ToDisplayString())
                            .ToList();

                        if (t.HasConstructorConstraint)
                        {
                            constraints.Add("new()");
                        }

                        if (t.HasNotNullConstraint)
                        {
                            constraints.Add("notnull");
                        }

                        if (t.HasValueTypeConstraint)
                        {
                            constraints.Add("struct");
                        }

                        if (t.HasReferenceTypeConstraint)
                        {
                            constraints.Add("class");
                        }

                        if (t.HasUnmanagedTypeConstraint)
                        {
                            constraints.Add("unmanaged");
                        }

                        if (constraints.Count == 0)
                        {
                            return null;
                        }

                        return $"where {t.Name} : {constraints.Join(", ")}";
                    }
                )
                .Where(t => t != null)
                .Cast<string>()
                .ToArray();

            var isExtendClass = baseModelClass is not null;

            var immutableModelClassName = $"ReadOnly{mutableModelClassName}";

            var joinedSettingsParameterTypes = settingsParameterTypes is not null
                ? string.Join(", ", settingsParameterTypes)
                : null;
            var markedSettingsParameterTypes =
                joinedSettingsParameterTypes is not null
                    ? $"<{joinedSettingsParameterTypes}>"
                    : "";

            var dtoNameBase = $"{mutableModelClassName.Split('<')[0]}Settings";

            var settingsInterfaceName = $"I{dtoNameBase}{markedSettingsParameterTypes}";
            var settingsInterfaceNameWithoutIOKeyword = settingsInterfaceName.Replace("out ", "").Replace("in ", "");

            var settingsDtoName =
                $"{dtoNameBase}{markedSettingsParameterTypes.Replace("out ", "").Replace("in ", "")}";

            var baseSettingsParameterTypes = workState.CurrentSymbol?.BaseType is not null
                                             && workState.CurrentSymbol.BaseType.FullName()
                                                 .StartsWith("WodiLib.Sys.BaseModel")
                ? null
                : workState.CurrentSymbol!.BaseType!.TypeParameters.Select(t =>
                        {
                            var keyword = t.Variance switch
                            {
                                VarianceKind.None => "",
                                VarianceKind.In => "in ",
                                VarianceKind.Out => "out ",
                                _ => "",
                            };

                            return $"{keyword}{t.Name}";
                        }
                    )
                    .ToArray();

            var joinedBaseSettingsParameterTypes =
                baseSettingsParameterTypes is not null
                    ? string.Join(", ", baseSettingsParameterTypes)
                    : null;
            var markedBaseSettingsParameterTypes =
                joinedBaseSettingsParameterTypes is not null
                    ? $"<{joinedBaseSettingsParameterTypes}>"
                    : "";

            var extendsSettingsInterface =
                $" : WodiLib.Sys.IEqualityComparable<{settingsInterfaceNameWithoutIOKeyword}>"
                + (
                    baseModelClassNoGeneric is not null
                        ? $", I{baseModelClassNoGeneric}Settings{markedBaseSettingsParameterTypes}"
                        : ""
                );
            var extendsSettingsDto = baseModelClassNoGeneric is not null
                ? $"{baseModelClassNoGeneric}Settings{markedBaseSettingsParameterTypes}, {settingsInterfaceNameWithoutIOKeyword}"
                : settingsInterfaceNameWithoutIOKeyword;

            // クラス自身に実装されている設定インタフェースとのItemEqualsメソッドのコードを取得する。
            // 設定DTOに同じ内容で実装する。
            // ターゲットとなるItemEqualsメソッドが未実装の場合、NotImplementedException を投げるようにする
            var settingsInterfaceCompareCode =
                GetSettingsInterfaceItemEqualsMethodBody(currentSymbol, settingsInterfaceName);

            return new ModelInformation(
                typeParamConstraints,
                description,
                accessibility,
                isExtendClass,
                isAbstract,
                new SettingsInterfaceInformation(
                    settingsInterfaceName,
                    settingsInterfaceNameWithoutIOKeyword,
                    extendsSettingsInterface
                ),
                new SettingsDtoInformation(
                    settingsDtoName,
                    extendsSettingsDto,
                    settingsInterfaceCompareCode
                ),
                new MutableInformation(
                    mutableModelClassName,
                    mutableModelClassName.Replace("in ", "").Replace("out ", "")
                ),
                new ImmutableInformation(
                    immutableModelClassName,
                    immutableModelClassName.Replace("in ", "").Replace("out ", "")
                )
            );
        }

        /// <summary>
        ///     設定インタフェースと比較するItemEqualsメソッドの実装ソースコードを文字列として取得する。
        ///     取得できない場合は <see cref="NotImplementedException"/> をスローするソースコードを返す。
        /// </summary>
        /// <param name="currentSymbol"></param>
        /// <param name="settingsInterfaceName"></param>
        /// <returns></returns>
        private static string GetSettingsInterfaceItemEqualsMethodBody(
            INamedTypeSymbol currentSymbol,
            string settingsInterfaceName
        )
        {
            var targetMethod = currentSymbol
                .GetMembers()
                .OfType<IMethodSymbol>()
                .FirstOrDefault(m =>
                    m.Name == "ItemEquals"
                    && m.Parameters.Length == 1
                    && m.Parameters[0].Type.ToString() == $"{settingsInterfaceName}?" // nullable
                    && m.ReturnType.SpecialType == SpecialType.System_Boolean
                );

            var syntaxReference = targetMethod?.DeclaringSyntaxReferences.FirstOrDefault();
            var syntaxNode = syntaxReference?.GetSyntax();

            if (syntaxNode is MethodDeclarationSyntax methodNode)
            {
                return methodNode.ToString();
            }

            return @$"public bool ItemEquals({settingsInterfaceName}? other)
        {{
            throw new System.NotImplementedException();
        }}";
        }

        private ModelGenerator()
        {
        }

        public static ModelGenerator Instance { get; } = new();

        private record ModelInformation(
            string[]? TypeParamConstraints,
            string Description,
            string Accessibility,
            bool IsExtendClass,
            bool IsAbstract,
            SettingsInterfaceInformation SettingsInterfaceInfo,
            SettingsDtoInformation SettingsDtoInfo,
            MutableInformation MutableInfo,
            ImmutableInformation ImmutableInfo
        )
        {
            public readonly string AbstractKeyword = IsAbstract
                ? "abstract "
                : "";

            public ModelMembers Members { get; } = new();
        }

        private record SettingsInterfaceInformation(
            string SettingsInterfaceName,
            string SettingsInterfaceNameWithoutIOKeyword,
            string ExtendsSettingsInterface
        );

        private record SettingsDtoInformation(
            string SettingsDtoName,
            string ExtendsSettingsDto,
            string SettingsInterfaceCompareCode
        );

        private record MutableInformation(
            string MutableModelClassName,
            string MutableModelClassNameWithoutInOutKeyword
        );

        private record ImmutableInformation(
            string ImmutableModelClassName,
            string ImmutableModelClassNameWithoutInOutKeyword
        );

        private class ModelMembers
        {
            public readonly List<MethodDefinition> Methods = new();
            public readonly List<MutableModelPropertyDefinition> MutableModelProperties = new();
            public readonly List<ModelSettingsPropertyDefinition> SettingsProperties = new();
            public readonly List<ConstructorDefinition> Constructors = new();
        }
    }
}
