using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.BinarySerializer.Database
{
    [TestFixture]
    public class DatabaseTypeTableBinarySerializerTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Methods

        #region Serialize

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [Test]
        public static void SerializeTest()
        {
            var src = new DatabaseTypeTable(
                new DatabaseTypeTableSettings(
                    new List<IDatabaseNamedDataRowSettings>
                    {
                        new DatabaseNamedDataRowSettings(
                            new List<DatabaseFieldValue>
                            {
                                new(0),
                                new("1"),
                                new("2"),
                                new(3),
                            }
                        )
                        {
                            DataName = "Data 0",
                        },
                        new DatabaseNamedDataRowSettings(
                            new List<DatabaseFieldValue>
                            {
                                new(1000),
                                new("1001"),
                                new("1002"),
                                new(1003),
                            }
                        )
                        {
                            DataName = "Data 1",
                        },
                    }
                )
                {
                    TypeName = "TestType",
                    Memo = "TestType メモ",
                    DataNamingDefinition = new DatabaseDataNamingDefinition(DatabaseDataNamingType.Manual),
                    FieldDefinitionList = new DatabaseFieldDefinitionListSettings(
                        new List<IDatabaseFieldDefinitionSettings>
                        {
                            new DatabaseFieldDefinitionSettings
                            {
                                FieldName = "Field 0",
                                FieldType = DatabaseFieldType.Int,
                                FieldMemo = "Field 0 Memo",
                                SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionNormalSettings(),
                            },
                            new DatabaseFieldDefinitionSettings
                            {
                                FieldName = "Field 1",
                                FieldType = DatabaseFieldType.String,
                                FieldMemo = "Field 1 Memo",
                                SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionLoadFileSettings(),
                            },
                            new DatabaseFieldDefinitionSettings
                            {
                                FieldName = "Field 2",
                                FieldType = DatabaseFieldType.Int,
                                FieldMemo = "Field 2 Memo",
                                SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionManualSettings(),
                            },
                            new DatabaseFieldDefinitionSettings
                            {
                                FieldName = "Field 3",
                                FieldType = DatabaseFieldType.Int,
                                FieldMemo = "Field 3 Memo",
                                SpecialSettingDefinition =
                                    new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
                                    {
                                        DatabaseReferKind = DatabaseReferType.User,
                                    },
                            },
                        }
                    ),
                }
            );

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseTypeTableBinarySerializer.Serialize(src)
            );

            // 取得した結果の正しさは IO 名前空間の Read/Write テストで検証する
        }

        #endregion

        #endregion
    }
}
