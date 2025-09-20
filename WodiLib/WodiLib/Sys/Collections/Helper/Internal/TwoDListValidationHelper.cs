// ========================================
// Project Name : WodiLib
// File Name    : TwoDListValidationHelper.cs
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
    internal static class TwoDListValidationHelper
    {
        /// <summary>
        ///     各要素が null でないことを検証する。
        /// </summary>
        /// <param name="target">検証対象</param>
        /// <typeparam name="TRowElementSettings">行要素設定型</typeparam>
        /// <typeparam name="TListElementSettings">リスト要素設定型</typeparam>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="target"/> の行、または列に <see langword="null"/> 要素が存在する場合。
        /// </exception>
        public static void ItemNotNull<TRowElementSettings, TListElementSettings>(
            NamedValue<IEnumerable<TRowElementSettings>> target
        )
            where TRowElementSettings : IListSettings<TListElementSettings>
        {
            var targetArray = target.Value.Select(row => row.Settings).ToArray();

            ThrowHelper.ValidateArgumentItemsHasNotNull(targetArray.HasNullItem(), target.Name);
            ThrowHelper.ValidateArgumentItemsHasNotNull(targetArray.Any(x => x.HasNullItem()), $"{target.Name}の要素");
        }

        /// <summary>
        ///     各要素が null でないことを検証する。
        /// </summary>
        /// <param name="target">検証対象</param>
        /// <typeparam name="TListElementSettings">リスト要素設定型</typeparam>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="target"/> の行、または列に <see langword="null"/> 要素が存在する場合。
        /// </exception>
        public static void ItemNotNull<TListElementSettings>(
            NamedValue<IEnumerable<IEnumerable<TListElementSettings>>> target
        )
        {
            var targetArray = target.Value.Select(row => row.ToArray()).ToArray();

            ThrowHelper.ValidateArgumentItemsHasNotNull(targetArray.HasNullItem(), target.Name);
            ThrowHelper.ValidateArgumentItemsHasNotNull(targetArray.Any(x => x.HasNullItem()), $"{target.Name}の要素");
        }

        /// <summary>
        ///     各行の要素数が一致することを検証する。
        /// </summary>
        /// <param name="target">検証対象</param>
        /// <typeparam name="TRowElementSettings">行要素設定型</typeparam>
        /// <typeparam name="TListElementSettings">リスト要素設定型</typeparam>
        /// <exception cref="ArgumentException">
        ///     行数が2以上であり、かつ0行目の要素数と異なる行が存在する場合
        /// </exception>
        public static void ItemsNotJag<TRowElementSettings, TListElementSettings>(
            IEnumerable<TRowElementSettings> target
        )
            where TRowElementSettings : IListSettings<TListElementSettings>
            => ItemsNotJag(target.Select(x => x.Settings).ToArray());

        /// <summary>
        ///     各行の要素数が一致することを検証する。
        /// </summary>
        /// <param name="target">検証対象</param>
        /// <typeparam name="TListElementSettings">リスト要素設定型</typeparam>
        /// <exception cref="ArgumentException">
        ///     行数が2以上であり、かつ0行目の要素数と異なる行が存在する場合
        /// </exception>
        public static void ItemsNotJag<TListElementSettings>(
            IEnumerable<IEnumerable<TListElementSettings>> target
        )
        {
            var targetArray = target.To2DArray();

            if (targetArray.Length < 2) return;

            var baseLength = targetArray[0].Length;
            var errorRowIndex = targetArray.Skip(1)
                .FindIndex(x => x.Length != baseLength);
            ThrowHelper.ValidateTwoDimListInnerItemLength(errorRowIndex != -1, errorRowIndex);
        }

        /// <summary>
        ///     行・列方向のサイズが一致することを検証する。
        /// </summary>
        /// <param name="settings">検証対象</param>
        /// <param name="rowCount">二次元リストの行数</param>
        /// <param name="columnCount">一致すべき列数</param>
        /// <typeparam name="TRowElementSettings">行要素設定型</typeparam>
        /// <typeparam name="TListElementSettings">リスト要素設定型</typeparam>
        /// <exception cref="ArgumentException"></exception>
        public static void SizeEqual<TRowElementSettings, TListElementSettings>(
            NamedValue<IEnumerable<TRowElementSettings>> settings,
            int rowCount,
            int columnCount
        )
            where TRowElementSettings : IListSettings<TListElementSettings>
        {
            var settings2DArray = settings.Value.Select(s => s.Settings).ToArray();

            ListValidationHelper.ItemCount(
                count: settings2DArray.Length,
                capacity: rowCount,
                itemName: "行数"
            );

            if (rowCount == 0)
            {
                return;
            }

            ThrowHelper.ValidateTwoDimListInnerItemLength(settings2DArray[0].Count != columnCount, columnCount);
            ItemsNotJag<TRowElementSettings, TListElementSettings>(settings.Value);
        }

        /// <summary>
        ///     行方向のサイズが一致することを検証する。
        /// </summary>
        /// <param name="settings">検証対象</param>
        /// <param name="rowCount">二次元リストの行数</param>
        /// <typeparam name="TListElementSettings">リスト要素設定型</typeparam>
        /// <exception cref="ArgumentException"></exception>
        public static void RowSizeEqual<TListElementSettings>(
            NamedValue<IEnumerable<IEnumerable<TListElementSettings>>> settings,
            int rowCount
        )
        {
            var settings2DArray = settings.Value.To2DArray();
            if (settings2DArray.Length == 0)
            {
                return;
            }

            ItemsNotJag(settings.Value);
            ThrowHelper.ValidateTwoDimListInnerItemLength(settings2DArray[0].Length != rowCount, rowCount);
        }

        /// <summary>
        ///     列方向のサイズが一致することを検証する。
        /// </summary>
        /// <param name="settings">検証対象</param>
        /// <param name="rowCount">二次元リストの行数</param>
        /// <param name="columnCount">一致すべき列数</param>
        /// <typeparam name="TRowElementSettings">行要素設定型</typeparam>
        /// <typeparam name="TListElementSettings">リスト要素設定型</typeparam>
        /// <exception cref="ArgumentException"></exception>
        public static void ColumnSizeEqual<TRowElementSettings, TListElementSettings>(
            NamedValue<IEnumerable<TRowElementSettings>> settings,
            int rowCount,
            int columnCount
        )
            where TRowElementSettings : IListSettings<TListElementSettings>
        {
            var settings2DArray = settings.Value.Select(s => s.Settings).ToArray();
            if (settings2DArray.Length == 0)
            {
                return;
            }

            ItemsNotJag<TRowElementSettings, TListElementSettings>(settings.Value);

            if (rowCount == 0)
            {
                return;
            }

            ThrowHelper.ValidateTwoDimListInnerItemLength(settings2DArray[0].Count != columnCount, columnCount);
        }

        /// <summary>
        ///     リストの列数が0でないことを検証する。
        /// </summary>
        /// <param name="columnCount">列数</param>
        /// <exception cref="InvalidOperationException">
        ///     要素数が0の場合
        /// </exception>
        public static void ColumnSizeNotZero(NamedValue<int> columnCount)
        {
            ThrowHelper.InvalidOperationIf(
                columnCount.Value == 0,
                () => ErrorMessage.NotExecute($"{columnCount.Name}数が0のため")
            );
        }
    }
}
