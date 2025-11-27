using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.IO.TestData.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DBProjectFileIOTest : TestFixtureBase
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
            DatabaseProjectFileTestItemGenerator.OutputFile();
        }

        [Test]
        public static void UserDatabaseProjectIOTest()
        {
            UserDatabaseProjectFilePath inputFilePath =
                $@"{IoTestDataConstants.TestWorkRootDir}\Database1.project";
            UserDatabaseProjectFilePath outputFilePath =
                $@"{IoTestDataConstants.TestWorkRootDir}\OutputDatabase1.project";

            // ----------------------------------------
            // 読み取りテスト

            var reader =
                new DBProjectFileReader(
                    inputFilePath,
                    DatabaseKind.User
                );

            DBProject data = null!;
            pureFunctionTestHelper.PureFuncSuccess(
                instance: reader,
                execFunc: target => target.ReadAsync().GetAwaiter().GetResult(),
                resultValueVerifier: new ValueVerifier<DBProject>(result => { data = result; })
            );

            // ----------------------------------------
            // 書き込みテスト

            var writer = new DBProjectFileWriter(outputFilePath);
            pureActionTestHelper.PureActionSuccess(
                instance: writer,
                execAction: target => target.WriteAsync(data).GetAwaiter().GetResult()
            );

            // ----------------------------------------

            Console.WriteLine(
                $@"Written FilePath : {outputFilePath}"
            );
        }

        [Test]
        public static void ChangeableDatabaseProjectIOTest()
        {
            ChangeableDatabaseProjectFilePath inputFilePath =
                $@"{IoTestDataConstants.TestWorkRootDir}\CDatabase1.project";
            ChangeableDatabaseProjectFilePath outputFilePath =
                $@"{IoTestDataConstants.TestWorkRootDir}\OutputCDatabase1.project";

            // ----------------------------------------
            // 読み取りテスト

            var reader =
                new DBProjectFileReader(
                    inputFilePath,
                    DatabaseKind.Changeable
                );

            DBProject data = null!;
            pureFunctionTestHelper.PureFuncSuccess(
                instance: reader,
                execFunc: target => target.ReadAsync().GetAwaiter().GetResult(),
                resultValueVerifier: new ValueVerifier<DBProject>(result => { data = result; })
            );

            // ----------------------------------------
            // 書き込みテスト

            var writer = new DBProjectFileWriter(outputFilePath);
            pureActionTestHelper.PureActionSuccess(
                instance: writer,
                execAction: target => target.WriteAsync(data).GetAwaiter().GetResult()
            );

            // ----------------------------------------

            Console.WriteLine(
                $@"Written FilePath : {outputFilePath}"
            );
        }

        [Test]
        public static void SystemDatabaseProjectIOTest()
        {
            SystemDatabaseProjectFilePath inputFilePath =
                $@"{IoTestDataConstants.TestWorkRootDir}\SysDatabase1.project";
            SystemDatabaseProjectFilePath outputFilePath =
                $@"{IoTestDataConstants.TestWorkRootDir}\OutputSysDatabase1.project";

            // ----------------------------------------
            // 読み取りテスト

            var reader =
                new DBProjectFileReader(
                    inputFilePath,
                    DatabaseKind.System
                );

            DBProject data = null!;
            pureFunctionTestHelper.PureFuncSuccess(
                instance: reader,
                execFunc: target => target.ReadAsync().GetAwaiter().GetResult(),
                resultValueVerifier: new ValueVerifier<DBProject>(result => { data = result; })
            );

            // ----------------------------------------
            // 書き込みテスト

            var writer = new DBProjectFileWriter(outputFilePath);
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
            DatabaseProjectFileTestItemGenerator.DeleteFile();
        }
    }
}
