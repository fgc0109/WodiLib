// ========================================
// Project Name : WodiLib
// File Name    : DatabaseValueCaseList.cs
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
    public partial record DatabaseValueCaseListSettings
    {
        /// <summary>
        ///     最小要素を持つコンストラクタ
        /// </summary>
        public DatabaseValueCaseListSettings() : this(
            DatabaseValueCaseList.MinCapacity.Iterate(_ => DatabaseValueCase.Default).ToList()
        )
        {
        }
    }

    /// <summary>
    ///     選択肢情報リスト
    /// </summary>
    [RestrictedCapacityListImplementTemplate(
        Description = "選択肢情報リスト",
        ElementType = typeof(DatabaseValueCase),
        MaxCapacity = "int.MaxValue",
        MinCapacity = 0
    )]
    public partial class DatabaseValueCaseList
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
        public DatabaseValueCaseList(IDatabaseValueCaseListSettings settings)
            : this(
                ValidateInitSettings(settings),
                BuildSimpleList(settings.Settings),
                BuildItemFromSettings
            )
        {
        }

        private static IDatabaseValueCaseListSettings ValidateInitSettings(IDatabaseValueCaseListSettings settings)
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

        private static SimpleList<DatabaseValueCase> BuildSimpleList(IEnumerable<DatabaseValueCase> settings)
        {
            return new SimpleList<DatabaseValueCase>(
                new SimpleListValueBuilder<DatabaseValueCase>(_ => DatabaseValueCase.Default),
                settings
            );
        }

        /*
         * SourceGenerator が作成する RequiredConstructor で使用されるメソッド。
         * クラス内に保持する ExtendedList のコンストラクタ引数として指定する。
         */
        private static DatabaseValueCase BuildItemFromSettings(int index, DatabaseValueCase settings)
        {
            return settings;
        }

        /*
         * SourceGenerator が作成する RequiredConstructor で使用されるメソッド。
         * クラス内に保持する ExtendedList のコンストラクタ引数として指定する。
         */
        private IWodiLibListValidator<IDatabaseValueCaseListSettings, DatabaseValueCase> BuildValidator(
            IDatabaseValueCaseListSettings _,
            SimpleList<DatabaseValueCase> itemsImpl
        )
        {
            return new RestrictedCapacityListValidator<IDatabaseValueCaseListSettings, DatabaseValueCase>(
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
        public DatabaseValueCaseList() : this(
            new DatabaseValueCaseListSettings()
        )
        {
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <summary>
        ///     選択肢番号から選択肢情報を取得する。
        /// </summary>
        /// <param name="caseNumber">選択肢番号</param>
        /// <returns>選択肢情報。情報が存在しない場合 <see langword="null"/>。</returns>
        [ImmutableMethod]
        [FixedLengthListMethod]
        [Pure]
        public DatabaseValueCase? GetForCaseNumber(DatabaseValueCaseNumber? caseNumber)
        {
            return Items.FirstOrDefault(x => x.CaseNumber == caseNumber);
        }

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IDatabaseValueCaseListSettings? other)
        {
            return other is not null
                   && Settings.SequenceEqual(other.Settings, EqualityComparerFactory.Create<DatabaseValueCase>());
        }

        #endregion

        #endregion
    }
}
