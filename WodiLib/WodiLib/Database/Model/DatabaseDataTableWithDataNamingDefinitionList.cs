// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDataTableWithDataNamingDefinitionList.cs
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
    public partial record DatabaseDataTableWithDataNamingDefinitionListSettings
    {
        /// <summary>
        ///     最小要素を持つコンストラクタ
        /// </summary>
        public DatabaseDataTableWithDataNamingDefinitionListSettings() : this(
            DatabaseDataTableWithDataNamingDefinitionList.MinCapacity
                .Iterate<IDatabaseDataTableWithDataNamingDefinitionSettings>(_
                    => new DatabaseDataTableWithDataNamingDefinitionSettings()
                )
                .ToList()
        )
        {
        }
    }

    [RestrictedCapacityListImplementTemplate(
        Description = "DBテーブルデータ &amp; データ名の設定方法",
        ElementType = typeof(DatabaseDataTableWithDataNamingDefinition),
        ReadOnlyElementType = typeof(ReadOnlyDatabaseDataTableWithDataNamingDefinition),
        SettingsType = typeof(IDatabaseDataTableWithDataNamingDefinitionSettings),
        MaxCapacity = "DatabaseConst.MaxTypeLength",
        MinCapacity = "DatabaseConst.MinTypeLength"
    )]
    public partial class DatabaseDataTableWithDataNamingDefinitionList
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
        public DatabaseDataTableWithDataNamingDefinitionList(
            IDatabaseDataTableWithDataNamingDefinitionListSettings settings
        ) : this(
            ValidateInitSettings(settings),
            BuildSimpleList(settings.Settings),
            BuildItemFromSettings
        )
        {
        }

        private static IDatabaseDataTableWithDataNamingDefinitionListSettings ValidateInitSettings(
            IDatabaseDataTableWithDataNamingDefinitionListSettings settings
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

        private static SimpleList<DatabaseDataTableWithDataNamingDefinition> BuildSimpleList(
            IEnumerable<IDatabaseDataTableWithDataNamingDefinitionSettings> settings
        )
        {
            return new SimpleList<DatabaseDataTableWithDataNamingDefinition>(
                valueBuilder: new SimpleListValueBuilder<DatabaseDataTableWithDataNamingDefinition>(_
                    => new DatabaseDataTableWithDataNamingDefinition()
                ),
                initValues: settings.Select(setting => new DatabaseDataTableWithDataNamingDefinition(setting))
            );
        }

        /*
         * SourceGenerator が作成する RequiredConstructor で使用されるメソッド。
         * クラス内に保持する ExtendedList のコンストラクタ引数として指定する。
         */
        private static DatabaseDataTableWithDataNamingDefinition BuildItemFromSettings(
            int index,
            IDatabaseDataTableWithDataNamingDefinitionSettings settings
        )
        {
            return new DatabaseDataTableWithDataNamingDefinition(settings);
        }

        /*
         * SourceGenerator が作成する RequiredConstructor で使用されるメソッド。
         * クラス内に保持する ExtendedList のコンストラクタ引数として指定する。
         */
        private protected
            IWodiLibListValidator<IDatabaseDataTableWithDataNamingDefinitionListSettings,
                IDatabaseDataTableWithDataNamingDefinitionSettings> BuildValidator(
                IDatabaseDataTableWithDataNamingDefinitionListSettings settings,
                SimpleList<DatabaseDataTableWithDataNamingDefinition> itemsImpl
            )
        {
            return new RestrictedCapacityListValidator<IDatabaseDataTableWithDataNamingDefinitionListSettings,
                IDatabaseDataTableWithDataNamingDefinitionSettings>(
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
        public DatabaseDataTableWithDataNamingDefinitionList() : this(
            new DatabaseDataTableWithDataNamingDefinitionListSettings()
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
        public bool ItemEquals(IDatabaseDataTableWithDataNamingDefinitionListSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Settings.SequenceEqual(
                other.Settings,
                EqualityComparerFactory.Create<IDatabaseDataTableWithDataNamingDefinitionSettings>()
            );
        }

        #endregion

        #endregion
    }
}
