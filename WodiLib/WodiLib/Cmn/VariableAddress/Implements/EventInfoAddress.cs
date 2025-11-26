// ========================================
// Project Name : WodiLib
// File Name    : EventInfoAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [Range(9100000, 9179999)] イベント情報アドレス値
    /// </summary>
    [VariableAddress(MinValue = 9100000, MaxValue = 9179999)]
    [VariableAddressGapCalculatable(
        OtherTypes = new[] { typeof(EventInfoAddress), typeof(VariableAddress) }
    )]
    [IntValueObjectJsonConvert]
    public partial record EventInfoAddress : VariableAddress
    {
        /// <summary>変数種別</summary>
        public override VariableAddressValueType ValueType
            => VariableAddressValueType.Numeric;

        /// <summary>取得情報</summary>
        public InfoAddressInfoType InfoType => InfoAddressInfoType.FromCode(RawValue.SubInt(0, 1));

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public EventInfoAddress() : this(MinValue)
        {
        }

        partial void DoConstructorExpansion(int value)
        {
            // 未対応チェック 未対応の場合警告ログ出力
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

            if (VersionConfig.IsUnderVersion(WoditorVersion.Ver2_01))
            {
                // 「イベントの座標」のうち、10の位が5または6のアドレスは
                // ウディタVer2.01未満では非対応
                if (infoCode is 5 or 6)
                {
                    WodiLibLogger.GetInstance()
                        .Warning(
                            VersionWarningMessage.NotUnderInVariableAddress(
                                value,
                                VersionConfig.GetConfigWoditorVersion(),
                                WoditorVersion.Ver2_01
                            )
                        );
                }
            }

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
