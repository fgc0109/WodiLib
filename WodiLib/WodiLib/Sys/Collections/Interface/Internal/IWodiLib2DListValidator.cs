// ========================================
// Project Name : WodiLib
// File Name    : IWodiLib2DListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.ComponentModel;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     WodiLib 独自実装二次元リスト検証処理インタフェース
    /// </summary>
    /// <remarks>
    ///     各種検証において不正な引数の場合例外を発生させる。
    /// </remarks>
    /// <typeparam name="TListSettings">リストの入力パラメータ型</typeparam>
    /// <typeparam name="TRowElementSettings">行要素設定型</typeparam>
    /// <typeparam name="TListElementSettings">リスト要素設定型</typeparam>
    internal interface IWodiLib2DListValidator<TListSettings, TRowElementSettings, TListElementSettings>
    {
        /// <summary>
        ///     コンストラクタの検証処理
        /// </summary>
        /// <param name="initSettings">初期要素</param>
        void Constructor(NamedValue<TListSettings> initSettings);

        /// <summary>
        ///     GetRowRange メソッドの検証処理
        /// </summary>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="count">行数</param>
        void GetRow(NamedValue<int> rowIndex, NamedValue<int> count);

        /// <summary>
        ///     GetColumnRange メソッドの検証処理
        /// </summary>
        /// <param name="columnIndex">列インデックス</param>
        /// <param name="count">列数</param>
        void GetColumn(NamedValue<int> columnIndex, NamedValue<int> count);

        /// <summary>
        ///     GetCell メソッドの検証処理
        /// </summary>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnIndex">列インデックス</param>
        void GetCell(NamedValue<int> rowIndex, NamedValue<int> columnIndex);

        /// <summary>
        ///     SetRowRange メソッドの検証処理
        /// </summary>
        /// <param name="rowIndex">更新開始行インデックス</param>
        /// <param name="settings">更新行要素</param>
        void SetRow(NamedValue<int> rowIndex, NamedValue<IEnumerable<TRowElementSettings>> settings);

        /// <summary>
        ///     SetColumnRange メソッドの検証処理
        /// </summary>
        /// <param name="columnIndex">更新開始列インデックス</param>
        /// <param name="settings">更新列要素</param>
        void SetColumn(
            NamedValue<int> columnIndex,
            NamedValue<IEnumerable<IEnumerable<TListElementSettings>>> settings
        );

        /// <summary>
        ///     SetCell メソッドの検証処理
        /// </summary>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnIndex">列インデックス</param>
        /// <param name="settings">更新要素</param>
        void SetCell(NamedValue<int> rowIndex, NamedValue<int> columnIndex, NamedValue<TListElementSettings> settings);

        /// <summary>
        ///     AddRowRange, InsertRowRange メソッドの検証処理
        /// </summary>
        /// <param name="rowIndex">挿入先行インデックス</param>
        /// <param name="settings">挿入行要素</param>
        void InsertRow(NamedValue<int> rowIndex, NamedValue<IEnumerable<TRowElementSettings>> settings);

        /// <summary>
        ///     AddColumnRange, InsertColumnRange メソッドの検証処理
        /// </summary>
        /// <param name="columnIndex">挿入先列インデックス</param>
        /// <param name="settings">挿入列要素</param>
        void InsertColumn(
            NamedValue<int> columnIndex,
            NamedValue<IEnumerable<IEnumerable<TListElementSettings>>> settings
        );

        /// <summary>
        ///     OverwriteRow メソッドの検証処理
        /// </summary>
        /// <param name="rowIndex">上書き開始行インデックス</param>
        /// <param name="settings">上書き行要素</param>
        void OverwriteRow(NamedValue<int> rowIndex, NamedValue<IEnumerable<TRowElementSettings>> settings);

        /// <summary>
        ///     OverwriteColumn メソッドの検証処理
        /// </summary>
        /// <param name="columnIndex">上書き開始列インデックス</param>
        /// <param name="settings">上書き列要素</param>
        void OverwriteColumn(
            NamedValue<int> columnIndex,
            NamedValue<IEnumerable<IEnumerable<TListElementSettings>>> settings
        );

        /// <summary>
        ///     MoveRowRange メソッドの検証処理
        /// </summary>
        /// <param name="oldRowIndex">移動する行のインデックス開始位置</param>
        /// <param name="newRowIndex">移動先の行インデックス開始位置</param>
        /// <param name="count">移動させる行数</param>
        void MoveRow(NamedValue<int> oldRowIndex, NamedValue<int> newRowIndex, NamedValue<int> count);

        /// <summary>
        ///     MoveColumnRange メソッドの検証処理
        /// </summary>
        /// <param name="oldColumnIndex">移動する列のインデックス開始位置</param>
        /// <param name="newColumnIndex">移動先の列インデックス開始位置</param>
        /// <param name="count">移動させる列数</param>
        void MoveColumn(NamedValue<int> oldColumnIndex, NamedValue<int> newColumnIndex, NamedValue<int> count);

        /// <summary>
        ///     RemoveRowRange メソッドの検証処理
        /// </summary>
        /// <param name="rowIndex">除去開始行インデックス</param>
        /// <param name="count">除去する行数</param>
        void RemoveRow(NamedValue<int> rowIndex, NamedValue<int> count);

        /// <summary>
        ///     RemoveColumnRange メソッドの検証処理
        /// </summary>
        /// <param name="columnIndex">除去開始列インデックス</param>
        /// <param name="count">除去する列数</param>
        void RemoveColumn(NamedValue<int> columnIndex, NamedValue<int> count);

        /// <summary>
        ///     AdjustRowLength メソッドの検証処理
        /// </summary>
        /// <param name="length">調整行数</param>
        void AdjustRowLength(NamedValue<int> length);

        /// <summary>
        ///     AdjustColumnLength メソッドの検証処理
        /// </summary>
        /// <param name="length">調整列数</param>
        void AdjustColumnLength(NamedValue<int> length);

        /// <summary>
        ///     Reset メソッドの検証処理
        /// </summary>
        /// <param name="settings">初期化要素</param>
        /// <param name="canChangeSize">サイズ変更を許容するか</param>
        void Reset(
            NamedValue<IEnumerable<TRowElementSettings>> settings,
            bool canChangeSize = true
        );

        /// <summary>
        ///     Reset メソッドの検証処理
        /// </summary>
        void Reset();

        /// <summary>
        ///     Clear メソッドの検証処理
        /// </summary>
        void Clear();
    }

    /// <summary>
    ///     WodiLib 独自実装二次元リスト検証処理インタフェースデフォルト実装用拡張クラス
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static class WodiLib2DListValidatorInterfaceExtension
    {
        /// <summary>
        ///     インデクサによる行要素取得の検証処理
        /// </summary>
        /// <param name="validator">validator</param>
        /// <param name="rowIndex">行インデックス</param>
        public static void GetRow<TList, TRow, TElem>(
            this IWodiLib2DListValidator<TList, TRow, TElem> validator,
            NamedValue<int> rowIndex
        )
            => validator.GetRow(rowIndex, ("count", 1));

        /// <summary>
        ///     インデクサによる行要素更新の検証処理
        /// </summary>
        /// <param name="validator">validator</param>
        /// <param name="rowIndex">更新行インデックス</param>
        /// <param name="settings">更新行要素</param>
        public static void SetRow<TList, TRow, TElem>(
            this IWodiLib2DListValidator<TList, TRow, TElem> validator,
            NamedValue<int> rowIndex,
            NamedValue<TRow> settings
        )
            => validator.SetRow(rowIndex, (settings.Name, new[] { settings.Value }));

        /// <summary>
        ///     単一列取得の検証処理
        /// </summary>
        /// <param name="validator">validator</param>
        /// <param name="columnIndex">列インデックス</param>
        public static void GetColumn<TList, TRow, TElem>(
            this IWodiLib2DListValidator<TList, TRow, TElem> validator,
            NamedValue<int> columnIndex
        )
            => validator.GetColumn(columnIndex, ("count", 1));

        /// <summary>
        ///     単一列更新の検証処理
        /// </summary>
        /// <param name="validator">validator</param>
        /// <param name="columnIndex">更新列インデックス</param>
        /// <param name="settings">更新列要素</param>
        public static void SetColumn<TList, TRow, TElem>(
            this IWodiLib2DListValidator<TList, TRow, TElem> validator,
            NamedValue<int> columnIndex,
            NamedValue<IEnumerable<TElem>> settings
        )
            => validator.SetColumn(
                columnIndex,
                new NamedValue<IEnumerable<IEnumerable<TElem>>>(settings.Name, new[] { settings.Value })
            );

        /// <summary>
        ///     AddRow, InsertRow メソッドの検証処理
        /// </summary>
        /// <param name="validator">validator</param>
        /// <param name="rowIndex">挿入先行インデックス</param>
        /// <param name="settings">挿入行要素</param>
        public static void InsertRow<TList, TRow, TElem>(
            this IWodiLib2DListValidator<TList, TRow, TElem> validator,
            NamedValue<int> rowIndex,
            NamedValue<TRow> settings
        )
            => validator.InsertRow(rowIndex, (settings.Name, new[] { settings.Value }));

        /// <summary>
        ///     AddColumn, InsertColumn メソッドの検証処理
        /// </summary>
        /// <param name="validator">validator</param>
        /// <param name="columnIndex">挿入先列インデックス</param>
        /// <param name="settings">挿入列要素</param>
        public static void InsertColumn<TList, TRow, TElem>(
            this IWodiLib2DListValidator<TList, TRow, TElem> validator,
            NamedValue<int> columnIndex,
            NamedValue<IEnumerable<TElem>> settings
        )
            => validator.InsertColumn(columnIndex, (settings.Name, new[] { settings.Value }));

        /// <summary>
        ///     MoveRow メソッドの検証処理
        /// </summary>
        /// <param name="validator">validator</param>
        /// <param name="oldRowIndex">移動する行のインデックス位置</param>
        /// <param name="newRowIndex">移動先の行インデックス位置</param>
        public static void MoveRow<TList, TRow, TElem>(
            this IWodiLib2DListValidator<TList, TRow, TElem> validator,
            NamedValue<int> oldRowIndex,
            NamedValue<int> newRowIndex
        )
            => validator.MoveRow(oldRowIndex, newRowIndex, ("count", 1));

        /// <summary>
        ///     MoveColumn メソッドの検証処理
        /// </summary>
        /// <param name="validator">validator</param>
        /// <param name="oldColumnIndex">移動する列のインデックス位置</param>
        /// <param name="newColumnIndex">移動先の列インデックス位置</param>
        public static void MoveColumn<TList, TRow, TElem>(
            this IWodiLib2DListValidator<TList, TRow, TElem> validator,
            NamedValue<int> oldColumnIndex,
            NamedValue<int> newColumnIndex
        )
            => validator.MoveColumn(oldColumnIndex, newColumnIndex, ("count", 1));

        /// <summary>
        ///     RemoveRow メソッドの検証処理
        /// </summary>
        /// <param name="validator">validator</param>
        /// <param name="rowIndex">除去行インデックス</param>
        public static void RemoveRow<TList, TRow, TElem>(
            this IWodiLib2DListValidator<TList, TRow, TElem> validator,
            NamedValue<int> rowIndex
        )
            => validator.RemoveRow(rowIndex, ("count", 1));

        /// <summary>
        ///     RemoveColumn メソッドの検証処理
        /// </summary>
        /// <param name="validator">validator</param>
        /// <param name="columnIndex">除去列インデックス</param>
        public static void RemoveColumn<TList, TRow, TElem>(
            this IWodiLib2DListValidator<TList, TRow, TElem> validator,
            NamedValue<int> columnIndex
        )
            => validator.RemoveColumn(columnIndex, ("count", 1));
    }
}
