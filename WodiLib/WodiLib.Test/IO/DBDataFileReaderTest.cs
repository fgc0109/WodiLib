using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.IO.TestData.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DBDataFileReaderTest : TestFixtureBase
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
            DBDataFileTestItemGenerator.OutputFile();
        }

        private static readonly object[] DBDataReadTestCaseSource =
        {
            // [readFilePath, expected]
            new object[]
            {
                new DBDataFilePath($@"{IoTestDataConstants.TestWorkRootDir}\UDB0_データ_001to003_7.dbdata"),
                DBDataFileTestItemGenerator.GenerateUDB0DBData(),
            },
            new object[]
            {
                new DBDataFilePath($@"{IoTestDataConstants.TestWorkRootDir}\あいうえお_データ_000to000_a.dbdata"),
                DBDataFileTestItemGenerator.GenerateCDB0DBData(),
            },
        };

        [TestCaseSource(nameof(DBDataReadTestCaseSource))]
        public static void DBDataReadTest(DBDataFilePath readFilePath, DBData expected)
        {
            var reader = new DBDataFileReader(readFilePath);

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
            DBDataFileTestItemGenerator.DeleteFile();
        }
    }
}
