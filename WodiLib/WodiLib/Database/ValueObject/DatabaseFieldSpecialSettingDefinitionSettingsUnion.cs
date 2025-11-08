// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldSpecialSettingDefinitionSettingsUnion.cs
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
    ///     <see cref="IDatabaseFieldSpecialSettingDefinitionNormalSettings"/>,
    ///     <see cref="IDatabaseFieldSpecialSettingDefinitionLoadFileSettings"/>,
    ///     <see cref="IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings"/>,
    ///     <see cref="IDatabaseFieldSpecialSettingDefinitionManualSettings"/> の
    ///     ユニオン型インスタンス
    /// </summary>
    public class DatabaseFieldSpecialSettingDefinitionSettingsUnion :
        IEquatable<DatabaseFieldSpecialSettingDefinitionSettingsUnion>,
        IEqualityComparable<DatabaseFieldSpecialSettingDefinitionSettingsUnion>,
        IEqualityComparable<IReadOnlyDatabaseFieldSpecialSettingDefinition>
    {
        #region Properties

        /// <summary>
        ///     保持しているDTOインスタンス種別
        /// </summary>
        public DatabaseFieldSpecialSettingType DtoType { get; }

        /// <summary>
        ///     保持しているDTOインスタンス種別
        /// </summary>
        /*
            SourceGenerator の都合で用意しているプロパティ。
            実際には DtoType を使用する。
        */
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DatabaseFieldSpecialSettingType SettingType => DtoType;

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Fields

        private readonly IDatabaseFieldSpecialSettingDefinitionNormalSettings? normalSettings = null;
        private readonly IDatabaseFieldSpecialSettingDefinitionLoadFileSettings? loadFileSettings = null;

        private readonly IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings? databaseReferenceSettings =
            null;

        private readonly IDatabaseFieldSpecialSettingDefinitionManualSettings? manualSettings = null;

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Constructor

        #region Required

        /// <summary>
        ///     <see cref="DatabaseFieldSpecialSettingDefinitionNormal"/> を格納するコンストラクタ
        /// </summary>
        /// <param name="settings">格納する設定DTO</param>
        public DatabaseFieldSpecialSettingDefinitionSettingsUnion(
            IDatabaseFieldSpecialSettingDefinitionNormalSettings settings
        )
        {
            ThrowHelper.ValidateArgumentNotNull(settings is null, nameof(settings));

            normalSettings = settings;
            DtoType = DatabaseFieldSpecialSettingType.Normal;
        }

        /// <summary>
        ///     <see cref="DatabaseFieldSpecialSettingDefinitionNormal"/> を格納するコンストラクタ
        /// </summary>
        /// <param name="settings">格納する設定DTO</param>
        public DatabaseFieldSpecialSettingDefinitionSettingsUnion(
            IDatabaseFieldSpecialSettingDefinitionLoadFileSettings settings
        )
        {
            ThrowHelper.ValidateArgumentNotNull(settings is null, nameof(settings));

            loadFileSettings = settings;
            DtoType = DatabaseFieldSpecialSettingType.LoadFile;
        }

        /// <summary>
        ///     <see cref="DatabaseFieldSpecialSettingDefinitionNormal"/> を格納するコンストラクタ
        /// </summary>
        /// <param name="settings">格納する設定DTO</param>
        public DatabaseFieldSpecialSettingDefinitionSettingsUnion(
            IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings settings
        )
        {
            ThrowHelper.ValidateArgumentNotNull(settings is null, nameof(settings));

            databaseReferenceSettings = settings;
            DtoType = DatabaseFieldSpecialSettingType.ReferDatabase;
        }

        /// <summary>
        ///     <see cref="DatabaseFieldSpecialSettingDefinitionNormal"/> を格納するコンストラクタ
        /// </summary>
        /// <param name="settings">格納する設定DTO</param>
        public DatabaseFieldSpecialSettingDefinitionSettingsUnion(
            IDatabaseFieldSpecialSettingDefinitionManualSettings settings
        )
        {
            ThrowHelper.ValidateArgumentNotNull(settings is null, nameof(settings));

            manualSettings = settings;
            DtoType = DatabaseFieldSpecialSettingType.Manual;
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = normalSettings != null
                    ? normalSettings.GetHashCode()
                    : 0;
                hashCode = (hashCode * 397)
                           ^ (loadFileSettings != null
                               ? loadFileSettings.GetHashCode()
                               : 0);
                hashCode = (hashCode * 397)
                           ^ (databaseReferenceSettings != null
                               ? databaseReferenceSettings.GetHashCode()
                               : 0);
                hashCode = (hashCode * 397)
                           ^ (manualSettings != null
                               ? manualSettings.GetHashCode()
                               : 0);
                hashCode = (hashCode * 397) ^ DtoType.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        ///     <see cref="IDatabaseFieldSpecialSettingDefinitionNormalSettings"/> 型の設定DTOを取り出す。
        /// </summary>
        /// <returns>設定DTO</returns>
        /// <exception cref="InvalidCastException">
        ///     <see cref="DtoType"/> が <see cref="DatabaseFieldSpecialSettingType.Normal"/> 以外の場合。
        /// </exception>
        public IDatabaseFieldSpecialSettingDefinitionNormalSettings AsNormalSettings()
        {
            if (DtoType != DatabaseFieldSpecialSettingType.Normal) throw new InvalidCastException();
            return normalSettings!;
        }

        /// <summary>
        ///     <see cref="IDatabaseFieldSpecialSettingDefinitionLoadFileSettings"/> 型の設定DTOを取り出す。
        /// </summary>
        /// <returns>設定DTO</returns>
        /// <exception cref="InvalidCastException">
        ///     <see cref="DtoType"/> が <see cref="DatabaseFieldSpecialSettingType.LoadFile"/> 以外の場合。
        /// </exception>
        public IDatabaseFieldSpecialSettingDefinitionLoadFileSettings AsLoadFileSettings()
        {
            if (DtoType != DatabaseFieldSpecialSettingType.LoadFile) throw new InvalidCastException();
            return loadFileSettings!;
        }

        /// <summary>
        ///     <see cref="IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings"/> 型の設定DTOを取り出す。
        /// </summary>
        /// <returns>設定DTO</returns>
        /// <exception cref="InvalidCastException">
        ///     <see cref="DtoType"/> が <see cref="DatabaseFieldSpecialSettingType.ReferDatabase"/> 以外の場合。
        /// </exception>
        public IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings AsDatabaseReferenceSettings()
        {
            if (DtoType != DatabaseFieldSpecialSettingType.ReferDatabase) throw new InvalidCastException();
            return databaseReferenceSettings!;
        }

        /// <summary>
        ///     <see cref="IDatabaseFieldSpecialSettingDefinitionManualSettings"/> 型の設定DTOを取り出す。
        /// </summary>
        /// <returns>設定DTO</returns>
        /// <exception cref="InvalidCastException">
        ///     <see cref="DtoType"/> が <see cref="DatabaseFieldSpecialSettingType.Manual"/> 以外の場合。
        /// </exception>
        public IDatabaseFieldSpecialSettingDefinitionManualSettings AsManualSettings()
        {
            if (DtoType != DatabaseFieldSpecialSettingType.Manual) throw new InvalidCastException();
            return manualSettings!;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => Equals(obj as DatabaseFieldSpecialSettingDefinitionSettingsUnion);

        /// <inheritdoc/>
        public bool Equals(DatabaseFieldSpecialSettingDefinitionSettingsUnion? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!DtoType.Equals(other.DtoType)) return false;

            if (DtoType == DatabaseFieldSpecialSettingType.Normal)
            {
                return normalSettings!.ItemEquals(other.normalSettings);
            }

            if (DtoType == DatabaseFieldSpecialSettingType.LoadFile)
            {
                return loadFileSettings!.ItemEquals(other.loadFileSettings);
            }

            if (DtoType == DatabaseFieldSpecialSettingType.ReferDatabase)
            {
                return databaseReferenceSettings!.ItemEquals(other.databaseReferenceSettings);
            }

            if (DtoType == DatabaseFieldSpecialSettingType.Manual)
            {
                return manualSettings!.ItemEquals(other.manualSettings);
            }

            return false;
        }

        /// <inheritdoc/>
        public bool ItemEquals(DatabaseFieldSpecialSettingDefinitionSettingsUnion? other) => Equals(other);

        /// <inheritdoc/>
        public bool ItemEquals(IReadOnlyDatabaseFieldSpecialSettingDefinition? other)
        {
            if (other is null) return false;

            if (!DtoType.Equals(other.SettingType)) return false;

            if (DtoType == DatabaseFieldSpecialSettingType.Normal)
            {
                return normalSettings!.ItemEquals(other.AsNormalSettings());
            }

            if (DtoType == DatabaseFieldSpecialSettingType.LoadFile)
            {
                return loadFileSettings!.ItemEquals(other.AsLoadFileSettings());
            }

            if (DtoType == DatabaseFieldSpecialSettingType.ReferDatabase)
            {
                return databaseReferenceSettings!.ItemEquals(other.AsDatabaseReferenceSettings());
            }

            if (DtoType == DatabaseFieldSpecialSettingType.Manual)
            {
                return manualSettings!.ItemEquals(other.AsManualSettings());
            }

            return false;
        }

        /// <inheritdoc/>
        public bool ItemEquals(object? other) => Equals(other as DatabaseFieldSpecialSettingDefinitionSettingsUnion);

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Operators

        /// <summary>
        ///     <see cref="DatabaseFieldSpecialSettingDefinitionSettingsUnion"/> から
        ///     <see cref="DatabaseFieldSpecialSettingDefinitionNormalSettings"/> への暗黙的な型変換
        /// </summary>
        /// <param name="settings">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator DatabaseFieldSpecialSettingDefinitionSettingsUnion(
            DatabaseFieldSpecialSettingDefinitionNormalSettings settings
        )
            => new(settings);

        /// <summary>
        ///     <see cref="DatabaseFieldSpecialSettingDefinitionSettingsUnion"/> から
        ///     <see cref="DatabaseFieldSpecialSettingDefinitionLoadFileSettings"/> への暗黙的な型変換
        /// </summary>
        /// <param name="settings">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator DatabaseFieldSpecialSettingDefinitionSettingsUnion(
            DatabaseFieldSpecialSettingDefinitionLoadFileSettings settings
        )
            => new(settings);

        /// <summary>
        ///     <see cref="DatabaseFieldSpecialSettingDefinitionSettingsUnion"/> から
        ///     <see cref="DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings"/> への暗黙的な型変換
        /// </summary>
        /// <param name="settings">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator DatabaseFieldSpecialSettingDefinitionSettingsUnion(
            DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings settings
        )
            => new(settings);

        /// <summary>
        ///     <see cref="DatabaseFieldSpecialSettingDefinitionSettingsUnion"/> から
        ///     <see cref="DatabaseFieldSpecialSettingDefinitionManualSettings"/> への暗黙的な型変換
        /// </summary>
        /// <param name="settings">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator DatabaseFieldSpecialSettingDefinitionSettingsUnion(
            DatabaseFieldSpecialSettingDefinitionManualSettings settings
        )
            => new(settings);

        /// <summary>== 演算子</summary>
        /// <param name="left">左項</param>
        /// <param name="right">右項</param>
        /// <returns><paramref name="left"/> と <paramref name="right"/> が同一要素である場合 <see langword="true"/></returns>
        public static bool operator ==(
            DatabaseFieldSpecialSettingDefinitionSettingsUnion? left,
            DatabaseFieldSpecialSettingDefinitionSettingsUnion? right
        )
        {
            if (left is null && right is null) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        /// <summary>!= 演算子</summary>
        /// <param name="left">左項</param>
        /// <param name="right">右項</param>
        /// <returns><paramref name="left"/> と <paramref name="right"/> が同一要素ではない場合 <see langword="true"/></returns>
        public static bool operator !=(
            DatabaseFieldSpecialSettingDefinitionSettingsUnion? left,
            DatabaseFieldSpecialSettingDefinitionSettingsUnion? right
        )
            => !(left == right);

        #endregion
    }
}
