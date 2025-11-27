using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.BinarySerializer.Database
{
    [TestFixture]
    public class DatabaseFieldValuesBinarySerializerTest : TestFixtureBase
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
            var src = new DatabaseFieldValue[]
            {
                new(1),
                new(2),
                new("3"),
                new("4"),
                new(-2),
                new(-1),
                new("うでぃた"),
            };
            var expected = new byte[]
            {
                // 1
                0x01, 0x00, 0x00, 0x00,
                // 2
                0x02, 0x00, 0x00, 0x00,
                // "3"
                0x02, 0x00, 0x00, 0x00, 0x33, 0x00,
                // "4"
                0x02, 0x00, 0x00, 0x00, 0x34, 0x00,
                // -2
                0xFE, 0xFF, 0xFF, 0xFF,
                // -1
                0xFF, 0xFF, 0xFF, 0xFF,
                // "うでぃた"
                0x09, 0x00, 0x00, 0x00, 0x82, 0xA4, 0x82, 0xC5, 0x82, 0xA1, 0x82, 0xBD, 0x00,
            };

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseFieldValuesBinarySerializer.Serialize(src),
                resultValueVerifier: ValueVerifier.AreItemSequenceEquals(expected)
            );
        }

        #endregion

        #endregion
    }
}
