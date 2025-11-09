// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ImmutableModelGenerator.MutableModelPropertyDefinition.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using Microsoft.CodeAnalysis;

namespace WodiLib.SourceGenerator.Domain.Model.Generation.Main
{
    internal partial class ModelGenerator
    {
        /// <summary>
        ///     プロパティ定義情報クラス
        /// </summary>
        public class ImmutableModelPropertyDefinition
        {
            public string[] ImplementationCode => new StringList()
                .AppendLine(DocComment)
                .Append($"{accessibility} {returnType} {Name}")
                .Append(PropertyBody)
                .ToArray();

            private readonly IPropertySymbol propertySymbol;
            private readonly string accessibility;
            private readonly ITypeSymbol returnType;

            private string Name => propertySymbol.Name;

            private string DocComment
                => $"/// <inheritdoc/>";

            private string[] PropertyBody => new[]
            {
                $" => MutableInstance.{Name};",
            };

            public ImmutableModelPropertyDefinition(
                IPropertySymbol propertySymbol,
                string accessibility,
                ITypeSymbol? forceReturnType
            )
            {
                this.propertySymbol = propertySymbol;
                this.accessibility = accessibility;
                returnType = forceReturnType ?? propertySymbol.Type;
            }
        }
    }
}
