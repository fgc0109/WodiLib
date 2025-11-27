// ========================================
// Project Name : WodiLib
// File Name    : DatabaseTypeMetadataTable.cs
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
    public partial record DatabaseTypeMetadataTableSettings
    {
        /// <summary>
        ///     最小要素を持つコンストラクタ
        /// </summary>
        public DatabaseTypeMetadataTableSettings() : this(
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
    public partial class DatabaseTypeMetadataTable
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

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
            [Pure] get => typeMetadata.TypeName;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(TypeName));

                typeMetadata.TypeName = value;
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
            [Pure] get => typeMetadata.Memo;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(Memo));

                typeMetadata.Memo = value;
            }
        }

        /// <summary>データ名の設定方法</summary>
        [ImmutableProperty]
        [FixedLengthListProperty]
        [SettingsProperty(DefaultValue = "DatabaseDataNamingDefinition.Default")]
        public DatabaseDataNamingDefinition DataNamingDefinition
        {
            [Pure] get => typeMetadata.DataNamingDefinition;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(DataNamingDefinition));

                typeMetadata.DataNamingDefinition = value;
            }
        }

        /// <summary>
        ///     項目定義一覧
        /// </summary>
        [InstanceNotChange]
        [ImmutableProperty(
            ReturnType = typeof(ReadOnlyDatabaseFieldMetadataList)
        )]
        [FixedLengthListProperty(SetterAccessibility = "NONE")]
        [SettingsProperty(
            ReturnType = typeof(IDatabaseFieldMetadataListSettings),
            DefaultValue = "new DatabaseFieldMetadataListSettings()"
        )]
        [Pure]
        public FixedDatabaseFieldMetadataList FieldMetadataList
            => typeMetadata.FieldMetadataList;

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Fields

        /// <summary>DoConstructorExpansion 以外での更新禁止</summary>
        private DatabaseTypeMetadata typeMetadata = null!;

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Constructors

        #region Required

        protected virtual partial void DoConstructorExpansion(IDatabaseTypeMetadataTableSettings settings)
        {
            typeMetadata = new DatabaseTypeMetadata(
                new DatabaseTypeMetadataSettings
                {
                    DataNamingDefinition = settings.DataNamingDefinition,
                    FieldMetadataList = settings.FieldMetadataList,
                    Memo = settings.Memo,
                    TypeName = settings.TypeName,
                }
            );
            PropagatePropertyChangeEvent(
                typeMetadata,
                new[]
                {
                    nameof(DatabaseTypeMetadata.DataNamingDefinition),
                    nameof(DatabaseTypeMetadata.TypeName),
                    nameof(DatabaseTypeMetadata.Memo),
                    nameof(DatabaseTypeMetadata.FieldCount),
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
        public DatabaseTypeMetadataTable(IDatabaseTypeMetadataTableSettings settings) : this(
            ValidateInitSettings(settings),
            BuildSimpleList(settings.Settings)
        )
        {
        }

        private static IDatabaseTypeMetadataTableSettings ValidateInitSettings(
            IDatabaseTypeMetadataTableSettings settings
        )
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

        private IWodiLib2DListValidator<IDatabaseTypeMetadataTableSettings, IDatabaseNamedDataRowSettings,
                DatabaseFieldValue>
            BuildValidator(IDatabaseTypeMetadataTableSettings settings, SimpleList<DatabaseNamedDataRow> itemsImpl)
        {
            var getSelf = new Func<DatabaseTypeMetadataTable>(() => this);
            return new DatabaseDataTableValidator<IDatabaseTypeMetadataTableSettings, IDatabaseNamedDataRowSettings>(
                rowCountGetter: () => getSelf.Invoke().DataCount,
                columnCountGetter: () => getSelf.Invoke().FieldCount,
                fieldTypesGetter: () => getSelf.Invoke().GetDataInternal(0).Select(row => row.Type).ToArray()
            );
        }

        #endregion

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public DatabaseTypeMetadataTable() : this(new DatabaseTypeMetadataTableSettings())
        {
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        #region ItemEquals

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IDatabaseTypeMetadataTableSettings? other)
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
                   && FieldMetadataList.ItemEquals(other.FieldMetadataList);
        }

        #endregion

        #endregion

        #endregion
    }
}
