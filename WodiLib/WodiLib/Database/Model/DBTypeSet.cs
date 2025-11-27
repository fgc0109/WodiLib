// ========================================
// Project Name : WodiLib
// File Name    : DBTypeSet.cs
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
    [Model(Description = "DBタイプセット（XXX.dbtypeset）")]
    public partial class DBTypeSet
    {
        #region Properties

        #region public

        /// <summary>
        ///     DBタイプ情報
        /// </summary>
        [ImmutableProperty(
            ReturnType = typeof(ReadOnlyDatabaseTypeDefinition)
        )]
        [SettingsProperty(
            ReturnType = typeof(IDatabaseTypeDefinitionSettings),
            DefaultValue = "new DatabaseTypeDefinitionSettings()"
        )]
        public DatabaseTypeDefinition TypeDefinition
        {
            get => typeDefinition;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(TypeDefinition));

                SetField(ref typeDefinition, value);
            }
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Fields

        private DatabaseTypeDefinition typeDefinition;

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
        public DBTypeSet(IDBTypeSetSettings settings)
        {
            ThrowHelper.ValidateArgumentNotNull(settings is null, nameof(settings));
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.TypeDefinition is null,
                nameof(settings),
                nameof(settings.TypeDefinition)
            );

            typeDefinition = new DatabaseTypeDefinition(settings.TypeDefinition);
        }

        #endregion

        #region Convenience

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public DBTypeSet() : this(new DBTypeSetSettings())
        {
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IDBTypeSetSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return TypeDefinition.ItemEquals(other.TypeDefinition);
        }

        #endregion

        #endregion
    }
}
