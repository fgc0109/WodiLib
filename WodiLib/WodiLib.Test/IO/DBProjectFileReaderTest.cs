using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.IO.TestData.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DBProjectFileReaderTest : TestFixtureBase
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
            DatabaseProjectFileTestItemGenerator.OutputFile();
        }

        private static readonly object[] DatabaseProjectReadTestCaseSource =
        {
            // [filePath, dbKind, expected]
            new object[]
            {
                new UserDatabaseProjectFilePath($@"{IoTestDataConstants.TestWorkRootDir}\Database0.project"),
                DatabaseKind.User,
                DatabaseProjectFileTestItemGenerator.GenerateDatabase0Project(),
            },
            new object[]
            {
                new ChangeableDatabaseProjectFilePath($@"{IoTestDataConstants.TestWorkRootDir}\CDatabase0.project"),
                DatabaseKind.Changeable,
                DatabaseProjectFileTestItemGenerator.GenerateCDatabase0Project(),
            },
        };

        [TestCaseSource(nameof(DatabaseProjectReadTestCaseSource))]
        public static void DatabaseProjectReadTest(
            DatabaseProjectFilePath filePath,
            DatabaseKind dbKind,
            DBProject expected
        )
        {
            var reader = new DBProjectFileReader(filePath, dbKind);

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
            DBDatFileTestItemGenerator.DeleteFile();
        }
    }
}
