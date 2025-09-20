// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ConstructorDefinition.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Extensions;
using WodiLib.SourceGenerator.Core.SourceBuilder;

namespace WodiLib.SourceGenerator.Domain.Collection.Generation.Main.Common
{
    /// <summary>
    ///     コンストラクタ定義情報クラス
    /// </summary>
    internal class ConstructorDefinition
    {
        public string[] ImplementationCode => new StringList()
            .AppendLine(DocComment)
            .Append(
                $"{Accessibility} {mutableClassName}{TypeParamDefinition}({ArgTypeAndNamesDefinition})"
            )
            .Append(MethodBody)
            .ToArray();


        private readonly IMethodSymbol methodSymbol;
        private readonly string mutableClassName;

        private string Accessibility
            => AccessibilityConverter.ConvertSourceText(methodSymbol.DeclaredAccessibility);

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

                var defaultValue = !p.HasExplicitDefaultValue
                    ? null
                    : p.Type.FullName() == typeof(string).FullName
                        ? $"\"{p.ExplicitDefaultValue}\""
                        : p.ExplicitDefaultValue!.ToString();

                acc.Item2.Add(
                    defaultValue is not null
                        ? $"{refKind}{p.Type} {p.Name} = {defaultValue}"
                        : $"{refKind}{p.Type} {p.Name}"
                );
                return acc;
            }
        );

        private string ArgTypeAndNamesDefinition => string.Join(", ", ArgDefinitions.TypeAndNames);

        private string DocComment => $"/// <inheritdoc/>";

        private string[] MethodBody
        {
            get
            {
                var bodyParam = string.Join(", ", methodSymbol.Parameters.Select(p => p.Name));
                return new[]
                {
                    $" : base({bodyParam}) {{}}",
                };
            }
        }

        public ConstructorDefinition(
            IMethodSymbol methodSymbol,
            string mutableClassName
        )
        {
            this.methodSymbol = methodSymbol;
            this.mutableClassName = mutableClassName;
        }
    }
}
