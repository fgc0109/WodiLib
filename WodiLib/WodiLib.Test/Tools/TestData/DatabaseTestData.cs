using System.Collections.Generic;
using System.Linq;
using WodiLib.Database;
using WodiLib.Sys;

namespace WodiLib.Test.Tools.TestData
{
    internal static class DatabaseTestData
    {
        #region DatabaseDataNamingDefinition

        public static DatabaseDataNamingDefinition CreateDatabaseDataNamingDefinitionType1(
            int typeId = -1,
            DatabaseKind? dBKind = null
        )
        {
            return DatabaseDataNamingDefinition.BuildDesignatedType(
                dBKind ?? DatabaseKind.Changeable,
                typeId == -1
                    ? new TypeId(99)
                    : new TypeId(typeId % 100)
            );
        }

        public static DatabaseDataNamingDefinition CreateDatabaseDataNamingDefinitionType2()
        {
            return DatabaseDataNamingDefinition.BuildFirstStringData();
        }

        #endregion

        #region DatabaseDataTableWithDataNamingDefinitionList

        public static DatabaseDataTableWithDataNamingDefinitionList
            CreateDatabaseDataTableWithDataNamingDefinitionListType1(int typeLength = 3, DatabaseKind? dbKind = null)
        {
            return new DatabaseDataTableWithDataNamingDefinitionList(
                CreateDatabaseDataTableWithDataNamingDefinitionListSettingsType1(typeLength, dbKind)
            );
        }

        public static DatabaseDataTableWithDataNamingDefinitionList
            CreateDatabaseDataTableWithDataNamingDefinitionListType2()
        {
            return new DatabaseDataTableWithDataNamingDefinitionList(
                CreateDatabaseDataTableWithDataNamingDefinitionListSettingsType2()
            );
        }

        #endregion

        #region DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings

        public static DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
            CreateDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettingsType1(
                TypeId? typeId = null,
                DatabaseReferType? databaseReferType = null
            )
        {
            return new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
            {
                DatabaseDbTypeId = typeId ?? 99,
                DatabaseReferKind = databaseReferType ?? DatabaseReferType.System,
            };
        }

        public static DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
            CreateDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettingsType2()
        {
            return new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
            {
                DatabaseDbTypeId = 21,
                DatabaseReferKind = DatabaseReferType.System,
                AdditionalCase1 = "Case1",
                AdditionalCase2 = "Case2",
                AdditionalCase3 = "Case3",
            };
        }

        #endregion

        #region DatabaseFieldSpecialSettingDefinitionSettings

        public static DatabaseFieldSpecialSettingDefinitionSettings
            CreateDatabaseFieldSpecialSettingDefinitionSettingsType1(
                DatabaseValueInt? initValue = null
            )
        {
            return new DatabaseFieldSpecialSettingDefinitionSettings(
                new DatabaseFieldSpecialSettingDefinitionNormalSettings
                {
                    InitValue = initValue ?? new DatabaseValueInt(65535),
                }
            );
        }

        public static DatabaseFieldSpecialSettingDefinitionSettings
            CreateDatabaseFieldSpecialSettingDefinitionSettingsType2()
        {
            return new DatabaseFieldSpecialSettingDefinitionSettings(
                new DatabaseFieldSpecialSettingDefinitionNormalSettings()
            );
        }

        #endregion

        #region DatabaseNamedDataTable

        public static DatabaseNamedDataTable CreateDatabaseNamedDataTableType1(
            int dataLength = 1,
            int fieldLength = 1
        )
        {
            return new DatabaseNamedDataTable(
                new DatabaseNamedDataTableSettings(
                    dataLength.Iterate(r => CreateDatabaseNamedDataRowSettingsType1(r, fieldLength)).ToArray()
                )
            );
        }

        public static DatabaseNamedDataTable CreateDatabaseNamedDataTableType2()
        {
            return new DatabaseNamedDataTable(
                new DatabaseNamedDataTableSettings(
                    2.Iterate(_ => CreateDatabaseNamedDataRowSettingsType2()).ToArray()
                )
            );
        }

        #endregion

        #region DatabaseProjectTypeList

        public static DatabaseProjectTypeList CreateDatabaseProjectTypeListType1()
        {
            return new DatabaseProjectTypeList(
                CreateDatabaseProjectTypeListSettingsType1()
            );
        }

        public static DatabaseProjectTypeList CreateDatabaseProjectTypeListType2()
        {
            return new DatabaseProjectTypeList(
                CreateDatabaseProjectTypeListSettingsType2()
            );
        }

