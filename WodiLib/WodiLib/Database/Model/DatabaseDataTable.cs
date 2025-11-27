// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDataTable.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using WodiLib.SourceGenerator.Domain.Collection.Attributes;
using WodiLib.SourceGenerator.Domain.Model.Attributes;
using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Database
{
    public partial record DatabaseDataTableSettings
    {
        /// <summary>
        ///     最小要素を持つコンストラクタ
        /// </summary>
        public DatabaseDataTableSettings() : this(
            DatabaseDataTable.MinDataCapacity
                .Iterate<IDatabaseDataRowSettings>(_ => new DatabaseDataRowSettings())
                .ToList()
        )
        {
        }
    }

    /// <remarks>
    ///     1タイプ分の設定値を保持するクラス。
    /// </remarks>
    public partial class DatabaseDataTable
    {
    }

    /// <remarks>
    ///     1タイプ分の設定値を保持するクラス。
    /// </remarks>
    public partial class FixedDatabaseDataTable
    {
    }

    /// <summary>
    ///     DB項目設定値リスト
    /// </summary>
    /// <remarks>
    ///     1タイプ分の設定値を保持するクラス。
    /// </remarks>
    [RestrictedCapacity2DListImplementTemplate(
        Description = "DB項目設定値リスト",
        RowElementType = typeof(DatabaseDataRow),
        FixedRowElementType = typeof(FixedDatabaseDataRow),
        ReadOnlyRowElementType = typeof(ReadOnlyDatabaseDataRow),
        RowSettingsType = typeof(IDatabaseDataRowSettings),
        CellElementType = typeof(DatabaseFieldValue),
        RowPhysicalName = "Data",
        RowLogicalName = "データ",
        ColumnPhysicalName = "Field",
        ColumnLogicalName = "項目",
        CellPhysicalName = "Item",
        CellLogicalName = "項目値",
        MaxRowCapacity = "DatabaseConst.MaxDataLength",
        MinRowCapacity = "DatabaseConst.MinDataLength",
        MaxColumnCapacity = "DatabaseConst.MaxFieldLength",
        MinColumnCapacity = "DatabaseConst.MinFieldLength"
    )]
    public partial class DatabaseDataTable
    {
        #region Constructors

        #region Required

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="settings">設定DTO</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="settings"/> に不適切な <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        public DatabaseDataTable(IDatabaseDataTableSettings settings) : this(
            ValidateInitSettings(settings),
            BuildSimpleList(settings.Settings)
        )
        {
        }

        private static IDatabaseDataTableSettings ValidateInitSettings(IDatabaseDataTableSettings settings)
        {
            ThrowHelper.ValidateArgumentNotNull(settings is null, nameof(settings));
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.Settings is null,
                nameof(settings),
                nameof(settings.Settings)
            );
            ThrowHelper.ValidateArgumentPropertyItemsHasNotNull(
                settings.Settings.HasNullItem(),
                nameof(settings),
                nameof(settings.Settings)
            );
            for (var i = 0; i < settings.Settings.Count; i++)
            {
                var rowSettings = settings.Settings[i];
                ThrowHelper.ValidateArgumentPropertyNotNull(
                    rowSettings is null,
                    nameof(settings),
                    $"{nameof(settings.Settings)}[{i}]"
                );
                ThrowHelper.ValidateArgumentPropertyItemsHasNotNull(
                    settings.Settings.HasNullItem(),
                    nameof(settings),
                    $"{nameof(settings.Settings)}[{i}]"
                );
            }

            return settings;
        }

        private static SimpleList<DatabaseDataRow> BuildSimpleList(
            IEnumerable<IDatabaseDataRowSettings> settings
        )
        {
            return new SimpleList<DatabaseDataRow>(
                valueBuilder: new SimpleListValueBuilder<DatabaseDataRow>((list, index)
                    => new DatabaseDataRow(BuildRowSettingsFromRowIndex(index, list.Count, list))
                ),
                initValues: settings.Select(setting => new DatabaseDataRow(setting))
            );
        }

        /*
         * SourceGenerator が作成する RequiredConstructor で使用されるメソッド。
         * クラス内に保持する Config のコンストラクタ引数として指定する。
         */
        private static IDatabaseDataRowSettings BuildRowSettingsFromRowIndex(
            int rowIndex,
            int columnLength,
            SimpleList<DatabaseDataRow> list
        )
        {
            var settings = new DatabaseDataRowSettings();
            for (var columnIndex = 0; columnIndex < columnLength; columnIndex++)
            {
                settings.Settings.Add(list[0][columnIndex].GetDefaultValue());
            }

            return settings;
        }

        private static DatabaseDataRow BuildRowFromSettings(
            int rowIndex,
            IDatabaseDataRowSettings settings
        )
            => new(settings);

        private static DatabaseFieldValue BuildListElementFromSetting(DatabaseFieldValue settings)
            => settings;

        private IWodiLib2DListValidator<IDatabaseDataTableSettings, IDatabaseDataRowSettings,
                DatabaseFieldValue>
            BuildValidator(
                IDatabaseDataTableSettings _,
                SimpleList<DatabaseDataRow> _2
            )
        {
            var getSelf = new Func<ReadOnlyDatabaseDataTable>(() => this);
            return new DatabaseDataTableValidator<IDatabaseDataTableSettings, IDatabaseDataRowSettings>(
                rowCountGetter: () => getSelf.Invoke().DataCount,
                columnCountGetter: () => getSelf.Invoke().FieldCount,
                fieldTypesGetter: () => getSelf.Invoke().GetDataInternal(0).Select(row => row.Type).ToArray()
            );
        }

        #endregion

        #region Convenience

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public DatabaseDataTable() : this(new DatabaseDataTableSettings())
        {
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        #region GetFieldTypes

        /// <summary>
        ///     このインスタンスが保持するテーブル各列の値種別を返却する。
        /// </summary>
        /// <returns>各列の値種別</returns>
        [ImmutableMethod]
        [FixedLengthListMethod]
        public IEnumerable<DatabaseFieldType> GetFieldTypes()
            => GetDataInternal(0).Select(row => row.Type);

        #endregion

        #region ItemEquals

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IDatabaseDataTableSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Settings.SequenceEqual(
                other.Settings,
                EqualityComparerFactory.Create<IDatabaseDataRowSettings>()
            );
        }

        #endregion

        #endregion

        #endregion
    }
}
