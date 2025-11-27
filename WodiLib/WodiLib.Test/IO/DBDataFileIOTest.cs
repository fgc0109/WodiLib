using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.IO.TestData.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DBDataFileIOTest : TestFixtureBase
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
            DBDataFileTestItemGenerator.OutputFile();
        }

        private static readonly object[][] DBDataIOTestCaseSource =
        {
            // [inputFilePath, outputFilePath]
            new object[]
            {
                new DBDataFilePath($@"{IoTestDataConstants.TestWorkRootDir}\┣ 主人公行動AI_データ_003to018_.dbdata"),
                new DBDataFilePath($@"{IoTestDataConstants.TestWorkRootDir}\Output┣ 主人公行動AI_データ_003to018_.dbdata"),
            },
            new object[]
            {
                new DBDataFilePath($@"{IoTestDataConstants.TestWorkRootDir}\状態設定_データ_000to023_戦闘不能.dbdata"),
                new DBDataFilePath($@"{IoTestDataConstants.TestWorkRootDir}\Output状態設定_データ_000to023_戦闘不能.dbdata"),
            },
        };

        /// <summary>
        ///     読み取りと書き出しが正常に終了すること。
        /// </summary>
        [TestCaseSource(nameof(DBDataIOTestCaseSource))]
        public static void DBDataIOTest(DBDataFilePath inputFilePath, DBDataFilePath outputFilePath)
        {
            // ----------------------------------------
            // 読み取りテスト

            var reader = new DBDataFileReader(inputFilePath);

            DBData data = null!;
            pureFunctionTestHelper.PureFuncSuccess(
                instance: reader,
                execFunc: target => target.ReadAsync().GetAwaiter().GetResult(),
                resultValueVerifier: new ValueVerifier<DBData>(result => { data = result; })
            );

            // ----------------------------------------
            // 書き込みテスト

            var writer = new DBDataFileWriter(outputFilePath);
            pureActionTestHelper.PureActionSuccess(
                instance: writer,
                execAction: target => target.WriteAsync(data).GetAwaiter().GetResult()
            );

            // ----------------------------------------

            Console.WriteLine(
                $@"Written FilePath : {outputFilePath}"
            );
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            // テスト用ファイル削除
            DBDataFileTestItemGenerator.DeleteFile();
        }
    }
}
