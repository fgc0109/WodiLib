using System;
using System.Linq;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class DatabaseFieldSpecialSettingDefinitionFactoryTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Create

        #region From SettingsUnion

        /// <summary>
        ///     意図した値が取得されること。
        /// </summary>
        [Test]
        public static void CreateFromSettingsUnionTest_Success_FromNormalSettingsUnion()
        {
            var settings = new DatabaseFieldSpecialSettingDefinitionSettingsUnion(
                new DatabaseFieldSpecialSettingDefinitionNormalSettings
                {
                    InitValue = new DatabaseValueInt(100),
                }
            );

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseFieldSpecialSettingDefinitionFactory.Create(settings),
                resultValueVerifier: new ValueVerifier<IDatabaseFieldSpecialSettingDefinition>(result =>
                    {
                        // DatabaseFieldSpecialSettingDefinitionNormal 型インスタンスであること
                        Assert.AreEqual(typeof(DatabaseFieldSpecialSettingDefinitionNormal), result.GetType());

                        // settings と同一値であること
                        Assert.IsTrue(settings.AsNormalSettings().ItemEquals(result));
                    }
                )
            );
        }

        /// <summary>
        ///     意図した値が取得されること。
        /// </summary>
        [Test]
        public static void CreateFromSettingsUnionTest_Success_FromLoadFileSettingsUnion()
        {
            var settings = new DatabaseFieldSpecialSettingDefinitionSettingsUnion(
                new DatabaseFieldSpecialSettingDefinitionLoadFileSettings
                {
                    FolderName = "TestDirName",
                    IsOmitFolderName = true,
                }
            );

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseFieldSpecialSettingDefinitionFactory.Create(settings),
                resultValueVerifier: new ValueVerifier<IDatabaseFieldSpecialSettingDefinition>(result =>
                    {
                        // DatabaseFieldSpecialSettingDefinitionLoadFile 型インスタンスであること
                        Assert.AreEqual(typeof(DatabaseFieldSpecialSettingDefinitionLoadFile), result.GetType());

                        // settings と同一値であること
                        Assert.IsTrue(settings.AsLoadFileSettings().ItemEquals(result));
                    }
                )
            );
        }

        /// <summary>
        ///     意図した値が取得されること。
        /// </summary>
        [Test]
        public static void CreateFromSettingsUnionTest_Success_FromDatabaseReferenceSettingsUnion()
        {
            var settings = new DatabaseFieldSpecialSettingDefinitionSettingsUnion(
                new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
                {
                    DatabaseDbTypeId = 2,
                    DatabaseReferKind = DatabaseReferType.Changeable,
                    InitValue = new DatabaseValueInt(20),
                    IsUseAdditionalItems = true,
                    AdditionalCase1 = "Case 1",
                    AdditionalCase2 = "Case 2",
                    AdditionalCase3 = "Case 3",
                }
            );

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseFieldSpecialSettingDefinitionFactory.Create(settings),
                resultValueVerifier: new ValueVerifier<IDatabaseFieldSpecialSettingDefinition>(result =>
                    {
                        // DatabaseFieldSpecialSettingDefinitionDatabaseReference 型インスタンスであること
                        Assert.AreEqual(
                            typeof(DatabaseFieldSpecialSettingDefinitionDatabaseReference),
                            result.GetType()
                        );

                        // settings と同一値であること
                        Assert.IsTrue(settings.AsDatabaseReferenceSettings().ItemEquals(result));
                    }
                )
            );
        }

        /// <summary>
        ///     意図した値が取得されること。
        /// </summary>
        [Test]
        public static void CreateFromSettingsUnionTest_Success_FromManualSettingsUnion()
        {
            var settings = new DatabaseFieldSpecialSettingDefinitionSettingsUnion(
                new DatabaseFieldSpecialSettingDefinitionManualSettings
                {
                    SpecialCases = new DatabaseValueCaseListSettings(
                        new DatabaseValueCase[]
                        {
                            new(0, "Case 0"),
                            new(10, "Case 10"),
                            new(100, "Case 100"),
                        }
                    ),
                }
            );

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseFieldSpecialSettingDefinitionFactory.Create(settings),
                resultValueVerifier: new ValueVerifier<IDatabaseFieldSpecialSettingDefinition>(result =>
                    {
                        // DatabaseFieldSpecialSettingDefinitionManual 型インスタンスであること
                        Assert.AreEqual(typeof(DatabaseFieldSpecialSettingDefinitionManual), result.GetType());

                        // settings と同一値であること
                        Assert.IsTrue(settings.AsManualSettings().ItemEquals(result));
                    }
                )
            );
        }

        /// <summary>
        ///     settings が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void CreateFromSettingsUnionTest_Failure_NullArgs()
        {
            staticFunctionTestHelper.StaticFuncFailure(
                execFunc: () => DatabaseFieldSpecialSettingDefinitionFactory.Create(null!),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion

        #region From SettingTypeAndCases

        /// <summary>
        ///     意図した値が取得されること。
        /// </summary>
        [Test]
        public static void CreateFromSettingTypeAndCasesTest_Success_Normal()
        {
            var type = DatabaseFieldSpecialSettingType.Normal;
            var expected = new DatabaseFieldSpecialSettingDefinitionNormalSettings();

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseFieldSpecialSettingDefinitionFactory.Create(type, cases: null),
                resultValueVerifier: new ValueVerifier<IDatabaseFieldSpecialSettingDefinition>(result =>
                    {
                        Assert.AreEqual(DatabaseFieldSpecialSettingType.Normal, result.SettingType);
                        Assert.IsTrue(result.ItemEquals(expected));
                    }
                )
            );
        }

        /// <summary>
        ///     意図した値が取得されること。
        /// </summary>
        [Test]
        public static void CreateFromSettingTypeAndCasesTest_Success_LoadFile()
        {
            /*
                cases の組み合わせパターンテストは CreateLoadFile メソッドテストで行う
            */
            var type = DatabaseFieldSpecialSettingType.LoadFile;
            var expected = new DatabaseFieldSpecialSettingDefinitionLoadFileSettings();

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseFieldSpecialSettingDefinitionFactory.Create(type, cases: null),
                resultValueVerifier: new ValueVerifier<IDatabaseFieldSpecialSettingDefinition>(result =>
                    {
                        Assert.AreEqual(DatabaseFieldSpecialSettingType.LoadFile, result.SettingType);
                        Assert.IsTrue(result.AsLoadFileSettings().ItemEquals(expected));
                    }
                )
            );
        }

        /// <summary>
        ///     意図した値が取得されること。
        /// </summary>
        [Test]
        public static void CreateFromSettingTypeAndCasesTest_Success_ReferDatabase()
        {
            /*
                cases の組み合わせパターンテストは CreateReferDatabase メソッドテストで行う
            */
            var type = DatabaseFieldSpecialSettingType.ReferDatabase;
            var expected = new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings();

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseFieldSpecialSettingDefinitionFactory.Create(type, cases: null),
                resultValueVerifier: new ValueVerifier<IDatabaseFieldSpecialSettingDefinition>(result =>
                    {
                        Assert.AreEqual(DatabaseFieldSpecialSettingType.ReferDatabase, result.SettingType);
                        Assert.IsTrue(result.AsDatabaseReferenceSettings().ItemEquals(expected));
                    }
                )
            );
        }

        /// <summary>
        ///     意図した値が取得されること。
        /// </summary>
        [Test]
        public static void CreateFromSettingTypeAndCasesTest_Success_Manual()
        {
            var type = DatabaseFieldSpecialSettingType.Manual;
            var expected = new DatabaseFieldSpecialSettingDefinitionManualSettings();

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseFieldSpecialSettingDefinitionFactory.Create(type, cases: null),
                resultValueVerifier: new ValueVerifier<IDatabaseFieldSpecialSettingDefinition>(result =>
                    {
                        Assert.AreEqual(DatabaseFieldSpecialSettingType.Manual, result.SettingType);
                        Assert.IsTrue(result.AsManualSettings().ItemEquals(expected));
                    }
                )
            );
        }

        /// <summary>
        ///     type が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void CreateFromSettingTypeAndCasesTest_Failure_NullArgs()
        {
            DatabaseFieldSpecialSettingType type = null!;

            staticFunctionTestHelper.StaticFuncFailure(
                execFunc: () => DatabaseFieldSpecialSettingDefinitionFactory.Create(type, cases: null),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     cases に null 要素が含まれる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void CreateFromSettingTypeAndCasesTest_Failure_NullCases()
        {
            var type = DatabaseFieldSpecialSettingType.Normal;
            var cases = new DatabaseValueCase[]
            {
                null!,
            };

            staticFunctionTestHelper.StaticFuncFailure(
                execFunc: () => DatabaseFieldSpecialSettingDefinitionFactory.Create(type, cases),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region CreateNormal

        /// <summary>
        ///     意図した値が取得されること。
        /// </summary>
        [Test]
        public static void CreateNormalTest_Success()
        {
            var cases = new DatabaseValueCase[]
            {
                new(0, "Case 0"),
                new(10, "Case 10"),
                new(100, "Case 100"),
            };
            var expected = new DatabaseFieldSpecialSettingDefinitionNormalSettings();

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseFieldSpecialSettingDefinitionFactory.CreateNormal(cases),
                resultValueVerifier: new ValueVerifier<DatabaseFieldSpecialSettingDefinitionNormal>(result =>
                    {
                        Assert.IsTrue(result.ItemEquals(expected));
                    }
                )
            );
        }

        #endregion

        #region CreateLoadFile

        /// <summary>
        ///     意図した値が取得されること。
        /// </summary>
        [Test]
        public static void CreateLoadFileTest_Success_NoCases()
        {
            var expected = new DatabaseFieldSpecialSettingDefinitionLoadFileSettings
            {
                FolderName = "",
                IsOmitFolderName = false,
            };

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseFieldSpecialSettingDefinitionFactory.CreateLoadFile(cases: null),
                resultValueVerifier: new ValueVerifier<DatabaseFieldSpecialSettingDefinitionLoadFile>(result =>
                    {
                        Assert.IsTrue(result.ItemEquals(expected));
                    }
                )
            );
        }

        /// <summary>
        ///     意図した値が取得されること。
        /// </summary>
        [Test]
        public static void CreateLoadFileTest_Success_NotOmitFolderName()
        {
            const string folderName = "TestDirName";

            var cases = new DatabaseValueCase[]
            {
                new(0, folderName), // CaseNumber == 0 の場合、「保存時にフォルダ名省略フラグ」 = false
            };
            var expected = new DatabaseFieldSpecialSettingDefinitionLoadFileSettings
            {
                FolderName = folderName,
                IsOmitFolderName = false,
            };

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseFieldSpecialSettingDefinitionFactory.CreateLoadFile(cases),
                resultValueVerifier: new ValueVerifier<DatabaseFieldSpecialSettingDefinitionLoadFile>(result =>
                    {
                        Assert.IsTrue(result.ItemEquals(expected));
                    }
                )
            );
        }

        /// <summary>
        ///     意図した値が取得されること。
        /// </summary>
        [Test]
        public static void CreateLoadFileTest_Success_OmitFolderName()
        {
            const string folderName = "TestDirName";

            var cases = new DatabaseValueCase[]
            {
                new(1, folderName), // CaseNumber == 1 の場合、「保存時にフォルダ名省略フラグ」 = true
            };
            var expected = new DatabaseFieldSpecialSettingDefinitionLoadFileSettings
            {
                FolderName = folderName,
                IsOmitFolderName = true,
            };

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseFieldSpecialSettingDefinitionFactory.CreateLoadFile(cases),
                resultValueVerifier: new ValueVerifier<DatabaseFieldSpecialSettingDefinitionLoadFile>(result =>
                    {
                        Assert.IsTrue(result.ItemEquals(expected));
                    }
                )
            );
        }

        /// <summary>
        ///     特殊設定の要素数が1以外の場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(0)]
        [TestCase(2)]
        public static void CreateLoadFileTest_Failure_CaseLengthNot1(int caseLength)
        {
            var cases = caseLength.Iterate(i => new DatabaseValueCase(i, $"Case {i}")).ToList();

            staticFunctionTestHelper.StaticFuncFailure(
                execFunc: () => DatabaseFieldSpecialSettingDefinitionFactory.CreateLoadFile(cases),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     特殊設定の選択肢番号が 0, 1 以外の場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(2)]
        public static void CreateLoadFileTest_Failure_CaseNumberNot0Or1(int caseNumber)
        {
            var cases = new DatabaseValueCase[]
            {
                new(caseNumber, "Invalid Case"),
            };

            staticFunctionTestHelper.StaticFuncFailure(
                execFunc: () => DatabaseFieldSpecialSettingDefinitionFactory.CreateLoadFile(cases),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region CreateReferDatabase

        /// <summary>
        ///     意図した値が取得されること。
        /// </summary>
        [Test]
        public static void CreateReferDatabaseTest_Success_NoCases()
        {
            var expected = new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
            {
                DatabaseDbTypeId = 0,
                DatabaseReferKind = DatabaseReferType.Changeable,
                IsUseAdditionalItems = false,
                AdditionalCase1 = "",
                AdditionalCase2 = "",
                AdditionalCase3 = "",
            };

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseFieldSpecialSettingDefinitionFactory.CreateReferDatabase(cases: null),
                resultValueVerifier: new ValueVerifier<DatabaseFieldSpecialSettingDefinitionDatabaseReference>(result =>
                    {
                        Assert.IsTrue(result.ItemEquals(expected));
                    }
                )
            );
        }

        /// <summary>
        ///     意図した値が取得されること。
        /// </summary>
        [Test]
        public static void CreateReferDatabaseTest_Success_PartialCases()
        {
            const string expectedAdditionalCase1 = "Case 1";
            const string expectedAdditionalCase2 = "";
            const string expectedAdditionalCase3 = "Case 3";
            var cases = new DatabaseValueCase[]
            {
                new(-1, expectedAdditionalCase1),
                new(-3, expectedAdditionalCase3),
            };

            var expected = new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
            {
                DatabaseDbTypeId = 0,
                DatabaseReferKind = DatabaseReferType.Changeable,
                IsUseAdditionalItems = true,
                AdditionalCase1 = expectedAdditionalCase1,
                AdditionalCase2 = expectedAdditionalCase2,
                AdditionalCase3 = expectedAdditionalCase3,
            };

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseFieldSpecialSettingDefinitionFactory.CreateReferDatabase(cases),
                resultValueVerifier: new ValueVerifier<DatabaseFieldSpecialSettingDefinitionDatabaseReference>(result =>
                    {
                        Assert.IsTrue(result.ItemEquals(expected));
                    }
                )
            );
        }

        /// <summary>
        ///     意図した値が取得されること。
        /// </summary>
        [Test]
        public static void CreateReferDatabaseTest_Success_AllCases()
        {
            const string expectedAdditionalCase1 = "Case 1";
            const string expectedAdditionalCase2 = "Case 2";
            const string expectedAdditionalCase3 = "Case 3";
            var cases = new DatabaseValueCase[]
            {
                new(-1, expectedAdditionalCase1),
                new(-2, expectedAdditionalCase2),
                new(-3, expectedAdditionalCase3),
            };

            var expected = new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
            {
                DatabaseDbTypeId = 0,
                DatabaseReferKind = DatabaseReferType.Changeable,
                IsUseAdditionalItems = true,
                AdditionalCase1 = expectedAdditionalCase1,
                AdditionalCase2 = expectedAdditionalCase2,
                AdditionalCase3 = expectedAdditionalCase3,
            };

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseFieldSpecialSettingDefinitionFactory.CreateReferDatabase(cases),
                resultValueVerifier: new ValueVerifier<DatabaseFieldSpecialSettingDefinitionDatabaseReference>(result =>
                    {
                        Assert.IsTrue(result.ItemEquals(expected));
                    }
                )
            );
        }

        #endregion

        #region CreateManual

        /// <summary>
        ///     意図した値が取得されること。
        /// </summary>
        [Test]
        public static void CreateManualTest_Success_NoCases()
        {
            var expected = new DatabaseFieldSpecialSettingDefinitionManualSettings
            {
                SpecialCases = new DatabaseValueCaseListSettings(
                    Array.Empty<DatabaseValueCase>()
                ),
            };

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseFieldSpecialSettingDefinitionFactory.CreateManual(cases: null),
                resultValueVerifier: new ValueVerifier<DatabaseFieldSpecialSettingDefinitionManual>(result =>
                    {
                        Assert.IsTrue(result.ItemEquals(expected));
                    }
                )
            );
        }

        /// <summary>
        ///     意図した値が取得されること。
        /// </summary>
        [Test]
        public static void CreateManualTest_Success_SomeCases()
        {
            var cases = new DatabaseValueCase[]
            {
                new(0, "Case 0"),
                new(10, "Case 10"),
                new(100, "Case 100"),
            };
            var expected = new DatabaseFieldSpecialSettingDefinitionManualSettings
            {
                SpecialCases = new DatabaseValueCaseListSettings(cases),
            };

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseFieldSpecialSettingDefinitionFactory.CreateManual(cases),
                resultValueVerifier: new ValueVerifier<DatabaseFieldSpecialSettingDefinitionManual>(result =>
                    {
                        Assert.IsTrue(result.ItemEquals(expected));
                    }
                )
            );
        }

        #endregion

        #endregion
    }
}
