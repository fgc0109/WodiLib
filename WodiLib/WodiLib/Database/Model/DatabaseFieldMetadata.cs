// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldMetadata.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using WodiLib.SourceGenerator.Domain.Model.Attributes;
using WodiLib.Sys;

namespace WodiLib.Database
{
    [Model(Description = "DB項目メタ情報")]
    public partial class DatabaseFieldMetadata
    {
        #region Properties

        #region public

        /// <summary>
        ///     項目名
        /// </summary>
        [ImmutableProperty]
        [SettingsProperty(DefaultValue = "FieldName.Default")]
        public FieldName FieldName
        {
            [Pure] get => fieldName;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(FieldName));

                SetField(ref fieldName, value);
            }
        }

        /// <summary>
        ///     DB項目特殊指定
        /// </summary>
        [ImmutableProperty(
            ReturnType = typeof(ReadOnlyDatabaseFieldSpecialSettingDefinition)
        )]
        [SettingsProperty(
            ReturnType = typeof(IDatabaseFieldSpecialSettingDefinitionSettings),
            DefaultValue =
                "new DatabaseFieldSpecialSettingDefinitionSettings(new DatabaseFieldSpecialSettingDefinitionNormalSettings())"
        )]
        public DatabaseFieldSpecialSettingDefinition SpecialSettingDefinition
        {
            [Pure] get => specialSettingDefinition;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(SpecialSettingDefinition));

                SetField(ref specialSettingDefinition, value);
            }
        }

        /// <summary>
        ///     項目メモ
        /// </summary>
        [ImmutableProperty]
        [SettingsProperty(DefaultValue = "\"\"")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public FieldMemo FieldMemo
        {
            [Pure] get => fieldMemo;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(FieldMemo));

                SetField(ref fieldMemo, value);
            }
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Fields

        private FieldName fieldName;
        private DatabaseFieldSpecialSettingDefinition specialSettingDefinition;
        private FieldMemo fieldMemo;

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
        public DatabaseFieldMetadata(
            IDatabaseFieldMetadataSettings settings
        )
        {
            ThrowHelper.ValidateArgumentNotNull(settings is null, nameof(settings));
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.FieldName is null,
                nameof(settings),
                nameof(settings.FieldName)
            );
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.SpecialSettingDefinition is null,
                nameof(settings),
                nameof(settings.SpecialSettingDefinition)
            );
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.FieldMemo is null,
                nameof(settings),
                nameof(settings.FieldMemo)
            );

            fieldName = settings.FieldName;
            specialSettingDefinition =
                DatabaseFieldSpecialSettingDefinitionFactory.Create(settings.SpecialSettingDefinition);
            fieldMemo = settings.FieldMemo;
        }

        #endregion

        #region Convenience

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public DatabaseFieldMetadata() : this(new DatabaseFieldMetadataSettings())
        {
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IDatabaseFieldMetadataSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!(FieldName == other.FieldName
                  && FieldMemo == other.FieldMemo)
               )
            {
                return false;
            }

            return SpecialSettingDefinition.ItemEquals(other.SpecialSettingDefinition);
        }

        #endregion

        #endregion
    }
}
