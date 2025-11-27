// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : TwoDListGeneratorBase.cs
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
using WodiLib.SourceGenerator.Domain.Collection.Generation.PostInitAction.Attributes;

namespace WodiLib.SourceGenerator.Domain.Collection.Generation.Main.Common
{
    internal abstract partial class TwoDListGeneratorBase : MainSourceAddableTemplate
    {
        private protected abstract bool IsRestrictedCapacityList { get; }

        public override InitializeAttributeSourceAddable TargetAttribute =>
            IsRestrictedCapacityList
                ? RestrictedCapacity2DListImplementTemplateAttribute.Instance
                : FixedLength2DListImplementTemplateAttribute.Instance;

        private protected override SourceFormatTargetBlock GenerateTImportUsingSource(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol source,
            AttributeData selfAttributeData,
            ILogger logger
        )
        {
            return new[]
            {
                "using System;", // 例外のため
                "using System.Collections;", // IEnumerable のため
                "using System.Collections.Generic;", // IEnumerable<T> などのため
                "using System.Collections.Specialized;", // INotifyCollectionChanged のため
                "using System.ComponentModel;", // PropertyChangedEventHandler のため
                "using System.Diagnostics.Contracts;", // PureAttribute 使用のため
                "using System.Linq;", // IEnumerable<T> 拡張メソッド使用のため
                "using WodiLib.Sys;", // EqualityComparerFactory 使用のため
                "using WodiLib.Sys.Collections;", // TwoDimensionalList 使用のため
            };
        }

