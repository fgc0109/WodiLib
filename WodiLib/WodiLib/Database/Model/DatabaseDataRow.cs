// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDataRow.cs
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
    public partial record DatabaseDataRowSettings
    {
        /// <summary>
        ///     項目値種別から設定DTOを作成する。
        /// </summary>
        /// <param name="fieldTypes">項目値種別</param>
        /// <returns>設定DTO</returns>
        public static DatabaseDataRowSettings CreateFromFieldTypes(IEnumerable<DatabaseFieldType> fieldTypes)
        {
            ThrowHelper.ValidateArgumentNotNull(fieldTypes is null, nameof(fieldTypes));
            ThrowHelper.ValidateArgumentItemsHasNotNull(fieldTypes.HasNullItem(), nameof(fieldTypes));

            var fieldValues = fieldTypes
                .Select(fieldType => new DatabaseFieldValue(fieldType))
                .ToList();

            return new DatabaseDataRowSettings(fieldValues);
        }

        /// <summary>
        ///     最小要素を持つコンストラクタ
        /// </summary>
        public DatabaseDataRowSettings() : this(
            DatabaseDataRow.MinCapacity.Iterate(_ => new DatabaseFieldValue(0)).ToList()
        )
        {
        }
    }

    [RestrictedCapacityListImplementTemplate(
        Description = "DB行データクラス",
        ElementType = typeof(DatabaseFieldValue),
        MaxCapacity = "DatabaseConst.MaxFieldLength",
        MinCapacity = "DatabaseConst.MinFieldLength"
    )]
    public partial class DatabaseDataRow
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
        public DatabaseDataRow(IDatabaseDataRowSettings settings)
            : this(
                ValidateInitSettings(settings),
                BuildSimpleList(settings.Settings),
                BuildItemFromSettings
            )
        {
        }

        private static IDatabaseDataRowSettings ValidateInitSettings(IDatabaseDataRowSettings settings)
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

        private static SimpleList<DatabaseFieldValue> BuildSimpleList(
            IEnumerable<DatabaseFieldValue> settings
        )
        {
            return new SimpleList<DatabaseFieldValue>(
                valueBuilder: new SimpleListValueBuilder<DatabaseFieldValue>(_ => new DatabaseFieldValue(0)),
                initValues: settings
            );
        }

        /*
         * SourceGenerator が作成する RequiredConstructor で使用されるメソッド。
         * クラス内に保持する ExtendedList のコンストラクタ引数として指定する。
         */
        private static DatabaseFieldValue BuildItemFromSettings(int index, DatabaseFieldValue settings)
        {
            return settings;
        }

        /*
         * SourceGenerator が作成する RequiredConstructor で使用されるメソッド。
         * クラス内に保持する ExtendedList のコンストラクタ引数として指定する。
         */
        private IWodiLibListValidator<IDatabaseDataRowSettings, DatabaseFieldValue> BuildValidator(
            IDatabaseDataRowSettings _,
            SimpleList<DatabaseFieldValue> itemsImpl
        )
        {
            return new RestrictedCapacityListValidator<IDatabaseDataRowSettings, DatabaseFieldValue>(
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
        public DatabaseDataRow() : this(new DatabaseDataRowSettings())
        {
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IDatabaseDataRowSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Settings.SequenceEqual(other.Settings);
        }

        #endregion

        #endregion
    }
}
