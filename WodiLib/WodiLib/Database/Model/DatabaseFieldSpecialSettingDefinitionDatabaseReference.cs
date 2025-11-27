// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldSpecialSettingDefinitionDatabaseReference.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using WodiLib.SourceGenerator.Domain.Model.Attributes;
using WodiLib.Sys;

namespace WodiLib.Database
{
    public partial interface IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings :
        IDatabaseFieldSpecialSettingDefinitionSettings
    {
    }

    public partial record DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
    {
        /// <inheritdoc/>
        public bool ItemEquals(IDatabaseFieldSpecialSettingDefinitionSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (other.TryCastDatabaseReferenceSettings(out var otherDatabaseReference))
            {
                return ItemEquals(otherDatabaseReference);
            }

            return false;
        }

        /// <inheritdoc/>
        [Pure]
        public bool TryCastNormalSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionNormalSettings? result
        )
        {
            result = null;
            return false;
        }

        /// <inheritdoc/>
        [Pure]
        public bool TryCastLoadFileSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionLoadFileSettings? result
        )
        {
            result = null;
            return false;
        }

        /// <inheritdoc/>
        [Pure]
        public bool TryCastDatabaseReferenceSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings? result
        )
        {
            result = this;
            return true;
        }

        /// <inheritdoc/>
        [Pure]
        public bool TryCastManualSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionManualSettings? result
        )
        {
            result = null;
            return false;
        }
    }

    [Model(
        Description = "データ内容の特殊設定＝「データベース参照」の場合の特殊設定内容"
    )]
    public partial class DatabaseFieldSpecialSettingDefinitionDatabaseReference : IDatabaseFieldSpecialSettingDefinition
    {
        #region Constants

        #region private

        /// <summary>選択値番号最大値</summary>
        private const int CaseNumberMax = -1;

        /// <summary>選択値番号最小値</summary>
        private const int CaseNumberMin = -3;

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Properties

        #region public

        /// <inheritdoc/>
        [ImmutableProperty]
        [Pure]
        public DatabaseFieldSpecialSettingType SettingType => DatabaseFieldSpecialSettingType.ReferDatabase;

        /// <inheritdoc/>
        [ImmutableProperty]
        [Pure]
        public DatabaseFieldType DefaultType => DatabaseFieldType.Int;

        /// <inheritdoc cref="IDatabaseFieldSpecialSettingDefinition.InitValue"/>
        [SettingsProperty(DefaultValue = "DatabaseValueInt.Default")]
        [ImmutableProperty]
        public DatabaseValueInt InitValue
        {
            [Pure] get => initValue;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(InitValue));

                SetField(ref initValue, value);
            }
        }

        /// <summary>DB種別</summary>
        [SettingsProperty(DefaultValue = "DatabaseReferType.Changeable")]
        [ImmutableProperty]
        public DatabaseReferType DatabaseReferKind
        {
            [Pure] get => databaseReferKind;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(DatabaseReferKind));
                SetField(ref databaseReferKind, value);
            }
        }

        /// <summary>タイプID</summary>
        [SettingsProperty(DefaultValue = "TypeId.Default")]
        [ImmutableProperty]
        public TypeId DatabaseDbTypeId
        {
            [Pure] get => databaseDbTypeId;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(DatabaseDbTypeId));
                SetField(ref databaseDbTypeId, value);
            }
        }

        /// <summary>追加項目使用フラグ</summary>
        [SettingsProperty(DefaultValue = "false")]
        [ImmutableProperty]
        public bool IsUseAdditionalItems
        {
            [Pure] get => isUseAdditionalItems;
            set => SetField(ref isUseAdditionalItems, value);
        }

        /// <summary>追加項目（値=-1）の選択肢文字列</summary>
        /// <remarks>
        ///     <see cref="IsUseAdditionalItems"/> が <see langword="false"/> であっても値を返す。
        /// </remarks>
        [SettingsProperty(DefaultValue = "DatabaseValueCaseDescription.Default")]
        [ImmutableProperty]
        public DatabaseValueCaseDescription AdditionalCase1
        {
            [Pure] get => specialCases[OuterIndexToInnerIndex(-1)].Description;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(value));

                var index = OuterIndexToInnerIndex(-1);
                var beforeDescription = specialCases.GetInternal(index).Description;

                if (beforeDescription.Equals(value)) return;

                specialCases.SetInternal(index, new DatabaseValueCase(-1, value));
                NotifyPropertyChanged();
            }
        }

        /// <summary>追加項目（値=-2）の選択肢文字列</summary>
        /// <remarks>
        ///     <see cref="IsUseAdditionalItems"/> が <see langword="false"/> であっても値を返す。
        /// </remarks>
        [SettingsProperty(DefaultValue = "DatabaseValueCaseDescription.Default")]
        [ImmutableProperty]
        public DatabaseValueCaseDescription AdditionalCase2
        {
            [Pure] get => specialCases[OuterIndexToInnerIndex(-2)].Description;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(value));
                var index = OuterIndexToInnerIndex(-2);
                var beforeDescription = specialCases.GetInternal(index).Description;

                if (beforeDescription.Equals(value)) return;

                specialCases.SetInternal(index, new DatabaseValueCase(-2, value));
                NotifyPropertyChanged();
            }
        }

        /// <summary>追加項目（値=-3）の選択肢文字列</summary>
        /// <remarks>
        ///     <see cref="IsUseAdditionalItems"/> が <see langword="false"/> であっても値を返す。
        /// </remarks>
        [SettingsProperty(DefaultValue = "DatabaseValueCaseDescription.Default")]
        [ImmutableProperty]
        public DatabaseValueCaseDescription AdditionalCase3
        {
            [Pure] get => specialCases[OuterIndexToInnerIndex(-3)].Description;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(value));
                var index = OuterIndexToInnerIndex(-3);
                var beforeDescription = specialCases.GetInternal(index).Description;

                if (beforeDescription.Equals(value)) return;

                specialCases.SetInternal(index, new DatabaseValueCase(-3, value));
                NotifyPropertyChanged();
            }
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Fields

        private DatabaseValueInt initValue;
        private readonly DatabaseValueCaseList specialCases;
        private DatabaseReferType databaseReferKind;
        private TypeId databaseDbTypeId;
        private bool isUseAdditionalItems;

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
        public DatabaseFieldSpecialSettingDefinitionDatabaseReference(
            IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings settings
        )
        {
            ThrowHelper.ValidateArgumentNotNull(settings is null, nameof(settings));
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.InitValue is null,
                nameof(settings),
                nameof(settings.InitValue)
            );
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.DatabaseReferKind is null,
                nameof(settings),
                nameof(settings.DatabaseReferKind)
            );
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.DatabaseDbTypeId is null,
                nameof(settings),
                nameof(settings.DatabaseDbTypeId)
            );
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.AdditionalCase1 is null,
                nameof(settings),
                nameof(settings.AdditionalCase1)
            );
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.AdditionalCase2 is null,
                nameof(settings),
                nameof(settings.AdditionalCase2)
            );
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.AdditionalCase3 is null,
                nameof(settings),
                nameof(settings.AdditionalCase3)
            );

            initValue = settings.InitValue;
            specialCases = new DatabaseValueCaseList();
            specialCases.AdjustLength(3);
            databaseReferKind = settings.DatabaseReferKind;
            databaseDbTypeId = settings.DatabaseDbTypeId;
            isUseAdditionalItems = settings.IsUseAdditionalItems;
            AdditionalCase1 = settings.AdditionalCase1;
            AdditionalCase2 = settings.AdditionalCase2;
            AdditionalCase3 = settings.AdditionalCase3;
        }

        #endregion

        #region Convenience

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public DatabaseFieldSpecialSettingDefinitionDatabaseReference() : this(
            new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings()
        )
        {
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Static Methods

        #region private

        /// <summary>
        ///     -3 ～ -1 を 0 ～ 2 に変換する。
        /// </summary>
        /// <remarks>
        ///     範囲外の値であっても一様に変換処理を行う。必要であれば変換後に値のチェックを行うこと。
        /// </remarks>
        /// <param name="outerIndex">変換元</param>
        /// <returns>変換後の値</returns>
        private static int OuterIndexToInnerIndex(int outerIndex)
        {
            return outerIndex * -1 - 1;
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <summary>
        ///     追加項目を取得する。
        /// </summary>
        /// <param name="caseNumber">[Range[-3, -1)] 選択肢番号</param>
        /// <returns>追加項目の選択肢文字列</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="caseNumber"/> が指定範囲外の場合。
        /// </exception>
        [Pure]
        [ImmutableMethod]
        public DatabaseValueCaseDescription GetAdditionalItem(int caseNumber)
        {
            ThrowHelper.ValidateArgumentValueRange(
                caseNumber < CaseNumberMin || CaseNumberMax < caseNumber,
                nameof(caseNumber),
                caseNumber,
                CaseNumberMin,
                CaseNumberMax
            );

            return caseNumber switch
            {
                -1 => AdditionalCase1,
                -2 => AdditionalCase2,
                _ => AdditionalCase3,
            };
        }

        /// <inheritdoc/>
        [Pure]
        [ImmutableMethod]
        public bool CanChangeFieldType(DatabaseFieldType type)
        {
            ThrowHelper.ValidateArgumentNotNull(type is null, nameof(type));

            return type == DatabaseFieldType.Int;
        }

        /// <summary>
        ///     追加選択肢文字列を更新する。
        /// </summary>
        /// <param name="caseNumber">[Range[-3, -1)] 選択肢番号</param>
        /// <param name="description">文字列</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="caseNumber"/>が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="description"/> が <see langword="null"/>の場合。
        /// </exception>
        public void UpdateAdditionalItem(
            DatabaseValueCaseNumber caseNumber,
            DatabaseValueCaseDescription description
        )
        {
            ThrowHelper.ValidateArgumentNotNull(caseNumber is null, nameof(caseNumber));
            ThrowHelper.ValidateArgumentNotNull(description is null, nameof(description));

            ThrowHelper.ValidateArgumentValueRange(
                caseNumber.RawValue < CaseNumberMin || CaseNumberMax < caseNumber.RawValue,
                nameof(caseNumber),
                caseNumber,
                CaseNumberMin,
                CaseNumberMax
            );

            switch (caseNumber.RawValue)
            {
                case -1:
                    AdditionalCase1 = description;
                    break;
                case -2:
                    AdditionalCase2 = description;
                    break;
                default:
                    AdditionalCase3 = description;
                    break;
            }
        }

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return DatabaseDbTypeId == other.DatabaseDbTypeId
                   && IsUseAdditionalItems == other.IsUseAdditionalItems
                   && DatabaseReferKind == other.DatabaseReferKind
                   && AdditionalCase1 == other.AdditionalCase1
                   && AdditionalCase2 == other.AdditionalCase2
                   && AdditionalCase3 == other.AdditionalCase3
                   && InitValue == other.InitValue;
        }

        /// <inheritdoc/>
        [Pure]
        [ImmutableMethod]
        public bool ItemEquals(IReadOnlyDatabaseFieldSpecialSettingDefinition? other)
            => ItemEquals(other as IDatabaseFieldSpecialSettingDefinitionSettings);

        /// <inheritdoc/>
        [Pure]
        [ImmutableMethod]
        public bool ItemEquals(IDatabaseFieldSpecialSettingDefinitionSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (other.TryCastDatabaseReferenceSettings(out var otherDatabaseReference))
            {
                return ItemEquals(otherDatabaseReference);
            }

            return false;
        }

        /// <inheritdoc/>
        [Pure]
        public bool TryCastNormal(
            [NotNullWhen(true)] out DatabaseFieldSpecialSettingDefinitionNormal? result
        )
        {
            result = null;
            return false;
        }

        /// <inheritdoc/>
        [Pure]
        public bool TryCastLoadFile(
            [NotNullWhen(true)] out DatabaseFieldSpecialSettingDefinitionLoadFile? result
        )
        {
            result = null;
            return false;
        }

        /// <inheritdoc/>
        [Pure]
        public bool TryCastDatabaseReference(
            [NotNullWhen(true)] out DatabaseFieldSpecialSettingDefinitionDatabaseReference? result
        )
        {
            result = this;
            return true;
        }

        /// <inheritdoc/>
        [Pure]
        public bool TryCastManual(
            [NotNullWhen(true)] out DatabaseFieldSpecialSettingDefinitionManual? result
        )
        {
            result = null;
            return false;
        }

        /// <inheritdoc/>
        [Pure]
        public bool TryCastNormalSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionNormalSettings? result
        )
        {
            result = null;
            return false;
        }

        /// <inheritdoc/>
        [Pure]
        public bool TryCastLoadFileSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionLoadFileSettings? result
        )
        {
            result = null;
            return false;
        }

        /// <inheritdoc/>
        [Pure]
        public bool TryCastDatabaseReferenceSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings? result
        )
        {
            result = this;
            return true;
        }

        /// <inheritdoc/>
        [Pure]
        public bool TryCastManualSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionManualSettings? result
        )
        {
            result = null;
            return false;
        }

        #endregion

        #region Interface Implements

        #region IReadOnlyDatabaseFieldSpecialSettingDefinition

        [Pure]
        IEnumerable<DatabaseValueCase> IReadOnlyDatabaseFieldSpecialSettingDefinition.GetSpecialCases()
        {
            return specialCases;
        }

        [Pure]
        bool IReadOnlyDatabaseFieldSpecialSettingDefinition.TryCastNormal(
            [NotNullWhen(true)] out ReadOnlyDatabaseFieldSpecialSettingDefinitionNormal? result
        )
        {
            result = null;
            return false;
        }

        [Pure]
        bool IReadOnlyDatabaseFieldSpecialSettingDefinition.
            TryCastLoadFile(
                [NotNullWhen(true)] out ReadOnlyDatabaseFieldSpecialSettingDefinitionLoadFile? result
            )
        {
            result = null;
            return false;
        }

        [Pure]
        bool IReadOnlyDatabaseFieldSpecialSettingDefinition.
            TryCastDatabaseReference(
                [NotNullWhen(true)] out ReadOnlyDatabaseFieldSpecialSettingDefinitionDatabaseReference? result
            )
        {
            result = this;
            return true;
        }

        [Pure]
        bool IReadOnlyDatabaseFieldSpecialSettingDefinition.TryCastManual(
            [NotNullWhen(true)] out ReadOnlyDatabaseFieldSpecialSettingDefinitionManual? result
        )
        {
            result = null;
            return false;
        }

        #endregion

        #endregion

        #endregion
    }

    public partial class ReadOnlyDatabaseFieldSpecialSettingDefinitionDatabaseReference :
        IReadOnlyDatabaseFieldSpecialSettingDefinition
    {
        /// <inheritdoc/>
        [Pure]
        public bool TryCastNormalSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionNormalSettings? result
        ) => MutableInstance.TryCastNormalSettings(out result);

        /// <inheritdoc/>
        [Pure]
        public bool TryCastLoadFileSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionLoadFileSettings? result
        ) => MutableInstance.TryCastLoadFileSettings(out result);

        /// <inheritdoc/>
        [Pure]
        public bool TryCastDatabaseReferenceSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings? result
        ) => MutableInstance.TryCastDatabaseReferenceSettings(out result);

        /// <inheritdoc/>
        [Pure]
        public bool TryCastManualSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionManualSettings? result
        ) => MutableInstance.TryCastManualSettings(out result);

        /// <inheritdoc/>
        [Pure]
        public bool TryCastNormal(
            [NotNullWhen(true)] out ReadOnlyDatabaseFieldSpecialSettingDefinitionNormal? result
        ) => ((IReadOnlyDatabaseFieldSpecialSettingDefinition)MutableInstance).TryCastNormal(out result);

        /// <inheritdoc/>
        [Pure]
        public bool TryCastLoadFile(
            [NotNullWhen(true)] out ReadOnlyDatabaseFieldSpecialSettingDefinitionLoadFile? result
        ) => ((IReadOnlyDatabaseFieldSpecialSettingDefinition)MutableInstance).TryCastLoadFile(out result);

        /// <inheritdoc/>
        [Pure]
        public bool TryCastDatabaseReference(
            [NotNullWhen(true)] out ReadOnlyDatabaseFieldSpecialSettingDefinitionDatabaseReference? result
        ) => ((IReadOnlyDatabaseFieldSpecialSettingDefinition)MutableInstance).TryCastDatabaseReference(out result);

        /// <inheritdoc/>
        [Pure]
        public bool TryCastManual(
            [NotNullWhen(true)] out ReadOnlyDatabaseFieldSpecialSettingDefinitionManual? result
        ) => ((IReadOnlyDatabaseFieldSpecialSettingDefinition)MutableInstance).TryCastManual(out result);

        #region IReadOnlyDatabaseFieldSpecialSettingDefinition

        [Pure]
        IEnumerable<DatabaseValueCase> IReadOnlyDatabaseFieldSpecialSettingDefinition.GetSpecialCases()
            => ((IReadOnlyDatabaseFieldSpecialSettingDefinition)MutableInstance).GetSpecialCases();

        #endregion
    }
}
