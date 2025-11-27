using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.BinarySerializer.Database
{
    [TestFixture]
    public class DatabaseDataRowBinarySerializerTest : TestFixtureBase
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
            var src = new DatabaseDataRow(
                new DatabaseDataRowSettings(
                    new List<DatabaseFieldValue>
                    {
                        new(1),
                        new(2),
                        new("3"),
                        new("4"),
                        new(5),
                    }
                )
            );

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseDataRowBinarySerializer.SerializeValuesDividedType(src)
            );

            // 取得した結果の正しさは IO 名前空間の Read/Write テストで検証する
        }

        #endregion

        #endregion
    }
}
