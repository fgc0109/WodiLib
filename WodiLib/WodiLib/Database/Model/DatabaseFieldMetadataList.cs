// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldMetadataList.cs
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
    public partial record DatabaseFieldMetadataListSettings
    {
        /// <summary>
        ///     最小要素を持つコンストラクタ
        /// </summary>
        public DatabaseFieldMetadataListSettings() : this(
            DatabaseFieldMetadataList.MinCapacity
                .Iterate<IDatabaseFieldMetadataSettings>(_ => new DatabaseFieldMetadataSettings())
                .ToList()
        )
        {
        }
    }

    [RestrictedCapacityListImplementTemplate(
        Description = "DB項目メタ情報リスト",
        ElementType = typeof(DatabaseFieldMetadata),
        ReadOnlyElementType = typeof(ReadOnlyDatabaseFieldMetadata),
        SettingsType = typeof(IDatabaseFieldMetadataSettings),
        MaxCapacity = "DatabaseConst.MaxFieldLength",
        MinCapacity = "DatabaseConst.MinFieldLength"
    )]
    public partial class DatabaseFieldMetadataList
    {
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
        public DatabaseFieldMetadataList(IDatabaseFieldMetadataListSettings settings)
            : this(
                ValidateInitSettings(settings),
                BuildSimpleList(settings.Settings),
                BuildItemFromSettings
            )
        {
        }

        private static IDatabaseFieldMetadataListSettings ValidateInitSettings(
            IDatabaseFieldMetadataListSettings settings
        )
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

            return settings;
        }

        private static SimpleList<DatabaseFieldMetadata> BuildSimpleList(
            IEnumerable<IDatabaseFieldMetadataSettings> settings
        )
        {
            return new SimpleList<DatabaseFieldMetadata>(
                valueBuilder: new SimpleListValueBuilder<DatabaseFieldMetadata>(_ => new DatabaseFieldMetadata()),
                initValues: settings.Select((setting, i) => BuildItemFromSettings(i, setting))
            );
        }

        /*
         * SourceGenerator が作成する RequiredConstructor で使用されるメソッド。
         * クラス内に保持する ExtendedList のコンストラクタ引数として指定する。
         */
        private static DatabaseFieldMetadata BuildItemFromSettings(
            int _,
            IDatabaseFieldMetadataSettings settings
        )
        {
            return new DatabaseFieldMetadata(settings);
        }

        /*
         * SourceGenerator が作成する RequiredConstructor で使用されるメソッド。
         * クラス内に保持する ExtendedList のコンストラクタ引数として指定する。
         */
        private IWodiLibListValidator<IDatabaseFieldMetadataListSettings, IDatabaseFieldMetadataSettings>
            BuildValidator(
                IDatabaseFieldMetadataListSettings _,
                SimpleList<DatabaseFieldMetadata> itemsImpl
            )
        {
            return new RestrictedCapacityListValidator<IDatabaseFieldMetadataListSettings,
                IDatabaseFieldMetadataSettings>(
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
        public DatabaseFieldMetadataList() : this(
            new DatabaseFieldMetadataListSettings()
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
        public bool ItemEquals(IDatabaseFieldMetadataListSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Settings.SequenceEqual(
                other.Settings,
                EqualityComparerFactory.Create<IDatabaseFieldMetadataSettings>()
            );
        }

        #endregion

        #endregion
    }
}
