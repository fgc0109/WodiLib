using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.IO.TestData.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DBDatFileIOTest : TestFixtureBase
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
            DBDatFileTestItemGenerator.OutputFile();
        }

        private static readonly object[][] DatabaseDatIOTestCaseSource =
        {
            // [inputFilePath, outputFilePath]
            new object[]
            {
                new UserDatabaseDatFilePath($@"{IoTestDataConstants.TestWorkRootDir}\Database1.dat"),
                new UserDatabaseDatFilePath($@"{IoTestDataConstants.TestWorkRootDir}\OutputDatabase1.dat"),
            },
            new object[]
            {
                new UserDatabaseDatFilePath($@"{IoTestDataConstants.TestWorkRootDir}\CDatabase1.dat"),
                new UserDatabaseDatFilePath($@"{IoTestDataConstants.TestWorkRootDir}\OutputCDatabase1.dat"),
            },
            new object[]
            {
                new UserDatabaseDatFilePath($@"{IoTestDataConstants.TestWorkRootDir}\SysDatabase1.dat"),
                new UserDatabaseDatFilePath($@"{IoTestDataConstants.TestWorkRootDir}\OutputSysDatabase1.dat"),
            },
        };

        /// <summary>
        ///     読み取りと書き出しが正常に終了すること。
        /// </summary>
        [TestCaseSource(nameof(DatabaseDatIOTestCaseSource))]
        public static void DatabaseDatIOTest(DBDatFilePath inputFilePath, DBDatFilePath outputFilePath)
        {
            // ----------------------------------------
            // 読み取りテスト

            var reader =
                new DBDatFileReader(
                    inputFilePath,
                    DatabaseKind.User
                );

            DBDat data = null!;
            pureFunctionTestHelper.PureFuncSuccess(
                instance: reader,
                execFunc: target => target.ReadAsync().GetAwaiter().GetResult(),
                resultValueVerifier: new ValueVerifier<DBDat>(result => { data = result; })
            );

            // ----------------------------------------
            // 書き込みテスト

            var writer = new DBDatFileWriter(outputFilePath);
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
            DBDatFileTestItemGenerator.DeleteFile();
        }
    }
}
