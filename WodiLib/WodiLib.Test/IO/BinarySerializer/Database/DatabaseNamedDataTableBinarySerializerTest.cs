using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.BinarySerializer.Database
{
    [TestFixture]
    public class DatabaseNamedDataTableBinarySerializerTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Methods

        #region SerializeValuesDividedType

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [Test]
        public static void SerializeValuesDividedTypeTest()
        {
            var src = new DatabaseNamedDataTable(
                new DatabaseNamedDataTableSettings(
                    new IDatabaseNamedDataRowSettings[]
                    {
                        new DatabaseNamedDataRowSettings(
                            new List<DatabaseFieldValue>
                            {
                                new(0),
                                new("1"),
                                new(2),
                                new("3"),
                                new(4),
                            }
                        )
                        {
                            DataName = "Data 0 Name",
                        },
                        new DatabaseNamedDataRowSettings(
                            new List<DatabaseFieldValue>
                            {
                                new(1000),
                                new("1001"),
                                new(1002),
                                new("1003"),
                                new(1004),
                            }
                        )
                        {
                            DataName = "Data 1 Name",
                        },
                        new DatabaseNamedDataRowSettings(
                            new List<DatabaseFieldValue>
                            {
                                new(2000),
                                new("2001"),
                                new(2002),
                                new("2003"),
                                new(2004),
                            }
                        )
                        {
                            DataName = "Data 2 Name",
                        },
                    }
                )
            );

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseNamedDataTableBinarySerializer.SerializeValuesDividedType(src)
            );

            // 取得した結果の正しさは IO 名前空間の Read/Write テストで検証する
        }

        #endregion

        #endregion
    }
}
