// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldValueList.cs
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
    public partial record DatabaseFieldValueListSettings
    {
        /// <summary>
        ///     項目種別と項目数を指定して設定DTOを作成する。
        /// </summary>
        /// <param name="fieldType">項目種別</param>
        /// <param name="length">項目数</param>
        /// <returns>設定DTO</returns>
        public static DatabaseFieldValueListSettings BuildByFieldTypeAndLength(
            DatabaseFieldType fieldType,
            int length
        )
        {
            ThrowHelper.ValidateArgumentNotNull(fieldType is null, nameof(fieldType));

            return new DatabaseFieldValueListSettings(length.Iterate(_ => new DatabaseFieldValue(fieldType)).ToList())
            {
                FieldType = fieldType,
            };
        }

        /// <summary>
        ///     最小要素を持つコンストラクタ
        /// </summary>
        public DatabaseFieldValueListSettings() : this(
            DatabaseFieldValueList.MinCapacity.Iterate(_ => new DatabaseFieldValue(0)).ToList()
        )
        {
        }
    }

    [RestrictedCapacityListImplementTemplate(
        Description = "1項目分のDB項目値リスト",
        ElementType = typeof(DatabaseFieldValue),
        MaxCapacity = "DatabaseConst.MaxDataLength",
        MinCapacity = "DatabaseConst.MinDataLength"
    )]
    public partial class DatabaseFieldValueList
    {
        #region Properties

        #region public

        /// <summary>項目種別</summary>
        [SettingsProperty(
            DefaultValue = "DatabaseFieldType.Int"
        )]
        [FixedLengthListProperty]
        [ImmutableProperty]
        public DatabaseFieldType FieldType
        {
            get => fieldType;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(FieldType));

                fieldType = value;
            }
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Fields

        private DatabaseFieldType fieldType = DatabaseFieldType.Int;

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
        public DatabaseFieldValueList(IDatabaseFieldValueListSettings settings)
            : this(
                ValidateInitSettings(settings),
                BuildSimpleList(settings.Settings),
                BuildItemFromSettings
            )
        {
            FieldType = settings.FieldType;
        }

        private static IDatabaseFieldValueListSettings ValidateInitSettings(IDatabaseFieldValueListSettings settings)
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
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.FieldType is null,
                nameof(settings),
                nameof(settings.FieldType)
            );

            return settings;
        }

        private static SimpleList<DatabaseFieldValue> BuildSimpleList(
            IEnumerable<DatabaseFieldValue> settings
        )
        {
            return new SimpleList<DatabaseFieldValue>(
                valueBuilder: new SimpleListValueBuilder<DatabaseFieldValue>((listImpl, _)
                    => new DatabaseFieldValue(listImpl[0].Type)
                ),
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
        private IWodiLibListValidator<IDatabaseFieldValueListSettings, DatabaseFieldValue> BuildValidator(
            IDatabaseFieldValueListSettings settings,
            SimpleList<DatabaseFieldValue> itemsImpl
        )
        {
            return new DatabaseFieldValueListValidator(
                countGetter: () => itemsImpl.Count,
                minCapacityGetter: GetMinCapacity,
                maxCapacityGetter: GetMaxCapacity,
                fieldType: settings.FieldType
            );
        }

        #endregion

        #region Convenience

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public DatabaseFieldValueList() : this(new DatabaseFieldValueListSettings())
        {
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IDatabaseFieldValueListSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return FieldType == other.FieldType
                   && Settings.SequenceEqual(other.Settings);
        }

        #endregion

        #endregion
    }
}
