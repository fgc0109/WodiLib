// ========================================
// Project Name : WodiLib
// File Name    : DatabaseConst.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Database
{
    /// <summary>
    ///     データベース関連定数クラス
    /// </summary>
    public static class DatabaseConst
    {
        /// <summary>タイプ数最大値</summary>
        public static int MaxTypeLength => 100;

        /// <summary>タイプ数最小値</summary>
        public static int MinTypeLength => 1;

        /// <summary>データ数最大値</summary>
        public static int MaxDataLength => 10000;

        /// <summary>データ数最小値</summary>
        public static int MinDataLength => 1;

        /// <summary>項目数最大値</summary>
        public static int MaxFieldLength => 100;

        /// <summary>項目数最小値</summary>
        public static int MinFieldLength => 0;
    }
}
