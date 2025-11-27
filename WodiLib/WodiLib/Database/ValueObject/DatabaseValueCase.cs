// ========================================
// Project Name : WodiLib
// File Name    : DatabaseValueCase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     DB項目特殊指定選択肢
    /// </summary>
    [CommonMultiValueObject]
    public partial record DatabaseValueCase
    {
        /// <summary>デフォルト値</summary>
        public static readonly DatabaseValueCase Default = new();

        /// <summary>
        ///     選択肢番号
        /// </summary>
        public DatabaseValueCaseNumber CaseNumber { get; }

        /// <summary>
        ///     選択肢文字列
        /// </summary>
        public DatabaseValueCaseDescription Description { get; }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public DatabaseValueCase() : this(0, "")
        {
        }
    }
}
