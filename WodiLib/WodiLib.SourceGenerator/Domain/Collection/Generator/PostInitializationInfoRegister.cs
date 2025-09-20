// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : PostInitializationInfoRegister.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.SourceGenerator.Core;
using WodiLib.SourceGenerator.Domain.Collection.Generation.PostInitAction.Attributes;

namespace WodiLib.SourceGenerator.Domain.Collection
{
    public partial class Generator
    {
        /// <summary>
        ///     自動生成に必要なクラスソースコード生成情報登録処理
        /// </summary>
        private static class PostInitializationInfoRegister
        {
            public static IEnumerable<IInitializeSourceAddable> MakePostInitializationRegisterInfoList()
            {
                var result = new List<IInitializeSourceAddable>
                {
                    // attributes
                    FixedLength2DListImplementTemplateAttribute.Instance,
                    FixedLengthListConstructorAttribute.Instance,
                    FixedLengthListImplementTemplateAttribute.Instance,
                    FixedLengthListMethodAttribute.Instance,
                    FixedLengthListPropertyAttribute.Instance,
                    RestrictedCapacity2DListImplementTemplateAttribute.Instance,
                    RestrictedCapacityListImplementTemplateAttribute.Instance,
                };

                return result;
            }
        }
    }
}
