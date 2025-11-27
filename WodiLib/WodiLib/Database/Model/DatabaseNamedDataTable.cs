// ========================================
// Project Name : WodiLib
// File Name    : DatabaseNamedDataTable.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using WodiLib.SourceGenerator.Domain.Collection.Attributes;
using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Database
{
    public partial record DatabaseNamedDataTableSettings
    {
        /// <summary>
        ///     最小要素を持つコンストラクタ
        /// </summary>
        public DatabaseNamedDataTableSettings() : this(
            DatabaseNamedDataTable.MinDataCapacity
                .Iterate<IDatabaseNamedDataRowSettings>(_ => new DatabaseNamedDataRowSettings())
                .ToList()
        )
        {
        }
    }

    [RestrictedCapacity2DListImplementTemplate(
        Description = "DBレコード情報リストクラス",
        RowElementType = typeof(DatabaseNamedDataRow),
        FixedRowElementType = typeof(FixedDatabaseNamedDataRow),
        ReadOnlyRowElementType = typeof(ReadOnlyDatabaseNamedDataRow),
        RowSettingsType = typeof(IDatabaseNamedDataRowSettings),
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
    public partial class DatabaseNamedDataTable
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
        public DatabaseNamedDataTable(IDatabaseNamedDataTableSettings settings) : this(
            ValidateInitSettings(settings),
            BuildSimpleList(settings.Settings)
        )
        {
        }

        private static IDatabaseNamedDataTableSettings ValidateInitSettings(IDatabaseNamedDataTableSettings settings)
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

        private static SimpleList<DatabaseNamedDataRow> BuildSimpleList(
            IEnumerable<IDatabaseNamedDataRowSettings> settings
        )
        {
            return new SimpleList<DatabaseNamedDataRow>(
                valueBuilder: new SimpleListValueBuilder<DatabaseNamedDataRow>((list, index)
                    => new DatabaseNamedDataRow(BuildRowSettingsFromRowIndex(index, list.Count, list))
                ),
                initValues: settings.Select(setting => new DatabaseNamedDataRow(setting))
            );
        }

        /*
         * SourceGenerator が作成する RequiredConstructor で使用されるメソッド。
         * クラス内に保持する Config のコンストラクタ引数として指定する。
         */
        private static IDatabaseNamedDataRowSettings BuildRowSettingsFromRowIndex(
            int rowIndex,
            int columnLength,
            SimpleList<DatabaseNamedDataRow> list
        )
        {
            var settings = new DatabaseNamedDataRowSettings();
            for (var columnIndex = 0; columnIndex < columnLength; columnIndex++)
            {
                settings.Settings.Add(list[0][columnIndex].GetDefaultValue());
            }

            return settings;
        }

        private static DatabaseNamedDataRow BuildRowFromSettings(
            int rowIndex,
            IDatabaseNamedDataRowSettings settings
        )
            => new(settings);

        private static DatabaseFieldValue BuildListElementFromSetting(DatabaseFieldValue settings)
            => settings;

        private IWodiLib2DListValidator<
                IDatabaseNamedDataTableSettings,
                IDatabaseNamedDataRowSettings,
                DatabaseFieldValue
            >
            BuildValidator(
                IDatabaseNamedDataTableSettings _,
                SimpleList<DatabaseNamedDataRow> _2
            )
        {
            var getSelf = new Func<ReadOnlyDatabaseNamedDataTable>(() => this);
            return new RestrictedCapacity2DListValidator<IDatabaseNamedDataTableSettings, IDatabaseNamedDataRowSettings,
                DatabaseFieldValue>(
                rowCountGetter: () => getSelf.Invoke().DataCount,
                columnCountGetter: () => getSelf.Invoke().FieldCount,
                minRowCapacityGetter: () => MinDataCapacity,
                maxRowCapacityGetter: () => MaxDataCapacity,
                minColumnCapacityGetter: () => MinFieldCapacity,
                maxColumnCapacityGetter: () => MaxFieldCapacity
            );
        }

        #endregion

        #region Convenience

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public DatabaseNamedDataTable() : this(new DatabaseNamedDataTableSettings())
        {
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IDatabaseNamedDataTableSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Settings.SequenceEqual(
                other.Settings,
                EqualityComparerFactory.Create<IDatabaseNamedDataRowSettings>()
            );
        }

        #endregion

        #endregion
    }
}
