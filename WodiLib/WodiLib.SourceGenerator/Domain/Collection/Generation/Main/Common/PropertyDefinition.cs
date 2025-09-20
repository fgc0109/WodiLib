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
    internal class PropertyDefinition
    {
        public string[] ImplementationCode => new StringList()
            .AppendLine(DocComment)
            .Append($"public new {returnType}{nullableMark} {Name}")
            .Append(PropertyBody)
            .ToArray();

        private readonly IPropertySymbol propertySymbol;
        private readonly ITypeSymbol returnType;
        private readonly string setterAccessibility;
        private readonly string nullableMark;
        private readonly bool requireCastGetBody;

        private string Name => propertySymbol.Name;

        private string DocComment
            => $"/// <inheritdoc/>";

        private string[] PropertyBody
        {
            get
            {
                if (propertySymbol.IsAbstract)
                {
                    return new[]
                    {
                        " { get; }",
                    };
                }

                var getBodyCastKeyword = requireCastGetBody
                    ? $"({returnType})"
                    : "";

                var list = new StringList()
                    .AppendLine("")
                    .AppendLine("{")
                    .AppendLine($"{__}get => {getBodyCastKeyword}base.{Name};");
                if (setterAccessibility != "NONE")
                {
                    var accessibility = setterAccessibility == "public"
                        ? ""
                        : $"{setterAccessibility} ";
                    list.AppendLine(
                        $"{__}{accessibility}set => base.{Name} = value;"
                    );
                }

                return list.AppendLine("}")
                    .ToArray();
            }
        }

        public PropertyDefinition(
            IPropertySymbol propertySymbol,
            ITypeSymbol? forceReturnType,
            string setterAccessibility
        )
        {
            this.propertySymbol = propertySymbol;
            if (forceReturnType is null)
            {
                returnType = propertySymbol.Type;
                nullableMark = propertySymbol.Type.NullableAnnotation == NullableAnnotation.Annotated
                    ? "?"
                    : "";
                requireCastGetBody = false;
            }
            else
            {
                returnType = forceReturnType;
                nullableMark = "";
                requireCastGetBody = true;
            }

            this.setterAccessibility = setterAccessibility;
        }
    }
}
