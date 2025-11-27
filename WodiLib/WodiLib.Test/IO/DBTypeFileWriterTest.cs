using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.IO.TestData.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DBTypeFileWriterTest : TestFixtureBase
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
                DBTypeFileTestItemGenerator.GenerateCDB0DBType(),
                new DBTypeFilePath($@"{IoTestDataConstants.TestWorkRootDir}\Outputタイプ(データ含む)_000_あいうえお.dbtype"),
            },
            new object[]
            {
                DBTypeFileTestItemGenerator.GenerateUDB0DBType(),
                new DBTypeFilePath($@"{IoTestDataConstants.TestWorkRootDir}\Outputタイプ(データ含む)_000_UDB0.dbtype"),
            },
        };

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCaseSource(nameof(WriteSyncTestCaseSource))]
        public static void WriteSyncTest(DBType outputData, DBTypeFilePath outputFileName)
        {
            var writer = new DBTypeFileWriter(outputFileName);

            pureActionTestHelper.PureActionSuccess(
                instance: writer,
                execAction: target => target.WriteSync(outputData)
            );
        }
    }
}
