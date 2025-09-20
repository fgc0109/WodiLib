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
        public readonly List<MethodDefinition> RestrictedListMethods = new();
        public readonly List<PropertyDefinition> RestrictedListProperties = new();
        public readonly List<ConstructorDefinition> RestrictedListConstructors = new();

        public readonly List<MethodDefinition> FixedLengthListMethods = new();
        public readonly List<PropertyDefinition> FixedLengthListProperties = new();
        public readonly List<ConstructorDefinition> FixedLengthListConstructors = new();

        public readonly List<ModelSettingsPropertyDefinition> SettingsProperties = new();

        public ListMembers()
        {
        }

        public void Initialize(
            INamedTypeSymbol currentSymbol,
            string restrictedCapacityListClassNameWithoutInOutKeyword,
            string fixedLengthListClassNameWithoutInOutKeyword,
            string readOnlyListClassNameWithoutInOutKeyword,
            string settingsInterfaceNameWithoutInOutKeyword
        )
        {
            RestrictedListMethods.Clear();
            RestrictedListProperties.Clear();
            RestrictedListConstructors.Clear();
            FixedLengthListMethods.Clear();
            FixedLengthListProperties.Clear();
            FixedLengthListConstructors.Clear();
            SettingsProperties.Clear();

            foreach (var member in currentSymbol.GetMembers())
            {
                if (member.IsStatic) continue;

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
                            this.RestrictedListMethods.Add(
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
                            this.RestrictedListConstructors.Add(
                                new ConstructorDefinition(
                                    methodSymbol,
                                    restrictedCapacityListClassNameWithoutInOutKeyword
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

                        // FixedLengthListConstructorAttribute 探索
                        var findFixedConstantAttrResult = methodSymbol.GetAttributes()
                            .FirstOrDefault(attr => attr.AttributeClass?.FullName()
                                                    == FixedLengthListConstructorAttribute.Instance.TypeFullName
                            );
                        if (findFixedConstantAttrResult is not null)
                        {
                            this.FixedLengthListConstructors.Add(
                                new ConstructorDefinition(
                                    methodSymbol,
                                    fixedLengthListClassNameWithoutInOutKeyword
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
                            this.RestrictedListProperties.Add(
                                new PropertyDefinition(
                                    propertySymbol,
                                    returnType,
                                    setterAccessibility
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
                            // setter アクセシビリティ
                            var setterAccessibility =
                                findFixedPropertyAttrResult.GetPropertyData<string>(
                                    nameof(FixedLengthListPropertyAttribute.Accessibility)
                                )!;
                            this.FixedLengthListProperties.Add(
                                new PropertyDefinition(
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
                            this.SettingsProperties.Add(
                                new ModelSettingsPropertyDefinition(
                                    propertySymbol,
                                    returnType ?? propertySymbol.Type,
                                    readOnlyListClassNameWithoutInOutKeyword,
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
