using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.IO.TestData.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DBTypeSetFileIOTest : TestFixtureBase
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
            DBTypeSetFileTestItemGenerator.OutputFile();
        }

        private static readonly object[][] TypeSetIOTestCaseSource =
        {
            // [inputFilePath, outputFilePath]
            new object[]
            {
                new DBTypeSetFilePath($@"{IoTestDataConstants.TestWorkRootDir}\タイプ設定_002_┣ 主人公行動AI.dbtypeset"),
                new DBTypeSetFilePath($@"{IoTestDataConstants.TestWorkRootDir}\Outputタイプ設定_002_┣ 主人公行動AI.dbtypeset"),
            },
            new object[]
            {
                new DBTypeSetFilePath($@"{IoTestDataConstants.TestWorkRootDir}\タイプ設定_008_状態設定.dbtypeset"),
                new DBTypeSetFilePath($@"{IoTestDataConstants.TestWorkRootDir}\Outputタイプ設定_008_状態設定.dbtypeset"),
            },
        };

        /// <summary>
        ///     読み取りと書き出しが正常に終了すること。
        /// </summary>
        [TestCaseSource(nameof(TypeSetIOTestCaseSource))]
        public static void TypeSetIOTest(DBTypeSetFilePath inputFilePath, DBTypeSetFilePath outputFilePath)
        {
            // ----------------------------------------
            // 読み取りテスト

            var reader = new DBTypeSetFileReader(inputFilePath);

            DBTypeSet data = null!;
            pureFunctionTestHelper.PureFuncSuccess(
                instance: reader,
                execFunc: target => target.ReadAsync().GetAwaiter().GetResult(),
                resultValueVerifier: new ValueVerifier<DBTypeSet>(result => { data = result; })
            );

            // ----------------------------------------
            // 書き込みテスト

            var writer = new DBTypeSetFileWriter(outputFilePath);
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
            DBTypeSetFileTestItemGenerator.DeleteFile();
        }
    }
}