        #endregion

        #region DatabaseTypeDefinition

        public static DatabaseTypeDefinition CreateDatabaseTypeDefinitionType1(int typeId = 0)
        {
            return new DatabaseTypeDefinition(CreateDatabaseTypeDefinitionSettingsType1(typeId));
        }

        public static DatabaseTypeDefinition CreateDatabaseTypeDefinitionType2()
        {
            return new DatabaseTypeDefinition(CreateDatabaseTypeDefinitionSettingsType2());
        }

        #endregion

        #region DatabaseTypeMetadataTable

        public static DatabaseTypeMetadataTable CreateDatabaseTypeMetadataTableType1(
            int typeId = 0,
            DatabaseKind? dbKind = null
        )
        {
            return new DatabaseTypeMetadataTable(
                CreateTypeMetadataTableSettingsType1(typeId, dbKind)
            );
        }

        public static DatabaseTypeMetadataTable CreateDatabaseTypeMetadataTableType2()
        {
            return new DatabaseTypeMetadataTable(
                CreateTypeMetadataTableSettingsType2()
            );
        }

        #endregion

        #region IDatabaseDataNameListSettings

        public static IDatabaseDataNameListSettings CreateDatabaseDataNameListSettingsType1(int dataLength = 1)
        {
            return new DatabaseDataNameListSettings(
                dataLength.Iterate(i => new DataName($"DataName_{i}")).ToArray()
            );
        }

        public static IDatabaseDataNameListSettings CreateDatabaseDataNameListSettingsType2()
        {
            return new DatabaseDataNameListSettings(
                3.Iterate(i => new DataName($"Diff DataName_{i}")).ToArray()
            );
        }

        #endregion

        #region IDatabaseDataRowSettings

        public static IDatabaseDataRowSettings CreateDatabaseDataRowSettingsType1(int value = 0)
        {
            return new DatabaseDataRowSettings(
                new List<DatabaseFieldValue>
                {
                    new(value),
                }
            );
        }

        public static IDatabaseDataRowSettings CreateDatabaseDataRowSettingsType2(string value = "0")
        {
            return new DatabaseDataRowSettings(
                new List<DatabaseFieldValue>
                {
                    new(value),
                }
            );
        }

        #endregion

        #region IDatabaseDataTableSettings

        public static IDatabaseDataTableSettings CreateDatabaseDataTableSettingsType1(int value = 0)
        {
            return new DatabaseDataTableSettings(
                new List<IDatabaseDataRowSettings>
                {
                    CreateDatabaseDataRowSettingsType1(value),
                }
            );
        }

        public static IDatabaseDataTableSettings CreateDatabaseDataTableSettingsType2(string value = "0")
        {
            return new DatabaseDataTableSettings(
                new List<IDatabaseDataRowSettings>
                {
                    CreateDatabaseDataRowSettingsType2(value),
                }
            );
        }

        #endregion

        #region IDatabaseDataTableWithDataNamingDefinitionListSettings

        public static IDatabaseDataTableWithDataNamingDefinitionListSettings
            CreateDatabaseDataTableWithDataNamingDefinitionListSettingsType1(
                int typeLength = 4,
                DatabaseKind? dbKind = null
            )
        {
            return new DatabaseDataTableWithDataNamingDefinitionListSettings(
                typeLength.Iterate(typeId
                        => CreateDatabaseDataTableWithDataNamingDefinitionSettingsType1(typeId, dbKind)
                    )
                    .ToArray()
            );
        }

        public static IDatabaseDataTableWithDataNamingDefinitionListSettings
            CreateDatabaseDataTableWithDataNamingDefinitionListSettingsType2()
        {
            return new DatabaseDataTableWithDataNamingDefinitionListSettings(
                new[]
                {
                    CreateDatabaseDataTableWithDataNamingDefinitionSettingsType2(),
                }
            );
        }

        #endregion

        #region IDatabaseDataTableWithDataNamingDefinitionSettings

        public static IDatabaseDataTableWithDataNamingDefinitionSettings
            CreateDatabaseDataTableWithDataNamingDefinitionSettingsType1(
                int typeId = -1,
                DatabaseKind? dBKind = null
            )
        {
            return new DatabaseDataTableWithDataNamingDefinitionSettings
            {
                DataNamingDefinition = CreateDatabaseDataNamingDefinitionType1(typeId, dBKind),
            };
        }

