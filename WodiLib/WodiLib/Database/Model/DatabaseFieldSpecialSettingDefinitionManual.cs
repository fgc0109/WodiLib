// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldSpecialSettingDefinitionManual.cs
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
    public partial interface IDatabaseFieldSpecialSettingDefinitionManualSettings :
        IDatabaseFieldSpecialSettingDefinitionSettings
    {
    }

    public partial record DatabaseFieldSpecialSettingDefinitionManualSettings
    {
        /// <inheritdoc/>
        public bool ItemEquals(IDatabaseFieldSpecialSettingDefinitionSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (other.TryCastManualSettings(out var otherManual))
            {
                return ItemEquals(otherManual);
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
            result = null;
            return false;
        }

        /// <inheritdoc/>
        [Pure]
        public bool TryCastManualSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionManualSettings? result
        )
        {
            result = this;
            return true;
        }
    }

    [Model(
        Description = "データ内容の特殊設定＝「選択肢手動生成」の場合の特殊設定内容"
    )]
    public partial class DatabaseFieldSpecialSettingDefinitionManual :
        IDatabaseFieldSpecialSettingDefinition
    {
        /*
         * 選択肢リストの機能は DatabaseValueCaseList に委譲する。
         * バリデーション処理も委譲先で行う。
         */

        #region Properties

        #region public

        /// <inheritdoc/>
        [ImmutableProperty]
        [Pure]
        public DatabaseFieldSpecialSettingType SettingType => DatabaseFieldSpecialSettingType.Manual;

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

        /// <summary>選択肢リスト</summary>
        [SettingsProperty(
            ReturnType = typeof(IDatabaseValueCaseListSettings),
            DefaultValue = "new DatabaseValueCaseListSettings()"
        )]
        [ImmutableProperty(ReturnType = typeof(ReadOnlyDatabaseValueCaseList))]
        [InstanceNotChange]
        [Pure]
        public DatabaseValueCaseList SpecialCases { get; }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Fields

        private DatabaseValueInt initValue;

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
        public DatabaseFieldSpecialSettingDefinitionManual(
            IDatabaseFieldSpecialSettingDefinitionManualSettings settings
        )
        {
            ThrowHelper.ValidateArgumentNotNull(settings is null, nameof(settings));
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.InitValue is null,
                nameof(settings),
                nameof(settings.InitValue)
            );
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.SpecialCases is null,
                nameof(settings),
                nameof(settings.SpecialCases)
            );
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.SpecialCases.Settings is null,
                nameof(settings),
                $"{nameof(settings.SpecialCases)}.{nameof(settings.SpecialCases.Settings)}"
            );
            ThrowHelper.ValidateArgumentPropertyItemsHasNotNull(
                settings.SpecialCases.Settings.HasNullItem(),
                nameof(settings),
                $"{nameof(settings.SpecialCases)}.{nameof(settings.SpecialCases.Settings)}"
            );

            initValue = settings.InitValue;
            SpecialCases = new DatabaseValueCaseList(settings.SpecialCases);
        }

        #endregion

        #region Convenience

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public DatabaseFieldSpecialSettingDefinitionManual() : this(
            new DatabaseFieldSpecialSettingDefinitionManualSettings()
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
        [ImmutableMethod]
        public IEnumerable<DatabaseValueCase> GetSpecialCases()
        {
            return SpecialCases;
        }

        /// <inheritdoc/>
        [Pure]
        [ImmutableMethod]
        public bool CanChangeFieldType(DatabaseFieldType type)
        {
            ThrowHelper.ValidateArgumentNotNull(type is null, nameof(type));

            return type == DatabaseFieldType.Int;
        }

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IDatabaseFieldSpecialSettingDefinitionManualSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return InitValue == other.InitValue
                   && SpecialCases.ItemEquals(other.SpecialCases);
        }

        /// <inheritdoc/>
        [Pure]
        [ImmutableMethod]
        public bool ItemEquals(IReadOnlyDatabaseFieldSpecialSettingDefinition? other)
            => ItemEquals(other as IDatabaseFieldSpecialSettingDefinitionManualSettings);

        /// <inheritdoc/>
        [Pure]
        [ImmutableMethod]
        public bool ItemEquals(IDatabaseFieldSpecialSettingDefinitionSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (other.TryCastManualSettings(out var otherManual))
            {
                return ItemEquals(otherManual);
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
            result = null;
            return false;
        }

        /// <inheritdoc/>
        [Pure]
        public bool TryCastManual(
            [NotNullWhen(true)] out DatabaseFieldSpecialSettingDefinitionManual? result
        )
        {
            result = this;
            return true;
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
            result = null;
            return false;
        }

        /// <inheritdoc/>
        [Pure]
        public bool TryCastManualSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionManualSettings? result
        )
        {
            result = this;
            return true;
        }

        #endregion

        #region Interface Implements

        #region IReadOnlyDatabaseFieldSpecialSettingDefinition

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
            result = null;
            return false;
        }

        [Pure]
        bool IReadOnlyDatabaseFieldSpecialSettingDefinition.TryCastManual(
            [NotNullWhen(true)] out ReadOnlyDatabaseFieldSpecialSettingDefinitionManual? result
        )
        {
            result = this;
            return true;
        }

        #endregion

        #endregion

        #endregion
    }

    public partial class ReadOnlyDatabaseFieldSpecialSettingDefinitionManual :
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
    }
}
