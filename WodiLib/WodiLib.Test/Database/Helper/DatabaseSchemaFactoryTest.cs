using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class DatabaseSchemaFactoryTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region CreateMerged

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void CreateMergedTest_Success()
        {
            var dataTableList = TestData.DataTableWithDataNamingDefinitionList;
            var projectTypeList = TestData.ProjectTypeList;
            var dbKind = TestData.DbKind;

            var expected = TestData.ExpectedSchema;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseSchemaFactory.CreateMerged(dataTableList, projectTypeList, dbKind),
                resultValueVerifier: ValueVerifier.AreItemEquals(expected)
            );
        }

        /// <summary>
        ///     dbKind, dataTableList, projectTypeList が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        /// <param name="nullArgName"></param>
        [TestCase("dataTableList")]
        [TestCase("projectTypeList")]
        public static void CreateMergedTest_Failure_NullArgs(string nullArgName)
        {
            var dbKind = TestData.DbKind;
            var dataTableList = nullArgName == "dataTableList"
                ? null!
                : TestData.DataTableWithDataNamingDefinitionList;
            var projectTypeList = nullArgName == "projectTypeList"
                ? null!
                : TestData.ProjectTypeList;

            staticFunctionTestHelper.StaticFuncFailure(
                execFunc: () => DatabaseSchemaFactory.CreateMerged(dataTableList, projectTypeList, dbKind),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     dataTableList と projectTypeList のタイプ数に差異がある場合、
        ///     ArgumentExceptionが発生すること。
        /// </summary>
        [Test]
        public static void CreateMergedTest_Failure_NotMatchTypeLength()
        {
            var dataTableList = new DatabaseDataTableWithDataNamingDefinitionList(
                new DatabaseDataTableWithDataNamingDefinitionListSettings(
                    new List<IDatabaseDataTableWithDataNamingDefinitionSettings>
                    {
                        new DatabaseDataTableWithDataNamingDefinitionSettings(),
                        new DatabaseDataTableWithDataNamingDefinitionSettings(),
                    }
                )
            );
            var projectTypeList = new DatabaseProjectTypeList(
                new DatabaseProjectTypeListSettings(
                    new List<IDatabaseProjectTypeSettings>
                    {
                        new DatabaseProjectTypeSettings(),
                    }
                )
            );
            var dbKind = TestData.DbKind;

            staticFunctionTestHelper.StaticFuncFailure(
                execFunc: () => DatabaseSchemaFactory.CreateMerged(dataTableList, projectTypeList, dbKind),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     dataTableList と projectTypeList のデータ数に差異がある場合、
        ///     ArgumentExceptionが発生すること。
        /// </summary>
        [Test]
        public static void CreateMergedTest_Failure_NotMatchDataLength()
        {
            var dataTableList = new DatabaseDataTableWithDataNamingDefinitionList(
                new DatabaseDataTableWithDataNamingDefinitionListSettings(
                    new List<IDatabaseDataTableWithDataNamingDefinitionSettings>
                    {
                        new DatabaseDataTableWithDataNamingDefinitionSettings
                        {
                            DataTable = new DatabaseDataTableSettings(
                                new IDatabaseDataRowSettings[]
                                {
                                    new DatabaseDataRowSettings(),
                                    new DatabaseDataRowSettings(),
                                }
                            ),
                        },
                        new DatabaseDataTableWithDataNamingDefinitionSettings
                        {
                            DataTable = new DatabaseDataTableSettings(
                                new IDatabaseDataRowSettings[]
                                {
                                    new DatabaseDataRowSettings(),
                                    new DatabaseDataRowSettings(),
                                    new DatabaseDataRowSettings(),
                                }
                            ),
                        },
                    }
                )
            );
            var projectTypeList = new DatabaseProjectTypeList(
                new DatabaseProjectTypeListSettings(
                    new List<IDatabaseProjectTypeSettings>
                    {
                        new DatabaseProjectTypeSettings
                        {
                            DataNameList = new DatabaseDataNameListSettings(
                                new List<DataName>
                                {
                                    new(),
                                    new(),
                                }
                            ),
                        },
                        new DatabaseProjectTypeSettings
                        {
                            DataNameList = new DatabaseDataNameListSettings(
                                new List<DataName>
                                {
                                    new(),
                                    new(),
                                    // ここが不足している
                                }
                            ),
                        },
                    }
                )
            );
            var dbKind = TestData.DbKind;

            staticFunctionTestHelper.StaticFuncFailure(
                execFunc: () => DatabaseSchemaFactory.CreateMerged(dataTableList, projectTypeList, dbKind),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     dataTableList と projectTypeList の項目数に差異がある場合、
        ///     ArgumentExceptionが発生すること。
        /// </summary>
        [Test]
        public static void CreateMergedTest_Failure_NotMatchFieldLength()
        {
            var dataTableList = new DatabaseDataTableWithDataNamingDefinitionList(
                new DatabaseDataTableWithDataNamingDefinitionListSettings(
                    new List<IDatabaseDataTableWithDataNamingDefinitionSettings>
                    {
                        new DatabaseDataTableWithDataNamingDefinitionSettings
                        {
                            DataTable = new DatabaseDataTableSettings(
                                new IDatabaseDataRowSettings[]
                                {
                                    new DatabaseDataRowSettings(
                                        new DatabaseFieldValue[]
                                        {
                                            new(0),
                                            new(1),
                                        }
                                    ),
                                }
                            ),
                        },
                    }
                )
            );
            var projectTypeList = new DatabaseProjectTypeList(
                new DatabaseProjectTypeListSettings(
                    new List<IDatabaseProjectTypeSettings>
                    {
                        new DatabaseProjectTypeSettings
                        {
                            DataNameList = new DatabaseDataNameListSettings(
                                new List<DataName>
                                {
                                    new(),
                                }
                            ),
                            FieldDefinitionList = new DatabaseFieldDefinitionListSettings(
                                new List<IDatabaseFieldDefinitionSettings>
                                {
                                    new DatabaseFieldDefinitionSettings(),
                                    new DatabaseFieldDefinitionSettings(),
                                    new DatabaseFieldDefinitionSettings(), // ここが超過している
                                }
                            ),
                        },
                    }
                )
            );
            var dbKind = TestData.DbKind;

            staticFunctionTestHelper.StaticFuncFailure(
                execFunc: () => DatabaseSchemaFactory.CreateMerged(dataTableList, projectTypeList, dbKind),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region TestData

        private static class TestData
        {
            public static readonly DatabaseKind DbKind = DatabaseKind.System;

            private static readonly DatabaseTypeTableSettings Type0Settings = new(
                new IDatabaseNamedDataRowSettings[]
                {
                    new DatabaseNamedDataRowSettings(new DatabaseFieldValue[] { new(0), new("0") }),
                    new DatabaseNamedDataRowSettings(new DatabaseFieldValue[] { new(1), new("1") }),
                }
            )
            {
                TypeName = "Type0",
                DataNamingDefinition = new DatabaseDataNamingDefinition(
                    namingType: DatabaseDataNamingType.Manual
                ),
                FieldDefinitionList = new DatabaseFieldDefinitionListSettings(
                    new IDatabaseFieldDefinitionSettings[]
                    {
                        new DatabaseFieldDefinitionSettings
                        {
                            FieldName = "Type0-Field0",
                            FieldType = DatabaseFieldType.Int,
                            SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionSettingsUnion(
                                new DatabaseFieldSpecialSettingDefinitionNormalSettings()
                            ),
                            FieldMemo = "Type0-Field0 メモ",
                        },
                        new DatabaseFieldDefinitionSettings
                        {
                            FieldName = "Type0-Field1",
                            FieldType = DatabaseFieldType.String,
                            SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionSettingsUnion(
                                new DatabaseFieldSpecialSettingDefinitionNormalSettings()
                            ),
                            FieldMemo = "Type0-Field1 メモ",
                        },
                    }
                ),
                Memo = "Type0 の設定",
            };

            private static readonly DatabaseTypeTableSettings Type1Settings = new(
                new IDatabaseNamedDataRowSettings[]
                {
                    new DatabaseNamedDataRowSettings(new DatabaseFieldValue[] { new("0-0"), new("0-1") })
                    {
                        DataName = "Type1 - Data0",
                    },
                    new DatabaseNamedDataRowSettings(new DatabaseFieldValue[] { new("1-0"), new("1-1") })
                    {
                        DataName = "Type1 - Data1",
                    },
                }
            )
            {
                TypeName = "Type1",
                DataNamingDefinition = new DatabaseDataNamingDefinition(
                    namingType: DatabaseDataNamingType.DesignatedType,
                    dbKind: DatabaseKind.User,
                    typeId: 3
                ),
                FieldDefinitionList = new DatabaseFieldDefinitionListSettings(
                    new IDatabaseFieldDefinitionSettings[]
                    {
                        new DatabaseFieldDefinitionSettings
                        {
                            FieldName = "Type1-Field0",
                            FieldType = DatabaseFieldType.String,
                            SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionSettingsUnion(
                                new DatabaseFieldSpecialSettingDefinitionLoadFileSettings()
                                {
                                    FolderName = "FolderName",
                                    IsOmitFolderName = true,
                                }
                            ),
                        },
                        new DatabaseFieldDefinitionSettings
                        {
                            FieldName = "Type1-Field1",
                            FieldType = DatabaseFieldType.String,
                            SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionSettingsUnion(
                                new DatabaseFieldSpecialSettingDefinitionNormalSettings()
                            ),
                        },
                    }
                ),
                Memo = "Type1 の設定",
            };

            private static readonly DatabaseTypeTableSettings Type2Settings = new(
                new IDatabaseNamedDataRowSettings[]
                {
                    new DatabaseNamedDataRowSettings(Array.Empty<DatabaseFieldValue>())
                    {
                        DataName = "Type2 - Data0",
                    },
                }
            )
            {
                TypeName = "Type2",
                DataNamingDefinition = new DatabaseDataNamingDefinition(
                    namingType: DatabaseDataNamingType.Manual
                ),
                FieldDefinitionList = new DatabaseFieldDefinitionListSettings(),
                Memo = "Type2 の設定",
            };

            private static readonly DatabaseSchemaSettings SchemaSettings = new()
            {
                DbKind = DbKind,
                TypeTableList = new DatabaseTypeTableListSettings(
                    new IDatabaseTypeTableSettings[]
                    {
                        Type0Settings,
                        Type1Settings,
                        Type2Settings,
                    }
                ),
            };

            public static readonly DatabaseSchema ExpectedSchema = new(SchemaSettings);

            public static readonly DatabaseDataTableWithDataNamingDefinitionList
                DataTableWithDataNamingDefinitionList = new(
                    new DatabaseDataTableWithDataNamingDefinitionListSettings(
                        new IDatabaseDataTableWithDataNamingDefinitionSettings[]
                        {
                            new DatabaseDataTableWithDataNamingDefinitionSettings
                            {
                                DataNamingDefinition = Type0Settings.DataNamingDefinition,
                                DataTable = new DatabaseDataTableSettings(
                                    Type0Settings.Settings
                                        .Select<IDatabaseNamedDataRowSettings, IDatabaseDataRowSettings>(namedRow
                                            => new DatabaseDataRowSettings(namedRow.Settings)
                                        )
                                        .ToArray()
                                ),
                            },
                            new DatabaseDataTableWithDataNamingDefinitionSettings
                            {
                                DataNamingDefinition = Type1Settings.DataNamingDefinition,
                                DataTable = new DatabaseDataTableSettings(
                                    Type1Settings.Settings
                                        .Select<IDatabaseNamedDataRowSettings, IDatabaseDataRowSettings>(namedRow
                                            => new DatabaseDataRowSettings(namedRow.Settings)
                                        )
                                        .ToArray()
                                ),
                            },
                            new DatabaseDataTableWithDataNamingDefinitionSettings
                            {
                                DataNamingDefinition = Type2Settings.DataNamingDefinition,
                                DataTable = new DatabaseDataTableSettings(
                                    Type2Settings.Settings
                                        .Select<IDatabaseNamedDataRowSettings, IDatabaseDataRowSettings>(namedRow
                                            => new DatabaseDataRowSettings(namedRow.Settings)
                                        )
                                        .ToArray()
                                ),
                            },
                        }
                    )
                );

            public static readonly DatabaseProjectTypeList ProjectTypeList = new(
                new DatabaseProjectTypeListSettings(
                    new IDatabaseProjectTypeSettings[]
                    {
                        new DatabaseProjectTypeSettings
                        {
                            TypeName = Type0Settings.TypeName,
                            Memo = Type0Settings.Memo,
                            FieldDefinitionList = Type0Settings.FieldDefinitionList,
                            DataNameList = new DatabaseDataNameListSettings(
                                Type0Settings.Settings.Select(namedRow => namedRow.DataName).ToArray()
                            ),
                        },
                        new DatabaseProjectTypeSettings
                        {
                            TypeName = Type1Settings.TypeName,
                            Memo = Type1Settings.Memo,
                            FieldDefinitionList = Type1Settings.FieldDefinitionList,
                            DataNameList = new DatabaseDataNameListSettings(
                                Type1Settings.Settings.Select(namedRow => namedRow.DataName).ToArray()
                            ),
                        },
                        new DatabaseProjectTypeSettings
                        {
                            TypeName = Type2Settings.TypeName,
                            Memo = Type2Settings.Memo,
                            FieldDefinitionList = Type2Settings.FieldDefinitionList,
                            DataNameList = new DatabaseDataNameListSettings(
                                Type2Settings.Settings.Select(namedRow => namedRow.DataName).ToArray()
                            ),
                        },
                    }
                )
            );
        }

        #endregion
    }
}
