// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : BinaryOperatorGenerator.cs
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
    WodiLib.SourceGenerator.Operation.Generation.PostInitAction.Attributes.BinaryOperateAttribute;

namespace WodiLib.SourceGenerator.Operation.Generation.Main.Binary
{
    internal class BinaryOperatorGenerator : MainSourceAddableTemplate
    {
        public override InitializeAttributeSourceAddable TargetAttribute => MyAttr.Instance;

        private protected override string HintName(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol source,
            AttributeData selfAttributeData,
            ILogger logger
        )
        {
            var otherTypesChars = selfAttributeData.GetArrayPropertyData(MyAttr.OtherTypes.Name)
                                      ?
                                      .SelectMany(type => type.ToArray())
                                  ?? Array.Empty<char>();
            var operation = selfAttributeData.GetPropertyData<int>(MyAttr.Operation.Name);
            var isLeft = selfAttributeData.GetPropertyData<int>(MyAttr.OtherPosition.Name)
                         == BinaryOperateOtherPosition.Code_Left;

            var bytes = new byte[4];
            var idx = 0;
            foreach (var c in otherTypesChars)
            {
                bytes[idx % 4] ^= (byte)c;
                idx++;
            }

            var otherTypesCode = BitConverter.ToInt32(bytes, 0);

            var operationCodeHex = $"{operation:X}";

            var toFrom = isLeft
                ? "to"
                : "from";

            return
                $"{source.FullName().CompressNameSpace()}.BinaryOperation0x{operationCodeHex}_{toFrom}_{otherTypesCode}";
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
            var otherTypes = selfAttributeData.GetArrayPropertyData(MyAttr.OtherTypes.Name)!;
            var innerCastType = selfAttributeData.GetPropertyData<INamedTypeSymbol>(MyAttr.InnerCastType.Name)!;
            var returnType = selfAttributeData.GetPropertyData<INamedTypeSymbol>(MyAttr.ReturnType.Name)!;
            var targetClassIsLeft = selfAttributeData.GetPropertyData<int>(MyAttr.OtherPosition.Name)
                                    == BinaryOperateOtherPosition.Code_Right;
            var returnTypeCode = selfAttributeData.GetPropertyData<int>(MyAttr.ReturnCodeType.Name);

            var codeMaker =
                new OperationCodeMaker(
                    thisTypeName,
                    otherTypes,
                    innerCastType.FullName(),
                    returnType.FullName(),
                    targetClassIsLeft,
                    returnTypeCode
                );
            return SourceTextFormatter.Format(
                new SourceFormatTarget[]
                {
                    $"{DefinitionSource(source)} {thisTypeName}",
                    $"{{",
                },
                SourceTextFormatter.Format(SourceConstants.IndentSpace, OperationBlock(codeMaker, operation)),
                new SourceFormatTarget[]
                {
                    $"}}",
                }
            );
        }

        /// <summary>
        ///     定義宣言部のソースを生成する。
        /// </summary>
        /// <param name="thisType">型定義情報</param>
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
                            ("+", BinaryOperationType.CanAdd),
                            ("-", BinaryOperationType.CanSubtract),
                            ("*", BinaryOperationType.CanMultiple),
                            ("/", BinaryOperationType.CanDivide),
                            ("%", BinaryOperationType.CanModulo),
                            ("&", BinaryOperationType.CanAnd),
                            ("|", BinaryOperationType.CanOr),
                            ("^", BinaryOperationType.CanXor),
                        }.Where(param => param.determineMake(operationCode))
                        .Select(param => codeMaker.MakeSourceFormatTargetBinaryOperator(param.ope))
                        .ToArray()
                )
                .TrimLastEmptyLine();

        private BinaryOperatorGenerator()
        {
        }

        public static BinaryOperatorGenerator Instance { get; } = new();
    }
}
