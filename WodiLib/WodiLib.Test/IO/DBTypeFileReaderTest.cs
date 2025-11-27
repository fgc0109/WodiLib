using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.IO.TestData.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DBTypeFileReaderTest : TestFixtureBase
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
            DBTypeFileTestItemGenerator.OutputFile();
        }

        private static readonly object[] DBTypeReadTestCaseSource =
        {
            // [filePath, expected]
            new object[]
            {
                new DBTypeFilePath($@"{IoTestDataConstants.TestWorkRootDir}\タイプ(データ含む)_000_あいうえお.dbtype"),
                DBTypeFileTestItemGenerator.GenerateCDB0DBType(),
            },
            new object[]
            {
                new DBTypeFilePath($@"{IoTestDataConstants.TestWorkRootDir}\タイプ(データ含む)_000_UDB0.dbtype"),
                DBTypeFileTestItemGenerator.GenerateUDB0DBType(),
            },
        };

        [TestCaseSource(nameof(DBTypeReadTestCaseSource))]
        public static void DBTypeReadTest(DBTypeFilePath filePath, DBType expected)
        {
            var reader = new DBTypeFileReader(filePath);

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
            DBTypeFileTestItemGenerator.DeleteFile();
        }
    }
}
