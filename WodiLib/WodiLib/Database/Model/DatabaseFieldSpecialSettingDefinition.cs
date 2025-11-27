// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldSpecialSettingDefinition.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using WodiLib.SourceGenerator.Domain.Model.Attributes;
using WodiLib.Sys;

namespace WodiLib.Database
{
    public partial interface IDatabaseFieldSpecialSettingDefinitionSettings
    {
        /// <summary>
        ///     <see cref="IDatabaseFieldSpecialSettingDefinitionNormalSettings"/> にキャストする。
        /// </summary>
        /// <param name="result">キャスト結果</param>
        /// <returns>キャスト成否</returns>
        public bool TryCastNormalSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionNormalSettings? result
        );

        /// <summary>
        ///     <see cref="IDatabaseFieldSpecialSettingDefinitionLoadFileSettings"/> にキャストする。
        /// </summary>
        /// <param name="result">キャスト結果</param>
        /// <returns>キャスト成否</returns>
        public bool TryCastLoadFileSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionLoadFileSettings? result
        );

        /// <summary>
        ///     <see cref="IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings"/> にキャストする。
        /// </summary>
        /// <param name="result">キャスト結果</param>
        /// <returns>キャスト成否</returns>
        public bool TryCastDatabaseReferenceSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings? result
        );

        /// <summary>
        ///     <see cref="IDatabaseFieldSpecialSettingDefinitionManualSettings"/> にキャストする。
        /// </summary>
        /// <param name="result">キャスト結果</param>
        /// <returns>キャスト成否</returns>
        public bool TryCastManualSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionManualSettings? result
        );
    }

    public partial record DatabaseFieldSpecialSettingDefinitionSettings
    {
        [Pure] internal IDatabaseFieldSpecialSettingDefinitionSettings Impl { get; }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="impl">設定DTO実体</param>
        public DatabaseFieldSpecialSettingDefinitionSettings(
            IDatabaseFieldSpecialSettingDefinitionSettings impl
        )
        {
            Impl = impl;
        }

        /// <inheritdoc/>
        [Pure]
        public bool TryCastNormalSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionNormalSettings? result
        ) => Impl.TryCastNormalSettings(out result);

        /// <inheritdoc/>
        [Pure]
        public bool TryCastLoadFileSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionLoadFileSettings? result
        ) => Impl.TryCastLoadFileSettings(out result);

        /// <inheritdoc/>
        [Pure]
        public bool TryCastDatabaseReferenceSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings? result
        ) => Impl.TryCastDatabaseReferenceSettings(out result);

        /// <inheritdoc/>
        [Pure]
        public bool TryCastManualSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionManualSettings? result
        ) => Impl.TryCastManualSettings(out result);

        #region Operators

        #region From

        /// <summary>
        ///     <see cref="DatabaseFieldSpecialSettingDefinitionNormal"/> からの暗黙的型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        [return: NotNullIfNotNull(nameof(src))]
        public static implicit operator DatabaseFieldSpecialSettingDefinitionSettings?(
            DatabaseFieldSpecialSettingDefinitionNormalSettings? src
        )
            => src is null
                ? null
                : new DatabaseFieldSpecialSettingDefinitionSettings(
                    (IDatabaseFieldSpecialSettingDefinitionSettings)src
                );

        /// <summary>
        ///     <see cref="DatabaseFieldSpecialSettingDefinitionLoadFile"/> からの暗黙的型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        [return: NotNullIfNotNull(nameof(src))]
        public static implicit operator DatabaseFieldSpecialSettingDefinitionSettings?(
            DatabaseFieldSpecialSettingDefinitionLoadFileSettings? src
        )
            => src is null
                ? null
                : new DatabaseFieldSpecialSettingDefinitionSettings(
                    (IDatabaseFieldSpecialSettingDefinitionSettings)src
                );

        /// <summary>
        ///     <see cref="DatabaseFieldSpecialSettingDefinitionDatabaseReference"/> からの暗黙的型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        [return: NotNullIfNotNull(nameof(src))]
        public static implicit operator DatabaseFieldSpecialSettingDefinitionSettings?(
            DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings? src
        )
            => src is null
                ? null
                : new DatabaseFieldSpecialSettingDefinitionSettings(
                    (IDatabaseFieldSpecialSettingDefinitionSettings)src
                );

        /// <summary>
        ///     <see cref="DatabaseFieldSpecialSettingDefinitionManual"/> からの暗黙的型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        [return: NotNullIfNotNull(nameof(src))]
        public static implicit operator DatabaseFieldSpecialSettingDefinitionSettings?(
            DatabaseFieldSpecialSettingDefinitionManualSettings? src
        )
            => src is null
                ? null
                : new DatabaseFieldSpecialSettingDefinitionSettings(
                    (IDatabaseFieldSpecialSettingDefinitionSettings)src
                );

        #endregion

        #endregion
    }

    [Model(Description = "データベース設定値特殊指定クラス")]
    public partial class DatabaseFieldSpecialSettingDefinition :
        IDatabaseFieldSpecialSettingDefinition
    {
        #region Properties

        #region public

        /// <summary>値特殊指定タイプ</summary>
        [ImmutableProperty]
        [Pure]
        public DatabaseFieldSpecialSettingType SettingType => Impl.SettingType;

        /// <summary>デフォルト設定値種別</summary>
        [ImmutableProperty]
        [Pure]
        public DatabaseFieldType DefaultType => Impl.DefaultType;

        /// <inheritdoc cref="IReadOnlyDatabaseFieldSpecialSettingDefinition.InitValue"/>
        /// <exception cref="PropertyNullException">
        ///     <see langword="null"/> をセットしようとした場合。
        /// </exception>
        [ImmutableProperty]
        public DatabaseValueInt InitValue
        {
            [Pure] get => Impl.InitValue;
            set => Impl.InitValue = value;
        }

        #endregion

        #region private

        private IDatabaseFieldSpecialSettingDefinition Impl { get; }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Constructors

        #region Required

        private DatabaseFieldSpecialSettingDefinition(IDatabaseFieldSpecialSettingDefinition impl)
        {
            Impl = impl;
            PropagatePropertyChangeEvent(
                Impl,
                (_, name) => name is nameof(Impl.DefaultType) or nameof(Impl.InitValue)
            );
        }

        #endregion

        #region Convenience

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="settings">設定DTO</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合、
        ///     または <paramref name="settings"/> が <see cref="IDatabaseFieldSpecialSettingDefinitionNormalSettings"/>,
        ///     <see cref="IDatabaseFieldSpecialSettingDefinitionLoadFileSettings"/>,
        ///     <see cref="IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings"/>,
        ///     <see cref="IDatabaseFieldSpecialSettingDefinitionManualSettings"/>
        ///     いずれにもキャストできない場合
        /// </exception>
        public DatabaseFieldSpecialSettingDefinition(IDatabaseFieldSpecialSettingDefinitionSettings settings)
            : this(
                new Func<IDatabaseFieldSpecialSettingDefinition>(() =>
                    {
                        ThrowHelper.ValidateArgumentNotNull(settings is null, nameof(settings));

                        if (settings.TryCastNormalSettings(out var normalSettings))
                        {
                            return new DatabaseFieldSpecialSettingDefinitionNormal(normalSettings);
                        }

                        if (settings.TryCastLoadFileSettings(out var loadFileSettings))
                        {
                            return new DatabaseFieldSpecialSettingDefinitionLoadFile(loadFileSettings);
                        }

                        if (settings.TryCastDatabaseReferenceSettings(out var databaseReferenceSettings))
                        {
                            return new DatabaseFieldSpecialSettingDefinitionDatabaseReference(
                                databaseReferenceSettings
                            );
                        }

                        if (settings.TryCastManualSettings(out var manualSettings))
                        {
                            return new DatabaseFieldSpecialSettingDefinitionManual(manualSettings);
                        }

                        throw new ArgumentException(
                            ErrorMessage.InvalidAnyCast(
                                nameof(settings),
                                nameof(IDatabaseFieldSpecialSettingDefinitionNormalSettings),
                                nameof(IDatabaseFieldSpecialSettingDefinitionLoadFileSettings),
                                nameof(IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings),
                                nameof(IDatabaseFieldSpecialSettingDefinitionManualSettings)
                            )
                        );
                    }
                )()
            )
        {
        }

        /// <summary>
        ///     読取専用クラスの DeepClone メソッド専用のコピーコンストラクタ
        /// </summary>
        /// <param name="src"></param>
        internal DatabaseFieldSpecialSettingDefinition(ReadOnlyDatabaseFieldSpecialSettingDefinition src)
            : this((IDatabaseFieldSpecialSettingDefinition)src.MutableInstance.DeepClone())
        {
        }

        /// <summary>
        ///     編集可能クラスの DeepClone メソッド専用のコピーコンストラクタ
        /// </summary>
        /// <param name="src"></param>
        private DatabaseFieldSpecialSettingDefinition(DatabaseFieldSpecialSettingDefinition src)
            : this(
                new Func<IDatabaseFieldSpecialSettingDefinition>(() =>
                    {
                        return src.Impl switch
                        {
                            DatabaseFieldSpecialSettingDefinitionNormal normal
                                => normal.DeepClone(),
                            DatabaseFieldSpecialSettingDefinitionLoadFile loadFile
                                => loadFile.DeepClone(),
                            DatabaseFieldSpecialSettingDefinitionDatabaseReference databaseReference =>
                                databaseReference.DeepClone(),
                            DatabaseFieldSpecialSettingDefinitionManual manual
                                => manual.DeepClone(),
                            _ => throw new InvalidOperationException(),
                        };
                    }
                )()
            )
        {
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        /// <summary>
        ///     すべての選択肢リストを取得する。
        /// </summary>
        [ImmutableMethod]
        [Pure]
        public IEnumerable<DatabaseValueCase> GetSpecialCases()
            => Impl.GetSpecialCases();

        /// <summary>
        ///     指定した値種別が設定可能かどうかを判定する。
        /// </summary>
        /// <param name="type">値種別</param>
        /// <returns>設定可能な場合 <see langword="true"/>。</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="type"/> が <see langword="null"/> の場合。
        /// </exception>
        [ImmutableMethod]
        [Pure]
        public bool CanChangeFieldType(DatabaseFieldType type)
            => Impl.CanChangeFieldType(type);

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IReadOnlyDatabaseFieldSpecialSettingDefinition? other)
            => Impl.ItemEquals(other);

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IDatabaseFieldSpecialSettingDefinitionSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Impl.ItemEquals(other);
        }

        /// <inheritdoc/>
        [Pure]
        public bool TryCastNormalSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionNormalSettings? result
        ) => Impl.TryCastNormalSettings(out result);

        /// <inheritdoc/>
        [Pure]
        public bool TryCastLoadFileSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionLoadFileSettings? result
        ) => Impl.TryCastLoadFileSettings(out result);

        /// <inheritdoc/>
        [Pure]
        public bool TryCastDatabaseReferenceSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings? result
        ) => Impl.TryCastDatabaseReferenceSettings(out result);

        /// <inheritdoc/>
        [Pure]
        public bool TryCastManualSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionManualSettings? result
        ) => Impl.TryCastManualSettings(out result);

        /// <inheritdoc/>
        [Pure]
        public bool TryCastNormal(
            [NotNullWhen(true)] out DatabaseFieldSpecialSettingDefinitionNormal? result
        ) => Impl.TryCastNormal(out result);

        /// <inheritdoc/>
        [Pure]
        public bool TryCastLoadFile(
            [NotNullWhen(true)] out DatabaseFieldSpecialSettingDefinitionLoadFile? result
        ) => Impl.TryCastLoadFile(out result);

        /// <inheritdoc/>
        [Pure]
        public bool TryCastDatabaseReference(
            [NotNullWhen(true)] out DatabaseFieldSpecialSettingDefinitionDatabaseReference? result
        ) => Impl.TryCastDatabaseReference(out result);

        /// <inheritdoc/>
        [Pure]
        public bool TryCastManual(
            [NotNullWhen(true)] out DatabaseFieldSpecialSettingDefinitionManual? result
        ) => Impl.TryCastManual(out result);

        [Pure]
        bool IReadOnlyDatabaseFieldSpecialSettingDefinition.
            TryCastNormal(
                [NotNullWhen(true)] out ReadOnlyDatabaseFieldSpecialSettingDefinitionNormal? result
            )
        {
            var resultValue = TryCastNormal(out var resultMutable);
            result = resultMutable;
            return resultValue;
        }

        [Pure]
        bool IReadOnlyDatabaseFieldSpecialSettingDefinition.
            TryCastLoadFile(
                [NotNullWhen(true)] out ReadOnlyDatabaseFieldSpecialSettingDefinitionLoadFile? result
            )
        {
            var resultValue = TryCastLoadFile(out var resultMutable);
            result = resultMutable;
            return resultValue;
        }

        [Pure]
        bool IReadOnlyDatabaseFieldSpecialSettingDefinition.
            TryCastDatabaseReference(
                [NotNullWhen(true)] out ReadOnlyDatabaseFieldSpecialSettingDefinitionDatabaseReference? result
            )
        {
            var resultValue = TryCastDatabaseReference(out var resultMutable);
            result = resultMutable;
            return resultValue;
        }

        [Pure]
        bool IReadOnlyDatabaseFieldSpecialSettingDefinition.
            TryCastManual(
                [NotNullWhen(true)] out ReadOnlyDatabaseFieldSpecialSettingDefinitionManual? result
            )
        {
            var resultValue = TryCastManual(out var resultMutable);
            result = resultMutable;
            return resultValue;
        }

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Operators

        #region From

        /// <summary>
        ///     <see cref="DatabaseFieldSpecialSettingDefinitionNormal"/> からの暗黙的型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        [return: NotNullIfNotNull(nameof(src))]
        public static implicit operator DatabaseFieldSpecialSettingDefinition?(
            DatabaseFieldSpecialSettingDefinitionNormal? src
        )
            => src is null
                ? null
                : new DatabaseFieldSpecialSettingDefinition((IDatabaseFieldSpecialSettingDefinition)src);

        /// <summary>
        ///     <see cref="DatabaseFieldSpecialSettingDefinitionLoadFile"/> からの暗黙的型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        [return: NotNullIfNotNull(nameof(src))]
        public static implicit operator DatabaseFieldSpecialSettingDefinition?(
            DatabaseFieldSpecialSettingDefinitionLoadFile? src
        )
            => src is null
                ? null
                : new DatabaseFieldSpecialSettingDefinition((IDatabaseFieldSpecialSettingDefinition)src);

        /// <summary>
        ///     <see cref="DatabaseFieldSpecialSettingDefinitionDatabaseReference"/> からの暗黙的型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        [return: NotNullIfNotNull(nameof(src))]
        public static implicit operator DatabaseFieldSpecialSettingDefinition?(
            DatabaseFieldSpecialSettingDefinitionDatabaseReference? src
        )
            => src is null
                ? null
                : new DatabaseFieldSpecialSettingDefinition((IDatabaseFieldSpecialSettingDefinition)src);

        /// <summary>
        ///     <see cref="DatabaseFieldSpecialSettingDefinitionManual"/> からの暗黙的型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        [return: NotNullIfNotNull(nameof(src))]
        public static implicit operator DatabaseFieldSpecialSettingDefinition?(
            DatabaseFieldSpecialSettingDefinitionManual? src
        )
            => src is null
                ? null
                : new DatabaseFieldSpecialSettingDefinition((IDatabaseFieldSpecialSettingDefinition)src);

        #endregion

        #endregion
    }

    public partial class ReadOnlyDatabaseFieldSpecialSettingDefinition : IReadOnlyDatabaseFieldSpecialSettingDefinition
    {
        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IReadOnlyDatabaseFieldSpecialSettingDefinition? other)
            => MutableInstance.ItemEquals(other);

        /// <inheritdoc/>
        [Pure]
        public bool TryCastNormalSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionNormalSettings? result
        ) => MutableInstance.TryCastNormalSettings(out result);

        /// <inheritdoc/>
        [Pure]
        public bool TryCastLoadFileSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionLoadFileSettings? result
        ) => MutableInstance.TryCastLoadFileSettings(out result);

        /// <inheritdoc/>
        [Pure]
        public bool TryCastDatabaseReferenceSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings? result
        ) => MutableInstance.TryCastDatabaseReferenceSettings(out result);

        /// <inheritdoc/>
        [Pure]
        public bool TryCastManualSettings(
            [NotNullWhen(true)] out IDatabaseFieldSpecialSettingDefinitionManualSettings? result
        ) => MutableInstance.TryCastManualSettings(out result);

        /// <inheritdoc/>
        [Pure]
        public bool TryCastNormal(
            [NotNullWhen(true)] out ReadOnlyDatabaseFieldSpecialSettingDefinitionNormal? result
        ) => ((IReadOnlyDatabaseFieldSpecialSettingDefinition)MutableInstance).TryCastNormal(out result);

        /// <inheritdoc/>
        [Pure]
        public bool TryCastLoadFile(
            [NotNullWhen(true)] out ReadOnlyDatabaseFieldSpecialSettingDefinitionLoadFile? result
        ) => ((IReadOnlyDatabaseFieldSpecialSettingDefinition)MutableInstance).TryCastLoadFile(out result);

        /// <inheritdoc/>
        [Pure]
        public bool TryCastDatabaseReference(
            [NotNullWhen(true)] out ReadOnlyDatabaseFieldSpecialSettingDefinitionDatabaseReference? result
        ) => ((IReadOnlyDatabaseFieldSpecialSettingDefinition)MutableInstance).TryCastDatabaseReference(out result);

        /// <inheritdoc/>
        [Pure]
        public bool TryCastManual(
            [NotNullWhen(true)] out ReadOnlyDatabaseFieldSpecialSettingDefinitionManual? result
        ) => ((IReadOnlyDatabaseFieldSpecialSettingDefinition)MutableInstance).TryCastManual(out result);
    }
}
