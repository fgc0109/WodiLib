// ========================================
// Project Name : WodiLib
// File Name    : DatabaseTypeDefinition.cs
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
    public partial class DatabaseTypeDefinition
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
        public int FieldCount => FieldDefinitionList.Count;

        /// <summary>
        ///     DB項目設定リスト
        /// </summary>
        [InstanceNotChange]
        [ImmutableProperty(
            ReturnType = typeof(ReadOnlyDatabaseFieldDefinitionList)
        )]
        [SettingsProperty(
            ReturnType = typeof(IDatabaseFieldDefinitionListSettings),
            DefaultValue = "new DatabaseFieldDefinitionListSettings()"
        )]
        [Pure]
        public DatabaseFieldDefinitionList FieldDefinitionList => fieldDefinitionList;

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Fields

        private TypeName typeName;
        private DatabaseMemo memo;
        private readonly DatabaseFieldDefinitionList fieldDefinitionList;

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
        public DatabaseTypeDefinition(IDatabaseTypeDefinitionSettings settings)
        {
            ThrowHelper.ValidateArgumentNotNull(settings is null, nameof(settings));
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.TypeName is null,
                nameof(settings),
                nameof(settings.TypeName)
            );
            ThrowHelper.ValidateArgumentPropertyNotNull(settings.Memo is null, nameof(settings), nameof(settings.Memo));
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.FieldDefinitionList is null,
                nameof(settings),
                nameof(settings.FieldDefinitionList)
            );
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.FieldDefinitionList.Settings is null,
                nameof(settings),
                $"{nameof(settings.FieldDefinitionList)}.{nameof(settings.FieldDefinitionList.Settings)}"
            );
            ThrowHelper.ValidateArgumentPropertyItemsHasNotNull(
                settings.FieldDefinitionList.Settings.HasNullItem(),
                nameof(settings),
                nameof(settings.FieldDefinitionList)
            );

            typeName = settings.TypeName;
            memo = settings.Memo;
            fieldDefinitionList = new DatabaseFieldDefinitionList(settings.FieldDefinitionList);

            PropagatePropertyChangeEvent();
        }

        /// <summary>
        ///     各プロパティのプロパティ変更通知を自身に伝播させる。
        /// </summary>
        private void PropagatePropertyChangeEvent()
        {
            PropagatePropertyChangeEvent(
                FieldDefinitionList,
                (_, name) =>
                {
                    if (name.Equals(nameof(FieldDefinitionList.Count)))
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
        public DatabaseTypeDefinition() : this(new DatabaseTypeDefinitionSettings())
        {
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Method

        #region public

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IDatabaseTypeDefinitionSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return TypeName.Equals(other.TypeName)
                   && Memo.Equals(other.Memo)
                   && FieldDefinitionList.ItemEquals(other.FieldDefinitionList);
        }

        #endregion

        #endregion
    }
}
