using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.BinarySerializer.Database
{
    [TestFixture]
    public class DatabaseFieldValueBinarySerializerTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Methods

        #region Serialize

        private static readonly object[][] Serialize_TestCaseSource =
        {
            // [src, expectedBytes]
            new object[] { new DatabaseFieldValue(0), new byte[] { 0x00, 0x00, 0x00, 0x00 } },
            new object[] { new DatabaseFieldValue(1), new byte[] { 0x01, 0x00, 0x00, 0x00 } },
            new object[] { new DatabaseFieldValue(-1), new byte[] { 0xFF, 0xFF, 0xFF, 0xFF } },
            new object[] { new DatabaseFieldValue(-2), new byte[] { 0xFE, 0xFF, 0xFF, 0xFF } },
            new object[] { new DatabaseFieldValue(""), new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00 } },
            new object[]
            {
                new DatabaseFieldValue("Woditor"),
                new byte[] { 0x08, 0x00, 0x00, 0x00, 0x57, 0x6F, 0x64, 0x69, 0x74, 0x6F, 0x72, 0x00 },
            },
            new object[]
            {
                new DatabaseFieldValue("うでぃた"), // マルチバイト文字
                new byte[] { 0x09, 0x00, 0x00, 0x00, 0x82, 0xA4, 0x82, 0xC5, 0x82, 0xA1, 0x82, 0xBD, 0x00 },
            },
        };

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [TestCaseSource(nameof(Serialize_TestCaseSource))]
        public static void Serialize(DatabaseFieldValue src, byte[] expected)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseFieldValueBinarySerializer.Serialize(src),
                resultValueVerifier: ValueVerifier.AreItemSequenceEquals(expected)
            );
        }

        #endregion

        #endregion
    }
}
