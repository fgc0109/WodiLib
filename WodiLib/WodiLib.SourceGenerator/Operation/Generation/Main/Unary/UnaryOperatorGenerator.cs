// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : UnaryOperatorGenerator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WodiLib.SourceGenerator.Core;
using WodiLib.SourceGenerator.Core.Extensions;
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using WodiLib.SourceGenerator.Core.Templates.FromAttribute;
using WodiLib.SourceGenerator.Operation.Generation.PostInitAction.Enums;
using MyAttr =
    WodiLib.SourceGenerator.Operation.Generation.PostInitAction.Attributes.UnaryOperateAttribute;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.Operation.Generation.Main.Unary
{
    internal class UnaryOperatorGenerator : MainSourceAddableTemplate
    {
        public override InitializeAttributeSourceAddable TargetAttribute => MyAttr.Instance;

        /// <inheritDoc/>
        private protected override string HintName(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol source,
            AttributeData selfAttributeData,
            ILogger logger
        )
        {
            var operationCode = selfAttributeData.GetPropertyData<int>(MyAttr.Operation.Name);

            var operationCodeHex = $"{operationCode:X}";

            return $"{source.FullName().CompressNameSpace()}.UnaryOperation0x{operationCodeHex}";
        }

        private protected override SourceFormatTargetBlock GenerateTypeDefinitionSource(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol source,
            AttributeData selfAttributeData,
            ILogger logger
        )
        {
            var thisTypeName = source.ClassName();

            var operation = selfAttributeData.GetPropertyData<int>(MyAttr.Operation.Name).ToString();
            var innerCastType = selfAttributeData.GetPropertyData<INamedTypeSymbol>(MyAttr.InnerCastType.Name)!;
            var returnTypeCode = selfAttributeData.GetPropertyData<int>(MyAttr.ReturnCodeType.Name);

            var codeMaker = new OperationCodeMaker(thisTypeName, innerCastType.FullName(), returnTypeCode);

            return SourceTextFormatter.Format(
                "",
                new SourceFormatTarget[]
                {
                    $"{DefinitionSource(source)} {thisTypeName}",
                    $"{{",
                },
                SourceTextFormatter.Format(IndentSpace, OperationBlock(codeMaker, operation)),
                new SourceFormatTarget[]
                {
                    $"}}",
                }
            );
        }

        /// <summary>
        ///     定義宣言部のソースを生成する。
        /// </summary>
        /// <param name="thisType">型情報</param>
        /// <returns>ソースコード文字列</returns>
        private static string DefinitionSource(INamedTypeSymbol thisType)
        {
            var resultBuilder = new StringBuilder();

            var accessibility = AccessibilityConverter.ConvertSourceText(thisType.DeclaredAccessibility);
            resultBuilder.Append(accessibility);
            resultBuilder.Append(" partial ");

            if (thisType.IsRecord)
            {
                resultBuilder.Append("record");
            }
            else if (thisType.TypeKind == TypeKind.Class)
            {
                resultBuilder.Append("class");
            }
            else
            {
                resultBuilder.Append("struct");
            }

            return resultBuilder.ToString();
        }

        /// <summary>
        ///     演算子オーバーロードソースコードブロックを生成する。
        /// </summary>
        /// <param name="codeMaker">演算子オーバーロードコード生成処理</param>
        /// <param name="operationCode">オーバーロードする演算子フラグ文字列</param>
        /// <returns></returns>
        private static SourceFormatTargetBlock OperationBlock(OperationCodeMaker codeMaker, string operationCode)
            => SourceFormatTargetBlock.Merge(
                    new (string ope, Func<string, bool> determineMake)[]
                        {
                            ("++", UnaryOperationType.CanIncrease),
                            ("--", UnaryOperationType.CanDecrease),
                            ("~", UnaryOperationType.CanComplement),
                        }.Where(param => param.determineMake(operationCode))
                        .Select(param => codeMaker.MakeSourceFormatTargetUnaryOperator(param.ope))
                        .ToArray()
                )
                .TrimLastEmptyLine();

        private UnaryOperatorGenerator()
        {
        }

        public static UnaryOperatorGenerator Instance { get; } = new();
    }
}
