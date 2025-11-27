using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.IO.TestData.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DatabaseMergedDataWriterTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            // テスト用ファイル出力先
            TestDirHelper.CreateDirIfNeed("DatabaseMergedDataWriterTest");
        }

        private static readonly object[] WriteSyncTestCaseSource =
        {
            // [outputData, dbKind]
            new object[]
            {
                DatabaseSchemaTestItemGenerator.GenerateCDB0MergedData(),
                DatabaseKind.Changeable,
            },
            new object[]
            {
                DatabaseSchemaTestItemGenerator.GenerateUDB0MergedData(),
                DatabaseKind.User,
            },
        };

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCaseSource(nameof(WriteSyncTestCaseSource))]
        public static void WriteSyncTest(DatabaseSchema outputData, DatabaseKind dbKind)
        {
            DatabaseSchemaWriter writer = null!;
            if (dbKind == DatabaseKind.User)
            {
                var datFilePath =
                    new ChangeableDatabaseDatFilePath(
                        $@"{IoTestDataConstants.TestWorkRootDir}\DatabaseMergedDataWriterTest\CDatabase.dat"
                    );
                var projectFilePath =
                    new ChangeableDatabaseProjectFilePath(
                        $@"{IoTestDataConstants.TestWorkRootDir}\DatabaseMergedDataWriterTest\CDatabase.project"
                    );
                writer = new DatabaseSchemaWriter(datFilePath, projectFilePath);
            }
            else if (dbKind == DatabaseKind.Changeable)
            {
                var datFilePath =
                    new ChangeableDatabaseDatFilePath(
                        $@"{IoTestDataConstants.TestWorkRootDir}\DatabaseMergedDataWriterTest\CDatabase.dat"
                    );
                var projectFilePath =
                    new ChangeableDatabaseProjectFilePath(
                        $@"{IoTestDataConstants.TestWorkRootDir}\DatabaseMergedDataWriterTest\CDatabase.project"
                    );
                writer = new DatabaseSchemaWriter(datFilePath, projectFilePath);
            }

            Assert.NotNull(writer);

            pureActionTestHelper.PureActionSuccess(
                instance: writer,
                execAction: target => target.WriteSync(outputData)
            );
        }
    }
}
