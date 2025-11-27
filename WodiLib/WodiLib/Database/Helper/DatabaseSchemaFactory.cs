// ========================================
// Project Name : WodiLib
// File Name    : DatabaseSchemaFactory.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     <see cref="DatabaseSchema"/> インスタンス作成処理実装
    /// </summary>
    public static class DatabaseSchemaFactory
    {
        /// <summary>
        ///     引数で受け取った情報から <see cref="DatabaseSchema"/> のインスタンスを作成する。
        /// </summary>
        /// <param name="dataTableList">DBテーブルデータ &amp; データ名の設定方法</param>
        /// <param name="projectTypeList">タイプ情報リスト</param>
        /// <param name="dbKind">DB種別</param>
        /// <returns><see cref="DatabaseSchema"/> インスタンス</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="dataTableList"/>, <paramref name="projectTypeList"/> が
        ///     <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="dataTableList"/>, <paramref name="projectTypeList"/> のタイプ数が異なる場合、
        ///     またはいずれかのタイプについてデータ数が異なる場合、
        ///     またはいずれかのデータについて項目数が異なる場合。
        /// </exception>
        public static DatabaseSchema CreateMerged(
            DatabaseDataTableWithDataNamingDefinitionList dataTableList,
            DatabaseProjectTypeList projectTypeList,
            DatabaseKind? dbKind
        )
        {
            ThrowHelper.ValidateArgumentNotNull(dataTableList is null, nameof(dataTableList));
            ThrowHelper.ValidateArgumentNotNull(projectTypeList is null, nameof(projectTypeList));
            ValidateMargeSource(dataTableList, projectTypeList);

            return CreateMergedInternal(dataTableList, projectTypeList, dbKind);
        }

        private static void ValidateMargeSource(
            DatabaseDataTableWithDataNamingDefinitionList dataTableList,
            DatabaseProjectTypeList projectTypeList
        )
        {
            // タイプ数チェック
            if (dataTableList.Count != projectTypeList.Count)
            {
                throw new ArgumentException(
                    ErrorMessage.NotMatch(
                        $"{nameof(dataTableList)}のタイプ数 ({dataTableList.Count})",
                        $"{nameof(projectTypeList)}のタイプ数 ({projectTypeList.Count})"
                    )
                );
            }

            // 各タイプチェック
            for (var typeId = 0; typeId < dataTableList.Count; typeId++)
            {
                var dataTable = dataTableList[typeId].DataTable;
                var projectType = projectTypeList[typeId];

                // データ数チェック
                if (dataTable.DataCount != projectType.DataCount)
                {
                    throw new ArgumentException(
                        ErrorMessage.NotMatch(
                            $"{nameof(dataTableList)} typeId = {typeId} のデータ数 ({dataTable.DataCount})",
                            $"{nameof(projectTypeList)} typeId = {typeId} のデータ数 ({projectType.DataCount})"
                        )
                    );
                }

                // 項目数チェック
                if (dataTable.FieldCount != projectType.FieldCount)
                {
                    throw new ArgumentException(
                        ErrorMessage.NotMatch(
                            $"{nameof(dataTableList)} typeId = {typeId} の項目数 ({dataTable.FieldCount})",
                            $"{nameof(projectTypeList)} typeId = {typeId} の項目数 ({projectType.FieldCount})"
                        )
                    );
                }
            }
        }

        private static DatabaseSchema CreateMergedInternal(
            DatabaseDataTableWithDataNamingDefinitionList dataTableList,
            DatabaseProjectTypeList projectTypeList,
            DatabaseKind? dbKind
        )
        {
            var typeTableSettingsList = new List<IDatabaseTypeTableSettings>();

            // 1タイプづつ作成
            for (var typeId = 0; typeId < dataTableList.Count; typeId++)
            {
                var dataTableWithDataNamingDefinition = dataTableList[typeId];
                var dataTable = dataTableWithDataNamingDefinition.DataTable;
                var projectType = projectTypeList[typeId];

                var fieldTypes = dataTable.GetFieldTypes().ToArray();

                var rowSettingsList = new List<IDatabaseNamedDataRowSettings>();

                // 1行ずつ作成
                for (var dataId = 0; dataId < dataTable.DataCount; dataId++)
                {
                    var rowSettings = new DatabaseNamedDataRowSettings(dataTable.GetDataInternal(dataId).Settings)
                    {
                        DataName = projectType.DataNameList[dataId],
                    };
                    rowSettingsList.Add(rowSettings);
                }

                var typeTableSettings = new DatabaseTypeTableSettings(rowSettingsList)
                {
                    DataNamingDefinition = dataTableWithDataNamingDefinition.DataNamingDefinition,
                    FieldDefinitionList = new DatabaseFieldDefinitionListSettings(
                        projectType.FieldMetadataList
                            .Select((metadata, i) => metadata.TransformMetadataSettings(fieldTypes[i]))
                            .ToArray()
                    ),
                    TypeName = projectType.TypeName,
                    Memo = projectType.Memo,
                };

                typeTableSettingsList.Add(typeTableSettings);
            }

            var typeTableListSettings = new DatabaseTypeTableListSettings(typeTableSettingsList);
            var settings = new DatabaseSchemaSettings
            {
                DbKind = dbKind,
                TypeTableList = typeTableListSettings,
            };
            return new DatabaseSchema(settings);
        }
    }
}
