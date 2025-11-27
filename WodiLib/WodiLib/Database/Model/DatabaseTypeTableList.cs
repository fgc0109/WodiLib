// ========================================
// Project Name : WodiLib
// File Name    : DatabaseSchemaList.cs
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
    public partial record DatabaseTypeTableListSettings
    {
        /// <summary>
        ///     最小要素を持つコンストラクタ
        /// </summary>
        public DatabaseTypeTableListSettings() : this(
            DatabaseTypeTableList.MinCapacity
                .Iterate<IDatabaseTypeTableSettings>(_ => new DatabaseTypeTableSettings())
                .ToList()
        )
        {
        }
    }

    [RestrictedCapacityListImplementTemplate(
        Description = "データベーススキーマリスト",
        ElementType = typeof(DatabaseTypeTable),
        ReadOnlyElementType = typeof(ReadOnlyDatabaseTypeTable),
        SettingsType = typeof(IDatabaseTypeTableSettings),
        MaxCapacity = "DatabaseConst.MaxTypeLength",
        MinCapacity = "DatabaseConst.MinTypeLength"
    )]
    public partial class DatabaseTypeTableList
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
        ///     <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        public DatabaseTypeTableList(IDatabaseTypeTableListSettings settings)
            : this(
                ValidateInitSettings(settings),
                BuildSimpleList(settings.Settings),
                BuildItemFromSettings
            )
        {
        }

        private static IDatabaseTypeTableListSettings ValidateInitSettings(IDatabaseTypeTableListSettings settings)
        {
            ThrowHelper.ValidateArgumentNotNull(settings is null, nameof(settings));
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.Settings.HasNullItem(),
                $"{nameof(settings)}",
                $"{nameof(settings.Settings)}"
            );
            ThrowHelper.ValidateArgumentPropertyItemsHasNotNull(
                settings.Settings.HasNullItem(),
                $"{nameof(settings)}",
                $"{nameof(settings.Settings)}"
            );

            return settings;
        }

        private static SimpleList<DatabaseTypeTable> BuildSimpleList(
            IEnumerable<IDatabaseTypeTableSettings> settings
        )
        {
            return new SimpleList<DatabaseTypeTable>(
                new SimpleListValueBuilder<DatabaseTypeTable>(_ => new DatabaseTypeTable()),
                settings.Select(setting => new DatabaseTypeTable(setting))
            );
        }

        /*
         * SourceGenerator が作成する RequiredConstructor で使用されるメソッド。
         * クラス内に保持する ExtendedList のコンストラクタ引数として指定する。
         */
        private static DatabaseTypeTable BuildItemFromSettings(int index, IDatabaseTypeTableSettings settings)
        {
            return new DatabaseTypeTable(settings);
        }

        /*
         * SourceGenerator が作成する RequiredConstructor で使用されるメソッド。
         * クラス内に保持する ExtendedList のコンストラクタ引数として指定する。
         */
        private IWodiLibListValidator<IDatabaseTypeTableListSettings, IDatabaseTypeTableSettings>
            BuildValidator(
                IDatabaseTypeTableListSettings _,
                SimpleList<DatabaseTypeTable> itemsImpl
            )
        {
            return new RestrictedCapacityListValidator<IDatabaseTypeTableListSettings, IDatabaseTypeTableSettings>(
                countGetter: () => itemsImpl.Count,
                minCapacityGetter: GetMinCapacity,
                maxCapacityGetter: GetMaxCapacity
            );
        }

        #endregion

        #region Convenience

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public DatabaseTypeTableList() : this(new DatabaseTypeTableListSettings())
        {
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IDatabaseTypeTableListSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Settings.SequenceEqual(other.Settings, EqualityComparerFactory.Create<IDatabaseTypeTableSettings>());
        }

        #endregion

        #endregion
    }
}
