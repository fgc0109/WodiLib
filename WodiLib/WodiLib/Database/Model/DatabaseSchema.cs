// ========================================
// Project Name : WodiLib
// File Name    : DatabaseSchema.cs
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
    [Model(Description = "DB情報")]
    public partial class ReadOnlyDatabaseSchema
    {
        #region Properties

        #region public

        /// <summary>
        ///     DB種別
        /// </summary>
        [MutableProperty]
        [SettingsProperty(DefaultValue = "null")]
        public DatabaseKind? DbKind
        {
            get => dbKind;
            protected set => SetField(ref dbKind, value);
        }

        /// <summary>
        ///     [InstanceNotChange] データベーススキーマリスト
        /// </summary>
        [MutableProperty(
            Accessibility = "NONE",
            ReturnType = typeof(DatabaseTypeTableList)
        )]
        [SettingsProperty(
            ReturnType = typeof(IDatabaseTypeTableListSettings),
            DefaultValue = "new DatabaseTypeTableListSettings()"
        )]
        [InstanceNotChange]
        public ReadOnlyDatabaseTypeTableList TypeTableList => typeTableList;

        #endregion

        #region Interface Implementations

        IDatabaseTypeTableListSettings IDatabaseSchemaSettings.TypeTableList => TypeTableList;

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Fields

        private DatabaseKind? dbKind = null;
        private readonly DatabaseTypeTableList typeTableList;

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
        [MutableConstructor]
        public ReadOnlyDatabaseSchema(IDatabaseSchemaSettings settings)
        {
            ThrowHelper.ValidateArgumentNotNull(settings is null, nameof(settings));
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.TypeTableList is null,
                nameof(settings),
                nameof(settings.TypeTableList)
            );
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.TypeTableList.Settings is null,
                nameof(settings),
                $"{nameof(settings.TypeTableList)}.{nameof(settings.TypeTableList.Settings)}"
            );
            ThrowHelper.ValidateArgumentPropertyItemsHasNotNull(
                settings.TypeTableList.Settings.HasNullItem(),
                nameof(settings),
                $"{nameof(settings.TypeTableList)}.{nameof(settings.TypeTableList.Settings)}"
            );

            DbKind = settings.DbKind;
            typeTableList = new DatabaseTypeTableList(settings.TypeTableList);
        }

        #endregion

        #region Convenience

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        [MutableConstructor]
        public ReadOnlyDatabaseSchema() : this(new DatabaseSchemaSettings())
        {
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IDatabaseSchemaSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return DbKind == other.DbKind
                   && TypeTableList.ItemEquals(other.TypeTableList);
        }

        #endregion

        #endregion
    }
}
