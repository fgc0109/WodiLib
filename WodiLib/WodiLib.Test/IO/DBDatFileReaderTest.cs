using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.IO.TestData.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DBDatFileReaderTest : TestFixtureBase
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
            DBDatFileTestItemGenerator.OutputFile();
        }

        private static readonly object[] DatabaseDatReadTestCaseSource =
        {
            // [filePath, dbKind, expected]
            new object[]
            {
                new UserDatabaseDatFilePath($@"{IoTestDataConstants.TestWorkRootDir}\Database0.dat"),
                DatabaseKind.User,
                DBDatFileTestItemGenerator.GenerateDataBaseDat0Data(),
            },
            new object[]
            {
                new ChangeableDatabaseDatFilePath($@"{IoTestDataConstants.TestWorkRootDir}\CDatabase0.dat"),
                DatabaseKind.Changeable,
                DBDatFileTestItemGenerator.GenerateCDatabaseData0Data(),
            },
        };

        [TestCaseSource(nameof(DatabaseDatReadTestCaseSource))]
        public static void DatabaseDatReadTest(DBDatFilePath filePath, DatabaseKind dbKind, DBDat expected)
        {
            var reader = new DBDatFileReader(filePath, dbKind);

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
