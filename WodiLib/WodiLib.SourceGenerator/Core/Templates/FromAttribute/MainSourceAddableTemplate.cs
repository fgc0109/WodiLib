// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : MainSourceAddableTemplate.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WodiLib.SourceGenerator.Core.Extensions;
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;
using WodiLib.SourceGenerator.Core.SourceBuilder;

namespace WodiLib.SourceGenerator.Core.Templates.FromAttribute
{
    internal abstract class MainSourceAddableTemplate : IMainSourceAddable
    {
        public string TargetAttributeFullName => TargetAttribute.TypeFullName;

        public abstract InitializeAttributeSourceAddable TargetAttribute { get; }

        private int cnt = 0;

        public void EmitGeneratedSource(
            SourceProductionContext context,
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol typeSymbol,
            ILogger logger
        )
        {
            try
            {
                // typeSymbol: 対象属性が付与されたクラスのコンテキスト
                var selfAttributeData = typeSymbol.GetAttributes()
                    .First(attr => attr.AttributeClass?.IsSameOrExtended(TargetAttribute.TypeFullName) ?? false);

                var hintName = HintName(semanticModel, typeDecl, typeSymbol, selfAttributeData, logger);
                try
                {
                    var codeBlock = GenerateSourceCode(semanticModel, typeDecl, typeSymbol, selfAttributeData, logger);
                    if (hintName == "") hintName = $"ERROR_{GetHashCode()}_{cnt++}_EmptyHintName";
                    if (codeBlock == "") codeBlock = "empty codeBlock";
                    context.AddSource(hintName, codeBlock);
                }
                catch (Exception ex)
                {
                    if (hintName == "") hintName = $"ERROR_{GetHashCode()}_{cnt++}_CodeBlockError";
                    context.AddSource(hintName, ex.ToString());
                }
            }
            catch (Exception ex)
            {
                context.AddSource($"ERROR_{GetHashCode()}_{cnt++}_HintNameError", ex.ToString());
                logger.AppendLine(ex.ToString());
            }
        }

        private protected virtual string HintName(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol typeSymbol,
            AttributeData selfAttributeData,
            ILogger logger
        )
            => typeSymbol.FullName().ReplaceAngleBracketsToUnderscore();

        #region SourceCode

        private protected string GenerateSourceCode(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol typeSymbol,
            AttributeData selfAttributeData,
            ILogger logger
        )
        {
            try
            {
                return SourceTextFormatter.Format(
                    GenerateTImportUsingSource(semanticModel, typeDecl, typeSymbol, selfAttributeData, logger),
                    $"namespace {typeSymbol.Namespace()}",
                    $"{{",
                    SourceTextFormatter.Format(
                        SourceConstants.IndentSpace,
                        GenerateBodyWithPartialOuter(semanticModel, typeDecl, typeSymbol, selfAttributeData, logger)
                    ),
                    $"}}"
                );
            }
            catch (Exception ex)
            {
                return string.Join(Environment.NewLine, ex.Message, ex.StackTrace);
            }
        }


        /// <summary>
        ///     外部クラスの定義で包んだ対象クラス本体のソースコードを生成する。
        /// </summary>
        /// <returns>ソース文字列</returns>
        private SourceFormatTargetBlock GenerateBodyWithPartialOuter(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol typeSymbol,
            AttributeData selfAttributeData,
            ILogger logger
        )
        {
            var nextIndentSpaces = "";

            if (typeDecl.Parent is NamespaceDeclarationSyntax)
            {
                // Outer Class なし
                return GenerateTypeDefinitionSource(
                    semanticModel,
                    typeDecl,
                    typeSymbol,
                    selfAttributeData,
                    nextIndentSpaces,
                    logger
                );
            }

            // Outer Class あり
            var forwardLines = new List<List<string>>();
            var backLines = new List<List<string>>();

            var current = typeDecl.Parent;
            while (current is BaseTypeDeclarationSyntax baseTypeDeclarationSyntax)
            {
                forwardLines.ForEach(line => line.Insert(0, SourceConstants.IndentSpace));
                backLines.ForEach(line => line.Insert(0, SourceConstants.IndentSpace));
                nextIndentSpaces += SourceConstants.IndentSpace;

                // Outer Class の定義部分作成
                var outerTypeDefinitionPart = GeneratePartialOuterTypeDefinitionPart(
                    semanticModel,
                    baseTypeDeclarationSyntax
                );

                forwardLines.Insert(0, new List<string> { $"{outerTypeDefinitionPart} {{" });
                backLines.Insert(0, new List<string> { $"}}" });

                current = current.Parent;
            }

            var myTypeDefinitionSource =
                GenerateTypeDefinitionSource(
                    semanticModel,
                    typeDecl,
                    typeSymbol,
                    selfAttributeData,
                    nextIndentSpaces,
                    logger
                );

            var forwardCode = string.Join(Environment.NewLine, forwardLines.Select(line => string.Join("", line)));
            var backCode = string.Join(Environment.NewLine, backLines.Select(line => string.Join("", line)));

            return new SourceFormatTargetBlock(
                new SourceFormatTargetBlock(forwardCode).Concat(myTypeDefinitionSource)
                    .Concat(new SourceFormatTargetBlock(backCode))
            );
        }

