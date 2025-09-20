// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ImmutableModelGenerator.MethodDefinition.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Extensions;

namespace WodiLib.SourceGenerator.Domain.Model.Generation.Main
{
    internal partial class ModelGenerator
    {
        /// <summary>
        ///     メソッド定義情報クラス
        /// </summary>
        public class MethodDefinition
        {
            public string[] ImplementationCode => new StringList()
                .AppendLine(DocComment)
                .Append(
                    $"{accessibility} new {ReturnTypeName}{NullableMark} {Name}{TypeParamDefinition}({ArgTypeAndNamesDefinition})"
                )
                .Append(MethodBody)
                .ToArray();


            private readonly IMethodSymbol methodSymbol;
            private readonly string accessibility;

            private string ReturnTypeName => methodSymbol.ReturnType.FullName();
            private string Name => methodSymbol.Name;

            private string TypeParamDefinition => methodSymbol.TypeParameters.Length == 0
                ? ""
                : $"<{string.Join(", ", methodSymbol.TypeParameters.Select(t => t.Name))}>";

            private (List<string> Types, List<string> TypeAndNames) ArgDefinitions => methodSymbol.Parameters.Aggregate(
                (
                    new List<string>(),
                    new List<string>()
                ),
                (acc, p) =>
                {
                    var refKind = p.RefKind switch
                    {
                        RefKind.Out => "out ",
                        RefKind.Ref => "ref ",
                        _ => "",
                    };
                    acc.Item1.Add(p.Type.ToString());
                    acc.Item2.Add($"{refKind}{p.Type} {p.Name}");
                    return acc;
                }
            );

            private string ArgTypeAndNamesDefinition => string.Join(", ", ArgDefinitions.TypeAndNames);

            private string DocComment => $"/// <inheritdoc/>";

            private string[] MethodBody
            {
                get
                {
                    if (methodSymbol.IsAbstract)
                    {
                        return new[]
                        {
                            ";",
                        };
                    }

                    var bodyParam = string.Join(", ", methodSymbol.Parameters.Select(p => p.Name));
                    return new[]
                    {
                        $" => base.{Name}{TypeParamDefinition}({bodyParam});",
                    };
                }
            }

            private string NullableMark => methodSymbol.ReturnType.NullableAnnotation == NullableAnnotation.Annotated
                ? "?"
                : "";

            public MethodDefinition(
                IMethodSymbol methodSymbol,
                string accessibility
            )
            {
                this.methodSymbol = methodSymbol;
                this.accessibility = accessibility;
            }
        }
    }
}
