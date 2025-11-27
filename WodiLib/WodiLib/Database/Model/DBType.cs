// ========================================
// Project Name : WodiLib
// File Name    : DBType.cs
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
    [Model(Description = "DBタイプ（XXX.dbtype）")]
    public partial class DBType
    {
        #region Properties

        #region public

        /// <summary>DBタイプデータ</summary>
        [ImmutableProperty(ReturnType = typeof(ReadOnlyDatabaseTypeMetadataTable))]
        [SettingsProperty(
            ReturnType = typeof(IDatabaseTypeMetadataTableSettings),
            DefaultValue = "new DatabaseTypeMetadataTableSettings()"
        )]
        public DatabaseTypeMetadataTable TypeMetadataTable
        {
            get => typeMetadataTable;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(TypeMetadataTable));

                SetField(ref typeMetadataTable, value);
            }
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Fields

        private DatabaseTypeMetadataTable typeMetadataTable;

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
        public DBType(IDBTypeSettings settings)
        {
            ThrowHelper.ValidateArgumentNotNull(settings is null, nameof(settings));
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.TypeMetadataTable is null,
                nameof(settings),
                nameof(settings.TypeMetadataTable)
            );
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.TypeMetadataTable.Settings is null,
                nameof(settings),
                nameof(settings.TypeMetadataTable.Settings)
            );
            ThrowHelper.ValidateArgumentPropertyItemsHasNotNull(
                settings.TypeMetadataTable.Settings.HasNullItem(),
                nameof(settings),
                nameof(settings.TypeMetadataTable.Settings)
            );

            typeMetadataTable = new DatabaseTypeMetadataTable(settings.TypeMetadataTable);
        }

        #endregion

        #region Convenience

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public DBType() : this(new DBTypeSettings())
        {
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IDBTypeSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return TypeMetadataTable.ItemEquals(other.TypeMetadataTable);
        }

        #endregion

        #endregion
    }
}
