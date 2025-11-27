// ========================================
// Project Name : WodiLib
// File Name    : DatabaseSchemaDivider.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     <see cref="ReadOnlyDatabaseSchema"/> から <see cref="DatabaseDataTableWithDataNamingDefinitionList"/> と
    ///     <see cref="DatabaseProjectTypeList"/> を取得するためのHelperクラス
    /// </summary>
    public static class DatabaseSchemaDivider
    {
        /// <summary>
        ///     <see cref="DatabaseTypeTableList"/> を <see cref="DatabaseDataTableWithDataNamingDefinitionList"/> と
        ///     <see cref="DatabaseProjectTypeList"/> に分割したインスタンスを取得する。
        /// </summary>
        /// <param name="src">処理対象</param>
        /// <returns>処理結果</returns>
        public static DivideResult Divide(ReadOnlyDatabaseSchema src)
        {
            ThrowHelper.ValidateArgumentNotNull(src is null, nameof(src));
            var dataTableList = ExtractDataTableWithDataNamingList(src);
            var typeList = ExtractProjectTypeList(src);

            return new DivideResult(dataTableList, typeList);
        }

        /// <summary>
        ///     <see cref="DatabaseTypeTableList"/> から <see cref="DatabaseDataTableWithDataNamingDefinitionList"/> を取得する。
        /// </summary>
        /// <param name="src">処理対象</param>
        /// <returns>処理結果</returns>
        public static DatabaseDataTableWithDataNamingDefinitionList ExtractDataTableWithDataNamingList(
            ReadOnlyDatabaseSchema src
        )
        {
            ThrowHelper.ValidateArgumentNotNull(src is null, nameof(src));

            var dataTableDefinitions = src.TypeTableList.Settings.Select(item =>
                    {
                        var dataRows = item.Settings.Select(namedRow
                                => new DatabaseDataRowSettings(namedRow.Settings) as IDatabaseDataRowSettings
                            )
                            .ToArray();

                        var dataTable = new DatabaseDataTableSettings(dataRows);

                        return new DatabaseDataTableWithDataNamingDefinitionSettings
                        {
                            DataNamingDefinition = item.DataNamingDefinition,
                            DataTable = dataTable,
                        } as IDatabaseDataTableWithDataNamingDefinitionSettings;
                    }
                )
                .ToArray();

            var settings = new DatabaseDataTableWithDataNamingDefinitionListSettings(dataTableDefinitions);

            return new DatabaseDataTableWithDataNamingDefinitionList(settings);
        }

        /// <summary>
        ///     <see cref="DatabaseTypeTableList"/> から <see cref="DatabaseProjectTypeList"/> を取得する。
        /// </summary>
        /// <param name="src">処理対象</param>
        /// <returns>処理結果</returns>
        public static DatabaseProjectTypeList ExtractProjectTypeList(
            ReadOnlyDatabaseSchema src
        )
        {
            ThrowHelper.ValidateArgumentNotNull(src is null, nameof(src));

            var dataNames = src.TypeTableList.Settings
                .Select(typeTableSettings => typeTableSettings.Settings.Select(row => row.DataName))
                .To2DArray();

            var settings = new DatabaseProjectTypeListSettings(
                src.TypeTableList.Settings.Select((row, i) => CreateProjectTypeSettings(row, dataNames[i]))
                    .ToArray()
            );
            return new DatabaseProjectTypeList(settings);
        }

        /// <summary>
        ///     プロジェクトタイプ設定を作成する。
        /// </summary>
        /// <param name="typeTableSettings">タイプテーブル設定</param>
        /// <param name="dataNames">データ名リスト</param>
        /// <returns>プロジェクトタイプ設定</returns>
        private static IDatabaseProjectTypeSettings CreateProjectTypeSettings(
            IDatabaseTypeTableSettings typeTableSettings,
            DataName[] dataNames
        )
        {
            return new DatabaseProjectTypeSettings
            {
                TypeName = typeTableSettings.TypeName,
                Memo = typeTableSettings.Memo,
                FieldMetadataList = typeTableSettings.FieldDefinitionList.TransformMetadataSettings(),
                DataNameList = new DatabaseDataNameListSettings(dataNames),
            };
        }

        /// <summary>
        ///     分割結果
        /// </summary>
        /// <param name="DataTableList">データテーブル一覧</param>
        /// <param name="TypeList">タイプ一覧</param>
        public record DivideResult(
            DatabaseDataTableWithDataNamingDefinitionList DataTableList,
            DatabaseProjectTypeList TypeList
        );
    }
}
