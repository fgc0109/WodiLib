// ========================================
// Project Name : WodiLib
// File Name    : RestrictedCapacity2DListValidationHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     二次元リスト検証 Helper クラス
    /// </summary>
    internal static class RestrictedCapacity2DListValidationHelper
    {
        /// <summary>
        ///     最大・最小行・列設定を検証する。
        /// </summary>
        /// <param name="rowMin">最小行数</param>
        /// <param name="rowMax">最大行数</param>
        /// <param name="columnMin">最小列数</param>
        /// <param name="columnMax">最大列数</param>
        public static void CapacityConfig(
            NamedValue<int> rowMin,
            NamedValue<int> rowMax,
            NamedValue<int> columnMin,
            NamedValue<int> columnMax
        )
        {
            ThrowHelper.InvalidOperationIf(
                rowMin.Value < 0,
                () => ErrorMessage.GreaterOrEqual(rowMin.Name, 0, rowMin.Value)
            );
            ThrowHelper.InvalidOperationIf(
                rowMin.Value > rowMax.Value,
                () => ErrorMessage.GreaterOrEqual(rowMax.Name, $"MinValue({rowMin})", rowMax.Value)
            );

            ThrowHelper.InvalidOperationIf(
                columnMin.Value < 0,
                () => ErrorMessage.GreaterOrEqual(columnMin.Name, 0, columnMin.Value)
            );
            ThrowHelper.InvalidOperationIf(
                columnMin.Value > columnMax.Value,
                () => ErrorMessage.GreaterOrEqual(columnMax.Name, $"MinValue({columnMin})", columnMax.Value)
            );
        }

        /// <summary>
        ///     行 or 列数が適切であることを検証する、
        /// </summary>
        /// <param name="count">行 or 列数</param>
        /// <param name="min">最小数</param>
        /// <param name="max">最大数</param>
        /// <param name="lineName">行名 or 列名</param>
        public static void ItemCount(int count, int min, int max, string lineName)
        {
            ThrowHelper.ValidateArgumentValueRange(count < min || max < count, lineName, count, min, max);
        }

        /// <summary>
        ///     行数および列数が適切であることを検証する。
        /// </summary>
        /// <remarks>
        ///     【事前条件】<br/>
        ///     - すべての行の要素数が一致すること
        /// </remarks>
        /// <param name="target">検証対象</param>
        /// <param name="rowMin">行数最小数</param>
        /// <param name="rowMax">行数最大数</param>
        /// <param name="colMin">列数最小数</param>
        /// <param name="colMax">列数最大数</param>
        /// <param name="itemName">エラーメッセージ中の項目名</param>
        /// <typeparam name="TRowElementSettings">行要素設定型</typeparam>
        /// <typeparam name="TListElementSettings">リスト要素設定型</typeparam>
        /// <exception cref="InvalidOperationException">targetの行数または列数が不適切な場合</exception>
        public static void RowAndColCount<TRowElementSettings, TListElementSettings>(
            IEnumerable<TRowElementSettings> target,
            int rowMin,
            int rowMax,
            int colMin,
            int colMax,
            string itemName = "initItems"
        )
            where TRowElementSettings : IListSettings<TListElementSettings>
        {
            var rowArray = target.ToArray();

            var rowCount = rowArray.Length;
            ThrowHelper.ValidateListMinItemCount(
                rowCount < rowMin,
                $"{itemName}の行数",
                rowMin
            );
            ThrowHelper.ValidateListMaxItemCount(
                rowMax < rowCount,
                $"{itemName}の行数",
                rowMax
            );

            if (rowCount == 0) return;

            var colCount = rowArray[0].Settings.Count;
            ThrowHelper.ValidateListMinItemCount(
                colCount < colMin,
                $"{itemName}の列数",
                colMin
            );
            ThrowHelper.ValidateListMaxItemCount(
                colMax < colCount,
                $"{itemName}の列数",
                colMax
            );
        }

        /// <summary>
        ///     要素数が最大値を超えないことを検証する。
        /// </summary>
        /// <param name="count">要素数</param>
        /// <param name="max">最大値</param>
        /// <param name="lineName">行 or 列名</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="count"/> &gt; <paramref name="max"/> の場合。
        /// </exception>
        public static void ItemMaxCount(int count, int max, string lineName)
        {
            ThrowHelper.ValidateListMaxItemCount(count > max, $"{lineName}数", max);
        }

        /// <summary>
        ///     要素数が最小値を下回らないことを検証する。
        /// </summary>
        /// <param name="count">要素数</param>
        /// <param name="min">最小値</param>
        /// <param name="lineName">行 or 列名</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="count"/> &lt; <paramref name="min"/> の場合。
        /// </exception>
        public static void ItemMinCount(int count, int min, string lineName)
        {
            ThrowHelper.ValidateListMinItemCount(count < min, $"{lineName}数", min);
        }
    }
}
