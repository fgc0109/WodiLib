using System;
using System.Linq;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class DatabaseSchemaDividerTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Divide

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void DivideTest_Success()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseSchemaDivider.Divide(TestData.Schema),
                resultValueVerifier: new ValueVerifier<DatabaseSchemaDivider.DivideResult>(result =>
                    {
                        Assert.IsTrue(
                            result.DataTableList.ItemEquals(TestData.ExpectedDataTableWithDataNamingDefinitionList)
                        );
                        Assert.IsTrue(result.TypeList.ItemEquals(TestData.ExpectedProjectTypeList));
                    }
                )
            );
        }

        /// <summary>
        ///     src が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void DivideTest_Failure_NullArgs()
        {
            staticFunctionTestHelper.StaticFuncFailure(
                execFunc: () => DatabaseSchemaDivider.Divide(null!),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion

        #region ExtractDataTableWithDataNamingList

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void ExtractDataTableWithDataNamingListTest_Success()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseSchemaDivider.ExtractDataTableWithDataNamingList(TestData.Schema),
                resultValueVerifier: ValueVerifier.AreItemEquals(TestData.ExpectedDataTableWithDataNamingDefinitionList)
            );
        }

        /// <summary>
        ///     src が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ExtractDataTableWithDataNamingListTest_Failure_NullArgs()
        {
            staticFunctionTestHelper.StaticFuncFailure(
                execFunc: () => DatabaseSchemaDivider.ExtractDataTableWithDataNamingList(null!),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion

        #region ExtractProjectTypeList

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void ExtractProjectTypeListTest_Success()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseSchemaDivider.ExtractProjectTypeList(TestData.Schema),
                resultValueVerifier: ValueVerifier.AreItemEquals(TestData.ExpectedProjectTypeList)
            );
        }

        /// <summary>
        ///     src が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ExtractProjectTypeListTest_Failure_NullArgs()
        {
            staticFunctionTestHelper.StaticFuncFailure(
                execFunc: () => DatabaseSchemaDivider.ExtractProjectTypeList(null!),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion

        #region TestData

        private static class TestData
        {
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
                            SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionSettings(
                                new DatabaseFieldSpecialSettingDefinitionNormalSettings()
                            ),
                            FieldMemo = "Type0-Field0 メモ",
                        },
                        new DatabaseFieldDefinitionSettings
                        {
                            FieldName = "Type0-Field1",
                            FieldType = DatabaseFieldType.String,
                            SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionSettings(
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
                            SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionSettings(
                                new DatabaseFieldSpecialSettingDefinitionLoadFileSettings
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
                            SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionSettings(
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
                DbKind = DatabaseKind.Changeable,
                TypeTableList = new DatabaseTypeTableListSettings(
                    new IDatabaseTypeTableSettings[]
                    {
                        Type0Settings,
                        Type1Settings,
                        Type2Settings,
                    }
                ),
            };

            public static readonly DatabaseSchema Schema = new(SchemaSettings);

            public static readonly DatabaseDataTableWithDataNamingDefinitionList
                ExpectedDataTableWithDataNamingDefinitionList = new(
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

            public static readonly DatabaseProjectTypeList ExpectedProjectTypeList = new(
                new DatabaseProjectTypeListSettings(
                    new IDatabaseProjectTypeSettings[]
                    {
                        new DatabaseProjectTypeSettings
                        {
                            TypeName = Type0Settings.TypeName,
                            Memo = Type0Settings.Memo,
                            FieldMetadataList = Type0Settings.FieldDefinitionList.TransformMetadataSettings(),
                            DataNameList = new DatabaseDataNameListSettings(
                                Type0Settings.Settings.Select(namedRow => namedRow.DataName).ToArray()
                            ),
                        },
                        new DatabaseProjectTypeSettings
                        {
                            TypeName = Type1Settings.TypeName,
                            Memo = Type1Settings.Memo,
                            FieldMetadataList = Type1Settings.FieldDefinitionList.TransformMetadataSettings(),
                            DataNameList = new DatabaseDataNameListSettings(
                                Type1Settings.Settings.Select(namedRow => namedRow.DataName).ToArray()
                            ),
                        },
                        new DatabaseProjectTypeSettings
                        {
                            TypeName = Type2Settings.TypeName,
                            Memo = Type2Settings.Memo,
                            FieldMetadataList = Type2Settings.FieldDefinitionList.TransformMetadataSettings(),
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
