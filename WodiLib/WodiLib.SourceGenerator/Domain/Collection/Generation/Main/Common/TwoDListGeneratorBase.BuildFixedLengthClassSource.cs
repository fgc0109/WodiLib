// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : TwoDListGeneratorBase.BuildFixedLengthClassSource.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.Core.SourceBuilder;

namespace WodiLib.SourceGenerator.Domain.Collection.Generation.Main.Common
{
    internal abstract partial class TwoDListGeneratorBase
    {
        private static SourceFormatTargetBlock BuildFixedLengthClassSource(ModelInformation modelInfo)
        {
            return modelInfo.IsFixed
                ? BuildFixedLengthClassSource_Standalone(modelInfo)
                : BuildFixedLengthClassSource_Delegating(modelInfo);
        }
    }
}
