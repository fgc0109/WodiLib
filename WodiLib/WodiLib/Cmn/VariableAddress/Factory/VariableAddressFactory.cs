// ========================================
// Project Name : WodiLib
// File Name    : VariableAddressFactory.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Diagnostics.CodeAnalysis;
using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     変数アドレス値Factory
    /// </summary>
    public static class VariableAddressFactory
    {
        /// <summary>
        ///     int値をVariableAddressに変換する。
        /// </summary>
        /// <param name="value">対象</param>
        /// <param name="result">
        ///     変換に成功した場合、変換値。
        ///     変換に失敗した場合、<see langword="null"/>。
        /// </param>
        /// <returns>変換に成功した場合 <see langword="false"/></returns>
        public static bool TryCreate(int value, [NotNullWhen(true)] out VariableAddress? result)
        {
            if (value.IsBetween(ChangeableDatabaseAddress.MinValue, ChangeableDatabaseAddress.MaxValue))
            {
                result = new ChangeableDatabaseAddress(value);
                return true;
            }

            if (value.IsBetween(CommonEventVariableAddress.MinValue, CommonEventVariableAddress.MaxValue))
            {
                result = new CommonEventVariableAddress(value);
                return true;
            }

            if (value.IsBetween(EventInfoAddress.MinValue, EventInfoAddress.MaxValue))
            {
                result = new EventInfoAddress(value);
                return true;
            }

            if (value.IsBetween(HeroInfoAddress.MinValue, HeroInfoAddress.MaxValue))
            {
                result = new HeroInfoAddress(value);
                return true;
            }

            if (value.IsBetween(ThisMapEventInfoAddress.MinValue, ThisMapEventInfoAddress.MaxValue))
            {
                result = new ThisMapEventInfoAddress(value);
                return true;
            }

            if (value.IsBetween(MapEventVariableAddress.MinValue, MapEventVariableAddress.MaxValue))
            {
                result = new MapEventVariableAddress(value);
                return true;
            }

            if (value.IsBetween(MemberInfoAddress.MinValue, MemberInfoAddress.MaxValue))
            {
                result = new MemberInfoAddress(value);
                return true;
            }

            if (value.IsBetween(NormalNumberVariableAddress.MinValue, NormalNumberVariableAddress.MaxValue))
            {
                result = new NormalNumberVariableAddress(value);
                return true;
            }

            if (value.IsBetween(RandomVariableAddress.MinValue, RandomVariableAddress.MaxValue))
            {
                result = new RandomVariableAddress(value);
                return true;
            }

            if (value.IsBetween(SpareNumberVariableAddress.MinValue, SpareNumberVariableAddress.MaxValue))
            {
                result = new SpareNumberVariableAddress(value);
                return true;
            }

            if (value.IsBetween(StringVariableAddress.MinValue, StringVariableAddress.MaxValue))
            {
                result = new StringVariableAddress(value);
                return true;
            }

            if (value.IsBetween(SystemDatabaseAddress.MinValue, SystemDatabaseAddress.MaxValue))
            {
                result = new SystemDatabaseAddress(value);
                return true;
            }

            if (value.IsBetween(SystemVariableAddress.MinValue, SystemVariableAddress.MaxValue))
            {
                result = new SystemVariableAddress(value);
                return true;
            }

            if (value.IsBetween(SystemStringVariableAddress.MinValue, SystemStringVariableAddress.MaxValue))
            {
                result = new SystemStringVariableAddress(value);
                return true;
            }

            if (value.IsBetween(ThisCommonEventVariableAddress.MinValue, ThisCommonEventVariableAddress.MaxValue))
            {
                result = new ThisCommonEventVariableAddress(value);
                return true;
            }

            if (value.IsBetween(ThisMapEventVariableAddress.MinValue, ThisMapEventVariableAddress.MaxValue))
            {
                result = new ThisMapEventVariableAddress(value);
                return true;
            }

            if (value.IsBetween(UserDatabaseAddress.MinValue, UserDatabaseAddress.MaxValue))
            {
                result = new UserDatabaseAddress(value);
                return true;
            }

            result = null;
            return false;
        }

        /// <summary>
        ///     int値から <see cref="VariableAddress"/> インスタンスを生成する
        /// </summary>
        /// <param name="value">対象値</param>
        /// <returns><see cref="VariableAddress"/>のインスタンス</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/>が変数アドレス値として適切でない場合。</exception>
        public static VariableAddress Create(int value)
        {
            if (!TryCreate(value, out var variableAddress))
            {
                throw new ArgumentOutOfRangeException(
                    $"指定された値は変数アドレス値ではありません。（value：{value}）"
                );
            }

            return variableAddress;
        }
    }
}
