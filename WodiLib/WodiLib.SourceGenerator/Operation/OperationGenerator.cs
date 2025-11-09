// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : OperationGenerator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core;
using WodiLib.SourceGenerator.Operation.Generation.Main.Binary;
using WodiLib.SourceGenerator.Operation.Generation.Main.Shift;
using WodiLib.SourceGenerator.Operation.Generation.Main.Unary;
using WodiLib.SourceGenerator.Operation.Generation.PostInitAction.Attributes;
using WodiLib.SourceGenerator.Operation.Generation.PostInitAction.Enums;

namespace WodiLib.SourceGenerator.Operation
{
    /// <summary>
    ///     二項演算子SourceGenerator
    /// </summary>
    [Generator]
    public class OperationGenerator : GeneratorBase
    {
        private protected override string Key => "Operation";

        private protected override IInitializeSourceAddable[] PostInitializationRegisterInfoList { get; } =
        {
            // attributes
            BinaryOperateAttribute.Instance,
            ShiftOperateAttribute.Instance,
            UnaryOperateAttribute.Instance,
            // enums
            BinaryOperateOtherPosition.Instance,
            BinaryOperationType.Instance,
            OperationResultReturnCodeType.Instance,
            ShiftOperationType.Instance,
            UnaryOperationType.Instance,
        };

        private protected override IMainSourceAddable[] TargetAttributeInfoList { get; } =
        {
            BinaryOperatorGenerator.Instance,
            UnaryOperatorGenerator.Instance,
            ShiftOperatorGenerator.Instance,
        };
    }
}
