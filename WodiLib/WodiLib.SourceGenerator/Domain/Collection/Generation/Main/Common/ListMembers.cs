// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ListMembers.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Extensions;
using WodiLib.SourceGenerator.Domain.Collection.Generation.PostInitAction.Attributes;
using WodiLib.SourceGenerator.Domain.Model.Generation.PostInitAction.Attributes;

namespace WodiLib.SourceGenerator.Domain.Collection.Generation.Main.Common
{
    internal class ListMembers
    {
        public readonly List<MethodDefinition> ReadOnlyListMethods = new();
        public readonly List<ModelPropertyDefinition> ReadOnlyListProperties = new();

        public readonly List<MethodDefinition> FixedLengthListMethods = new();
        public readonly List<ModelPropertyDefinition> FixedLengthListProperties = new();

        public readonly List<ModelSettingsPropertyDefinition> SettingsProperties = new();

        public void Initialize(
            INamedTypeSymbol currentSymbol,
            string listClassNameWithoutInOutKeyword,
            string settingsInterfaceNameWithoutInOutKeyword
        )
        {
            ReadOnlyListMethods.Clear();
            ReadOnlyListProperties.Clear();
            FixedLengthListMethods.Clear();
            FixedLengthListProperties.Clear();
            SettingsProperties.Clear();

            foreach (var member in currentSymbol.GetMembers())
            {
                if (member.IsStatic) continue;

                switch (member)
                {
                    case IMethodSymbol methodSymbol:
                    {
                        // ImmutableMethodAttribute 探索
                        var findMutableMethodAttrResult = methodSymbol.GetAttributes()
                            .FirstOrDefault(attr => attr.AttributeClass?.FullName()
                                                    == ImmutableMethodAttribute.Instance.TypeFullName
                            );
                        if (findMutableMethodAttrResult is not null)
                        {
                            var accessibility =
                                findMutableMethodAttrResult.GetPropertyData<string>(
                                    nameof(ImmutableMethodAttribute.Accessibility)
                                )!;
                            this.ReadOnlyListMethods.Add(
                                new MethodDefinition(
                                    methodSymbol,
                                    accessibility
                                )
                            );
                        }

                        // FixedLengthListMethodAttribute 探索
                        var findFixedMethodAttrResult = methodSymbol.GetAttributes()
                            .FirstOrDefault(attr => attr.AttributeClass?.FullName()
                                                    == FixedLengthListMethodAttribute.Instance.TypeFullName
                            );
                        if (findFixedMethodAttrResult is not null)
                        {
                            var accessibility =
                                findFixedMethodAttrResult.GetPropertyData<string>(
                                    nameof(FixedLengthListMethodAttribute.Accessibility)
                                )!;
                            this.FixedLengthListMethods.Add(
                                new MethodDefinition(
                                    methodSymbol,
                                    accessibility
                                )
                            );
                        }

                        break;
                    }
                    case IPropertySymbol propertySymbol:
                    {
                        // ImmutablePropertyAttribute 探索
                        var findImmutablePropertyAttrResult = propertySymbol.GetAttributes()
                            .FirstOrDefault(attr => attr.AttributeClass?.FullName()
                                                    == ImmutablePropertyAttribute.Instance.TypeFullName
                            );
                        if (findImmutablePropertyAttrResult is not null)
                        {
                            // 戻り値の型情報
                            var returnType =
                                findImmutablePropertyAttrResult.GetPropertyData<INamedTypeSymbol?>(
                                    nameof(ImmutablePropertyAttribute.ReturnType)
                                );
                            // アクセシビリティ
                            var accessibility =
                                findImmutablePropertyAttrResult.GetPropertyData<string>(
                                    nameof(ImmutablePropertyAttribute.Accessibility)
                                )!;
                            this.ReadOnlyListProperties.Add(
                                new ModelPropertyDefinition(
                                    propertySymbol,
                                    returnType,
                                    accessibility,
                                    setterAccessibility: "NONE"
                                )
                            );
                        }

                        // FixedLengthListPropertyAttribute 探索
                        var findFixedPropertyAttrResult = propertySymbol.GetAttributes()
                            .FirstOrDefault(attr => attr.AttributeClass?.FullName()
                                                    == FixedLengthListPropertyAttribute.Instance.TypeFullName
                            );
                        if (findFixedPropertyAttrResult is not null)
                        {
                            // 戻り値の型情報
                            var returnType =
                                findFixedPropertyAttrResult.GetPropertyData<INamedTypeSymbol?>(
                                    nameof(FixedLengthListPropertyAttribute.ReturnType)
                                );
                            // アクセシビリティ
                            var accessibility =
                                findFixedPropertyAttrResult.GetPropertyData<string>(
                                    nameof(FixedLengthListPropertyAttribute.Accessibility)
                                )!;
                            // setter アクセシビリティ
                            var setterAccessibility =
                                findFixedPropertyAttrResult.GetPropertyData<string>(
                                    nameof(FixedLengthListPropertyAttribute.SetterAccessibility)
                                )!;
                            this.FixedLengthListProperties.Add(
                                new ModelPropertyDefinition(
                                    propertySymbol,
                                    returnType,
                                    accessibility,
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
                            this.SettingsProperties.Add(
                                new ModelSettingsPropertyDefinition(
                                    propertySymbol,
                                    returnType ?? propertySymbol.Type,
                                    returnType is not null,
                                    listClassNameWithoutInOutKeyword,
                                    settingsInterfaceNameWithoutInOutKeyword,
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
            }
        }
    }
}
