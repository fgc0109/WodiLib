// ========================================
// Project Name : WodiLib
// File Name    : JsonSerializerOptionsExtensions.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Text.Json;

namespace WodiLib.Sys
{
    /// <summary>
    ///     <see cref="JsonSerializerOptions"/> 拡張クラス
    /// </summary>
    internal static class JsonSerializerOptionsExtensions
    {
        public static string GetConvertedPropertyName(
            this JsonSerializerOptions options,
            string propertyName
        )
        {
            return options.PropertyNamingPolicy?.ConvertName(propertyName) ?? propertyName;
        }
    }
}
