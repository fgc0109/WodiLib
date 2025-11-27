// ========================================
// Project Name : WodiLib
// File Name    : DatabaseTypeMetadata.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Diagnostics.Contracts;
using WodiLib.SourceGenerator.Domain.Model.Attributes;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     DBタイプ情報クラス
    /// </summary>
    [Model(Description = "DBタイプ情報クラス")]
    public partial class DatabaseTypeMetadata
    {
        #region Properties

        #region public

        /// <summary>DBタイプ名</summary>
        [ImmutableProperty]
        [SettingsProperty(DefaultValue = "TypeName.Default")]
        public TypeName TypeName
        {
            [Pure] get => typeName;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(TypeName));
                SetField(ref typeName, value);
            }
        }

        /// <summary>メモ</summary>
        [ImmutableProperty]
        [SettingsProperty(DefaultValue = "DatabaseMemo.Default")]
        public DatabaseMemo Memo
        {
            [Pure] get => memo;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(Memo));
                SetField(ref memo, value);
            }
        }

        /// <summary>項目数</summary>
        public int FieldCount => FieldMetadataList.Count;

        /// <summary>データ名の設定方法</summary>
        [ImmutableProperty]
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
        ///     DB項目設定リスト
        /// </summary>
        [InstanceNotChange]
        [ImmutableProperty(
            ReturnType = typeof(ReadOnlyDatabaseFieldMetadataList)
        )]
        [SettingsProperty(
            ReturnType = typeof(IDatabaseFieldMetadataListSettings),
            DefaultValue = "new DatabaseFieldMetadataListSettings()"
        )]
        [Pure]
        public DatabaseFieldMetadataList FieldMetadataList => fieldMetadataList;

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Fields

        private TypeName typeName;
        private DatabaseMemo memo;
        private DatabaseDataNamingDefinition dataNamingDefinition;
        private readonly DatabaseFieldMetadataList fieldMetadataList;

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
        public DatabaseTypeMetadata(IDatabaseTypeMetadataSettings settings)
        {
            ThrowHelper.ValidateArgumentNotNull(settings is null, nameof(settings));
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.TypeName is null,
                nameof(settings),
                nameof(settings.TypeName)
            );
            ThrowHelper.ValidateArgumentPropertyNotNull(settings.Memo is null, nameof(settings), nameof(settings.Memo));
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.DataNamingDefinition is null,
                nameof(settings),
                nameof(settings.DataNamingDefinition)
            );
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.FieldMetadataList is null,
                nameof(settings),
                nameof(settings.FieldMetadataList)
            );
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.FieldMetadataList.Settings is null,
                nameof(settings),
                $"{nameof(settings.FieldMetadataList)}.{nameof(settings.FieldMetadataList.Settings)}"
            );
            ThrowHelper.ValidateArgumentPropertyItemsHasNotNull(
                settings.FieldMetadataList.Settings.HasNullItem(),
                nameof(settings),
                nameof(settings.FieldMetadataList)
            );

            typeName = settings.TypeName;
            memo = settings.Memo;
            dataNamingDefinition = settings.DataNamingDefinition;
            fieldMetadataList = new DatabaseFieldMetadataList(settings.FieldMetadataList);

            PropagatePropertyChangeEvent();
        }

        /// <summary>
        ///     各プロパティのプロパティ変更通知を自身に伝播させる。
        /// </summary>
        private void PropagatePropertyChangeEvent()
        {
            PropagatePropertyChangeEvent(
                FieldMetadataList,
                (_, name) =>
                {
                    if (name.Equals(nameof(FieldMetadataList.Count)))
                    {
                        return new[] { nameof(FieldCount) };
                    }

                    return null;
                }
            );
        }

        #endregion

        #region Convenience

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public DatabaseTypeMetadata() : this(new DatabaseTypeMetadataSettings())
        {
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Method

        #region public

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IDatabaseTypeMetadataSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return TypeName.Equals(other.TypeName)
                   && Memo.Equals(other.Memo)
                   && DataNamingDefinition.Equals(other.DataNamingDefinition)
                   && FieldMetadataList.ItemEquals(other.FieldMetadataList);
        }

        #endregion

        #endregion
    }
}
