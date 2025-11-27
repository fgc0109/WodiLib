using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.IO.TestData.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DBDataFileWriterTest : TestFixtureBase
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
                DBDataFileTestItemGenerator.GenerateCDB0DBData(),
                new DBDataFilePath($@"{IoTestDataConstants.TestWorkRootDir}\Outputあいうえお_データ_000to000_a.dbdata"),
            },
            new object[]
            {
                DBDataFileTestItemGenerator.GenerateUDB0DBData(),
                new DBDataFilePath($@"{IoTestDataConstants.TestWorkRootDir}\OutputUDB0_データ_001to003_7.dbdata"),
            },
        };

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCaseSource(nameof(WriteSyncTestCaseSource))]
        public static void WriteSyncTest(DBData outputData, DBDataFilePath outputFileName)
        {
            var writer = new DBDataFileWriter(outputFileName);

            pureActionTestHelper.PureActionSuccess(
                instance: writer,
                execAction: target => target.WriteSync(outputData)
            );
        }
    }
}
