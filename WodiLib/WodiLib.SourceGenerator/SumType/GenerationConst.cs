// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : GenerationConst.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.SourceGenerator.SumType
{
    /// <summary>
    ///     共用型SourceGenerator用定数クラス
    /// </summary>
    public static class GenerationConst
    {
        /// <summary>名前空間</summary>
        public static string Namespace => $"{Core.Generation.GenerationConst.RootNameSpace}.Domain";

        /// <summary>クラス名</summary>
        public static string ClassName => "SumType";

        /// <summary>名前空間 + クラス名</summary>
        public static string FullName => $"{Namespace}.{ClassName}";
    }
}
