using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.ValueObject
{
    [TestFixture]
    public class DatabaseFieldSpecialSettingDefinitionSettingsUnionTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Properties

        #region public

        #region DtoType

        private static readonly object[][] DtoTypeGetterTest_Success_TestCaseSource =
        {
            // [instance, expected]
            new object[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(
                    new DatabaseFieldSpecialSettingDefinitionNormalSettings()
                ),
                DatabaseFieldSpecialSettingType.Normal,
            },
            new object[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(
                    new DatabaseFieldSpecialSettingDefinitionLoadFileSettings()
                ),
                DatabaseFieldSpecialSettingType.LoadFile,
            },
            new object[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(
                    new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings()
                ),
                DatabaseFieldSpecialSettingType.ReferDatabase,
            },
            new object[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(
                    new DatabaseFieldSpecialSettingDefinitionManualSettings()
                ),
                DatabaseFieldSpecialSettingType.Manual,
            },
        };

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [TestCaseSource(nameof(DtoTypeGetterTest_Success_TestCaseSource))]
        public static void DtoTypeGetterTest_Success(
            DatabaseFieldSpecialSettingDefinitionSettingsUnion instance,
            DatabaseFieldSpecialSettingType expected
        )
        {
            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.DtoType,
                getValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #region SettingType

        private static readonly object[][] SettingTypeGetterTest_Success_TestCaseSource =
        {
            // [instance, expected]
            new object[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(
                    new DatabaseFieldSpecialSettingDefinitionNormalSettings()
                ),
                DatabaseFieldSpecialSettingType.Normal,
            },
            new object[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(
                    new DatabaseFieldSpecialSettingDefinitionLoadFileSettings()
                ),
                DatabaseFieldSpecialSettingType.LoadFile,
            },
            new object[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(
                    new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings()
                ),
                DatabaseFieldSpecialSettingType.ReferDatabase,
            },
            new object[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(
                    new DatabaseFieldSpecialSettingDefinitionManualSettings()
                ),
                DatabaseFieldSpecialSettingType.Manual,
            },
        };

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [TestCaseSource(nameof(SettingTypeGetterTest_Success_TestCaseSource))]
        public static void SettingTypeGetterTest_Success(
            DatabaseFieldSpecialSettingDefinitionSettingsUnion instance,
            DatabaseFieldSpecialSettingType expected
        )
        {
            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.SettingType,
                getValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #endregion

        #endregion

        #region Constructors

        #region From IDatabaseFieldSpecialSettingDefinitionNormalSettings

        /// <summary>
        ///     コンストラクタが正常に終了すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_FromNormalSettings_Success()
        {
            var settings = TestData.CreateNormalSettings();

            constructorTestHelper.ConstructorSuccess(
                factory: () => new DatabaseFieldSpecialSettingDefinitionSettingsUnion(settings),
                instanceVerifier: new ValueVerifier<DatabaseFieldSpecialSettingDefinitionSettingsUnion>(instance =>
                    {
                        Assert.AreEqual(DatabaseFieldSpecialSettingType.Normal, instance.DtoType);
                        Assert.DoesNotThrow(() => { _ = instance.AsNormalSettings(); });
                        CustomAssert.AreItemEquals(settings, instance.AsNormalSettings());
                    }
                )
            );
        }

        /// <summary>
        ///     settings が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_FromNormalSettings_Failure_NullArgs()
        {
            DatabaseFieldSpecialSettingDefinitionNormalSettings settings = null!;

            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseFieldSpecialSettingDefinitionSettingsUnion(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion

        #region From IDatabaseFieldSpecialSettingDefinitionLoadFileSettings

        /// <summary>
        ///     コンストラクタが正常に終了すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_FromLoadFileSettings_Success()
        {
            var settings = TestData.CreateLoadFileSettings();

            constructorTestHelper.ConstructorSuccess(
                factory: () => new DatabaseFieldSpecialSettingDefinitionSettingsUnion(settings),
                instanceVerifier: new ValueVerifier<DatabaseFieldSpecialSettingDefinitionSettingsUnion>(instance =>
                    {
                        Assert.AreEqual(DatabaseFieldSpecialSettingType.LoadFile, instance.DtoType);
                        Assert.DoesNotThrow(() => { _ = instance.AsLoadFileSettings(); });
                        CustomAssert.AreItemEquals(settings, instance.AsLoadFileSettings());
                    }
                )
            );
        }

        /// <summary>
        ///     settings が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_FromLoadFileSettings_Failure_NullArgs()
        {
            DatabaseFieldSpecialSettingDefinitionLoadFileSettings settings = null!;

            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseFieldSpecialSettingDefinitionSettingsUnion(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion

        #region From IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings

        /// <summary>
        ///     コンストラクタが正常に終了すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_FromDatabaseReferenceSettings_Success()
        {
            var settings = TestData.CreateDatabaseReferenceSettings();

            constructorTestHelper.ConstructorSuccess(
                factory: () => new DatabaseFieldSpecialSettingDefinitionSettingsUnion(settings),
                instanceVerifier: new ValueVerifier<DatabaseFieldSpecialSettingDefinitionSettingsUnion>(instance =>
                    {
                        Assert.AreEqual(DatabaseFieldSpecialSettingType.ReferDatabase, instance.DtoType);
                        Assert.DoesNotThrow(() => { _ = instance.AsDatabaseReferenceSettings(); });
                        CustomAssert.AreItemEquals(settings, instance.AsDatabaseReferenceSettings());
                    }
                )
            );
        }

        /// <summary>
        ///     settings が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_FromDatabaseReferenceSettings_Failure_NullArgs()
        {
            DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings settings = null!;

            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseFieldSpecialSettingDefinitionSettingsUnion(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion

        #region From IDatabaseFieldSpecialSettingDefinitionManualSettings

        /// <summary>
        ///     コンストラクタが正常に終了すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_FromManualSettings_Success()
        {
            var settings = TestData.CreateManualSettings();

            constructorTestHelper.ConstructorSuccess(
                factory: () => new DatabaseFieldSpecialSettingDefinitionSettingsUnion(settings),
                instanceVerifier: new ValueVerifier<DatabaseFieldSpecialSettingDefinitionSettingsUnion>(instance =>
                    {
                        Assert.AreEqual(DatabaseFieldSpecialSettingType.Manual, instance.DtoType);
                        Assert.DoesNotThrow(() => { _ = instance.AsManualSettings(); });
                        CustomAssert.AreItemEquals(settings, instance.AsManualSettings());
                    }
                )
            );
        }

        /// <summary>
        ///     settings が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_FromManualSettings_Failure_NullArgs()
        {
            DatabaseFieldSpecialSettingDefinitionManualSettings settings = null!;

            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseFieldSpecialSettingDefinitionSettingsUnion(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion

        #endregion

        #region Methods

        #region public

        #region GetHashCode

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [Test]
        public static void GetHashCodeTest_Success()
        {
            var normalInstance1 =
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings());
            var normalInstance1a =
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings());
            var normalInstance2 =
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings(initValue: 123));
            var loadFileInstance1 =
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateLoadFileSettings());
            var loadFileInstance2 =
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(
                    TestData.CreateLoadFileSettings(folderName: "AnotherDirName")
                );
            var referDatabaseInstance1 =
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateDatabaseReferenceSettings());
            var referDatabaseInstance2 =
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(
                    TestData.CreateDatabaseReferenceSettings(initValue: 4)
                );
            var manualInstance1 =
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateManualSettings());
            var manualInstance2 =
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateManualSettings(initValue: 4));

            Assert.AreEqual(normalInstance1.GetHashCode(), normalInstance1a.GetHashCode());
            Assert.AreNotEqual(normalInstance1.GetHashCode(), normalInstance2.GetHashCode());
            Assert.AreNotEqual(normalInstance1.GetHashCode(), loadFileInstance1.GetHashCode());
            Assert.AreNotEqual(loadFileInstance1.GetHashCode(), loadFileInstance2.GetHashCode());
            Assert.AreNotEqual(normalInstance1.GetHashCode(), referDatabaseInstance1.GetHashCode());
            Assert.AreNotEqual(referDatabaseInstance1.GetHashCode(), referDatabaseInstance2.GetHashCode());
            Assert.AreNotEqual(normalInstance1.GetHashCode(), manualInstance1.GetHashCode());
            Assert.AreNotEqual(manualInstance1.GetHashCode(), manualInstance2.GetHashCode());
        }

        #endregion

        #region AsNormalSettings

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void AsNormalSettingsTest_Success()
        {
            var settings = TestData.CreateNormalSettings();

            pureFunctionTestHelper.PureFuncSuccess(
                instance: new DatabaseFieldSpecialSettingDefinitionSettingsUnion(settings),
                execFunc: target => target.AsNormalSettings(),
                resultValueVerifier: ValueVerifier.AreItemEquals<IDatabaseFieldSpecialSettingDefinitionNormalSettings>(
                    settings
                )
            );
        }

        private static readonly object[][] AsNormalSettingsTest_Failure_InvalidDtoType_TestCaseSource =
        {
            // [instance]
            new object[] { new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateLoadFileSettings()) },
            new object[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateDatabaseReferenceSettings()),
            },
            new object[] { new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateManualSettings()) },
        };

        /// <summary>
        ///     DtoType != Normal のユニオン型から NormalSettings に変換しようとした場合、
        ///     InvalidCastExceptino が発生すること。
        /// </summary>
        /// <param name="instance"></param>
        [TestCaseSource(nameof(AsNormalSettingsTest_Failure_InvalidDtoType_TestCaseSource))]
        public static void AsNormalSettingsTest_Failure_InvalidDtoType(
            DatabaseFieldSpecialSettingDefinitionSettingsUnion instance
        )
        {
            impureFunctionTestHelper.ImpureFuncFailure(
                instance: instance,
                execFunc: target => target.AsNormalSettings(),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(InvalidCastException))
            );
        }

        #endregion

        #region AsLoadFileSettings

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void AsLoadFileSettingsTest_Success()
        {
            var settings = TestData.CreateLoadFileSettings();

            pureFunctionTestHelper.PureFuncSuccess(
                instance: new DatabaseFieldSpecialSettingDefinitionSettingsUnion(settings),
                execFunc: target => target.AsLoadFileSettings(),
                resultValueVerifier:
                ValueVerifier.AreItemEquals<IDatabaseFieldSpecialSettingDefinitionLoadFileSettings>(settings)
            );
        }

        private static readonly object[][] AsLoadFileSettingsTest_Failure_InvalidDtoType_TestCaseSource =
        {
            // [instance]
            new object[] { new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()) },
            new object[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateDatabaseReferenceSettings()),
            },
            new object[] { new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateManualSettings()) },
        };

        /// <summary>
        ///     DtoType != LoadFile のユニオン型から LoadFileSettings に変換しようとした場合、
        ///     InvalidCastExceptino が発生すること。
        /// </summary>
        /// <param name="instance"></param>
        [TestCaseSource(nameof(AsLoadFileSettingsTest_Failure_InvalidDtoType_TestCaseSource))]
        public static void AsLoadFileSettingsTest_Failure_InvalidDtoType(
            DatabaseFieldSpecialSettingDefinitionSettingsUnion instance
        )
        {
            // 引数が不正な場合、
            // InvalidCastException が発生すること。
            impureFunctionTestHelper.ImpureFuncFailure(
                instance: instance,
                execFunc: target => target.AsLoadFileSettings(),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(InvalidCastException))
            );
        }

        #endregion

        #region AsDatabaseReferenceSettings

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void AsDatabaseReferenceSettingsTest_Success()
        {
            var settings = TestData.CreateDatabaseReferenceSettings();

            pureFunctionTestHelper.PureFuncSuccess(
                instance: new DatabaseFieldSpecialSettingDefinitionSettingsUnion(settings),
                execFunc: target => target.AsDatabaseReferenceSettings(),
                resultValueVerifier: ValueVerifier
                    .AreItemEquals<IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings>(settings)
            );
        }

        private static readonly object[][] AsDatabaseReferenceSettingsTest_Failure_InvalidDtoType_TestCaseSource =
        {
            // [instance]
            new object[] { new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()) },
            new object[] { new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateLoadFileSettings()) },
            new object[] { new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateManualSettings()) },
        };

        /// <summary>
        ///     DtoType != DatabaseReference のユニオン型から DatabaseReferenceSettings に変換しようとした場合、
        ///     InvalidCastExceptino が発生すること。
        /// </summary>
        /// <param name="instance"></param>
        [TestCaseSource(nameof(AsDatabaseReferenceSettingsTest_Failure_InvalidDtoType_TestCaseSource))]
        public static void AsDatabaseReferenceSettingsTest_Failure_InvalidDtoType(
            DatabaseFieldSpecialSettingDefinitionSettingsUnion instance
        )
        {
            impureFunctionTestHelper.ImpureFuncFailure(
                instance: instance,
                execFunc: target => target.AsDatabaseReferenceSettings(),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(InvalidCastException))
            );
        }

        #endregion

        #region AsManualSettings

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void AsManualSettingsTest_Success()
        {
            var settings = TestData.CreateManualSettings();

            pureFunctionTestHelper.PureFuncSuccess(
                instance: new DatabaseFieldSpecialSettingDefinitionSettingsUnion(settings),
                execFunc: target => target.AsManualSettings(),
                resultValueVerifier: ValueVerifier.AreItemEquals<IDatabaseFieldSpecialSettingDefinitionManualSettings>(
                    settings
                )
            );
        }

        private static readonly object[][] AsManualSettingsTest_Failure_InvalidDtoType_TestCaseSource =
        {
            // [instance]
            new object[] { new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()) },
            new object[] { new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateLoadFileSettings()) },
            new object[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateDatabaseReferenceSettings()),
            },
        };

        /// <summary>
        ///     DtoType != Manual のユニオン型から ManualSettings に変換しようとした場合、
        ///     InvalidCastExceptino が発生すること。
        /// </summary>
        /// <param name="instance"></param>
        [TestCaseSource(nameof(AsManualSettingsTest_Failure_InvalidDtoType_TestCaseSource))]
        public static void AsManualSettingsTest_Failure_InvalidDtoType(
            DatabaseFieldSpecialSettingDefinitionSettingsUnion instance
        )
        {
            impureFunctionTestHelper.ImpureFuncFailure(
                instance: instance,
                execFunc: target => target.AsManualSettings(),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(InvalidCastException))
            );
        }

        #endregion

        #endregion

        #region Equals

        #region DatabaseFieldSpecialSettingDefinitionSettingsUnion

        private static readonly object?[][]
            EqualsTest_DatabaseFieldSpecialSettingDefinitionSettingsUnion_TestCaseSource =
            {
                // [left, right, expected]
                new object?[]
                {
                    new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()), null,
                    false,
                },
                new object?[]
                {
                    new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()),
                    new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()), true,
                },
                new object?[]
                {
                    new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateLoadFileSettings()),
                    new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()), false,
                },
                new object?[]
                {
                    new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateDatabaseReferenceSettings()),
                    new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()), false,
                },
                new object?[]
                {
                    new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateManualSettings()),
                    new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()), false,
                },
            };

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと。
        /// </summary>
        [TestCaseSource(nameof(EqualsTest_DatabaseFieldSpecialSettingDefinitionSettingsUnion_TestCaseSource))]
        public static void EqualsTest_DatabaseFieldSpecialSettingDefinitionSettingsUnion(
            DatabaseFieldSpecialSettingDefinitionSettingsUnion left,
            DatabaseFieldSpecialSettingDefinitionSettingsUnion? right,
            bool expected
        )
        {
            equalsTestHelper.Equals(
                left,
                right,
                expected
            );
        }

        #endregion

        #region Object

        private static readonly object?[][] EqualsTest_Object_TestCaseSource =
        {
            // [left, right, expected]
            new object?[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()), null, false,
            },
            new object?[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()),
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()),
                true,
            },
            new object?[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()),
                "UnionData",
                false,
            },
        };

        [TestCaseSource(nameof(EqualsTest_Object_TestCaseSource))]
        public static void EqualsTest_Object(
            DatabaseFieldSpecialSettingDefinitionSettingsUnion left,
            object? right,
            bool expected
        )
        {
            equalsTestHelper.Equals(
                left,
                right,
                expected
            );
        }

        #endregion

        #endregion

        #region ItemEquals

        #region DatabaseFieldSpecialSettingDefinitionSettingsUnion

        private static readonly object?[][]
            ItemEqualsTest_DatabaseFieldSpecialSettingDefinitionSettingsUnion_TestCaseSource =
            {
                // [left, right, expected]
                new object?[]
                {
                    new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()), null,
                    false,
                },
                new object?[]
                {
                    new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()),
                    new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()), true,
                },
                new object?[]
                {
                    new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateLoadFileSettings()),
                    new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()), false,
                },
                new object?[]
                {
                    new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateDatabaseReferenceSettings()),
                    new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()), false,
                },
                new object?[]
                {
                    new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateManualSettings()),
                    new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()), false,
                },
            };

        /// <summary>
        ///     ItemEquals メソッドが意図した値を返すこと。
        /// </summary>
        [TestCaseSource(nameof(ItemEqualsTest_DatabaseFieldSpecialSettingDefinitionSettingsUnion_TestCaseSource))]
        public static void ItemEqualsTest_DatabaseFieldSpecialSettingDefinitionSettingsUnion(
            DatabaseFieldSpecialSettingDefinitionSettingsUnion left,
            DatabaseFieldSpecialSettingDefinitionSettingsUnion? right,
            bool expected
        )
        {
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected
            );
        }

        #endregion

        #region Object

        private static readonly object?[][] ItemEqualsTest_Object_TestCaseSource =
        {
            // [left, right, expected]
            new object?[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()), null, false,
            },
            new object?[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()),
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()), true,
            },
            new object?[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()), "UnionData",
                false,
            },
        };

        [TestCaseSource(nameof(ItemEqualsTest_Object_TestCaseSource))]
        public static void ItemEqualsTest_Object(
            DatabaseFieldSpecialSettingDefinitionSettingsUnion left,
            object? right,
            bool expected
        )
        {
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected
            );
        }

        #endregion

        #endregion

        #endregion

        #region Cast

        #region From

        #region IDatabaseFieldSpecialSettingDefinitionNormalSettings

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void CastFromNormalSettingsTest()
        {
            var settings = TestData.CreateNormalSettings();

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => settings,
                resultValueVerifier: new ValueVerifier<DatabaseFieldSpecialSettingDefinitionSettingsUnion>(result =>
                    {
                        CustomAssert.AreItemEquals(result.AsNormalSettings(), settings);
                    }
                )
            );
        }

        #endregion

        #region IDatabaseFieldSpecialSettingDefinitionLoadFileSettings

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void CastFromLoadFileSettingsTest()
        {
            var settings = TestData.CreateLoadFileSettings();

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => settings,
                resultValueVerifier: new ValueVerifier<DatabaseFieldSpecialSettingDefinitionSettingsUnion>(result =>
                    {
                        CustomAssert.AreItemEquals(result.AsLoadFileSettings(), settings);
                    }
                )
            );
        }

        #endregion

        #region IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void CastFromDatabaseReferenceSettingsTest()
        {
            var settings = TestData.CreateDatabaseReferenceSettings();

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => settings,
                resultValueVerifier: new ValueVerifier<DatabaseFieldSpecialSettingDefinitionSettingsUnion>(result =>
                    {
                        CustomAssert.AreItemEquals(result.AsDatabaseReferenceSettings(), settings);
                    }
                )
            );
        }

        #endregion

        #region IDatabaseFieldSpecialSettingDefinitionManualSettings

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void CastFromManualSettingsTest()
        {
            var settings = TestData.CreateManualSettings();

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => settings,
                resultValueVerifier: new ValueVerifier<DatabaseFieldSpecialSettingDefinitionSettingsUnion>(result =>
                    {
                        CustomAssert.AreItemEquals(result.AsManualSettings(), settings);
                    }
                )
            );
        }

        #endregion

        #endregion

        #endregion

        #region Operation

        private static readonly object?[][] OperatorEqualTestCaseSource =
        {
            // [left, right, expectedEqual]
            new object?[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()),
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()),
                true,
            },
            new object?[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()),
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings(initValue: 5)),
                false,
            },
            new object?[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()),
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateLoadFileSettings()),
                false,
            },
            new object?[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()),
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateDatabaseReferenceSettings()),
                false,
            },
            new object?[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()),
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateManualSettings()),
                false,
            },
            new object?[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()),
                null,
                false,
            },
            new object?[]
            {
                null,
                new DatabaseFieldSpecialSettingDefinitionSettingsUnion(TestData.CreateNormalSettings()),
                false,
            },
            new object?[] { null, null, true },
        };

        #region Equal

        /// <summary>
        ///     等価比較演算 == が意図した値を返すこと。
        /// </summary>
        [TestCaseSource(nameof(OperatorEqualTestCaseSource))]
        public static void OperatorEqualTest(
            DatabaseFieldSpecialSettingDefinitionSettingsUnion? left,
            DatabaseFieldSpecialSettingDefinitionSettingsUnion? right,
            bool expectedEqual
        )
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => left == right,
                resultValueVerifier: ValueVerifier.AreEquals(expectedEqual)
            );
        }

        #endregion

        #region NotEqual

        /// <summary>
        ///     等価比較演算 != が意図した値を返すこと。
        /// </summary>
        [TestCaseSource(nameof(OperatorEqualTestCaseSource))]
        public static void OperatorNotEqualTest(
            DatabaseFieldSpecialSettingDefinitionSettingsUnion? left,
            DatabaseFieldSpecialSettingDefinitionSettingsUnion? right,
            bool expectedEqual
        )
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => left != right,
                resultValueVerifier: ValueVerifier.AreEquals(!expectedEqual)
            );
        }

        #endregion

        #endregion

        #region TestData

        private static class TestData
        {
            public static DatabaseFieldSpecialSettingDefinitionNormalSettings CreateNormalSettings(int initValue = 3)
                => new()
                {
                    InitValue = new DatabaseValueInt(initValue),
                };

            public static DatabaseFieldSpecialSettingDefinitionLoadFileSettings CreateLoadFileSettings(
                string folderName = "DirName",
                bool isOmitFolderName = true
            )
                => new()
                {
                    FolderName = folderName,
                    IsOmitFolderName = isOmitFolderName,
                };

            public static DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
                CreateDatabaseReferenceSettings(
                    int initValue = 3,
                    int databaseDbTypeId = 4,
                    int databaseReferKindCode = 0,
                    bool isUseAdditionalItem = true,
                    string additionalCase1 = "Case 1",
                    string additionalCase2 = "Case 2",
                    string additionalCase3 = "Case 3"
                )
                => new()
                {
                    InitValue = new DatabaseValueInt(initValue),
                    DatabaseDbTypeId = databaseDbTypeId,
                    DatabaseReferKind = DatabaseReferType.FromCode(databaseReferKindCode),
                    IsUseAdditionalItems = isUseAdditionalItem,
                    AdditionalCase1 = additionalCase1,
                    AdditionalCase2 = additionalCase2,
                    AdditionalCase3 = additionalCase3,
                };

            public static DatabaseFieldSpecialSettingDefinitionManualSettings CreateManualSettings(
                int initValue = 3,
                IEnumerable<(int number, string description)>? cases = null
            )
                => new()
                {
                    InitValue = new DatabaseValueInt(initValue),
                    SpecialCases = new DatabaseValueCaseListSettings(
                        cases is null
                            ? new DatabaseValueCase[]
                            {
                                new(1, "Case 1"),
                                new(2, "Case 2"),
                                new(3, "Case 3"),
                            }
                            : cases.Select(item => new DatabaseValueCase(item.number, item.description)).ToArray()
                    ),
                };
        }

        #endregion
    }
}
