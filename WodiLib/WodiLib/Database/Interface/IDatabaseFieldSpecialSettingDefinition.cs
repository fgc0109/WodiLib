// ========================================
// Project Name : WodiLib
// File Name    : IDatabaseFieldSpecialSettingDefinition.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     【読み取り専用】データベース設定値特殊指定インタフェース
    /// </summary>
    public interface IReadOnlyDatabaseFieldSpecialSettingDefinition :
        IEqualityComparable<IReadOnlyDatabaseFieldSpecialSettingDefinition>,
        IEqualityComparable<DatabaseFieldSpecialSettingDefinitionSettingsUnion>
    {
        #region Properties

        /// <summary>値特殊指定タイプ</summary>
        public DatabaseFieldSpecialSettingType SettingType { get; }

        /// <summary>デフォルト設定値種別</summary>
        public DatabaseFieldType DefaultType { get; }

        /// <summary>項目の初期値</summary>
        public DatabaseValueInt InitValue { get; }

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        /// <summary>
        ///     すべての選択肢リストを取得する。
        /// </summary>
        public IEnumerable<DatabaseValueCase> GetSpecialCases();

        /// <summary>
        ///     指定した値種別が設定可能かどうかを判定する。
        /// </summary>
        /// <param name="type">値種別</param>
        /// <returns>設定可能な場合 <see langword="tru"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> が <see langword="null"/> の場合</exception>
        public bool CanChangeFieldType(DatabaseFieldType type);

        /// <summary>
        ///     特殊指定が「データベース参照」の場合の特殊設定情報型にキャストする。
        /// </summary>
        /// <returns>キャストした結果</returns>
        /// <exception cref="InvalidOperationException">
        ///     <see cref="SettingType"/> が <see cref="DatabaseFieldSpecialSettingType.ReferDatabase"/> 以外の場合。
        /// </exception>
        public IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings AsDatabaseReferenceSettings();

        /// <summary>
        ///     特殊指定が「ファイル読み込み」の場合の特殊設定情報型にキャストする。
        /// </summary>
        /// <returns>キャストした結果</returns>
        /// <exception cref="InvalidOperationException">
        ///     <see cref="SettingType"/> が <see cref="DatabaseFieldSpecialSettingType.LoadFile"/> 以外の場合。
        /// </exception>
        public IDatabaseFieldSpecialSettingDefinitionLoadFileSettings AsLoadFileSettings();

        /// <summary>
        ///     特殊指定が「手動設定」の場合の特殊設定情報型にキャストする。
        /// </summary>
        /// <returns>キャストした結果</returns>
        /// <exception cref="InvalidOperationException">
        ///     <see cref="SettingType"/> が <see cref="DatabaseFieldSpecialSettingType.Manual"/> 以外の場合。
        /// </exception>
        public IDatabaseFieldSpecialSettingDefinitionManualSettings AsManualSettings();

        /// <summary>
        ///     特殊指定が「特殊な設定方法を使用しない」の場合の特殊設定情報型にキャストする。
        /// </summary>
        /// <returns>キャストした結果</returns>
        /// <exception cref="InvalidOperationException">
        ///     <see cref="SettingType"/> が <see cref="DatabaseFieldSpecialSettingType.Normal"/> 以外の場合。
        /// </exception>
        public IDatabaseFieldSpecialSettingDefinitionNormalSettings AsNormalSettings();

        #endregion
    }

    /// <summary>
    ///     データベース設定値特殊指定インタフェース
    /// </summary>
    public interface IDatabaseFieldSpecialSettingDefinition : IReadOnlyDatabaseFieldSpecialSettingDefinition
    {
        #region Properties

        /// <inheritdoc cref="IReadOnlyDatabaseFieldSpecialSettingDefinition.InitValue"/>
        /// <exception cref="PropertyNullException">
        ///     <see langword="null"/> をセットしようとした場合。
        /// </exception>
        public new DatabaseValueInt InitValue { get; set; }

        #endregion
    }
}