        public static IDatabaseDataTableWithDataNamingDefinitionSettings
            CreateDatabaseDataTableWithDataNamingDefinitionSettingsType2()
        {
            return new DatabaseDataTableWithDataNamingDefinitionSettings
            {
                DataNamingDefinition = CreateDatabaseDataNamingDefinitionType2(),
            };
        }

        #endregion

        #region IDatabaseFieldDefinitionListSettings

        public static IDatabaseFieldDefinitionListSettings CreateDatabaseFieldDefinitionListSettingsType1(
            int fieldLength = 0
        )
        {
            return new DatabaseFieldDefinitionListSettings(
                fieldLength.Iterate(i => CreateDatabaseFieldDefinitionSettingsType1(i)).ToArray()
            );
        }

        public static IDatabaseFieldDefinitionListSettings CreateDatabaseFieldDefinitionListSettingsType2()
        {
            return new DatabaseFieldDefinitionListSettings(
                new[] { CreateDatabaseFieldDefinitionSettingsType2() }
            );
        }

        #endregion

        #region IDatabaseFieldDefinitionSettings

        public static IDatabaseFieldDefinitionSettings CreateDatabaseFieldDefinitionSettingsType1(
            int fieldId = 0
        )
        {
            return new DatabaseFieldDefinitionSettings
            {
                FieldType = DatabaseFieldType.Int,
                FieldName = $"Field_{fieldId}",
            };
        }

        public static IDatabaseFieldDefinitionSettings CreateDatabaseFieldDefinitionSettingsType2()
        {
            return new DatabaseFieldDefinitionSettings
            {
                FieldType = DatabaseFieldType.String,
                FieldName = $"TestField",
            };
        }

        #endregion

        #region IDatabaseFieldMetadataListSettings

        public static IDatabaseFieldMetadataListSettings CreateDatabaseFieldMetadataListSettingsType1(
            int fieldLength = 0
        )
        {
            return new DatabaseFieldMetadataListSettings(
                fieldLength.Iterate(i => CreateDatabaseFieldMetadataSettingsType1(i)).ToArray()
            );
        }

        public static IDatabaseFieldMetadataListSettings CreateDatabaseFieldMetadataListSettingsType2()
        {
            return new DatabaseFieldMetadataListSettings(
                new[] { CreateDatabaseFieldMetadataSettingsType2() }
            );
        }

        #endregion

        #region IDatabaseFieldMetadataSettings

        public static IDatabaseFieldMetadataSettings CreateDatabaseFieldMetadataSettingsType1(
            int fieldId = 0
        )
        {
            return new DatabaseFieldMetadataSettings
            {
                FieldName = $"Field_{fieldId}",
            };
        }

        public static IDatabaseFieldMetadataSettings CreateDatabaseFieldMetadataSettingsType2()
        {
            return new DatabaseFieldMetadataSettings
            {
                FieldName = $"TestField",
            };
        }

        #endregion

        #region DatabaseFieldSpecialSettingDefinition

        public static DatabaseFieldSpecialSettingDefinition CreateDatabaseFieldSpecialSettingDefinitionType1()
        {
            return new DatabaseFieldSpecialSettingDefinitionLoadFile();
        }

        public static DatabaseFieldSpecialSettingDefinition CreateDatabaseFieldSpecialSettingDefinitionType2()
        {
            return new DatabaseFieldSpecialSettingDefinitionNormal();
        }

        #endregion

        #region IDatabaseFieldValueListSettings

        public static IDatabaseFieldValueListSettings CreateDatabaseFieldValueListSettingsType1()
        {
            return new DatabaseFieldValueListSettings(
                new[] { new DatabaseFieldValue(0) }
            )
            {
                FieldType = DatabaseFieldType.Int,
            };
        }

        public static IDatabaseFieldValueListSettings CreateDatabaseFieldValueListSettingsType2()
        {
            return new DatabaseFieldValueListSettings(
                new[] { new DatabaseFieldValue("0") }
            )
            {
                FieldType = DatabaseFieldType.String,
            };
        }

        #endregion

        #region IDatabaseNamedDataRowSettings

        public static IDatabaseNamedDataRowSettings CreateDatabaseNamedDataRowSettingsType1(
            int rowIndex = 0,
            int fieldLength = 0
        )
        {
            return new DatabaseNamedDataRowSettings(
                fieldLength.Iterate(c => new DatabaseFieldValue($"{rowIndex}_{c}")).ToArray()
            );
        }

