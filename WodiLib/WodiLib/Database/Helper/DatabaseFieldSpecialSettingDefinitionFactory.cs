// ========================================
// Project Name : WodiLib
// File Name    : SpecialDataSpecificationFactory.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     DB項目特殊指定情報Factory
    /// </summary>
    public static class DatabaseFieldSpecialSettingDefinitionFactory
    {
        /// <summary>
        ///     設定DTOのユニオンからDB項目特殊指定情報を作成する。
        /// </summary>
        /// <param name="settings">設定DTOユニオン</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     設定DTOに不適切な <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        public static IDatabaseFieldSpecialSettingDefinition Create(
            DatabaseFieldSpecialSettingDefinitionSettingsUnion settings
        )
        {
            ThrowHelper.ValidateArgumentNotNull(settings is null, nameof(settings));

            if (settings.DtoType == DatabaseFieldSpecialSettingType.Normal)
            {
                return new DatabaseFieldSpecialSettingDefinitionNormal(settings.AsNormalSettings());
            }

            if (settings.DtoType == DatabaseFieldSpecialSettingType.LoadFile)
            {
                return new DatabaseFieldSpecialSettingDefinitionLoadFile(settings.AsLoadFileSettings());
            }

            if (settings.DtoType == DatabaseFieldSpecialSettingType.ReferDatabase)
            {
                return new DatabaseFieldSpecialSettingDefinitionDatabaseReference(
                    settings.AsDatabaseReferenceSettings()
                );
            }

            if (settings.DtoType == DatabaseFieldSpecialSettingType.Manual)
            {
                return new DatabaseFieldSpecialSettingDefinitionManual(settings.AsManualSettings());
            }

            throw new ArgumentException($"{settings.DtoType}のファクトリメソッドが未実装です。");
        }

        /// <summary>
        ///     特殊引数タイプと引数からDB項目特殊指定情報を作成する。
        /// </summary>
        /// <param name="type">特殊引数タイプ</param>
        /// <param name="cases">選択肢と文字列リスト</param>
        /// <returns>DB項目特殊指定情報</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="type"/> が <see langword="null"/> の場合、
        ///     または <paramref name="cases"/> が <see langword="null"/> ではなく
        ///     <paramref name="cases"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static IDatabaseFieldSpecialSettingDefinition Create(
            DatabaseFieldSpecialSettingType type,
            IReadOnlyCollection<DatabaseValueCase>? cases
        )
        {
            ThrowHelper.ValidateArgumentNotNull(type is null, nameof(type));

            if (cases is not null)
            {
                ThrowHelper.ValidateArgumentItemsHasNotNull(cases.HasNullItem(), nameof(cases));
            }

            if (type == DatabaseFieldSpecialSettingType.Normal)
            {
                return CreateNormal(cases);
            }

            if (type == DatabaseFieldSpecialSettingType.LoadFile)
            {
                return CreateLoadFile(cases);
            }

            if (type == DatabaseFieldSpecialSettingType.ReferDatabase)
            {
                return CreateReferDatabase(cases);
            }

            if (type == DatabaseFieldSpecialSettingType.Manual)
            {
                return CreateManual(cases);
            }

            throw new ArgumentException($"{nameof(type)}のファクトリメソッドが未実装です。");
        }

        /// <summary>
        ///     「特殊な設定方法を使用しない」のインスタンスを生成する。
        /// </summary>
        /// <param name="cases">引数と文字列リスト</param>
        /// <returns>DB項目特殊指定情報</returns>
        public static DatabaseFieldSpecialSettingDefinitionNormal CreateNormal(
            IReadOnlyCollection<DatabaseValueCase>? cases
        )
        {
            return new DatabaseFieldSpecialSettingDefinitionNormal(
                new DatabaseFieldSpecialSettingDefinitionNormalSettings()
            );
        }

        /// <summary>
        ///     「ファイル読み込み」のインスタンスを生成する。
        /// </summary>
        /// <param name="cases">引数と文字列リスト</param>
        /// <returns>DB項目特殊指定情報</returns>
        /// <exception cref="ArgumentException">argCaseList.Countが1以外の場合</exception>
        public static DatabaseFieldSpecialSettingDefinitionLoadFile CreateLoadFile(
            IReadOnlyCollection<DatabaseValueCase>? cases
        )
        {
            if (cases is null)
            {
                return new DatabaseFieldSpecialSettingDefinitionLoadFile();
            }

            var argCaseArray = cases.ToArray();

            if (argCaseArray.Length != 1)
            {
                throw new ArgumentException(ErrorMessage.LengthRange(nameof(cases), 1, 1, argCaseArray.Length));
            }

            var infoSet = argCaseArray[0];
            const int caseNumberMin = 0;
            const int caseNumberMax = 1;
            if (!infoSet.CaseNumber.RawValue.IsBetween(caseNumberMin, caseNumberMax))
            {
                throw new ArgumentException(
                    ErrorMessage.OutOfRange(
                        nameof(infoSet.CaseNumber),
                        caseNumberMin,
                        caseNumberMax,
                        infoSet.CaseNumber
                    )
                );
            }


            return new DatabaseFieldSpecialSettingDefinitionLoadFile(
                new DatabaseFieldSpecialSettingDefinitionLoadFileSettings
                {
                    IsOmitFolderName = infoSet.CaseNumber == 1,
                    FolderName = infoSet.Description.RawValue,
                }
            );
        }

        /// <summary>
        ///     「データベース参照」のインスタンスを生成する。
        /// </summary>
        /// <param name="cases">引数と文字列リスト</param>
        /// <returns>DB項目特殊指定情報</returns>
        public static DatabaseFieldSpecialSettingDefinitionDatabaseReference CreateReferDatabase(
            IReadOnlyCollection<DatabaseValueCase>? cases
        )
        {
            var result = new DatabaseFieldSpecialSettingDefinitionDatabaseReference();

            var argCaseMinus1 = "";
            var argCaseMinus2 = "";
            var argCaseMinus3 = "";
            if (cases is not null)
            {
                result.IsUseAdditionalItems = true;

                var argCaseArray = cases.ToArray();
                argCaseMinus1 = argCaseArray.FirstOrDefault(argCase => argCase.CaseNumber == -1)?.Description ?? "";
                argCaseMinus2 = argCaseArray.FirstOrDefault(argCase => argCase.CaseNumber == -2)?.Description ?? "";
                argCaseMinus3 = argCaseArray.FirstOrDefault(argCase => argCase.CaseNumber == -3)?.Description ?? "";
            }

            result.UpdateAdditionalItem(-1, argCaseMinus1);
            result.UpdateAdditionalItem(-2, argCaseMinus2);
            result.UpdateAdditionalItem(-3, argCaseMinus3);

            return result;
        }

        /// <summary>
        ///     「選択肢手動生成」のインスタンスを生成する。
        /// </summary>
        /// <param name="cases">引数と文字列リスト</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="cases"/> が <see langword="null"/> ではなく
        ///     <paramref name="cases"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <returns>DB項目特殊指定情報</returns>
        public static DatabaseFieldSpecialSettingDefinitionManual CreateManual(
            IReadOnlyCollection<DatabaseValueCase>? cases
        )
        {
            if (cases is not null)
            {
                ThrowHelper.ValidateArgumentItemsHasNotNull(cases.HasNullItem(), nameof(cases));
            }

            return new DatabaseFieldSpecialSettingDefinitionManual(
                new DatabaseFieldSpecialSettingDefinitionManualSettings
                {
                    SpecialCases =
                        new DatabaseValueCaseListSettings(cases?.ToArray() ?? Array.Empty<DatabaseValueCase>()),
                }
            );
        }
    }
}
