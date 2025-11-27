// ========================================
// Project Name : WodiLib
// File Name    : DatabaseItemDefinitionList.cs
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
    public partial record DatabaseFieldDefinitionListSettings
    {
        /// <summary>
        ///     最小要素を持つコンストラクタ
        /// </summary>
        public DatabaseFieldDefinitionListSettings() : this(
            DatabaseFieldDefinitionList.MinCapacity
                .Iterate<IDatabaseFieldDefinitionSettings>(_ => new DatabaseFieldDefinitionSettings())
                .ToList()
        )
        {
        }
    }

    [RestrictedCapacityListImplementTemplate(
        Description = "DB項目設定リスト",
        ElementType = typeof(DatabaseFieldDefinition),
        ReadOnlyElementType = typeof(ReadOnlyDatabaseFieldDefinition),
        SettingsType = typeof(IDatabaseFieldDefinitionSettings),
        MaxCapacity = "DatabaseConst.MaxFieldLength",
        MinCapacity = "DatabaseConst.MinFieldLength"
    )]
    public partial class DatabaseFieldDefinitionList
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
        public DatabaseFieldDefinitionList(IDatabaseFieldDefinitionListSettings settings)
            : this(
                ValidateInitSettings(settings),
                BuildSimpleList(settings.Settings),
                BuildItemFromSettings
            )
        {
        }

        private static IDatabaseFieldDefinitionListSettings ValidateInitSettings(
            IDatabaseFieldDefinitionListSettings settings
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

        private static SimpleList<DatabaseFieldDefinition> BuildSimpleList(
            IEnumerable<IDatabaseFieldDefinitionSettings> settings
        )
        {
            return new SimpleList<DatabaseFieldDefinition>(
                valueBuilder: new SimpleListValueBuilder<DatabaseFieldDefinition>(_ => new DatabaseFieldDefinition()),
                initValues: settings.Select((setting, i) => BuildItemFromSettings(i, setting))
            );
        }

        /*
         * SourceGenerator が作成する RequiredConstructor で使用されるメソッド。
         * クラス内に保持する ExtendedList のコンストラクタ引数として指定する。
         */
        private static DatabaseFieldDefinition BuildItemFromSettings(
            int _,
            IDatabaseFieldDefinitionSettings settings
        )
        {
            return new DatabaseFieldDefinition(settings);
        }

        /*
         * SourceGenerator が作成する RequiredConstructor で使用されるメソッド。
         * クラス内に保持する ExtendedList のコンストラクタ引数として指定する。
         */
        private IWodiLibListValidator<IDatabaseFieldDefinitionListSettings, IDatabaseFieldDefinitionSettings>
            BuildValidator(
                IDatabaseFieldDefinitionListSettings _,
                SimpleList<DatabaseFieldDefinition> itemsImpl
            )
        {
            return new RestrictedCapacityListValidator<IDatabaseFieldDefinitionListSettings,
                IDatabaseFieldDefinitionSettings>(
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
        public DatabaseFieldDefinitionList() : this(
            new DatabaseFieldDefinitionListSettings()
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
        public bool ItemEquals(IDatabaseFieldDefinitionListSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Settings.SequenceEqual(
                other.Settings,
                EqualityComparerFactory.Create<IDatabaseFieldDefinitionSettings>()
            );
        }

        #endregion

        #endregion
    }
}
