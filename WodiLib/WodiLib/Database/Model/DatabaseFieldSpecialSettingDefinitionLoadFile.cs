// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldSpecialSettingDefinitionLoadFile.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using WodiLib.SourceGenerator.Domain.Model.Attributes;
using WodiLib.Sys;

namespace WodiLib.Database
{
    public partial interface IDatabaseFieldSpecialSettingDefinitionLoadFileSettings :
        IDatabaseFieldSpecialSettingDefinitionSettings
    {
    }

    public partial record DatabaseFieldSpecialSettingDefinitionLoadFileSettings
    {
        /// <inheritdoc/>
        public bool ItemEquals(IDatabaseFieldSpecialSettingDefinitionSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (other.TryCastLoadFileSettings(out var otherLoadFile))
            {
                return ItemEquals(otherLoadFile);
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
            result = this;
            return true;
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
        Description = "データ内容の特殊設定＝「ファイル読み込み」の場合の特殊設定内容"
    )]
    public partial class DatabaseFieldSpecialSettingDefinitionLoadFile : IDatabaseFieldSpecialSettingDefinition
    {
        #region Properties

        #region public

        /// <inheritdoc/>
        [ImmutableProperty]
        [Pure]
        public DatabaseFieldSpecialSettingType SettingType => DatabaseFieldSpecialSettingType.LoadFile;

        /// <inheritdoc/>
        [ImmutableProperty]
        [Pure]
        public DatabaseFieldType DefaultType => DatabaseFieldType.String;

        /// <inheritdoc cref="IDatabaseFieldSpecialSettingDefinition.InitValue"/>
        [SettingsProperty(DefaultValue = "DatabaseValueInt.Default")]
        [ImmutableProperty]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public DatabaseValueInt InitValue
        {
            [Pure] get => initValue;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(InitValue));

                SetField(ref initValue, value);
            }
        }

        /// <summary>初期フォルダ</summary>
        [SettingsProperty(DefaultValue = "DBSettingFolderName.Default")]
        [ImmutableProperty]
        public DBSettingFolderName FolderName
        {
            [Pure] get => folderName;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(FolderName));
                SetField(ref folderName, value);
            }
        }

        /// <summary>保存時にフォルダ名省略フラグ</summary>
        [SettingsProperty(DefaultValue = "false")]
        [ImmutableProperty]
        public bool IsOmitFolderName
        {
            [Pure] get => omissionFolderNameFlag;
            set
            {
                SetField(ref omissionFolderNameFlag, value);
                specialCases.Reset(
                    new[]
                    {
                        new DatabaseValueCase(
                            IsOmitFolderName
                                ? 1
                                : 0,
                            new DatabaseValueCaseDescription(folderName)
                        ),
                    }
                );
            }
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Fields

        private DatabaseValueInt initValue;
        private DBSettingFolderName folderName;
        private bool omissionFolderNameFlag;
        private readonly DatabaseValueCaseList specialCases;

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
        public DatabaseFieldSpecialSettingDefinitionLoadFile(
            IDatabaseFieldSpecialSettingDefinitionLoadFileSettings settings
        )
        {
            ThrowHelper.ValidateArgumentNotNull(settings is null, nameof(settings));
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.InitValue is null,
                nameof(settings),
                nameof(settings.InitValue)
            );
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.FolderName is null,
                nameof(settings),
                nameof(settings.FolderName)
            );

            initValue = settings.InitValue;
            specialCases = new DatabaseValueCaseList();
            folderName = settings.FolderName;
            // specialCases を編集するために、omissionFolderNameFlag ではなく IsOmitFolderName を更新する
            IsOmitFolderName = settings.IsOmitFolderName;
        }

        #endregion

        #region Convenience

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public DatabaseFieldSpecialSettingDefinitionLoadFile() : this(
            new DatabaseFieldSpecialSettingDefinitionLoadFileSettings()
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

            return type == DatabaseFieldType.String;
        }

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IDatabaseFieldSpecialSettingDefinitionLoadFileSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return FolderName == other.FolderName
                   && IsOmitFolderName == other.IsOmitFolderName
                   && InitValue == other.InitValue;
        }

        /// <inheritdoc/>
        [Pure]
        [ImmutableMethod]
        public bool ItemEquals(IReadOnlyDatabaseFieldSpecialSettingDefinition? other)
            => ItemEquals(other as IDatabaseFieldSpecialSettingDefinitionLoadFileSettings);

        /// <inheritdoc/>
        [Pure]
        [ImmutableMethod]
        public bool ItemEquals(IDatabaseFieldSpecialSettingDefinitionSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (other.TryCastLoadFileSettings(out var otherLoadFile))
            {
                return ItemEquals(otherLoadFile);
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
            result = this;
            return true;
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
            result = null;
            return false;
        }

        /// <inheritdoc/>
        [Pure]
        public bool TryCastLoadFileSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionLoadFileSettings? result
        )
        {
            result = this;
            return true;
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
            result = this;
            return true;
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

    public partial class ReadOnlyDatabaseFieldSpecialSettingDefinitionLoadFile :
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
