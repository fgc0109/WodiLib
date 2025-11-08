// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : PropertyDefinition.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using Microsoft.CodeAnalysis;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.Domain.Collection.Generation.Main.Common
{
    /// <summary>
    ///     プロパティ定義情報クラス
    /// </summary>
    internal class ModelPropertyDefinition
    {
        public string[] ImplementationCode => new StringList()
            .AppendLine(DocComment)
            .Append($"{accessibility} {returnType} {Name}")
            .Append(PropertyBody)
            .ToArray();

        private readonly IPropertySymbol propertySymbol;
        private readonly ITypeSymbol returnType;
        private readonly string accessibility;
        private readonly string setterAccessibility;

        private string Name => propertySymbol.Name;

        private string DocComment
            => $"/// <inheritdoc/>";

        private string[] PropertyBody
        {
            get
            {
                if (setterAccessibility == "NONE")
                {
                    return new[]
                    {
                        $" => MutableInstance.{Name};",
                    };
                }

                return new StringList()
                    .AppendLine("")
                    .AppendLine("{")
                    .AppendLine($"{__}get => mutableInstance.{Name};")
                    .AppendLine($"{__}{accessibility} set => mutableInstance.{Name} = value;")
                    .AppendLine("}")
                    .ToArray();
            }
        }

        public ModelPropertyDefinition(
            IPropertySymbol propertySymbol,
            ITypeSymbol? forceReturnType,
            string accessibility,
            string setterAccessibility
        )
        {
            this.propertySymbol = propertySymbol;
            if (forceReturnType is null)
            {
                returnType = propertySymbol.Type;
            }
            else
            {
                returnType = forceReturnType;
            }

            this.accessibility = accessibility;
            this.setterAccessibility = setterAccessibility;
        }
    }
}
