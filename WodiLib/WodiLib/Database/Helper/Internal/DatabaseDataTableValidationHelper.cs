// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDataTableValidationHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Linq;
using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Database
{
    /// <summary>
    ///     <see cref="DatabaseDataTable"/> 引数検証 Helper クラス
    /// </summary>
    internal static class DatabaseDataTableValidationHelper
    {
        /// <summary>
        ///     引数値種別チェック
        /// </summary>
        /// <remarks>
        ///     <paramref name="values"/> の0要素を基準にする。<br/>
        ///     このメソッドを実行する前に <paramref name="values"/> の内側リストの要素数が全て同一であることを検証済み。
        /// </remarks>
        /// <param name="values">検証対象</param>
        /// <param name="direction">軸方向</param>
        /// <exception cref="ArgumentException">
        ///     行数が2以上かつ
        ///     1行目以降に0行目の値種別と異なるデータが含まれる場合
        /// </exception>
        public static void ValidateItemType(DatabaseFieldValue[][] values, Direction direction)
        {
            if (values.Length == 0) return;

            var types = direction != Direction.Column
                ? values[0].Select(value => value.Type)
                : values.Select(line => line.First().Type);

            ValidateItemType(values, types.ToArray(), direction);
        }

        /// <summary>
        ///     引数値種別チェック（判定基準あり）
        /// </summary>
        /// <remarks>
        ///     <paramref name="standardType"/> を基準にする。<br/>
        ///     このメソッドを実行する前に <paramref name="values"/> の内側リストの要素数が全て同一であることを検証済み。
        /// </remarks>
        /// <param name="values">検証対象</param>
        /// <param name="standardType">基準となる値型リスト</param>
        /// <param name="direction">軸方向</param>
        /// <exception cref="ArgumentException">
        ///     行数が2以上かつ
        ///     1行目以降に0行目の値種別と異なるデータが含まれる場合
        /// </exception>
        public static void ValidateItemType(
            DatabaseFieldValue[][] values,
            DatabaseFieldType[] standardType,
            Direction direction
        )
        {
            if (values.Length == 0) return;

            var hasDiffType = direction != Direction.Column
                ? HasDifferenceTypeInRow(values, standardType)
                : HasDifferenceTypeInColumn(values, standardType);

            if (hasDiffType)
            {
                throw new ArgumentException(
                    ErrorMessage.NotExecute("種類の異なる項目が含まれるため")
                );
            }
        }

        /// <summary>
        ///     同じ行内に異なる値種別の値が含まれる行が存在するかどうかを判定する。
        /// </summary>
        /// <param name="values">検証対象</param>
        /// <param name="standardType">基準となる値型リスト</param>
        /// <returns>存在する場合true</returns>
        private static bool HasDifferenceTypeInRow(DatabaseFieldValue[][] values, DatabaseFieldType[] standardType)
        {
            return values.Any(line => !standardType.SequenceEqual(line.Select(item => item.Type)));
        }

        /// <summary>
        ///     同じ列内に異なる値種別の値が含まれる行が存在するかどうかを判定する。
        /// </summary>
        /// <param name="values">検証対象</param>
        /// <param name="standardType">基準となる値型リスト</param>
        /// <returns>存在する場合true</returns>
        private static bool HasDifferenceTypeInColumn(DatabaseFieldValue[][] values, DatabaseFieldType[] standardType)
        {
            return values.Any((line, idx) =>
                line.Any(item => item.Type != standardType[idx])
            );
        }
    }
}
