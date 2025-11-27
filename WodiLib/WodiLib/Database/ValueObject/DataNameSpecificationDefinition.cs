// ========================================
// Project Name : WodiLib
// File Name    : DataNameSpecificationDesc.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     データの設定方法＝指定DBの場合の追加設定情報
    /// </summary>
    [CommonMultiValueObject]
    public partial record DataNameSpecificationDefinition
    {
        #region Constants

        #region public

        /// <summary>デフォルト値</summary>
        public static readonly DataNameSpecificationDefinition Default = new();

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Properties

        #region public

        /// <summary>DB種別</summary>
        public DatabaseKind DatabaseKind { get; init; } = DatabaseKind.Changeable;

        /// <summary>タイプID</summary>
        public TypeId TypeId { get; init; } = 0;

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Constructors

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="dbKind">DB種別（デフォルト値： <see cref="DatabaseKind.Changeable"/>）</param>
        /// <param name="typeId">タイプID（デフォルト値： 0）</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="dbKind"/>, <paramref name="typeId"/> が <see langword="null"/> の場合。
        /// </exception>
        public DataNameSpecificationDefinition(DatabaseKind? dbKind, TypeId? typeId) : this()
        {
            DatabaseKind = dbKind ?? DatabaseKind;
            TypeId = typeId ?? TypeId;
        }

        #endregion
    }
}
