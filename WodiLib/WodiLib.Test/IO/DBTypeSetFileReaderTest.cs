using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.IO.TestData.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DBTypeSetFileReaderTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            // テスト用ファイル出力
            DBTypeSetFileTestItemGenerator.OutputFile();
        }

        private static readonly object[] DBTypeSetReadTestCaseSource =
        {
            // [filePath, dbKind, expected]
            new object[]
            {
                new DBTypeSetFilePath($@"{IoTestDataConstants.TestWorkRootDir}\タイプ設定_000_あいうえお.dbtypeset"),
                DBTypeSetFileTestItemGenerator.GenerateCDB0Data(),
            },
            new object[]
            {
                new DBTypeSetFilePath($@"{IoTestDataConstants.TestWorkRootDir}\タイプ設定_000_UDB0.dbtypeset"),
                DBTypeSetFileTestItemGenerator.GenerateUDB0Data(),
            },
        };

        [TestCaseSource(nameof(DBTypeSetReadTestCaseSource))]
        public static void DBTypeSetReadTest(DBTypeSetFilePath filePath, DBTypeSet expected)
        {
            var reader = new DBTypeSetFileReader(filePath);

            pureFunctionTestHelper.PureFuncSuccess(
                instance: reader,
                execFunc: target => target.ReadSync(),
                resultValueVerifier: ValueVerifier.AreItemEquals(expected)
            );
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            // テスト用ファイル削除
            DBTypeSetFileTestItemGenerator.DeleteFile();
        }
    }
}
