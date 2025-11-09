// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : IMainSourceAddable.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WodiLib.SourceGenerator.Core
{
    internal interface IMainSourceAddable
    {
        /// <summary>SourceGenerator 対象に付与する Attribute のフルネーム</summary>
        public string TargetAttributeFullName { get; }

        /// <summary>
        ///     自動生成したソースファイル出力処理
        /// </summary>
        public void EmitGeneratedSource(
            SourceProductionContext context,
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol typeSymbol,
            ILogger logger
        );
    }
}
