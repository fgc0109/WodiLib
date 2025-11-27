using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.IO.TestData.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DBTypeFileIOTest : TestFixtureBase
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
            DBTypeFileTestItemGenerator.OutputFile();
        }

        private static readonly object[][] DBTypeIOTestCaseSource =
        {
            // [inputFilePath, outputFilePath]
            new object[]
            {
                new DBTypeFilePath($@"{IoTestDataConstants.TestWorkRootDir}\タイプ(データ含む)_002_┣ 主人公行動AI.dbtype"),
                new DBTypeFilePath($@"{IoTestDataConstants.TestWorkRootDir}\Outputタイプ(データ含む)_002_┣ 主人公行動AI.dbtype"),
            },
            new object[]
            {
                new DBTypeFilePath($@"{IoTestDataConstants.TestWorkRootDir}\タイプ(データ含む)_008_状態設定.dbtype"),
                new DBTypeFilePath($@"{IoTestDataConstants.TestWorkRootDir}\Outputタイプ(データ含む)_008_状態設定.dbtype"),
            },
        };

        /// <summary>
        ///     読み取りと書き出しが正常に終了すること。
        /// </summary>
        [TestCaseSource(nameof(DBTypeIOTestCaseSource))]
        public static void DBTypeIOTest(DBTypeFilePath inputFilePath, DBTypeFilePath outputFilePath)
        {
            // ----------------------------------------
            // 読み取りテスト

            var reader = new DBTypeFileReader(inputFilePath);

            DBType data = null!;
            pureFunctionTestHelper.PureFuncSuccess(
                instance: reader,
                execFunc: target => target.ReadAsync().GetAwaiter().GetResult(),
                resultValueVerifier: new ValueVerifier<DBType>(result => { data = result; })
            );

            // ----------------------------------------
            // 書き込みテスト

            var writer = new DBTypeFileWriter(outputFilePath);
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
            DBTypeFileTestItemGenerator.DeleteFile();
        }
    }
}
