using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.IO.TestData.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DBTypeSetFileWriterTest : TestFixtureBase
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
                DBTypeSetFileTestItemGenerator.GenerateCDB0Data(),
                new DBTypeSetFilePath($@"{IoTestDataConstants.TestWorkRootDir}\Outputタイプ設定_000_あいうえお.dbtypeset"),
            },
            new object[]
            {
                DBTypeSetFileTestItemGenerator.GenerateUDB0Data(),
                new DBTypeSetFilePath($@"{IoTestDataConstants.TestWorkRootDir}\Outputタイプ設定_000_UDB0.dbtypeset"),
            },
        };

        [TestCaseSource(nameof(WriteSyncTestCaseSource))]
        public static void WriteSyncTest(DBTypeSet outputData, DBTypeSetFilePath outputFileName)
        {
            var writer = new DBTypeSetFileWriter(outputFileName);

            pureActionTestHelper.PureActionSuccess(
                instance: writer,
                execAction: target => target.WriteSync(outputData)
            );
        }
    }
}
