// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ModelSettingsPropertyDefinition.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Extensions;

namespace WodiLib.SourceGenerator.Domain.Collection.Generation.Main.Common
{
    /// <summary>
    ///     設定DTOプロパティ定義情報クラス
    /// </summary>
    internal class ModelSettingsPropertyDefinition
    {
        public string[] InterfaceDefinitionCode => new[]
        {
            $"/// <inheritdoc cref=\"{immutableModelClassName}.{Name}\" />",
            $"{ReturnTypeName}{NullableMark} {Name} {{ get; }}",
        };

        public string[] ImplementationRecordCode => propertySymbol.IsAbstract || forceAbstract
            ? new[]
            {
                $"/// <inheritdoc cref=\"{interfaceName}.{Name}\" />",
                $"public {ExtendKeyword}{ReturnTypeName}{NullableMark} {Name} {{ get; {SetterKeyword} }}",
            }
            : SetterKeyword is not null
                ? new[]
                {
                    $"/// <inheritdoc cref=\"{interfaceName}.{Name}\" path=\"summary|remarks\" />",
                    $"public {ExtendKeyword}{ReturnTypeName}{NullableMark} {Name} {{ get; {SetterKeyword} }} = {defaultValue};",
                }
                : new[]
                {
                    $"/// <inheritdoc cref=\"{interfaceName}.{Name}\" path=\"summary|remarks\" />",
                    $"public {ExtendKeyword}{ReturnTypeName}{NullableMark} {Name} => {defaultValue};",
                };

        private string? SetterKeyword =>
            setterAccessibility == "public"
                ? "set;"
                : setterAccessibility != "NONE"
                    ? $"{setterAccessibility}  set;"
                    : null;

        private readonly IPropertySymbol propertySymbol;
        private readonly ITypeSymbol returnType;
        private readonly string immutableModelClassName;
        private readonly string interfaceName;
        private readonly string defaultValue;
        private readonly bool forceAbstract;
        private readonly bool forceVirtual;
        private readonly string setterAccessibility;

        private string Name => propertySymbol.Name;

        private string ReturnTypeName => returnType.FullName();

        private string ExtendKeyword => propertySymbol.IsVirtual || forceVirtual
            ? "virtual "
            : forceAbstract
                ? "abstract "
                : propertySymbol.IsOverride
                    ? "override "
                    : "";

        private string NullableMark => returnType.NullableAnnotation == NullableAnnotation.Annotated
            ? "?"
            : "";

        public ModelSettingsPropertyDefinition(
            IPropertySymbol propertySymbol,
            ITypeSymbol returnType,
            string immutableModelClassName,
            string interfaceName,
            string defaultValue,
            bool forceAbstract,
            bool forceVirtual,
            string setterAccessibility
        )
        {
            this.propertySymbol = propertySymbol;
            this.returnType = returnType;
            this.immutableModelClassName = immutableModelClassName;
            this.interfaceName = interfaceName;
            this.defaultValue = defaultValue;
            this.forceAbstract = forceAbstract;
            this.forceVirtual = forceVirtual;
            this.setterAccessibility = setterAccessibility;
        }
    }
}