        /// <summary>
        ///     外部クラスの partial 定義ソース部分を生成する。
        /// </summary>
        /// <returns>クラス定義開部</returns>
        private static string GeneratePartialOuterTypeDefinitionPart(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax targetTypeDecl
        )
        {
            var targetSymbol = semanticModel.GetDeclaredSymbol(targetTypeDecl);
            if (targetSymbol is null)
            {
                throw new InvalidOperationException("targetSymbol is null.");
            }

            var resultBuilder = new StringBuilder();

            var accessibility = AccessibilityConverter.ConvertSourceText(targetSymbol.DeclaredAccessibility);
            resultBuilder.Append(accessibility);

            if (
                targetSymbol.IsAbstract
                && targetSymbol.TypeKind != TypeKind.Interface // interface の場合 IsAbstract が true になる
            )
            {
                resultBuilder.Append(" abstract");
            }

            resultBuilder.Append(" partial ");

            var objTypeText = targetSymbol.TypeKind switch
            {
                TypeKind.Class => targetSymbol.IsRecord
                    ? "record "
                    : "class ",
                TypeKind.Struct => "struct ",
                TypeKind.Interface => "interface ",
                _ => throw new InvalidOperationException($"Unknown TypeKind. typeKind: ({targetSymbol.TypeKind})"),
            };
            resultBuilder.Append(objTypeText);

            resultBuilder.Append(targetSymbol.Name);

            return resultBuilder.ToString();
        }

        /// <summary>
        ///     タイプ定義宣言部のソースコード出力
        /// </summary>
        /// <param name="semanticModel">シンボル取得用</param>
        /// <param name="typeDecl">対象クラス</param>
        /// <param name="source">対象クラス</param>
        /// <param name="selfAttributeData">起動条件となった属性</param>
        /// <param name="indentSpaces">インデント用スペース文字列</param>
        /// <param name="logger">ロガー</param>
        /// <returns>ソースコード文字列情報</returns>
        private SourceFormatTargetBlock GenerateTypeDefinitionSource(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol source,
            AttributeData selfAttributeData,
            string indentSpaces,
            ILogger logger
        )
            => SourceTextFormatter.Format(
                indentSpaces,
                GenerateTypeDefinitionSource(semanticModel, typeDecl, source, selfAttributeData, logger)
            );

        /// <summary>
        ///     using 部分のソースコード出力
        /// </summary>
        /// <param name="semanticModel">シンボル取得用</param>
        /// <param name="typeDecl">対象クラス</param>
        /// <param name="source">対象クラス</param>
        /// <param name="selfAttributeData">起動条件となった属性</param>
        /// <param name="logger">ロガー</param>
        /// <returns>ソースコード文字列情報</returns>
        private protected virtual SourceFormatTargetBlock GenerateTImportUsingSource(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol source,
            AttributeData selfAttributeData,
            ILogger logger
        ) => SourceFormatTargetBlock.Empty;

        /// <summary>
        ///     タイプ定義宣言部のソースコード出力
        /// </summary>
        /// <param name="semanticModel">シンボル取得用</param>
        /// <param name="typeDecl">対象クラス</param>
        /// <param name="source">対象クラス</param>
        /// <param name="selfAttributeData">起動条件となった属性</param>
        /// <param name="logger">ロガー</param>
        /// <returns>ソースコード文字列情報</returns>
        private protected abstract SourceFormatTargetBlock GenerateTypeDefinitionSource(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol source,
            AttributeData selfAttributeData,
            ILogger logger
        );

        #endregion
    }
}
