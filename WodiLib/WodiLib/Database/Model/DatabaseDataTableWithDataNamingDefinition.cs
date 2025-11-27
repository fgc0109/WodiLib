// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDataTableWithDataNamingDefinition.cs
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
    [Model(Description = "DBテーブルデータ &amp; データ名の設定方法")]
    public partial class DatabaseDataTableWithDataNamingDefinition
    {
        /*
         * データテーブルの機能は DatabaseDataTable に完全に委譲する。
         */

        #region Properties

        #region public

        /// <summary>データテーブル</summary>
        [ImmutableProperty(
            ReturnType = typeof(ReadOnlyDatabaseDataTable)
        )]
        [SettingsProperty(
            ReturnType = typeof(IDatabaseDataTableSettings),
            DefaultValue = "new DatabaseDataTableSettings()"
        )]
        [Pure]
        public DatabaseDataTable DataTable { get; }

        /// <summary>データ名の設定方法</summary>
        [ImmutableProperty]
        [SettingsProperty(DefaultValue = "DatabaseDataNamingDefinition.Default")]
        public DatabaseDataNamingDefinition DataNamingDefinition
        {
            [Pure] get => dataNamingDefinition;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(value));

                SetField(ref dataNamingDefinition, value);
            }
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/


        #region Fields

        private DatabaseDataNamingDefinition dataNamingDefinition;

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
        public DatabaseDataTableWithDataNamingDefinition(
            IDatabaseDataTableWithDataNamingDefinitionSettings settings
        )
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
                $"{nameof(settings.DataTable)}.${nameof(settings.DataTable.Settings)}"
            );
            ThrowHelper.ValidateArgumentPropertyItemsHasNotNull(
                settings.DataTable.Settings.HasNullItem(),
                nameof(settings),
                $"{nameof(settings.DataTable)}.${nameof(settings.DataTable.Settings)}"
            );
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.DataNamingDefinition is null,
                nameof(settings),
                nameof(settings.DataNamingDefinition)
            );

            DataTable = new DatabaseDataTable(settings.DataTable);
            dataNamingDefinition = settings.DataNamingDefinition;
        }

        #endregion

        #region Convenience

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public DatabaseDataTableWithDataNamingDefinition() : this(
            new DatabaseDataTableWithDataNamingDefinitionSettings()
        )
        {
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IDatabaseDataTableWithDataNamingDefinitionSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return DataNamingDefinition.Equals(other.DataNamingDefinition)
                   && DataTable.ItemEquals(other.DataTable);
        }

        #endregion

        #endregion
    }
}
