// ========================================
// Project Name : WodiLib
// File Name    : IDatabaseFieldSpecialSettingDefinition.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     【読取専用】データベース設定値特殊指定インタフェース
    /// </summary>
    /// <remarks>
    ///     <see cref="ReadOnlyDatabaseFieldSpecialSettingDefinitionNormal"/>,
    ///     <see cref="ReadOnlyDatabaseFieldSpecialSettingDefinitionLoadFile"/>,
    ///     <see cref="ReadOnlyDatabaseFieldSpecialSettingDefinitionDatabaseReference"/>,
    ///     <see cref="ReadOnlyDatabaseFieldSpecialSettingDefinitionManual"/>
    ///     のいずれかにキャスト可能。
    /// </remarks>
    public interface IReadOnlyDatabaseFieldSpecialSettingDefinition :
        IDatabaseFieldSpecialSettingDefinitionSettings,
        INotifyPropertyChanged,
        IEqualityComparable<IReadOnlyDatabaseFieldSpecialSettingDefinition>
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
        ///     特殊指定が「特殊な設定方法を使用しない」の場合の特殊設定情報型にキャストする。
        /// </summary>
        /// <param name="result">キャスト結果</param>
        /// <returns>キャスト成否</returns>
        public bool TryCastNormal(
            [NotNullWhen(true)] out ReadOnlyDatabaseFieldSpecialSettingDefinitionNormal? result
        );

        /// <summary>
        ///     特殊指定が「ファイル読み込み」の場合の特殊設定情報型にキャストする。
        /// </summary>
        /// <param name="result">キャスト結果</param>
        /// <returns>キャスト成否</returns>
        public bool TryCastLoadFile(
            [NotNullWhen(true)] out ReadOnlyDatabaseFieldSpecialSettingDefinitionLoadFile? result
        );

        /// <summary>
        ///     特殊指定が「データベース参照」の場合の特殊設定情報型にキャストする。
        /// </summary>
        /// <param name="result">キャスト結果</param>
        /// <returns>キャスト成否</returns>
        public bool TryCastDatabaseReference(
            [NotNullWhen(true)] out ReadOnlyDatabaseFieldSpecialSettingDefinitionDatabaseReference? result
        );

        /// <summary>
        ///     特殊指定が「手動設定」の場合の特殊設定情報型にキャストする。
        /// </summary>
        /// <param name="result">キャスト結果</param>
        /// <returns>キャスト成否</returns>
        public bool TryCastManual(
            [NotNullWhen(true)] out ReadOnlyDatabaseFieldSpecialSettingDefinitionManual? result
        );

        #endregion
    }

    /// <summary>
    ///     データベース設定値特殊指定インタフェース
    /// </summary>
    /// <remarks>
    ///     <see cref="DatabaseFieldSpecialSettingDefinitionNormal"/>,
    ///     <see cref="DatabaseFieldSpecialSettingDefinitionLoadFile"/>,
    ///     <see cref="DatabaseFieldSpecialSettingDefinitionDatabaseReference"/>,
    ///     <see cref="DatabaseFieldSpecialSettingDefinitionManual"/>
    ///     のいずれかにキャスト可能。
    /// </remarks>
    public interface IDatabaseFieldSpecialSettingDefinition : IReadOnlyDatabaseFieldSpecialSettingDefinition
    {
        #region Properties

        /// <inheritdoc cref="IReadOnlyDatabaseFieldSpecialSettingDefinition.InitValue"/>
        /// <exception cref="PropertyNullException">
        ///     <see langword="null"/> をセットしようとした場合。
        /// </exception>
        public new DatabaseValueInt InitValue { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc cref="IReadOnlyDatabaseFieldSpecialSettingDefinition.TryCastNormal"/>
        public bool TryCastNormal(
            [NotNullWhen(true)] out DatabaseFieldSpecialSettingDefinitionNormal? result
        );

        /// <inheritdoc cref="IReadOnlyDatabaseFieldSpecialSettingDefinition.TryCastLoadFile"/>
        public bool TryCastLoadFile(
            [NotNullWhen(true)] out DatabaseFieldSpecialSettingDefinitionLoadFile? result
        );

        /// <inheritdoc cref="IReadOnlyDatabaseFieldSpecialSettingDefinition.TryCastDatabaseReference"/>
        public bool TryCastDatabaseReference(
            [NotNullWhen(true)] out DatabaseFieldSpecialSettingDefinitionDatabaseReference? result
        );

        /// <inheritdoc cref="IReadOnlyDatabaseFieldSpecialSettingDefinition.TryCastManual"/>
        public bool TryCastManual(
            [NotNullWhen(true)] out DatabaseFieldSpecialSettingDefinitionManual? result
        );

        #endregion
    }
}
