// ========================================
// Project Name : WodiLib
// File Name    : DatabaseSchema.cs
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
    /// <summary>
    ///     データベース1タイプ分の設定と実データを持つクラス設定DTO
    /// </summary>
    public partial record DatabaseTypeTableSettings
    {
        /// <summary>
        ///     最小要素を持つコンストラクタ
        /// </summary>
        public DatabaseTypeTableSettings() : this(
            DatabaseTypeTable.MinDataCapacity
                .Iterate<IDatabaseNamedDataRowSettings>(_ => new DatabaseNamedDataRowSettings())
                .ToArray()
        )
        {
        }
    }

    [RestrictedCapacity2DListImplementTemplate(
        Description = "データベース1タイプ分の設定と実データを持つクラス",
        RowElementType = typeof(DatabaseNamedDataRow),
        FixedRowElementType = typeof(FixedDatabaseNamedDataRow),
        ReadOnlyRowElementType = typeof(ReadOnlyDatabaseNamedDataRow),
        RowSettingsType = typeof(IDatabaseNamedDataRowSettings),
        CellElementType = typeof(DatabaseFieldValue),
        UseConstructorExpansion = true,
        RowPhysicalName = "Data",
        RowLogicalName = "データ",
        ColumnPhysicalName = "Field",
        ColumnLogicalName = "項目",
        CellPhysicalName = "Item",
        CellLogicalName = "項目値",
        MaxRowCapacity = "DatabaseConst.MaxDataLength",
        MinRowCapacity = "DatabaseConst.MinDataLength",
        MaxColumnCapacity = "DatabaseConst.MaxFieldLength",
        MinColumnCapacity = "DatabaseConst.MinFieldLength"
    )]
    public partial class DatabaseTypeTable
    {
        #region Properties

        #region public

        /// <summary>
        ///     DBタイプ名
        /// </summary>
        [ImmutableProperty]
        [FixedLengthListProperty]
        [SettingsProperty(DefaultValue = "TypeName.Default")]
        public TypeName TypeName
        {
            [Pure] get => typeDefinition.TypeName;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(TypeName));

                typeDefinition.TypeName = value;
            }
        }

        /// <summary>
        ///     メモ
        /// </summary>
        [ImmutableProperty]
        [FixedLengthListProperty]
        [SettingsProperty(DefaultValue = "DatabaseMemo.Default")]
        public DatabaseMemo Memo
        {
            [Pure] get => typeDefinition.Memo;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(Memo));

                typeDefinition.Memo = value;
            }
        }

        /// <summary>データ名の設定方法</summary>
        [ImmutableProperty]
        [FixedLengthListProperty]
        [SettingsProperty(DefaultValue = "DatabaseDataNamingDefinition.Default")]
        public DatabaseDataNamingDefinition DataNamingDefinition
        {
            [Pure] get => dataNamingDefinition;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(DataNamingDefinition));

                SetField(ref dataNamingDefinition, value);
            }
        }

        /// <summary>
        ///     項目定義一覧
        /// </summary>
        [InstanceNotChange]
        [ImmutableProperty(
            ReturnType = typeof(ReadOnlyDatabaseFieldDefinitionList)
        )]
        [FixedLengthListProperty(SetterAccessibility = "NONE")]
        [SettingsProperty(
            ReturnType = typeof(IDatabaseFieldDefinitionListSettings),
            DefaultValue = "new DatabaseFieldDefinitionListSettings()"
        )]
        [Pure]
        public FixedDatabaseFieldDefinitionList FieldDefinitionList
            => typeDefinition.FieldDefinitionList;

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Fields

        /// <summary>DoConstructorExpansion 以外での更新禁止</summary>
        private DatabaseTypeDefinition typeDefinition = null!;

        private DatabaseDataNamingDefinition dataNamingDefinition;

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Constructors

        #region Required

        protected virtual partial void DoConstructorExpansion(IDatabaseTypeTableSettings settings)
        {
            typeDefinition = new DatabaseTypeDefinition(
                new DatabaseTypeDefinitionSettings
                {
                    FieldDefinitionList = new DatabaseFieldDefinitionList(settings.FieldDefinitionList),
                    Memo = settings.Memo,
                    TypeName = settings.TypeName,
                }
            );
            dataNamingDefinition = settings.DataNamingDefinition;
            PropagatePropertyChangeEvent(
                typeDefinition,
                new[]
                {
                    nameof(DatabaseTypeDefinition.TypeName),
                    nameof(DatabaseTypeDefinition.Memo),
                    nameof(DatabaseTypeDefinition.FieldCount),
                }
            );
        }

        #endregion

        #region Convenience

        #region FromSettings

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
        public DatabaseTypeTable(IDatabaseTypeTableSettings settings) : this(
            ValidateInitSettings(settings),
            BuildSimpleList(settings.Settings)
        )
        {
        }

        private static IDatabaseTypeTableSettings ValidateInitSettings(IDatabaseTypeTableSettings settings)
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
            for (var i = 0; i < settings.Settings.Count; i++)
            {
                var rowSettings = settings.Settings[i];
                ThrowHelper.ValidateArgumentPropertyNotNull(
                    rowSettings is null,
                    nameof(settings),
                    $"{nameof(settings.Settings)}[{i}]"
                );
                ThrowHelper.ValidateArgumentPropertyItemsHasNotNull(
                    settings.Settings.HasNullItem(),
                    nameof(settings),
                    $"{nameof(settings.Settings)}[{i}]"
                );
            }

            return settings;
        }

        private static SimpleList<DatabaseNamedDataRow> BuildSimpleList(
            IEnumerable<IDatabaseNamedDataRowSettings> settings
        )
        {
            return new SimpleList<DatabaseNamedDataRow>(
                valueBuilder: new SimpleListValueBuilder<DatabaseNamedDataRow>((list, index)
                    => new DatabaseNamedDataRow(BuildRowSettingsFromRowIndex(index, list.Count, list))
                ),
                initValues: settings.Select(setting => new DatabaseNamedDataRow(setting))
            );
        }

        /*
         * SourceGenerator が作成する RequiredConstructor で使用されるメソッド。
         * クラス内に保持する Config のコンストラクタ引数として指定する。
         */
        private static IDatabaseNamedDataRowSettings BuildRowSettingsFromRowIndex(
            int rowIndex,
            int columnLength,
            SimpleList<DatabaseNamedDataRow> list
        )
        {
            var settings = new DatabaseNamedDataRowSettings();
            for (var columnIndex = 0; columnIndex < columnLength; columnIndex++)
            {
                settings.Settings.Add(list[0][columnIndex].GetDefaultValue());
            }

            return settings;
        }

        private static DatabaseNamedDataRow BuildRowFromSettings(
            int rowIndex,
            IDatabaseNamedDataRowSettings settings
        )
            => new(settings);

        private static DatabaseFieldValue BuildListElementFromSetting(DatabaseFieldValue settings)
            => settings;

        private IWodiLib2DListValidator<IDatabaseTypeTableSettings, IDatabaseNamedDataRowSettings,
                DatabaseFieldValue>
            BuildValidator(IDatabaseTypeTableSettings settings, SimpleList<DatabaseNamedDataRow> itemsImpl)
        {
            var getSelf = new Func<DatabaseTypeTable>(() => this);
            return new DatabaseTypeTableValidator(
                rowCountGetter: () => getSelf.Invoke().DataCount,
                columnCountGetter: () => getSelf.Invoke().FieldCount,
                fieldTypesGetter: () => getSelf.Invoke().GetDataInternal(0).Select(row => row.Type).ToArray(),
                fieldTypeChangeValidator: (fieldId, type)
                    => getSelf.Invoke().FieldDefinitionList.Get(fieldId).CanChangeFieldType(type)
            );
        }

        #endregion

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public DatabaseTypeTable() : this(new DatabaseTypeTableSettings())
        {
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        #region CRUD

        /// <summary>
        ///     項目の値種別を取得する。
        /// </summary>
        /// <param name="fieldId">[Range(0, <see cref="FieldCount"/> - 1)] 項目ID</param>
        /// <returns>指定した項目IDの値種別</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="fieldId"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="fieldId"/> が指定範囲外の場合。
        /// </exception>
        [FixedLengthListMethod]
        [ImmutableMethod]
        [Pure]
        public DatabaseFieldType GetFieldType(FieldId fieldId)
        {
            return typeDefinition.FieldDefinitionList[fieldId]
                .FieldType;
        }

        /// <summary>
        ///     指定範囲項目の値種別を取得する。
        /// </summary>
        /// <param name="fieldId">[Range(0, <see cref="FieldCount"/> - 1)] 項目ID</param>
        /// <param name="count">[Range(0, <see cref="FieldCount"/>)] 項目数</param>
        /// <returns>指定範囲項目の値種別</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="fieldId"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="fieldId"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合。</exception>
        [FixedLengthListMethod]
        [ImmutableMethod]
        [Pure]
        public IEnumerable<DatabaseFieldType> GetFieldTypeRange(FieldId fieldId, int count)
        {
            return typeDefinition.FieldDefinitionList
                .GetRange(fieldId, count)
                .Select(definition => definition.FieldType);
        }

        /// <summary>全項目の値種別を取得する。</summary>
        /// <returns>値種別一覧</returns>
        [FixedLengthListMethod]
        [ImmutableMethod]
        [Pure]
        public IEnumerable<DatabaseFieldType> GetFieldTypes()
        {
            return typeDefinition.FieldDefinitionList.Select(definition => definition.FieldType);
        }

        /// <summary>
        ///     指定した項目の値種別を変更する。
        /// </summary>
        /// <param name="fieldIndex">[Range(0, <see cref="FieldCount"/> - 1)] 項目インデックス</param>
        /// <param name="newType">変更したい値種別</param>
        /// <returns>種別変更後にセットし直した項目値</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="newType"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="fieldIndex"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     項目設定が指定された値種別を受け付けられない設定の場合。
        /// </exception>
        [FixedLengthListMethod]
        public IEnumerable<DatabaseFieldValue> ChangeFieldType(int fieldIndex, DatabaseFieldType newType)
        {
            ValidateChangeFieldType(fieldIndex, newType);
            return ChangeFieldTypeInternal(fieldIndex, newType);
        }

        /// <summary>
        ///     指定した項目の値種別を変更し、指定された値で初期化する。
        /// </summary>
        /// <param name="fieldIndex">[Range(0, <see cref="FieldCount"/> - 1)] 項目インデックス</param>
        /// <param name="fieldSettings">変更したい値種別と値一覧</param>
        /// <returns>種別変更後にセットし直した項目値</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="fieldSettings"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="fieldIndex"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="fieldSettings"/> に不適切な <see langword="null"/> 要素が含まれる場合。
        ///     項目設定が指定された値種別を受け付けられない設定の場合。
        /// </exception>
        [FixedLengthListMethod]
        public IEnumerable<DatabaseFieldValue> ChangeFieldType(
            int fieldIndex,
            IDatabaseFieldValueListSettings fieldSettings
        )
        {
            ValidateChangeFieldType(fieldIndex, fieldSettings);
            return ChangeFieldTypeInternal(fieldIndex, fieldSettings);
        }

        #endregion

        #region Validation

        /// <summary>
        ///     <see cref="ChangeFieldType(int, DatabaseFieldType)"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="ChangeFieldType(int, DatabaseFieldType)" path="param|exception"/>
        [FixedLengthListMethod]
        public void ValidateChangeFieldType(int fieldIndex, DatabaseFieldType newType)
        {
            ListValidationHelper.SelectIndex((nameof(fieldIndex), fieldIndex), (nameof(FieldCount), FieldCount));
            var fieldDefinition = FieldDefinitionList[fieldIndex].SpecialSettingDefinition;

            DatabaseFieldSpecialSettingDefinitionValidationHelper.ValidateDefinitionAndTypeAsArgs(
                new NamedValue<IReadOnlyDatabaseFieldSpecialSettingDefinition>($"項目{fieldIndex}", fieldDefinition),
                (nameof(newType), newType)
            );
        }

        /// <summary>
        ///     <see cref="ChangeFieldType(int, IDatabaseFieldValueListSettings)"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="ChangeFieldType(int, DatabaseFieldType)" path="param|exception"/>
        [FixedLengthListMethod]
        public void ValidateChangeFieldType(int fieldIndex, IDatabaseFieldValueListSettings fieldSettings)
        {
            ListValidationHelper.SelectIndex((nameof(fieldIndex), fieldIndex), (nameof(FieldCount), FieldCount));
            var fieldDefinition = FieldDefinitionList[fieldIndex].SpecialSettingDefinition;

            DatabaseFieldSpecialSettingDefinitionValidationHelper.ValidateDefinitionAndTypeAsArgs(
                new NamedValue<IReadOnlyDatabaseFieldSpecialSettingDefinition>($"項目{fieldIndex}", fieldDefinition),
                ($"{nameof(fieldSettings)}.{nameof(fieldSettings.FieldType)}", fieldSettings.FieldType)
            );

            ListValidationHelper.ItemCount(
                fieldSettings.Settings.Count,
                FieldCount,
                $"{nameof(fieldSettings)}.{nameof(fieldSettings.Settings)}"
            );
        }

        #endregion

        #region CRUD Core

        /// <summary>
        ///     <see cref="ChangeFieldType(int, DatabaseFieldType)"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="ChangeFieldType(int, DatabaseFieldType)" path="param"/>
        [FixedLengthListMethod]
        public IEnumerable<DatabaseFieldValue> ChangeFieldTypeInternal(
            int fieldIndex,
            DatabaseFieldType newType
        )
        {
            var values = FieldCount.Iterate(_ => new DatabaseFieldValue(newType));
            return Table.SetColumnInternal(fieldIndex, values);
        }

        /// <summary>
        ///     <see cref="ChangeFieldType(int, IDatabaseFieldValueListSettings)"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="ChangeFieldType(int, DatabaseFieldType)" path="param"/>
        [FixedLengthListMethod]
        public IEnumerable<DatabaseFieldValue> ChangeFieldTypeInternal(
            int fieldIndex,
            IDatabaseFieldValueListSettings fieldSettings
        ) => Table.SetColumnInternal(fieldIndex, fieldSettings.Settings);

        #endregion

        #region ItemEquals

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IDatabaseTypeTableSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return TypeName == other.TypeName
                   && Memo == other.Memo
                   && DataNamingDefinition == other.DataNamingDefinition
                   && Settings.SequenceEqual(
                       other.Settings,
                       EqualityComparerFactory.Create<IDatabaseNamedDataRowSettings>()
                   )
                   && FieldDefinitionList.ItemEquals(other.FieldDefinitionList);
        }

        #endregion

        #endregion

        #endregion
    }
}
