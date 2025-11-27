// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDataNamingDefinition.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     DBデータ名の設定方法
    /// </summary>
    public partial record DatabaseDataNamingDefinition
    {
        #region Constants

        #region public

        /// <summary>デフォルト値</summary>
        public static readonly DatabaseDataNamingDefinition Default = new(DatabaseDataNamingType.Manual);

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Properties

        #region public

        /// <summary>データ名の設定方法種別</summary>
        public DatabaseDataNamingType NamingType { get; }

        /// <summary>データの設定方法＝指定DBの場合のDB種別</summary>
        /// <remarks>
        ///     データ名の設定方法が「DB指定」ではない場合 <see langword="null"/> が格納される。
        /// </remarks>
        public DatabaseKind? DBKind => ReferDatabaseDefinition?.DatabaseKind;

        /// <summary>データの設定方法＝指定DBの場合のタイプID</summary>
        /// <remarks>
        ///     データ名の設定方法が「DB指定」ではない場合 <see langword="null"/> が格納される。
        /// </remarks>
        public TypeId? TypeId => ReferDatabaseDefinition?.TypeId;

        #endregion

        #region internal

        /// <summary>データの設定方法＝指定DBの場合の追加設定情報</summary>
        /// <remarks>
        ///     データ名の設定方法が「DB指定」ではない場合 <see langword="null"/> が格納される。
        /// </remarks>
        internal DataNameSpecificationDefinition? ReferDatabaseDefinition { get; } = null;

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Constructor

        #region Required

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="namingType">データ名の設定方法種別（デフォルト値： <see cref="DatabaseDataNamingType.Manual"/>）</param>
        /// <param name="dbKind">データの設定方法＝指定DBの場合のDB種別</param>
        /// <param name="typeId">データの設定方法＝指定DBの場合のタイプID</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="namingType"/> が <see cref="DatabaseDataNamingType.DesignatedType"/> かつ
        ///     <paramref name="dbKind"/>, <paramref name="typeId"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <remarks>
        ///     <paramref name="namingType"/> が <see cref="DatabaseDataNamingType.DesignatedType"/> ではない場合、
        ///     <paramref name="dbKind"/>, <paramref name="typeId"/> の値は不問。
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public DatabaseDataNamingDefinition(
            DatabaseDataNamingType? namingType = null,
            DatabaseKind? dbKind = null,
            TypeId? typeId = null
        )
        {
            if (namingType == DatabaseDataNamingType.DesignatedType)
            {
                ThrowHelper.ValidateArgumentNotNull(dbKind is null, nameof(dbKind));
                ThrowHelper.ValidateArgumentNotNull(typeId is null, nameof(typeId));

                ReferDatabaseDefinition = new DataNameSpecificationDefinition
                {
                    DatabaseKind = dbKind,
                    TypeId = typeId,
                };
            }
            // データ名の設定方法が「DB指定」ではない場合、引数 dBKind, typeId を無視する

            NamingType = namingType ?? DatabaseDataNamingType.Manual;
        }

        #endregion

        #region Convenience

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="namingType">データ名の設定方法種別</param>
        /// <param name="referDatabaseDefinition">データの設定方法＝指定DBの場合の追加設定情報</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="namingType"/> が <see cref="DatabaseDataNamingType.DesignatedType"/> かつ
        ///     <paramref name="referDatabaseDefinition"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <remarks>
        ///     <paramref name="namingType"/> が <see cref="DatabaseDataNamingType.DesignatedType"/> ではない場合、
        ///     <paramref name="referDatabaseDefinition"/> の値は不問。
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public DatabaseDataNamingDefinition(
            DatabaseDataNamingType? namingType,
            DataNameSpecificationDefinition? referDatabaseDefinition
        ) : this(
            namingType,
            referDatabaseDefinition?.DatabaseKind,
            referDatabaseDefinition?.TypeId
        )
        {
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Static Methods

        #region public

        #region Builder

        /// <summary>
        ///     データの設定方法＝「手動で設定」のインスタンスを作成する。
        /// </summary>
        /// <returns>
        ///     データの設定方法＝「手動で設定」のインスタンス
        /// </returns>
        public static DatabaseDataNamingDefinition BuildManual()
            => new(DatabaseDataNamingType.Manual);

        /// <summary>
        ///     データの設定方法＝「最初の文字列データと同じ」のインスタンスを作成する。
        /// </summary>
        /// <returns>
        ///     データの設定方法＝「最初の文字列データと同じ」のインスタンス
        /// </returns>
        public static DatabaseDataNamingDefinition BuildFirstStringData()
            => new(DatabaseDataNamingType.FirstStringData);

        /// <summary>
        ///     データの設定方法＝「1つ前のタイプのデータIDと同じ」のインスタンスを作成する。
        /// </summary>
        /// <returns>
        ///     データの設定方法＝「1つ前のタイプのデータIDと同じ」のインスタンス
        /// </returns>
        public static DatabaseDataNamingDefinition BuildEqualBefore()
            => new(DatabaseDataNamingType.EqualBefore);

        /// <summary>
        ///     データの設定方法＝「指定DBの指定タイプから」のインスタンスを作成する。
        /// </summary>
        /// <param name="dbKind">DB種別</param>
        /// <param name="typeId">タイプID</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="dbKind"/>, <paramref name="typeId"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <returns>
        ///     データの設定方法＝「指定DBの指定タイプから」のインスタンス
        /// </returns>
        public static DatabaseDataNamingDefinition BuildDesignatedType(
            DatabaseKind dbKind,
            TypeId typeId
        )
            => new(DatabaseDataNamingType.DesignatedType, dbKind, typeId);

        #endregion

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <inheritdoc/>
        public virtual bool Equals(DatabaseDataNamingDefinition? other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(other, null)) return false;

            return DataNamingTypeEqualityComparer.Instance.Equals(
                (NamingType, () => ReferDatabaseDefinition),
                (other.NamingType, () => other.ReferDatabaseDefinition)
            );
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                return (NamingType.GetHashCode() * 397)
                       ^ (ReferDatabaseDefinition is not null
                           ? ReferDatabaseDefinition.GetHashCode()
                           : 0);
            }
        }

        #endregion

        #endregion
    }
}
