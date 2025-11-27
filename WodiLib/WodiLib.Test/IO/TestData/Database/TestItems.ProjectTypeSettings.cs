using System.Collections.Generic;
using WodiLib.Database;

namespace WodiLib.Test.IO.TestData.Database
{
    internal static partial class TestItems
    {
        internal static class DatabaseProjectTypeSettings
        {
            public static readonly IDatabaseProjectTypeSettings Udb_Type0;
            public static readonly IDatabaseProjectTypeSettings Udb_Type1;
            public static readonly IDatabaseProjectTypeSettings Udb_Type2;
            public static readonly IDatabaseProjectTypeSettings Udb_Type3;

            public static readonly IDatabaseProjectTypeSettings Cdb_Type0;
            public static readonly IDatabaseProjectTypeSettings Cdb_Type1;

            static DatabaseProjectTypeSettings()
            {
                Udb_Type0 = new WodiLib.Database.DatabaseProjectTypeSettings
                {
                    TypeName = "UDB0",
                    Memo = "",
                    DataNameList = new DatabaseDataNameListSettings(
                        new List<DataName>
                        {
                            DatabaseDataName.Udb_Type0_Data0,
                            DatabaseDataName.Udb_Type0_Data1,
                            DatabaseDataName.Udb_Type0_Data2,
                            DatabaseDataName.Udb_Type0_Data3,
                        }
                    ),
                    FieldMetadataList = new DatabaseFieldMetadataListSettings(
                        new List<IDatabaseFieldMetadataSettings>
                        {
                            DatabaseFieldDefinitionSettings.Udb_Type0_Field0.TransformMetadataSettings(),
                            DatabaseFieldDefinitionSettings.Udb_Type0_Field1.TransformMetadataSettings(),
                            DatabaseFieldDefinitionSettings.Udb_Type0_Field2.TransformMetadataSettings(),
                            DatabaseFieldDefinitionSettings.Udb_Type0_Field3.TransformMetadataSettings(),
                            DatabaseFieldDefinitionSettings.Udb_Type0_Field4.TransformMetadataSettings(),
                            DatabaseFieldDefinitionSettings.Udb_Type0_Field5.TransformMetadataSettings(),
                            DatabaseFieldDefinitionSettings.Udb_Type0_Field6.TransformMetadataSettings(),
                        }
                    ),
                };
                Udb_Type1 = new WodiLib.Database.DatabaseProjectTypeSettings
                {
                    TypeName = "",
                    Memo = "",
                    DataNameList = new DatabaseDataNameListSettings(
                        new List<DataName>
                        {
                            DatabaseDataName.Udb_Type1_Data0,
                        }
                    ),
                    FieldMetadataList = new DatabaseFieldMetadataListSettings(),
                };
                Udb_Type2 = new WodiLib.Database.DatabaseProjectTypeSettings
                {
                    TypeName = "ゆーでーびーつー",
                    Memo = "UDB2メモ欄\r\n改行",
                    DataNameList = new DatabaseDataNameListSettings(
                        new List<DataName>
                        {
                            DatabaseDataName.Udb_Type2_Data0,
                            DatabaseDataName.Udb_Type2_Data1,
                        }
                    ),
                    FieldMetadataList = new DatabaseFieldMetadataListSettings(
                        new List<IDatabaseFieldMetadataSettings>
                        {
                            DatabaseFieldDefinitionSettings.Udb_Type2_Field0.TransformMetadataSettings(),
                            DatabaseFieldDefinitionSettings.Udb_Type2_Field1.TransformMetadataSettings(),
                            DatabaseFieldDefinitionSettings.Udb_Type2_Field2.TransformMetadataSettings(),
                            DatabaseFieldDefinitionSettings.Udb_Type2_Field3.TransformMetadataSettings(),
                            DatabaseFieldDefinitionSettings.Udb_Type2_Field4.TransformMetadataSettings(),
                        }
                    ),
                };
                Udb_Type3 = new WodiLib.Database.DatabaseProjectTypeSettings
                {
                    TypeName = "UDB3",
                    Memo = "",
                    DataNameList = new DatabaseDataNameListSettings(
                        new List<DataName>
                        {
                            DatabaseDataName.Udb_Type3_Data0,
                        }
                    ),
                    FieldMetadataList = new DatabaseFieldMetadataListSettings(
                        new List<IDatabaseFieldMetadataSettings>
                        {
                            DatabaseFieldDefinitionSettings.Udb_Type3_Field0.TransformMetadataSettings(),
                            DatabaseFieldDefinitionSettings.Udb_Type3_Field1.TransformMetadataSettings(),
                            DatabaseFieldDefinitionSettings.Udb_Type3_Field2.TransformMetadataSettings(),
                        }
                    ),
                };

                Cdb_Type0 = new WodiLib.Database.DatabaseProjectTypeSettings
                {
                    TypeName = "あいうえお",
                    Memo = "メモ欄",
                    DataNameList = new DatabaseDataNameListSettings(
                        new List<DataName>
                        {
                            DatabaseDataName.Cdb_Type0_Data0,
                            DatabaseDataName.Cdb_Type0_Data1,
                            DatabaseDataName.Cdb_Type0_Data2,
                        }
                    ),
                    FieldMetadataList = new DatabaseFieldMetadataListSettings(
                        new List<IDatabaseFieldMetadataSettings>
                        {
                            DatabaseFieldDefinitionSettings.Cdb_Type0_Field0.TransformMetadataSettings(),
                            DatabaseFieldDefinitionSettings.Cdb_Type0_Field1.TransformMetadataSettings(),
                            DatabaseFieldDefinitionSettings.Cdb_Type0_Field2.TransformMetadataSettings(),
                            DatabaseFieldDefinitionSettings.Cdb_Type0_Field3.TransformMetadataSettings(),
                            DatabaseFieldDefinitionSettings.Cdb_Type0_Field4.TransformMetadataSettings(),
                            DatabaseFieldDefinitionSettings.Cdb_Type0_Field5.TransformMetadataSettings(),
                            DatabaseFieldDefinitionSettings.Cdb_Type0_Field6.TransformMetadataSettings(),
                            DatabaseFieldDefinitionSettings.Cdb_Type0_Field7.TransformMetadataSettings(),
                            DatabaseFieldDefinitionSettings.Cdb_Type0_Field8.TransformMetadataSettings(),
                        }
                    ),
                };
                Cdb_Type1 = new WodiLib.Database.DatabaseProjectTypeSettings
                {
                    TypeName = "",
                    Memo = "",
                    DataNameList = new DatabaseDataNameListSettings(
                        new List<DataName>
                        {
                            DatabaseDataName.Cdb_Type1_Data0,
                        }
                    ),
                    FieldMetadataList = new DatabaseFieldMetadataListSettings(
                        new List<IDatabaseFieldMetadataSettings>
                        {
                            DatabaseFieldDefinitionSettings.Cdb_Type1_Field0.TransformMetadataSettings(),
                        }
                    ),
                };
            }
        }
    }
}
