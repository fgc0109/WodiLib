// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : SourceFormatTargetHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.Extensions;
using WodiLib.SourceGenerator.Core.SourceBuilder;

namespace WodiLib.SourceGenerator.Core.Templates.FromAttribute
{
    internal static class SourceFormatTargetHelper
    {
        /// <summary>
        ///     プロパティ情報から文字列定数定義コードを生成する。
        /// </summary>
        /// <param name="propertyInfo">プロパティ情報</param>
        /// <param name="targetAttributeData">起点となる対象属性データ</param>
        /// <param name="isOverwrittenProperty">プロパティ上書きフラグ</param>
        /// <returns>ソース文字列情報</returns>
        public static SourceFormatTargetBlock SourceFormatTargetsClassConstant_String(
            PropertyInfo propertyInfo,
            AttributeData targetAttributeData,
            bool? isOverwrittenProperty = null
        )
        {
            var value = targetAttributeData.GetPropertyDataRecursive<string?>(propertyInfo.Name);
            if (value is null)
            {
                return SourceFormatTargetBlock.Empty;
            }

            if (!value.StartsWith("\"") && !value.EndsWith("\""))
            {
                value = $"@\"{value}\"";
            }

            return SourceFormatTargetsClassConstant(propertyInfo, value, isOverwrittenProperty: isOverwrittenProperty);
        }

        /// <summary>
        ///     プロパティ情報から文字列定数定義コードを生成する。
        /// </summary>
        /// <param name="propertyInfo">プロパティ情報</param>
        /// <param name="targetAttributeData">起点となる対象属性データ</param>
        /// <param name="isOverwrittenProperty">プロパティ上書きフラグ</param>
        /// <returns>ソース文字列情報</returns>
        public static SourceFormatTargetBlock SourceFormatTargetsClassConstant_Numeric(
            PropertyInfo propertyInfo,
            AttributeData targetAttributeData,
            bool? isOverwrittenProperty = null
        )
        {
            var value = targetAttributeData.GetPropertyDataRecursive<int?>(propertyInfo.Name);
            if (!value.HasValue)
            {
                return SourceFormatTargetBlock.Empty;
            }

            return SourceFormatTargetsClassConstant(
                propertyInfo,
                value.ToString(),
                isOverwrittenProperty: isOverwrittenProperty
            );
        }

        /// <summary>
        ///     プロパティ情報から列挙値定義コードを生成する。
        /// </summary>
        /// <param name="propertyInfo">プロパティ情報</param>
        /// <param name="targetAttributeData">起点となる対象属性データ</param>
        /// <param name="isOverwrittenProperty">プロパティ上書きフラグ</param>
        /// <returns>ソース文字列情報</returns>
        public static SourceFormatTargetBlock SourceFormatTargetsClassConstant_Enum(
            PropertyInfo propertyInfo,
            AttributeData targetAttributeData,
            bool? isOverwrittenProperty = null
        )
        {
            var value = targetAttributeData.GetPropertyDataRecursive<int>(propertyInfo.Name);
            return SourceFormatTargetsClassConstant(
                propertyInfo,
                value.ToString(),
                isCastValue: true,
                isOverwrittenProperty: isOverwrittenProperty
            );
        }

        /// <summary>
        ///     プロパティ情報から定数定義コードを生成する。
        /// </summary>
        /// <param name="propertyInfo">プロパティ情報</param>
        /// <param name="setValue">定数値</param>
        /// <param name="isCastValue">プロパティ値キャストフラグ</param>
        /// <param name="isOverwrittenProperty">プロパティ上書きフラグ</param>
        /// <returns>ソース文字列情報</returns>
        public static SourceFormatTargetBlock SourceFormatTargetsClassConstant(
            PropertyInfo propertyInfo,
            string setValue,
            bool isCastValue = false,
            bool? isOverwrittenProperty = null
        )
        {
            if (isCastValue)
            {
                setValue = $"({propertyInfo.Type}) {setValue}";
            }

            var hasNewModifier = isOverwrittenProperty ?? false;

            return ClassConstants.Generate(
                ClassConstants.ConstantInfo.FromPropertyInfo(
                    propertyInfo,
                    setValue,
                    hasNewModifier
                )
            );
        }
    }
}
