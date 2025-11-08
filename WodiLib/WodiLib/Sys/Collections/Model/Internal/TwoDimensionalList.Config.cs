// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalList.Config.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys.Collections
{
    internal partial class TwoDimensionalList<
        TListSettings,
        TEditableRowElement,
        TFixedRowElement,
        TRowElementSettings,
        TEditableListElement,
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
            IWodiLib2DListValidator<TListSettings, TRowElementSettings, TListElementSettings>? Validator
        )
        {
            public delegate TRowElementSettings BuildRowSettingsFromRowIndexDelegate(
                int rowIndex,
                int columnLength,
                SimpleList<TEditableRowElement> list
            );

            public delegate TEditableRowElement BuildRowFromSettingsDelegate(
                int rowIndex,
                TRowElementSettings settings
            );

            public delegate TEditableListElement BuildListElementFromSettingsDelegate(TListElementSettings settings);

            public delegate bool CompareListElementDelegate(
                TListElementSettings settings,
                TEditableRowElement element
            );

            public int MaxRowCapacity { get; init; } = int.MaxValue;
            public int MinRowCapacity { get; init; } = 0;
            public int MaxColumnCapacity { get; init; } = int.MaxValue;
            public int MinColumnCapacity { get; init; } = 0;
        }
    }
}
