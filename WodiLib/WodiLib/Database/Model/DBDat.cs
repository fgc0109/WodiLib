// ========================================
// Project Name : WodiLib
// File Name    : DBDat.cs
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
    [Model(Description = "DBデータ情報（XXXDatabase.dat）")]
    public partial class DBDat
    {
        #region Properties

        #region public

        /// <summary>DB種別</summary>
        [ImmutableProperty]
        [SettingsProperty]
        public DatabaseKind? DbKind
        {
            get => dbKind;
            set => SetField(ref dbKind, value);
        }

        /// <summary>
        ///     DB項目設定値リスト
        /// </summary>
        [ImmutableProperty(
            ReturnType = typeof(ReadOnlyDatabaseDataTableWithDataNamingDefinitionList)
        )]
        [SettingsProperty(
            ReturnType = typeof(IDatabaseDataTableWithDataNamingDefinitionListSettings),
            DefaultValue = "new DatabaseDataTableWithDataNamingDefinitionListSettings()"
        )]
        public DatabaseDataTableWithDataNamingDefinitionList DataTableDefinitionList
        {
            get => dataTableDefinitionList;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(DataTableDefinitionList));

                SetField(ref dataTableDefinitionList, value);
            }
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Fields

        private DatabaseKind? dbKind;
        private DatabaseDataTableWithDataNamingDefinitionList dataTableDefinitionList;

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

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
        public DBDat(IDBDatSettings settings)
        {
            ThrowHelper.ValidateArgumentNotNull(settings is null, nameof(settings));
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.DataTableDefinitionList is null,
                nameof(settings),
                nameof(settings.DataTableDefinitionList)
            );
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.DataTableDefinitionList.Settings is null,
                nameof(settings),
                nameof(settings.DataTableDefinitionList.Settings)
            );
            ThrowHelper.ValidateArgumentPropertyItemsHasNotNull(
                settings.DataTableDefinitionList.Settings.HasNullItem(),
                nameof(settings),
                nameof(settings.DataTableDefinitionList.Settings)
            );

            dbKind = settings.DbKind;
            dataTableDefinitionList =
                new DatabaseDataTableWithDataNamingDefinitionList(settings.DataTableDefinitionList);
        }

        #endregion

        #region Convenience

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public DBDat() : this(new DBDatSettings())
        {
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IDBDatSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return DbKind == other.DbKind
                   && DataTableDefinitionList.ItemEquals(other.DataTableDefinitionList);
        }

        #endregion

        #endregion
    }
}
