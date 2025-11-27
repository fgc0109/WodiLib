using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.IO.TestData.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DatabaseMergedDataReaderTest : TestFixtureBase
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
            DatabaseSchemaTestItemGenerator.OutputFile();
        }

        private static readonly object[] DBDataReadTestCaseSource =
        {
            // [databaseKind, datFilePath, projectFilePath, expected]
            new object[]
            {
                DatabaseKind.Changeable,
                new ChangeableDatabaseDatFilePath($@"{IoTestDataConstants.TestWorkRootDir}\CDatabase.dat"),
                new ChangeableDatabaseProjectFilePath($@"{IoTestDataConstants.TestWorkRootDir}\CDatabase.project"),
                DatabaseSchemaTestItemGenerator.GenerateCDB0MergedData(),
            },
            new object[]
            {
                DatabaseKind.User,
                new UserDatabaseDatFilePath($@"{IoTestDataConstants.TestWorkRootDir}\Database.dat"),
                new UserDatabaseProjectFilePath($@"{IoTestDataConstants.TestWorkRootDir}\Database.project"),
                DatabaseSchemaTestItemGenerator.GenerateUDB0MergedData(),
            },
        };

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [TestCaseSource(nameof(DBDataReadTestCaseSource))]
        public static void DBDataReadTest(
            DatabaseKind databaseKind,
            DBDatFilePath datFilePath,
            DatabaseProjectFilePath projectFilePath,
            DatabaseSchema expected
        )
        {
            DatabaseSchemaReader reader = null!;

            if (databaseKind == DatabaseKind.User)
            {
                reader = new DatabaseSchemaReader(
                    (UserDatabaseDatFilePath)datFilePath,
                    (UserDatabaseProjectFilePath)projectFilePath
                );
            }
            else if (databaseKind == DatabaseKind.Changeable)
            {
                reader = new DatabaseSchemaReader(
                    (ChangeableDatabaseDatFilePath)datFilePath,
                    (ChangeableDatabaseProjectFilePath)projectFilePath
                );
            }
            else
            {
                Assert.Fail();
            }

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
            DatabaseSchemaTestItemGenerator.DeleteFile();
        }
    }
}
