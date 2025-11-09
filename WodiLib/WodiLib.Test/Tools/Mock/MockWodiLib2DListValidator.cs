// ========================================
// Project Name : WodiLib.Test
// File Name    : MockWodiLib2DListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Test.Tools
{
    /// <summary>
    ///     <see cref="IWodiLib2DListValidator{TListSettings,TRowElementSettings,TListElementSettings}"/> モック用
    /// </summary>
    internal class MockWodiLib2DListValidator<TListSettings, TRowElementSettings, TListElementSettings> :
        MockBase<IWodiLib2DListValidator<TListSettings, TRowElementSettings, TListElementSettings>>,
        IWodiLib2DListValidator<TListSettings, TRowElementSettings, TListElementSettings>
    {
        public void Constructor(NamedValue<TListSettings> initSettings)
        {
            AddCalledHistory(nameof(Constructor), initSettings.Value!);
        }

        public void GetRow(NamedValue<int> rowIndex, NamedValue<int> count)
        {
            AddCalledHistory(nameof(GetRow), rowIndex.Value, count.Value);
        }

        public void GetColumn(NamedValue<int> columnIndex, NamedValue<int> count)
        {
            AddCalledHistory(nameof(GetColumn), columnIndex.Value, count.Value);
        }

        public void GetCell(NamedValue<int> rowIndex, NamedValue<int> columnIndex)
        {
            AddCalledHistory(nameof(GetCell), rowIndex.Value, columnIndex.Value);
        }

        public void SetRow(NamedValue<int> rowIndex, NamedValue<IEnumerable<TRowElementSettings>> settings)
        {
            AddCalledHistory(nameof(SetRow), rowIndex.Value, settings.Value);
        }

        public void SetColumn(
            NamedValue<int> columnIndex,
            NamedValue<IEnumerable<IEnumerable<TListElementSettings>>> settings
        )
        {
            AddCalledHistory(nameof(SetColumn), columnIndex.Value, settings.Value);
        }

        public void SetCell(
            NamedValue<int> rowIndex,
            NamedValue<int> columnIndex,
            NamedValue<TListElementSettings> settings
        )
        {
            AddCalledHistory(nameof(SetCell), rowIndex.Value, columnIndex.Value, settings.Value!);
        }

        public void InsertRow(NamedValue<int> rowIndex, NamedValue<IEnumerable<TRowElementSettings>> settings)
        {
            AddCalledHistory(nameof(InsertRow), rowIndex.Value, settings.Value);
        }

        public void InsertColumn(
            NamedValue<int> columnIndex,
            NamedValue<IEnumerable<IEnumerable<TListElementSettings>>> settings
        )
        {
            AddCalledHistory(nameof(InsertColumn), columnIndex.Value, settings.Value);
        }

        public void OverwriteRow(NamedValue<int> rowIndex, NamedValue<IEnumerable<TRowElementSettings>> settings)
        {
            AddCalledHistory(nameof(OverwriteRow), rowIndex.Value, settings.Value);
        }

        public void OverwriteColumn(
            NamedValue<int> columnIndex,
            NamedValue<IEnumerable<IEnumerable<TListElementSettings>>> settings
        )
        {
            AddCalledHistory(nameof(OverwriteColumn), columnIndex.Value, settings.Value);
        }

        public void MoveRow(NamedValue<int> oldRowIndex, NamedValue<int> newRowIndex, NamedValue<int> count)
        {
            AddCalledHistory(nameof(MoveRow), oldRowIndex.Value, newRowIndex.Value, count.Value);
        }

        public void MoveColumn(NamedValue<int> oldColumnIndex, NamedValue<int> newColumnIndex, NamedValue<int> count)
        {
            AddCalledHistory(nameof(MoveColumn), oldColumnIndex.Value, newColumnIndex.Value, count.Value);
        }

        public void RemoveRow(NamedValue<int> rowIndex, NamedValue<int> count)
        {
            AddCalledHistory(nameof(RemoveRow), rowIndex.Value, count.Value);
        }

        public void RemoveColumn(NamedValue<int> columnIndex, NamedValue<int> count)
        {
            AddCalledHistory(nameof(RemoveColumn), columnIndex.Value, count.Value);
        }

        public void AdjustRowLength(NamedValue<int> length)
        {
            AddCalledHistory(nameof(AdjustRowLength), length.Value);
        }

        public void AdjustColumnLength(NamedValue<int> length)
        {
            AddCalledHistory(nameof(AdjustColumnLength), length.Value);
        }

        public void Reset(NamedValue<IEnumerable<TRowElementSettings>> settings, bool canChangeSize = true)
        {
            AddCalledHistory(nameof(Reset), settings.Value, canChangeSize);
        }

        public void Reset()
        {
            AddCalledHistory(nameof(Reset));
        }

        public void Clear()
        {
            AddCalledHistory(nameof(Clear));
        }
    }
}