        private protected override SourceFormatTargetBlock GenerateTypeDefinitionSource(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol source,
            AttributeData selfAttributeData,
            ILogger logger
        )
        {
            try
            {
                var modelInfo = BuildClassInformation(
                    source,
                    selfAttributeData
                );
                modelInfo.Members.Initialize(
                    source,
                    modelInfo.RestrictedCapacityListInfo.RestrictedCapacityListClassNameWithoutInOutKeyword,
                    modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword
                );

                return SourceTextFormatter.Format(
                    "",
                    // 設定インタフェース
                    BuildSettingsInterfaceSource(modelInfo),
                    SourceFormatTargetBlock.Empty,
                    // 設定DTO
                    BuildSettingsDtoSource(modelInfo),
                    SourceFormatTargetBlock.Empty,
                    // リストクラス
                    SourceTextFormatter.If(
                        IsRestrictedCapacityList,
                        "",
                        BuildRestrictedClassSource(modelInfo),
                        SourceFormatTargetBlock.Empty
                    ),
                    // 容量固定リストクラス
                    BuildFixedLengthClassSource(modelInfo),
                    SourceFormatTargetBlock.Empty,
                    // 読取専用リストクラス
                    BuildReadOnlyClassSource(modelInfo)
                );
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        private ModelInformation BuildClassInformation(
            INamedTypeSymbol source,
            AttributeData selfAttributeData
        )
        {
            var restrictedCapacityListClassName = source.ClassName();
            var accessibility = AccessibilityConverter.ConvertSourceText(source.DeclaredAccessibility);
            var isAbstract = source.IsAbstract;

            var description = selfAttributeData.GetPropertyDataRecursive<string?>(
                                  RestrictedCapacity2DListImplementTemplateAttribute.Description.Name
                              )
                              ?? "";

            var maxRowCapacity = selfAttributeData
                .GetPropertyDataRecursive<object>(
                    RestrictedCapacity2DListImplementTemplateAttribute.MaxRowCapacity.Name
                )
                ?.ToString()!;
            var minRowCapacity = selfAttributeData
                .GetPropertyDataRecursive<object>(
                    RestrictedCapacity2DListImplementTemplateAttribute.MinRowCapacity.Name
                )
                ?.ToString()!;
            var maxColumnCapacity = selfAttributeData
                .GetPropertyDataRecursive<object>(
                    RestrictedCapacity2DListImplementTemplateAttribute.MaxColumnCapacity.Name
                )
                ?.ToString()!;
            var minColumnCapacity = selfAttributeData
                .GetPropertyDataRecursive<object>(
                    RestrictedCapacity2DListImplementTemplateAttribute.MinColumnCapacity.Name
                )
                ?.ToString()!;

            var rowType = selfAttributeData
                .GetPropertyDataRecursive<INamedTypeSymbol?>(
                    RestrictedCapacity2DListImplementTemplateAttribute.RowElementType.Name
                )
                ?.FullName()!;
            var fixedLengthRowType =
                selfAttributeData.GetPropertyDataRecursive<INamedTypeSymbol?>(
                        RestrictedCapacity2DListImplementTemplateAttribute.FixedRowElementType.Name
                    )
                    ?.FullName()
                ?? rowType;
            var readOnlyRowType =
                selfAttributeData.GetPropertyDataRecursive<INamedTypeSymbol?>(
                        RestrictedCapacity2DListImplementTemplateAttribute.ReadOnlyRowElementType.Name
                    )
                    ?.FullName()
                ?? rowType;
            var rowSettingsType =
                selfAttributeData.GetPropertyDataRecursive<INamedTypeSymbol?>(
                        RestrictedCapacity2DListImplementTemplateAttribute.RowSettingsType.Name
                    )
                    ?.FullName()
                ?? rowType;
            var elementType = selfAttributeData
                .GetPropertyDataRecursive<INamedTypeSymbol?>(
                    RestrictedCapacity2DListImplementTemplateAttribute.CellElementType.Name
                )
                ?.FullName()!;
            var readOnlyElementType =
                selfAttributeData.GetPropertyDataRecursive<INamedTypeSymbol?>(
                        RestrictedCapacity2DListImplementTemplateAttribute.ReadOnlyCellElementType.Name
                    )
                    ?.FullName()
                ?? elementType;
            var elementSettingsType =
                selfAttributeData.GetPropertyDataRecursive<INamedTypeSymbol?>(
                        RestrictedCapacity2DListImplementTemplateAttribute.CellSettingsType.Name
                    )
                    ?.FullName()
                ?? elementType;

            var rowPhysicalName =
                selfAttributeData
                    .GetPropertyDataRecursive<string?>(
                        RestrictedCapacity2DListImplementTemplateAttribute.RowPhysicalName.Name
                    )
                ?? "Row";
            var rowLogicalName = selfAttributeData
                                     .GetPropertyDataRecursive<string?>(
                                         RestrictedCapacity2DListImplementTemplateAttribute.RowLogicalName.Name
                                     )
                                 ?? "行";
            var columnPhysicalName =
                selfAttributeData.GetPropertyDataRecursive<string?>(
                    RestrictedCapacity2DListImplementTemplateAttribute.ColumnPhysicalName.Name
                )
                ?? "Column";
            var columnLogicalName =
                selfAttributeData.GetPropertyDataRecursive<string?>(
                    RestrictedCapacity2DListImplementTemplateAttribute.ColumnLogicalName.Name
                )
                ?? "列";
            var cellPhysicalName =
                selfAttributeData
                    .GetPropertyDataRecursive<string?>(
                        RestrictedCapacity2DListImplementTemplateAttribute.CellPhysicalName.Name
                    )
                ?? "Cell";
            var cellLogicalName =
                selfAttributeData
                    .GetPropertyDataRecursive<string?>(
                        RestrictedCapacity2DListImplementTemplateAttribute.CellLogicalName.Name
                    )
                ?? "セル";

            var baseModelClass = selfAttributeData
                .GetPropertyDataRecursive<INamedTypeSymbol?>(
                    RestrictedCapacity2DListImplementTemplateAttribute.BaseModelClass.Name
                )
                ?.FullName();

            var useConstructorExpansion = selfAttributeData
                .GetPropertyDataRecursive<bool>(
                    RestrictedCapacity2DListImplementTemplateAttribute.UseConstructorExpansion.Name
                );

            var baseModelClassNoGeneric = baseModelClass?.Split('<')[0];

            var isExtendClass = baseModelClass is not null;

            var restrictedCapacityListClassNameWithoutInOutKeyword =
                restrictedCapacityListClassName.Replace("out ", "").Replace("in ", "");
            var fixedLengthListClassName = !IsRestrictedCapacityList
                ? restrictedCapacityListClassName
                : $"Fixed{restrictedCapacityListClassName}";
            var fixedLengthListClassNameWithoutInOutKeyword =
                fixedLengthListClassName.Replace("out ", "").Replace("in ", "");
            var readOnlyListClassName = $"ReadOnly{restrictedCapacityListClassName}";
            var readOnlyListClassNameWithoutInOutKeyword = readOnlyListClassName.Replace("out ", "").Replace("in ", "");

            var dtoNameBase = $"{restrictedCapacityListClassName.Split('<')[0]}Settings";

            var settingsInterfaceName = $"I{dtoNameBase}";
            var settingsInterfaceNameWithoutIOKeyword = settingsInterfaceName.Replace("out ", "").Replace("in ", "");

            var settingsDtoName = dtoNameBase;

            var baseSettingsParameterTypes = source.BaseType is not null
                                             && source.BaseType.FullName()
                                                 .StartsWith("WodiLib.Sys.BaseModel")
                ? null
                : source.BaseType!.TypeParameters.Select(t =>
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
                $" : WodiLib.Sys.IEqualityComparable<{settingsInterfaceNameWithoutIOKeyword}>, IListSettings<{rowSettingsType}>"
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
                GetSettingsInterfaceItemEqualsMethodBody(source, settingsInterfaceName);

            return new ModelInformation(
                !IsRestrictedCapacityList,
                rowType,
                fixedLengthRowType,
                readOnlyRowType,
                rowSettingsType,
                elementType,
                readOnlyElementType,
                elementSettingsType,
                description,
                accessibility,
                isExtendClass,
                isAbstract,
                maxRowCapacity,
                minRowCapacity,
                maxColumnCapacity,
                minColumnCapacity,
                rowPhysicalName,
                rowLogicalName,
                columnPhysicalName,
                columnLogicalName,
                cellPhysicalName,
                cellLogicalName,
                useConstructorExpansion,
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
                new RestrictedCapacityListInformation(
                    restrictedCapacityListClassName,
                    restrictedCapacityListClassNameWithoutInOutKeyword
                ),
                new FixedLengthListInformation(
                    fixedLengthListClassName,
                    fixedLengthListClassNameWithoutInOutKeyword
                ),
                new ReadOnlyListInformation(
                    readOnlyListClassName,
                    readOnlyListClassNameWithoutInOutKeyword
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
                    && m.Parameters[0].Type.ToDisplayString().EndsWith($"{settingsInterfaceName}?") // nullable
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

        private record ModelInformation(
            bool IsFixed,
            string RowType,
            string FixedLengthRowType,
            string ReadOnlyRowType,
            string RowSettingsType,
            string ElementType,
            string ReadOnlyElementType,
            string ElementSettingsType,
            string Description,
            string Accessibility,
            bool IsExtendClass,
            bool IsAbstract,
            string MaxRowCapacity,
            string MinRowCapacity,
            string MaxColumnCapacity,
            string MinColumnCapacity,
            string RowPhysicalName,
            string RowLogicalName,
            string ColumnPhysicalName,
            string ColumnLogicalName,
            string CellPhysicalName,
            string CellLogicalName,
            bool UseConstructorExpansion,
            SettingsInterfaceInformation SettingsInterfaceInfo,
            SettingsDtoInformation SettingsDtoInfo,
            RestrictedCapacityListInformation RestrictedCapacityListInfo,
            FixedLengthListInformation FixedLengthListInfo,
            ReadOnlyListInformation ReadOnlyListInfo
        )
        {
            public readonly string AbstractKeyword = IsAbstract
                ? "abstract "
                : "";

            public ListMembers Members { get; } = new();
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

        private record RestrictedCapacityListInformation(
            string RestrictedCapacityListClassName,
            string RestrictedCapacityListClassNameWithoutInOutKeyword
        );

        private record FixedLengthListInformation(
            string FixedLengthListClassName,
            string FixedLengthListClassNameWithoutInOutKeyword
        );

        private record ReadOnlyListInformation(
            string ReadOnlyListClassName,
            string ReadOnlyListClassNameWithoutInOutKeyword
        );
    }
}
