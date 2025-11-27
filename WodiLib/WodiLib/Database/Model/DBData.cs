// ========================================
// Project Name : WodiLib
// File Name    : DBData.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Diagnostics.Contracts;
using WodiLib.SourceGenerator.Domain.Model.Attributes;
using WodiLib.Sys;

namespace WodiLib.Database
{
    [Model(Description = "DBデータ（XXX.dbdata）")]
    public partial class DBData
    {
        #region Properties

        #region public

        /// <summary>
        ///     データリスト
        /// </summary>
        [ImmutableProperty(
            ReturnType = typeof(ReadOnlyDatabaseNamedDataTable)
        )]
        [SettingsProperty(
            ReturnType = typeof(IDatabaseNamedDataTableSettings),
            DefaultValue = "new DatabaseNamedDataTableSettings()"
        )]
        public DatabaseNamedDataTable DataTable
        {
            get => dataTable;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(DataTable));

                SetField(ref dataTable, value);
            }
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Fields

        private DatabaseNamedDataTable dataTable;

        #endregion

        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Constructor

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
        public DBData(IDBDataSettings settings)
        {
            ThrowHelper.ValidateArgumentNotNull(settings is null, nameof(settings));
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.DataTable is null,
                nameof(settings),
                nameof(settings.DataTable)
            );
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.DataTable.Settings is null,
                nameof(settings),
                nameof(settings.DataTable.Settings)
            );
            ThrowHelper.ValidateArgumentPropertyItemsHasNotNull(
                settings.DataTable.Settings.HasNullItem(),
                nameof(settings),
                nameof(settings.DataTable.Settings)
            );

            dataTable = new DatabaseNamedDataTable(settings.DataTable);
        }

        #endregion

        #region Convenience

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public DBData() : this(new DBDataSettings())
        {
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IDBDataSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return DataTable.ItemEquals(other.DataTable);
        }

        #endregion

        #endregion
    }
}
