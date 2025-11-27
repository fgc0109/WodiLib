// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldSpecialSettingDefinitionNormal.cs
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
    public partial interface IDatabaseFieldSpecialSettingDefinitionNormalSettings
        : IDatabaseFieldSpecialSettingDefinitionSettings
    {
    }

    public partial record DatabaseFieldSpecialSettingDefinitionNormalSettings
    {
        /// <inheritdoc/>
        public bool ItemEquals(IDatabaseFieldSpecialSettingDefinitionSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (other.TryCastNormalSettings(out var otherNormal))
            {
                return ItemEquals(otherNormal);
            }

            return false;
        }

        /// <inheritdoc/>
        [Pure]
        public bool TryCastNormalSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionNormalSettings? result
        )
        {
            result = this;
            return true;
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
            result = null;
            return false;
        }
    }

    [Model(
        Description = "データ内容の特殊設定＝「特殊な指定方法を使用しない」の場合の特殊設定内容"
    )]
    public partial class DatabaseFieldSpecialSettingDefinitionNormal :
        IDatabaseFieldSpecialSettingDefinition
    {
        #region Properties

        /// <inheritdoc cref="IReadOnlyDatabaseFieldSpecialSettingDefinition.SettingType"/>
        [ImmutableProperty]
        [Pure]
        public DatabaseFieldSpecialSettingType SettingType => DatabaseFieldSpecialSettingType.Normal;

        /// <inheritdoc cref="IReadOnlyDatabaseFieldSpecialSettingDefinition.DefaultType"/>
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

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Fields

        private DatabaseValueInt initValue;

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

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
        public DatabaseFieldSpecialSettingDefinitionNormal(
            IDatabaseFieldSpecialSettingDefinitionNormalSettings settings
        )
        {
            ThrowHelper.ValidateArgumentNotNull(settings is null, nameof(settings));
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.InitValue is null,
                nameof(settings),
                nameof(settings.InitValue)
            );

            initValue = settings.InitValue;
        }

        #endregion

        #region Convenience

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public DatabaseFieldSpecialSettingDefinitionNormal() : this(
            new DatabaseFieldSpecialSettingDefinitionNormalSettings()
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
        public bool CanChangeFieldType(DatabaseFieldType type)
        {
            ThrowHelper.ValidateArgumentNotNull(type is null, nameof(type));

            return type == DatabaseFieldType.Int
                   || type == DatabaseFieldType.String;
        }

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IDatabaseFieldSpecialSettingDefinitionNormalSettings? other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;

            return InitValue == other.InitValue;
        }

        /// <inheritdoc/>
        [Pure]
        [ImmutableMethod]
        public bool ItemEquals(IReadOnlyDatabaseFieldSpecialSettingDefinition? other)
            => ItemEquals(other as IDatabaseFieldSpecialSettingDefinitionNormalSettings);

        /// <inheritdoc/>
        [Pure]
        [ImmutableMethod]
        public bool ItemEquals(IDatabaseFieldSpecialSettingDefinitionSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (other.TryCastNormalSettings(out var otherNormal))
            {
                return ItemEquals(otherNormal);
            }

            return false;
        }

        /// <inheritdoc/>
        [Pure]
        public bool TryCastNormal(
            [NotNullWhen(true)] out DatabaseFieldSpecialSettingDefinitionNormal? result
        )
        {
            result = this;
            return true;
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
            result = null;
            return false;
        }

        /// <inheritdoc/>
        [Pure]
        public bool TryCastNormalSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionNormalSettings? result
        )
        {
            result = this;
            return true;
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
            result = null;
            return false;
        }

        #endregion

        #region Interface Implements

        #region IReadOnlyDatabaseFieldSpecialSettingDefinition

        [Pure]
        IEnumerable<DatabaseValueCase> IReadOnlyDatabaseFieldSpecialSettingDefinition.GetSpecialCases()
        {
            return Array.Empty<DatabaseValueCase>();
        }

        [Pure]
        bool IReadOnlyDatabaseFieldSpecialSettingDefinition.TryCastNormal(
            [NotNullWhen(true)] out ReadOnlyDatabaseFieldSpecialSettingDefinitionNormal? result
        )
        {
            result = this;
            return true;
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
            result = null;
            return false;
        }

        #endregion

        #endregion

        #endregion
    }

    public partial class ReadOnlyDatabaseFieldSpecialSettingDefinitionNormal :
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
