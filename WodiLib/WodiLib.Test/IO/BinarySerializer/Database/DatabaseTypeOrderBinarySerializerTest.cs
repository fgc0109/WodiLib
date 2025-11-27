using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.BinarySerializer.Database
{
    [TestFixture]
    public class DatabaseTypeOrderBinarySerializerTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Methods

        #region Serialize

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void SerializeTest()
        {
            var src = new List<DatabaseFieldType>
            {
                DatabaseFieldType.Int,
                DatabaseFieldType.Int,
                DatabaseFieldType.String,
                DatabaseFieldType.Int,
                DatabaseFieldType.String,
                DatabaseFieldType.Int,
                DatabaseFieldType.Int,
                DatabaseFieldType.String,
                DatabaseFieldType.String,
                DatabaseFieldType.Int,
            };

            var expected = new byte[]
            {
                // Int
                0xE8, 0x03, 0x00, 0x00,
                // Int
                0xE9, 0x03, 0x00, 0x00,
                // String
                0xD0, 0x07, 0x00, 0x00,
                // Int
                0xEA, 0x03, 0x00, 0x00,
                // String
                0xD1, 0x07, 0x00, 0x00,
                // Int
                0xEB, 0x03, 0x00, 0x00,
                // Int
                0xEC, 0x03, 0x00, 0x00,
                // String
                0xD2, 0x07, 0x00, 0x00,
                // String
                0xD3, 0x07, 0x00, 0x00,
                // Int
                0xED, 0x03, 0x00, 0x00,
            };

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseTypeOrderBinarySerializer.Serialize(src),
                resultValueVerifier: ValueVerifier.AreItemSequenceEquals(expected)
            );
        }

        #endregion

        #endregion
    }
}
