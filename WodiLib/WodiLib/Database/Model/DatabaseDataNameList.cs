// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDataNameList.cs
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
    public partial record DatabaseDataNameListSettings
    {
        /// <summary>
        ///     最小要素を持つコンストラクタ
        /// </summary>
        public DatabaseDataNameListSettings() : this(
            DatabaseDataNameList.MinCapacity.Iterate(_ => DataName.Default).ToList()
        )
        {
        }
    }

    [RestrictedCapacityListImplementTemplate(
        Description = "DBデータ名リスト",
        ElementType = typeof(DataName),
        MaxCapacity = "DatabaseConst.MaxDataLength",
        MinCapacity = "DatabaseConst.MinDataLength"
    )]
    public partial class DatabaseDataNameList
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
        public DatabaseDataNameList(IDatabaseDataNameListSettings settings)
            : this(
                ValidateInitSettings(settings),
                BuildSimpleList(settings.Settings),
                BuildItemFromSettings
            )
        {
        }

        private static IDatabaseDataNameListSettings ValidateInitSettings(IDatabaseDataNameListSettings settings)
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

        private static SimpleList<DataName> BuildSimpleList(IEnumerable<DataName> settings)
        {
            return new SimpleList<DataName>(
                valueBuilder: new SimpleListValueBuilder<DataName>(_ => DataName.Default),
                initValues: settings
            );
        }

        /*
         * SourceGenerator が作成する RequiredConstructor で使用されるメソッド。
         * クラス内に保持する ExtendedList のコンストラクタ引数として指定する。
         */
        private static DataName BuildItemFromSettings(int index, DataName settings)
        {
            return settings;
        }

        /*
         * SourceGenerator が作成する RequiredConstructor で使用されるメソッド。
         * クラス内に保持する ExtendedList のコンストラクタ引数として指定する。
         */
        private IWodiLibListValidator<IDatabaseDataNameListSettings, DataName> BuildValidator(
            IDatabaseDataNameListSettings _,
            SimpleList<DataName> itemsImpl
        )
        {
            return new RestrictedCapacityListValidator<IDatabaseDataNameListSettings, DataName>(
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
        public DatabaseDataNameList() : this(new DatabaseDataNameListSettings())
        {
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IDatabaseDataNameListSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Settings.SequenceEqual(other.Settings);
        }

        #endregion

        #endregion
    }
}
