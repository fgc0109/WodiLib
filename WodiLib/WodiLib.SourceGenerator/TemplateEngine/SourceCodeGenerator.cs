// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : SourceCodeGenerator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core;
using WodiLib.SourceGenerator.TemplateEngine.Generation.Main;
using WodiLib.SourceGenerator.TemplateEngine.Generation.PostInitAction.Attributes;

namespace WodiLib.SourceGenerator.TemplateEngine
{
    /// <summary>
    ///     ソースコードGenerator
    /// </summary>
    [Generator]
    public class SourceCodeGenerator : GeneratorBase
    {
        private protected override string Key => "SourceCode";

        private protected override IInitializeSourceAddable[] PostInitializationRegisterInfoList { get; } =
        {
            // attributes
            TemplateContextAttribute.Instance,
        };

        private protected override IMainSourceAddable[] TargetAttributeInfoList { get; } =
        {
            PartialClassGenerator.Instance,
        };
    }
}
