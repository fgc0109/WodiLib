using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.IO.TestData.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DBDatFileWriterTest : TestFixtureBase
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
                DBDatFileTestItemGenerator.GenerateDataBaseDat0Data(),
                new UserDatabaseDatFilePath($@"{IoTestDataConstants.TestWorkRootDir}\OutputDatabase0.dat"),
            },
            new object[]
            {
                DBDatFileTestItemGenerator.GenerateCDatabaseData0Data(),
                new ChangeableDatabaseDatFilePath($@"{IoTestDataConstants.TestWorkRootDir}\OutputCDatabase0.dat"),
            },
        };

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCaseSource(nameof(WriteSyncTestCaseSource))]
        public static void WriteSyncTest(DBDat outputData, DBDatFilePath outputFileName)
        {
            var writer = new DBDatFileWriter(outputFileName);

            pureActionTestHelper.PureActionSuccess(
                instance: writer,
                execAction: target => target.WriteSync(outputData)
            );
        }
    }
}
