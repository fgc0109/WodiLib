// ========================================
// Project Name : WodiLib
// File Name    : DatabaseNamedDataRow.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using WodiLib.SourceGenerator.Domain.Collection.Attributes;
using WodiLib.SourceGenerator.Domain.Model.Attributes;
using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Database
{
    public partial record DatabaseNamedDataRowSettings
    {
        /// <summary>
        ///     最小要素を持つコンストラクタ
        /// </summary>
        public DatabaseNamedDataRowSettings() : this(
            DatabaseNamedDataRow.MinCapacity.Iterate(_ => new DatabaseFieldValue(0)).ToList()
        )
        {
        }
    }

    /// <summary>
    ///     名前付きDBデータレコードクラス
    /// </summary>
    [RestrictedCapacityListImplementTemplate(
        Description = "名前付きDBデータレコードクラス",
        ElementType = typeof(DatabaseFieldValue),
        MaxCapacity = "DatabaseConst.MaxFieldLength",
        MinCapacity = "DatabaseConst.MinFieldLength"
    )]
    public partial class DatabaseNamedDataRow
    {
        #region Properties

        #region public

        /// <summary>データ名</summary>
        /// <exception cref="PropertyNullException">
        ///     <see langword="null"/> をセットしようとした場合。
        /// </exception>
        [SettingsProperty(DefaultValue = "DataName.Default")]
        [FixedLengthListProperty]
        [ImmutableProperty]
        public DataName DataName
        {
            get => dataName;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(DataName));

                SetField(ref dataName, value);
            }
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Fields

        private DataName dataName = "";

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
        public DatabaseNamedDataRow(IDatabaseNamedDataRowSettings settings) : this(
            ValidateInitSettings(settings),
            BuildSimpleList(settings.Settings),
            BuildItemFromSettings
        )
        {
            dataName = settings.DataName;
        }

        private static IDatabaseNamedDataRowSettings ValidateInitSettings(IDatabaseNamedDataRowSettings settings)
        {
            ThrowHelper.ValidateArgumentNotNull(settings is null, nameof(settings));
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.DataName is null,
                nameof(settings),
                nameof(settings.DataName)
            );
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
        private IWodiLibListValidator<IDatabaseNamedDataRowSettings, DatabaseFieldValue> BuildValidator(
            IDatabaseNamedDataRowSettings _,
            SimpleList<DatabaseFieldValue> itemsImpl
        )
        {
            return new RestrictedCapacityListValidator<IDatabaseNamedDataRowSettings, DatabaseFieldValue>(
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
        public DatabaseNamedDataRow() : this(new DatabaseNamedDataRowSettings())
        {
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IDatabaseNamedDataRowSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Settings.SequenceEqual(other.Settings)
                   && DataName == other.DataName;
        }

        #endregion

        #endregion
    }
}
