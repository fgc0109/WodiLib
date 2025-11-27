// ========================================
// Project Name : WodiLib
// File Name    : DatabaseProjectType.cs
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
    [Model(Description = "DBタイプ情報")]
    public partial class DatabaseProjectType
    {
        #region Properties

        #region public

        /// <summary>DBタイプ名</summary>
        /// <exception cref="PropertyNullException">
        ///     <see langword="null"/> をセットしようとした場合。
        /// </exception>
        [ImmutableProperty]
        [SettingsProperty(DefaultValue = "TypeName.Default")]
        public TypeName TypeName
        {
            get => typeName;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(TypeName));

                SetField(ref typeName, value);
            }
        }

        /// <summary>メモ</summary>
        /// <exception cref="PropertyNullException">
        ///     <see langword="null"/> をセットしようとした場合。
        /// </exception>
        [ImmutableProperty]
        [SettingsProperty(DefaultValue = "DatabaseMemo.Default")]
        public DatabaseMemo Memo
        {
            get => memo;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(Memo));

                SetField(ref memo, value);
            }
        }

        /// <summary>データ数 </summary>
        [ImmutableProperty]
        public int DataCount => DataNameList.Count;

        /// <summary>項目数</summary>
        [ImmutableProperty]
        public int FieldCount => FieldMetadataList.Count;

        /// <summary>[InstanceNotChanged] データ名リスト</summary>
        [InstanceNotChange]
        [ImmutableProperty(
            ReturnType = typeof(ReadOnlyDatabaseDataNameList)
        )]
        [SettingsProperty(
            ReturnType = typeof(IDatabaseDataNameListSettings),
            DefaultValue = "new DatabaseDataNameListSettings()"
        )]
        public DatabaseDataNameList DataNameList => dataNameList;

        /// <summary>[InstanceNotChange] DB項目メタ情報リスト</summary>
        [InstanceNotChange]
        [ImmutableProperty(
            ReturnType = typeof(ReadOnlyDatabaseFieldMetadataList)
        )]
        [SettingsProperty(
            ReturnType = typeof(IDatabaseFieldMetadataListSettings),
            DefaultValue = "new DatabaseFieldMetadataListSettings()"
        )]
        public DatabaseFieldMetadataList FieldMetadataList => fieldMetadataList;

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Fields

        private TypeName typeName;
        private DatabaseMemo memo;

        private readonly DatabaseDataNameList dataNameList;
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
        public DatabaseProjectType(IDatabaseProjectTypeSettings settings)
        {
            ThrowHelper.ValidateArgumentNotNull(settings is null, nameof(settings));
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.DataNameList is null,
                nameof(settings),
                nameof(settings.DataNameList)
            );
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.DataNameList.Settings is null,
                nameof(settings),
                $"{nameof(settings.DataNameList)}.{nameof(settings.DataNameList.Settings)}"
            );
            ThrowHelper.ValidateArgumentPropertyItemsHasNotNull(
                settings.DataNameList.Settings.HasNullItem(),
                nameof(settings),
                $"{nameof(settings.DataNameList)}.{nameof(settings.DataNameList.Settings)}"
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
                $"{nameof(settings.FieldMetadataList)}.{nameof(settings.FieldMetadataList.Settings)}"
            );
            ThrowHelper.ValidateArgumentPropertyNotNull(settings.Memo is null, nameof(settings), nameof(settings.Memo));
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.TypeName is null,
                nameof(settings),
                nameof(settings.TypeName)
            );

            typeName = settings.TypeName;
            memo = settings.Memo;
            dataNameList = new DatabaseDataNameList(settings.DataNameList);
            fieldMetadataList = new DatabaseFieldMetadataList(settings.FieldMetadataList);

            PropagatePropertyChangeEvent();
        }

        /// <summary>
        ///     各プロパティのプロパティ変更通知を自身に伝播させる。
        /// </summary>
        private void PropagatePropertyChangeEvent()
        {
            PropagatePropertyChangeEvent(
                DataNameList,
                (_, name) =>
                {
                    return name switch
                    {
                        nameof(DataNameList.Count) => new[] { nameof(DataCount) },
                        _ => new[] { name },
                    };
                }
            );
            PropagatePropertyChangeEvent(
                FieldMetadataList,
                (_, name) =>
                {
                    return name switch
                    {
                        nameof(FieldMetadataList.Count) => new[] { nameof(FieldCount) },
                        _ => new[] { name },
                    };
                }
            );
        }

        #endregion

        #region Convenience

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public DatabaseProjectType() : this(
            new DatabaseProjectTypeSettings()
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
        public bool ItemEquals(IDatabaseProjectTypeSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return FieldMetadataList.ItemEquals(other.FieldMetadataList)
                   && TypeName == other.TypeName
                   && Memo == other.Memo
                   && DataNameList.ItemEquals(other.DataNameList);
        }

        #endregion

        #endregion
    }
}
