using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.BinarySerializer.Database
{
    [TestFixture]
    public class DatabaseFieldDefinitionBinarySerializerTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Methods

        #region SerializeFieldNames

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [Test]
        public static void SerializeFieldNamesTest()
        {
            var src = TestData.DatabaseFieldDefinitions;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseFieldDefinitionBinarySerializer.SerializeFieldNames(src)
            );

            // 取得した結果の正しさは IO 名前空間の Read/Write テストで検証する
        }

        #endregion

        #region SerializeFieldTypesAndOrder

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [Test]
        public static void SerializeFieldTypesAndOrderTest()
        {
            var src = TestData.DatabaseFieldDefinitions;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseFieldDefinitionBinarySerializer.SerializeFieldTypesAndOrder(src)
            );

            // 取得した結果の正しさは IO 名前空間の Read/Write テストで検証する
        }

        #endregion

        #region SerializeSpecialSettingDescription

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [Test]
        public static void SerializeSpecialSettingDescriptionTest()
        {
            var src = TestData.DatabaseFieldDefinitions;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseFieldDefinitionBinarySerializer.SerializeSpecialSettingDescription(src)
            );

            // 取得した結果の正しさは IO 名前空間の Read/Write テストで検証する
        }

        #endregion

        #endregion

        #region TestData

        private static class TestData
        {
            public static ReadOnlyDatabaseFieldDefinition[] DatabaseFieldDefinitions { get; }

            static TestData()
            {
                DatabaseFieldDefinitions = new ReadOnlyDatabaseFieldDefinition[]
                {
                    new DatabaseFieldDefinition(
                        new DatabaseFieldDefinitionSettings
                        {
                            FieldType = DatabaseFieldType.Int,
                            FieldName = "Field 0",
                            FieldMemo = "Memo",
                        }
                    ),
                    new DatabaseFieldDefinition(
                        new DatabaseFieldDefinitionSettings
                        {
                            FieldType = DatabaseFieldType.String,
                            FieldName = "Field 1",
                        }
                    ),
                    new DatabaseFieldDefinition(
                        new DatabaseFieldDefinitionSettings
                        {
                            FieldType = DatabaseFieldType.Int,
                            FieldName = "Field 2",
                            FieldMemo = "TestMemo",
                        }
                    ),
                };
            }
        }

        #endregion
    }
}
