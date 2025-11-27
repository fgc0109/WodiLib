using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.BinarySerializer.Database
{
    [TestFixture]
    public class DatabaseProjectTypeBinarySerializerTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Methods

        #region Serialize

        #region DatabaseProjectTypes

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [Test]
        public static void SerializeTest_DatabaseProjectTypes()
        {
            var src = TestData.ProjectTypes;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseProjectTypeBinarySerializer.Serialize(src)
            );

            // 取得した結果の正しさは IO 名前空間の Read/Write テストで検証する
        }

        #endregion

        #region DatabaseProjectType

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [Test]
        public static void SerializeTest_DatabaseProjectType()
        {
            var src = TestData.ProjectTypes[0];

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseProjectTypeBinarySerializer.Serialize(src)
            );

            // 取得した結果の正しさは IO 名前空間の Read/Write テストで検証する
        }

        #endregion

        #endregion

        #endregion

        #region TestData

        private static class TestData
        {
            public static ReadOnlyDatabaseProjectType[] ProjectTypes { get; }

            static TestData()
            {
                ProjectTypes = new ReadOnlyDatabaseProjectType[]
                {
                    new DatabaseProjectType(
                        new DatabaseProjectTypeSettings
                        {
                            TypeName = "Type 0",
                            Memo = "Type0 Memo",
                            DataNameList = new DatabaseDataNameListSettings(
                                new List<DataName>
                                {
                                    new("Data 0"),
                                    new("Data 1"),
                                    new("Data 2"),
                                    new("Data 3"),
                                }
                            ),
                            FieldMetadataList = new DatabaseFieldMetadataListSettings(
                                new List<IDatabaseFieldMetadataSettings>
                                {
                                    new DatabaseFieldMetadataSettings
                                    {
                                        FieldName = "Field 0",
                                        FieldMemo = "Memo",
                                    },
                                }
                            ),
                        }
                    ),
                    new DatabaseProjectType(
                        new DatabaseProjectTypeSettings
                        {
                            TypeName = "Type 1",
                            // Memo = ,
                            DataNameList = new DatabaseDataNameListSettings(
                                new List<DataName>
                                {
                                    new(""),
                                    new(""),
                                    new(""),
                                    new(""),
                                }
                            ),
                            FieldMetadataList = new DatabaseFieldMetadataListSettings(
                                new List<IDatabaseFieldMetadataSettings>
                                {
                                    new DatabaseFieldMetadataSettings
                                    {
                                        FieldName = "Field 0",
                                        FieldMemo = "",
                                    },
                                    new DatabaseFieldMetadataSettings
                                    {
                                        FieldName = "Field 1",
                                        FieldMemo = "",
                                    },
                                }
                            ),
                        }
                    ),
                    new DatabaseProjectType(
                        new DatabaseProjectTypeSettings
                        {
                            TypeName = "Type 2",
                            // Memo = ,
                            DataNameList = new DatabaseDataNameListSettings(
                                new List<DataName>
                                {
                                    new("Data 0"),
                                }
                            ),
                            FieldMetadataList = new DatabaseFieldMetadataListSettings(
                                new List<IDatabaseFieldMetadataSettings>
                                {
                                    new DatabaseFieldMetadataSettings
                                    {
                                        FieldName = "Field 0",
                                        // FieldMemo = "Memo",
                                    },
                                }
                            ),
                        }
                    ),
                };
            }
        }

        #endregion
    }
}
