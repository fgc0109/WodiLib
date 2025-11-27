using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.IO.TestData.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DBProjectFileWriterTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        private static readonly object[] WriteSyncTestCaseSource =
        {
            // [outputData, outputFileName]
            new object[]
            {
                DatabaseProjectFileTestItemGenerator.GenerateDatabase0Project(),
                new UserDatabaseProjectFilePath($@"{IoTestDataConstants.TestWorkRootDir}\OutputDatabase0.project"),
            },
            new object[]
            {
                DatabaseProjectFileTestItemGenerator.GenerateCDatabase0Project(),
                new ChangeableDatabaseProjectFilePath(
                    $@"{IoTestDataConstants.TestWorkRootDir}\OutputCDatabase0.project"
                ),
            },
        };

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCaseSource(nameof(WriteSyncTestCaseSource))]
        public static void WriteSyncTest(DBProject outputData, DatabaseProjectFilePath outputFileName)
        {
            var writer = new DBProjectFileWriter(outputFileName);

            pureActionTestHelper.PureActionSuccess(
                instance: writer,
                execAction: target => target.WriteSync(outputData)
            );
        }
    }
}
