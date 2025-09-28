// ========================================
// Project Name : WodiLib
// File Name    : HeroInfoAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [Range(9180000, 9180009)] 主人公情報アドレス値
    /// </summary>
    [VariableAddress(MinValue = 9180000, MaxValue = 9180009)]
    [VariableAddressGapCalculatable(
        OtherTypes = new[] { typeof(HeroInfoAddress), typeof(VariableAddress) }
    )]
    public partial record HeroInfoAddress : VariableAddress
    {
        /// <summary>変数種別</summary>
        public override VariableAddressValueType ValueType
            => VariableAddressValueType.Numeric;

        /// <summary>取得情報</summary>
        public InfoAddressInfoType InfoType => InfoAddressInfoType.FromCode(RawValue.SubInt(0, 1));

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public HeroInfoAddress() : this(MinValue)
        {
        }

        partial void DoConstructorExpansion(int value)
        {
            VersionCheck(value);
        }

        /// <summary>
        ///     バージョンによる定義チェックを行い、警告メッセージを出力する
        /// </summary>
        /// <param name="value">変数アドレス値</param>
        [Obsolete("バージョンチェック処理・警告表示は処理を見直す")]
        private static void VersionCheck(int value)
        {
            var infoCode = value % 10;

            if (infoCode is 7 or 8)
            {
                WodiLibLogger.GetInstance()
                    .Warning(
                        VersionWarningMessage.NotUsingVariableAddress(value)
                    );
            }
        }
    }
}
