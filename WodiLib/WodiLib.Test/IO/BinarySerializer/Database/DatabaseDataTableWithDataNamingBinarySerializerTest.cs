using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.BinarySerializer.Database
{
    [TestFixture]
    public class DatabaseDataTableWithDataNamingBinarySerializerTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Methods

        #region Serialize

        #region DatabaseDataTableWithDataNamingDefinitions

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [Test]
        public static void SerializeTest_DatabaseDataTableWithDataNamingDefinitions()
        {
            var src = TestData.DataTableWithDataNamingDefinitions;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseDataTableWithDataNamingBinarySerializer.Serialize(src)
            );

            // 取得した結果の正しさは IO 名前空間の Read/Write テストで検証する
        }

        #endregion

        #region DatabaseDataTableWithDataNamingDefinitionList

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [Test]
        public static void SerializeTest_DatabaseDataTableWithDataNamingDefinitionList()
        {
            var src = TestData.DataTableWithDataNamingDefinitionList;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseDataTableWithDataNamingBinarySerializer.Serialize(src)
            );

            // 取得した結果の正しさは IO 名前空間の Read/Write テストで検証する
        }

        #endregion

        #region DatabaseDataTableWithDataNamingDefinition

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [Test]
        public static void SerializeTest_DatabaseDataTableWithDataNamingDefinition()
        {
            var src = TestData.DataTableWithDataNamingDefinitions[0];

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseDataTableWithDataNamingBinarySerializer.Serialize(src)
            );

            // 取得した結果の正しさは IO 名前空間の Read/Write テストで検証する
        }

        #endregion

        #endregion

        #endregion

        #region TestData

        private static class TestData
        {
            public static ReadOnlyDatabaseDataTableWithDataNamingDefinition[] DataTableWithDataNamingDefinitions
            {
                get;
            }

            public static ReadOnlyDatabaseDataTableWithDataNamingDefinitionList DataTableWithDataNamingDefinitionList
            {
                get;
            }

            private static IDatabaseDataTableWithDataNamingDefinitionSettings[]
                DataTableWithDataNamingDefinitionSettingsArray { get; }

            static TestData()
            {
                DataTableWithDataNamingDefinitionSettingsArray =
                    new IDatabaseDataTableWithDataNamingDefinitionSettings[]
                    {
                        new DatabaseDataTableWithDataNamingDefinitionSettings
                        {
                            DataNamingDefinition = new DatabaseDataNamingDefinition(DatabaseDataNamingType.Manual),
                            DataTable = new DatabaseDataTableSettings(
                                new List<IDatabaseDataRowSettings>
                                {
                                    new DatabaseDataRowSettings(
                                        new List<DatabaseFieldValue>
                                        {
                                            new(1),
                                            new(2),
                                            new("3"),
                                            new("4"),
                                            new(5),
                                        }
                                    ),
                                    new DatabaseDataRowSettings(
                                        new List<DatabaseFieldValue>
                                        {
                                            new(1001),
                                            new(1002),
                                            new("1003"),
                                            new("1004"),
                                            new(1005),
                                        }
                                    ),
                                    new DatabaseDataRowSettings(
                                        new List<DatabaseFieldValue>
                                        {
                                            new(2001),
                                            new(2002),
                                            new("2003"),
                                            new("2004"),
                                            new(2005),
                                        }
                                    ),
                                }
                            ),
                        },
                        new DatabaseDataTableWithDataNamingDefinitionSettings
                        {
                            DataNamingDefinition =
                                new DatabaseDataNamingDefinition(DatabaseDataNamingType.FirstStringData),
                            DataTable = new DatabaseDataTableSettings(
                                new List<IDatabaseDataRowSettings>
                                {
                                    new DatabaseDataRowSettings(
                                        new List<DatabaseFieldValue>
                                        {
                                            new("Data 0000"),
                                            new(0),
                                        }
                                    ),
                                    new DatabaseDataRowSettings(
                                        new List<DatabaseFieldValue>
                                        {
                                            new("Data 0001"),
                                            new(0),
                                        }
                                    ),
                                    new DatabaseDataRowSettings(
                                        new List<DatabaseFieldValue>
                                        {
                                            new("Data 0002"),
                                            new(100),
                                        }
                                    ),
                                }
                            ),
                        },
                        new DatabaseDataTableWithDataNamingDefinitionSettings
                        {
                            DataNamingDefinition = new DatabaseDataNamingDefinition(DatabaseDataNamingType.EqualBefore),
                            DataTable = new DatabaseDataTableSettings(
                                new List<IDatabaseDataRowSettings>
                                {
                                    new DatabaseDataRowSettings(
                                        new List<DatabaseFieldValue>
                                        {
                                            new(1),
                                            new(2),
                                        }
                                    ),
                                    new DatabaseDataRowSettings(
                                        new List<DatabaseFieldValue>
                                        {
                                            new(1001),
                                            new(1002),
                                        }
                                    ),
                                    new DatabaseDataRowSettings(
                                        new List<DatabaseFieldValue>
                                        {
                                            new(2001),
                                            new(2002),
                                        }
                                    ),
                                }
                            ),
                        },
                    };
                DataTableWithDataNamingDefinitions = DataTableWithDataNamingDefinitionSettingsArray.Select(settings
                        => (ReadOnlyDatabaseDataTableWithDataNamingDefinition)new
                            DatabaseDataTableWithDataNamingDefinition(
                                settings
                            )
                    )
                    .ToArray();
                DataTableWithDataNamingDefinitionList = new DatabaseDataTableWithDataNamingDefinitionList(
                    new DatabaseDataTableWithDataNamingDefinitionListSettings(
                        DataTableWithDataNamingDefinitions
                            .Select(IDatabaseDataTableWithDataNamingDefinitionSettings (item) => item)
                            .ToList()
                    )
                );
            }
        }

        #endregion
    }
}
