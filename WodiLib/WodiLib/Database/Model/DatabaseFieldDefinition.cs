// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldDefinition.cs
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
    [Model(Description = "DB項目設定")]
    public partial class DatabaseFieldDefinition
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
        ///     項目値種別
        /// </summary>
        [ImmutableProperty]
        [SettingsProperty(DefaultValue = "DatabaseFieldType.Int")]
        public DatabaseFieldType FieldType
        {
            [Pure] get => fieldType;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(FieldType));

                SetField(ref fieldType, value);
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
        private DatabaseFieldType fieldType;
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
        public DatabaseFieldDefinition(
            IDatabaseFieldDefinitionSettings settings
        )
        {
            ThrowHelper.ValidateArgumentNotNull(settings is null, nameof(settings));
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.FieldName is null,
                nameof(settings),
                nameof(settings.FieldName)
            );
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.FieldType is null,
                nameof(settings),
                nameof(settings.FieldType)
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


            DatabaseFieldSpecialSettingDefinitionValidationHelper.ValidateDefinitionAndTypeAsArgs(
                (nameof(specialSettingDefinition), settings.SpecialSettingDefinition),
                (nameof(fieldType), settings.FieldType)
            );

            fieldName = settings.FieldName;
            fieldType = settings.FieldType;
            specialSettingDefinition =
                DatabaseFieldSpecialSettingDefinitionFactory.Create(settings.SpecialSettingDefinition);
            fieldMemo = settings.FieldMemo;
        }

        #endregion

        #region Convenience

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public DatabaseFieldDefinition() : this(new DatabaseFieldDefinitionSettings())
        {
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <summary>
        ///     項目の値種別を指定した種別に変更可能かどうかを返す。
        /// </summary>
        /// <param name="fieldType">変更したい項目値種別</param>
        /// <returns>変更可能な場合 <see langword="true"/></returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="fieldType"/> が <see langword="null"/> の場合。
        /// </exception>
        [Pure]
        [ImmutableMethod]
        public bool CanChangeFieldType(DatabaseFieldType fieldType)
        {
            ThrowHelper.ValidateArgumentNotNull(fieldType is null, nameof(fieldType));

            return SpecialSettingDefinition.CanChangeFieldType(fieldType);
        }

        /// <summary>
        ///     項目の値種別を変更する。
        /// </summary>
        /// <remarks>
        ///     値種別が変化した場合、すべての要素が初期化される。
        /// </remarks>
        /// <param name="fieldType">変更したい項目値種別</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="fieldType"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     指定した値種別に変更不可能な場合。
        ///     （= <see cref="CanChangeFieldType"/> が <see langword="false"/> を返すような引数を指定した場合）
        /// </exception>
        public void ChangeFieldType(DatabaseFieldType fieldType)
        {
            ThrowHelper.ValidateArgumentNotNull(fieldType is null, nameof(fieldType));

            DatabaseFieldSpecialSettingDefinitionValidationHelper.ValidateDefinitionAndTypeAsArgs(
                new NamedValue<IReadOnlyDatabaseFieldSpecialSettingDefinition>(
                    nameof(SpecialSettingDefinition),
                    specialSettingDefinition
                ),
                (nameof(fieldType), fieldType)
            );

            FieldType = fieldType;
        }

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IDatabaseFieldDefinitionSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!(FieldName == other.FieldName
                  && FieldType == other.FieldType
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
