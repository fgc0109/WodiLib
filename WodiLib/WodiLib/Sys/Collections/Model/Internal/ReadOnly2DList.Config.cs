// ========================================
// Project Name : WodiLib
// File Name    : ReadOnly2DList.Config.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys.Collections
{
    internal partial class ReadOnly2DList<
        TEditableRowElement,
        TFixedRowElement,
        TReadOnlyRowElement,
        TRowElementSettings,
        TEditableListElement,
        TReadOnlyListElement,
        TListElementSettings
    >
    {
        /// <summary>
        ///     二次元リスト設定
        /// </summary>
        public record Config(
            Config.BuildRowSettingsFromRowIndexDelegate RowSettingsFactoryRowIndex,
            Config.BuildRowFromSettingsDelegate RowFactoryFromSettings,
            Config.BuildListElementFromSettingsDelegate ItemFactory,
            Config.CompareListElementDelegate ItemComparer,
            IWodiLib2DListValidator<TRowElementSettings, TListElementSettings>? Validator
        )
        {
            public delegate TRowElementSettings BuildRowSettingsFromRowIndexDelegate(int rowIndex, int columnLength);

            public delegate TEditableRowElement BuildRowFromSettingsDelegate(
                int rowIndex,
                TRowElementSettings settings
            );

            public delegate TEditableListElement BuildListElementFromSettingsDelegate(TListElementSettings settings);

            public delegate bool CompareListElementDelegate(
                TListElementSettings settings,
                TReadOnlyListElement element
            );

            public int MaxRowCapacity { get; init; } = int.MaxValue;
            public int MinRowCapacity { get; init; } = 0;
            public int MaxColumnCapacity { get; init; } = int.MaxValue;
            public int MinColumnCapacity { get; init; } = 0;
        }
    }
}
