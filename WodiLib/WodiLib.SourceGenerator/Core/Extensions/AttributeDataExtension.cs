// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : AttributeDataExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace WodiLib.SourceGenerator.Core.Extensions
{
    /// <summary>
    ///     <see cref="AttributeDataExtension"/> 拡張クラス
    /// </summary>
    internal static class AttributeDataExtension
    {
        /// <summary>
        ///     指定したプロパティの値を取得する。
        /// </summary>
        /// <param name="src">取得対象</param>
        /// <param name="propertyName">プロパティ名</param>
        /// <typeparam name="T">プロパティ型</typeparam>
        /// <returns>プロパティ値</returns>
        public static T? GetPropertyData<T>(this AttributeData src, string propertyName)
        {
            var initializedValue = src.GetPropertyInitializedValue<T?>(propertyName);

            if (initializedValue is not null)
            {
                return (T)initializedValue;
            }

            return src.GetPropertyDefaultValue<T?>(propertyName);
        }

        /// <summary>
        ///     指定したプロパティの値を取得する。
        /// </summary>
        /// <param name="src">取得対象</param>
        /// <param name="propertyName">プロパティ名</param>
        /// <returns>プロパティ値</returns>
        public static string[]? GetArrayPropertyData(this AttributeData src, string propertyName)
        {
            var initializedValue = src.GetPropertyInitializedValue<object[]?>(propertyName);

            if (initializedValue is not null)
            {
                return initializedValue.Select(item => item.ToString()).ToArray();
            }

            return src.GetPropertyDefaultValue<string[]?>(propertyName);
        }

        /// <summary>
        ///     指定したプロパティの値を取得する。
        /// </summary>
        /// <param name="src">取得対象</param>
        /// <param name="propertyName">プロパティ名</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <typeparam name="T">プロパティ型</typeparam>
        /// <returns>プロパティ値</returns>
        public static T? GetPropertyDataRecursive<T>(
            this AttributeData src,
            string propertyName,
            T? defaultValue = default
        )
        {
            var currentValue = GetPropertyData<T?>(src, propertyName);
            if (currentValue is not null)
            {
                return currentValue;
            }

            // 親クラスに同名プロパティの情報を再帰的に探しに行く
            return GetParentPropertyDataRecursive<T?>(src, propertyName, defaultValue);
        }

        /// <summary>
        ///     指定したプロパティの値を取得する。
        /// </summary>
        /// <param name="src">取得対象</param>
        /// <param name="propertyName">プロパティ名</param>
        /// <returns>プロパティ値</returns>
        public static string[]? GetArrayPropertyDataRecursive(this AttributeData src, string propertyName)
        {
            var initializedValue = GetArrayPropertyData(src, propertyName);

            if (initializedValue is not null)
            {
                return initializedValue;
            }

            // 親クラスに同名プロパティの情報を再帰的に探しに行く
            return GetParentArrayPropertyDataRecursive(src, propertyName);
        }

        /// <summary>
        ///     指定したプロパティの値を取得する。
        ///     ただしその属性クラス自身ではなくその親クラスを起点として探索する。
        /// </summary>
        /// <param name="src">取得対象</param>
        /// <param name="propertyName">プロパティ名</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <typeparam name="T">プロパティ型</typeparam>
        /// <returns>プロパティ値</returns>
        public static T? GetParentPropertyDataRecursive<T>(
            this AttributeData src,
            string propertyName,
            T? defaultValue = default
        )
        {
            // 親クラスに同名プロパティの情報を再帰的に探しに行く
            for (var baseType = src.AttributeClass?.BaseType; baseType is not null; baseType = baseType.BaseType)
            {
                if (baseType.GetMembers(propertyName).FirstOrDefault() is IPropertySymbol baseTypeProperty)
                {
                    return (T?)baseTypeProperty.GetDefaultValue()?.Value;
                }
            }

            return defaultValue;
        }

        /// <summary>
        ///     指定したプロパティの値を取得する。
        ///     ただしその属性クラス自身ではなくその親クラスを起点として探索する。
        /// </summary>
        /// <param name="src">取得対象</param>
        /// <param name="propertyName">プロパティ名</param>
        /// <returns>プロパティ値</returns>
        public static string[]? GetParentArrayPropertyDataRecursive(this AttributeData src, string propertyName)
        {
            // 親クラスに同名プロパティの情報を再帰的に探しに行く
            IPropertySymbol? baseTypeProperty = null;
            for (var baseType = src.AttributeClass?.BaseType;
                 baseType is not null || baseTypeProperty is not null;
                 baseType = baseType?.BaseType)
            {
                baseTypeProperty = baseType?.GetMembers(propertyName).FirstOrDefault() as IPropertySymbol;
                if (baseTypeProperty is not null)
                {
                    if (baseTypeProperty.GetDefaultValue()?.IsNull ?? true)
                    {
                        return null;
                    }

                    return baseTypeProperty.GetDefaultValue()
                        ?.Values.Select(v => v.Value?.ToString())
                        .Where(x => x is not null)
                        .OfType<string>()
                        .ToArray();
                }
            }

            return null;
        }

        /// <summary>
        ///     引数で指定された属性プロパティ値を取得する。
        /// </summary>
        /// <param name="src">取得対象</param>
        /// <param name="propertyName">プロパティ名</param>
        /// <typeparam name="T">プロパティ型</typeparam>
        /// <returns>属性プロパティが指定されていた場合、その値。指定されていない場合、<see langword="null"/></returns>
        private static T? GetPropertyInitializedValue<T>(this AttributeData src, string propertyName)
        {
            try
            {
                // コンストラクタで受け取っているか
                var findResult = src.NamedArguments.FirstOrDefault(arg => arg.Key.Equals(propertyName));
                if (findResult.Key is not null)
                {
                    if (typeof(T) == typeof(string[]))
                    {
                        // throw new NotImplementedException();
                        return (T?)(object)findResult.Value.Values.Select(v => (string?)v.Value).ToArray();
                    }

                    return typeof(T).IsArray
                        ? (T?)(object)findResult.Value.Values.Select(v => v.Value).ToArray()
                        : (T?)findResult.Value.Value;
                }

                // プロパティがそのクラスに定義されているか
                return GetPropertyDefaultValue<T>(src, propertyName);
            }
            catch (Exception e)
            {
                throw new Exception($"err: propertyName = {propertyName} ,{e}", e);
            }
        }

        /// <summary>
        ///     <see cref="DefaultValueAttribute"/> で指定されたデフォルト値を取得する。
        /// </summary>
        /// <param name="src">取得対象</param>
        /// <param name="propertyName">プロパティ名</param>
        /// <typeparam name="T">プロパティ型</typeparam>
        /// <returns>デフォルト指定された値</returns>
        private static T? GetPropertyDefaultValue<T>(this AttributeData src, string propertyName)
        {
            var defaultValue = src.AttributeClass
                ?.GetMembers(propertyName)
                .OfType<IPropertySymbol>()
                .FirstOrDefault(member => member.Name.Equals(propertyName))
                ?.GetDefaultValue();

            if (defaultValue is null)
            {
                return default;
            }

            if (!typeof(T).IsArray)
            {
                return (T?)defaultValue.Value.Value;
            }

            if (defaultValue.Value.IsNull)
            {
                return default;
            }

            return (T?)(object)defaultValue.Value.Values.Select(v => v.Value).ToArray();
        }
    }
}
