// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : TypeSymbolExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace WodiLib.SourceGenerator.Core.Extensions
{
    /// <summary>
    ///     <see cref="ITypeSymbol"/> 拡張クラス
    /// </summary>
    internal static class TypeSymbolExtension
    {
        /// <summary>
        ///     名前空間名を含む全名称を取得する。
        /// </summary>
        /// <param name="src">対象</param>
        /// <returns>名称</returns>
        public static string FullName(this ITypeSymbol src)
        {
            if (src is ITypeParameterSymbol typeParameterSymbol)
            {
                return typeParameterSymbol.Name;
            }

            if (src is INamedTypeSymbol namedTypeSymbol)
            {
                var resultBuilder = new StringBuilder();

                var nameSpace = namedTypeSymbol.Namespace();
                if (nameSpace != "")
                {
                    resultBuilder.Append($"{nameSpace}.");
                }

                var name = namedTypeSymbol.Name;
                resultBuilder.Append(name);

                if (namedTypeSymbol.Arity > 0)
                {
                    resultBuilder.Append("<");
                    resultBuilder.Append(
                        string.Join(
                            ",",
                            namedTypeSymbol.TypeArguments.Select(
                                t => t.FullName()
                            )
                        )
                    );
                    resultBuilder.Append(">");
                }

                var result = resultBuilder.ToString();
                return result switch
                {
                    "System.Void" => "void",
                    _ => result,
                };
            }

            return src.GetType().ToString();
        }
    }
}