        public static IDatabaseNamedDataRowSettings CreateDatabaseNamedDataRowSettingsType2()
        {
            return new DatabaseNamedDataRowSettings(
                3.Iterate(c => new DatabaseFieldValue($"FieldValue {c}")).ToArray()
            );
        }

        #endregion

        #region IDatabaseProjectTypeListSettings

        public static IDatabaseProjectTypeListSettings CreateDatabaseProjectTypeListSettingsType1()
        {
            return new DatabaseProjectTypeListSettings(
                new[] { CreateDatabaseProjectTypeSettingsType1() }
            );
        }

        public static IDatabaseProjectTypeListSettings CreateDatabaseProjectTypeListSettingsType2()
        {
            return new DatabaseProjectTypeListSettings(
                new[] { CreateDatabaseProjectTypeSettingsType2() }
            );
        }

        #endregion

        #region IDatabaseProjectTypeSettings

        public static IDatabaseProjectTypeSettings CreateDatabaseProjectTypeSettingsType1()
        {
            return new DatabaseProjectTypeSettings
            {
                TypeName = "Type 1",
            };
        }

        public static IDatabaseProjectTypeSettings CreateDatabaseProjectTypeSettingsType2()
        {
            return new DatabaseProjectTypeSettings
            {
                TypeName = "Type 2",
            };
        }

        #endregion

        #region IDatabaseTypeDefinitionSettings

        public static IDatabaseTypeDefinitionSettings CreateDatabaseTypeDefinitionSettingsType1(int typeId = 0)
        {
            return new DatabaseTypeDefinitionSettings
            {
                TypeName = $"Type_{typeId}",
            };
        }

        public static IDatabaseTypeDefinitionSettings CreateDatabaseTypeDefinitionSettingsType2()
        {
            return new DatabaseTypeDefinitionSettings
            {
                TypeName = $"Diff Type",
                Memo = "Diff DB Memo",
            };
        }

        #endregion

        #region IDatabaseTypeTableSettings

        public static IDatabaseTypeTableSettings CreateTypeTableSettingsType1(
            int typeId = -1,
            DatabaseKind? dBKind = null
        )
        {
            return new DatabaseTypeTableSettings
            {
                DataNamingDefinition = CreateDatabaseDataNamingDefinitionType1(typeId, dBKind),
            };
        }

        public static IDatabaseTypeTableSettings CreateTypeTableSettingsType2()
        {
            return new DatabaseTypeTableSettings
            {
                DataNamingDefinition = CreateDatabaseDataNamingDefinitionType2(),
            };
        }

        #endregion

        #region IDatabaseTypeMetadataTableSettings

        public static IDatabaseTypeMetadataTableSettings CreateTypeMetadataTableSettingsType1(
            int typeId = -1,
            DatabaseKind? dBKind = null
        )
        {
            return new DatabaseTypeMetadataTableSettings
            {
                DataNamingDefinition = CreateDatabaseDataNamingDefinitionType1(typeId, dBKind),
            };
        }

        public static IDatabaseTypeMetadataTableSettings CreateTypeMetadataTableSettingsType2()
        {
            return new DatabaseTypeMetadataTableSettings
            {
                DataNamingDefinition = CreateDatabaseDataNamingDefinitionType2(),
            };
        }

        #endregion

        #region IDatabaseTypeTableListSettings

        public static IDatabaseTypeTableListSettings CreateTypeTableListSettingsType1()
        {
            return new DatabaseTypeTableListSettings(
                new List<IDatabaseTypeTableSettings>
                {
                    CreateTypeTableSettingsType1(),
                }
            );
        }

        public static IDatabaseTypeTableListSettings CreateTypeTableListSettingsType2()
        {
            return new DatabaseTypeTableListSettings(
                new List<IDatabaseTypeTableSettings>
                {
                    CreateTypeTableSettingsType2(),
                }
            );
        }

        #endregion

        #region IDatabaseValueCaseListSettings

        public static IDatabaseValueCaseListSettings CreateValueCaseListSettingsType1()
        {
            return new DatabaseValueCaseListSettings(
                new[]
                {
                    new DatabaseValueCase(0, "Case 0"),
                    new DatabaseValueCase(1, "Case 1"),
                }
            );
        }

        public static IDatabaseValueCaseListSettings CreateValueCaseListSettingsType2()
        {
            return new DatabaseValueCaseListSettings(
                new[]
                {
                    new DatabaseValueCase(100, "Case 100"),
                    new DatabaseValueCase(101, "Case 101"),
                }
            );
        }

        #endregion
    }
}
