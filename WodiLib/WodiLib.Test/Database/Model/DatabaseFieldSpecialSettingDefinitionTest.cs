using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Model
{
    [TestFixture]
    public class DatabaseFieldSpecialSettingDefinitionSettingsTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Normal

        /// <summary>
        ///     DatabaseFieldSpecialSettingDefinitionNormalSettings を引数に取る場合、
        ///     正常に終了すること。
        ///     <para>
        ///         作成したインスタンスが DatabaseFieldSpecialSettingDefinitionNormalSettings にキャスト可能であり、
        ///         DatabaseFieldSpecialSettingDefinitionLoadFileSettings,
        ///         DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings,
        ///         DatabaseFieldSpecialSettingDefinitionManualSettings にキャスト不可能であること。
        ///     </para>
        /// </summary>
        [Test]
        public static void ConstructorTest_Normal_Success()
        {
            var impl = new DatabaseFieldSpecialSettingDefinitionNormalSettings();
            constructorTestHelper.ConstructorSuccess(
                factory: () => new DatabaseFieldSpecialSettingDefinitionSettings(impl),
                instanceVerifier: new ValueVerifier<IDatabaseFieldSpecialSettingDefinitionSettings>(instance =>
                    {
                        Assert.IsTrue(instance.TryCastNormalSettings(out _));
                        Assert.IsFalse(instance.TryCastLoadFileSettings(out _));
                        Assert.IsFalse(instance.TryCastDatabaseReferenceSettings(out _));
                        Assert.IsFalse(instance.TryCastManualSettings(out _));
                    }
                )
            );
        }

        /// <summary>
        ///     DatabaseFieldSpecialSettingDefinitionNormalSettings からキャストできること。
        /// </summary>
        [Test]
        public static void CastFromTest_Normal_Success()
        {
            var impl = new DatabaseFieldSpecialSettingDefinitionNormalSettings();
            DatabaseFieldSpecialSettingDefinitionSettings casted = impl;
            Assert.IsNotNull(casted);
        }

        #endregion

        #region LoadFile

        /// <summary>
        ///     DatabaseFieldSpecialSettingDefinitionNormalSettings を引数に取る場合、
        ///     正常に終了すること。
        ///     <para>
        ///         作成したインスタンスが DatabaseFieldSpecialSettingDefinitionLoadFileSettings にキャスト可能であり、
        ///         DatabaseFieldSpecialSettingDefinitionNormalSettings,
        ///         DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings,
        ///         DatabaseFieldSpecialSettingDefinitionManualSettings にキャスト不可能であること。
        ///     </para>
        /// </summary>
        [Test]
        public static void ConstructorTest_LoadFile_Success()
        {
            var impl = new DatabaseFieldSpecialSettingDefinitionLoadFileSettings();
            constructorTestHelper.ConstructorSuccess(
                factory: () => new DatabaseFieldSpecialSettingDefinitionSettings(impl),
                instanceVerifier: new ValueVerifier<IDatabaseFieldSpecialSettingDefinitionSettings>(instance =>
                    {
                        Assert.IsFalse(instance.TryCastNormalSettings(out _));
                        Assert.IsTrue(instance.TryCastLoadFileSettings(out _));
                        Assert.IsFalse(instance.TryCastDatabaseReferenceSettings(out _));
                        Assert.IsFalse(instance.TryCastManualSettings(out _));
                    }
                )
            );
        }

        /// <summary>
        ///     DatabaseFieldSpecialSettingDefinitionLoadFileSettings からキャストできること。
        /// </summary>
        [Test]
        public static void CastFromTest_LoadFile_Success()
        {
            var impl = new DatabaseFieldSpecialSettingDefinitionLoadFileSettings();
            DatabaseFieldSpecialSettingDefinitionSettings casted = impl;
            Assert.IsNotNull(casted);
        }

        #endregion

        #region DatabaseReference

        /// <summary>
        ///     DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings を引数に取る場合、
        ///     正常に終了すること。
        ///     <para>
        ///         作成したインスタンスが DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings にキャスト可能であり、
        ///         DatabaseFieldSpecialSettingDefinitionNormalSettings,
        ///         DatabaseFieldSpecialSettingDefinitionLoadFileSettings,
        ///         DatabaseFieldSpecialSettingDefinitionManualSettings にキャスト不可能であること。
        ///     </para>
        /// </summary>
        [Test]
        public static void ConstructorTest_DatabaseReference_Success()
        {
            var impl = new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings();
            constructorTestHelper.ConstructorSuccess(
                factory: () => new DatabaseFieldSpecialSettingDefinitionSettings(impl),
                instanceVerifier: new ValueVerifier<IDatabaseFieldSpecialSettingDefinitionSettings>(instance =>
                    {
                        Assert.IsFalse(instance.TryCastNormalSettings(out _));
                        Assert.IsFalse(instance.TryCastLoadFileSettings(out _));
                        Assert.IsTrue(instance.TryCastDatabaseReferenceSettings(out _));
                        Assert.IsFalse(instance.TryCastManualSettings(out _));
                    }
                )
            );
        }

        /// <summary>
        ///     DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings からキャストできること。
        /// </summary>
        [Test]
        public static void CastFromTest_DatabaseReference_Success()
        {
            var impl = new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings();
            DatabaseFieldSpecialSettingDefinitionSettings casted = impl;
            Assert.IsNotNull(casted);
        }

        #endregion

        #region Manual

        /// <summary>
        ///     DatabaseFieldSpecialSettingDefinitionManualSettings を引数に取る場合、
        ///     正常に終了すること。
        ///     <para>
        ///         作成したインスタンスが DatabaseFieldSpecialSettingDefinitionManualSettings にキャスト可能であり、
        ///         DatabaseFieldSpecialSettingDefinitionNormalSettings,
        ///         DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings,
        ///         DatabaseFieldSpecialSettingDefinitionLoadFileSettings にキャスト不可能であること。
        ///     </para>
        /// </summary>
        [Test]
        public static void ConstructorTest_Manual_Success()
        {
            var impl = new DatabaseFieldSpecialSettingDefinitionManualSettings();
            constructorTestHelper.ConstructorSuccess(
                factory: () => new DatabaseFieldSpecialSettingDefinitionSettings(impl),
                instanceVerifier: new ValueVerifier<IDatabaseFieldSpecialSettingDefinitionSettings>(instance =>
                    {
                        Assert.IsFalse(instance.TryCastNormalSettings(out _));
                        Assert.IsFalse(instance.TryCastLoadFileSettings(out _));
                        Assert.IsFalse(instance.TryCastDatabaseReferenceSettings(out _));
                        Assert.IsTrue(instance.TryCastManualSettings(out _));
                    }
                )
            );
        }

        /// <summary>
        ///     DatabaseFieldSpecialSettingDefinitionManualSettings からキャストできること。
        /// </summary>
        [Test]
        public static void CastFromTest_Manual_Success()
        {
            var impl = new DatabaseFieldSpecialSettingDefinitionManualSettings();
            DatabaseFieldSpecialSettingDefinitionSettings casted = impl;
            Assert.IsNotNull(casted);
        }

        #endregion
    }

    [TestFixture]
    public class DatabaseFieldSpecialSettingDefinitionTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Normal

        #region Properties

        #region SettingType

        /// <summary>
        ///     プロパティ SettingType の取得に成功すること。
        /// </summary>
        [Test]
        public static void SettingTypeGetTest_Normal_Success()
        {
            var instance = new DatabaseFieldSpecialSettingDefinition(
                new DatabaseFieldSpecialSettingDefinitionNormal()
            );
            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.SettingType,
                getValueVerifier: ValueVerifier<DatabaseFieldSpecialSettingType>.AreEquals(
                    DatabaseFieldSpecialSettingType.Normal
                )
            );
        }

        #endregion

        #region DefaultType

        /// <summary>
        ///     プロパティ DefaultType の取得に成功すること。
        /// </summary>
        [Test]
        public static void DefaultTypeGetTest_Normal_Success()
        {
            var instance = new DatabaseFieldSpecialSettingDefinition(
                new DatabaseFieldSpecialSettingDefinitionNormal()
            );
            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.DefaultType,
                getValueVerifier: ValueVerifier<DatabaseFieldType>.AreEquals(DatabaseFieldType.Int)
            );
        }

        #endregion

        #region InitValue

        /// <summary>
        ///     プロパティ InitValue の取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void InitValueGetAndSetTest_Normal_Success()
        {
            var initValue = new DatabaseValueInt(64);
            var instance = new DatabaseFieldSpecialSettingDefinition(
                new DatabaseFieldSpecialSettingDefinitionNormal
                {
                    InitValue = initValue,
                }
            );
            var setItem = new DatabaseValueInt(128);

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DatabaseFieldSpecialSettingDefinition.InitValue),
                setItem,
                isValueEqualsBefore: false,
                setter: (x, v) => x.InitValue = v,
                getter: x => x.InitValue,
                getValueVerifier: ValueVerifier.AreEquals(setItem)
            );
        }

        /// <summary>
        ///     プロパティ InitValue に null を設定した場合、
        ///     PropertyNullException が発生すること。
        /// </summary>
        [Test]
        public static void InitValueSetTest_Normal_Failure_PropertyNullException()
        {
            var instance = new DatabaseFieldSpecialSettingDefinition(
                new DatabaseFieldSpecialSettingDefinitionNormal()
            );
            propertyTestHelper.PropertySetFailure(
                instance,
                setItem: (DatabaseValueInt)null!,
                setter: (x, v) => x.InitValue = v,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(PropertyNullException))
            );
        }

        #endregion

        #endregion

        #region Constructors

        #region IDatabaseFieldSpecialSettingDefinitionSettings

        /// <summary>
        ///     コンストラクタが正常に終了すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_IDatabaseFieldSpecialSettingDefinitionSettings_Normal_Success()
        {
            IDatabaseFieldSpecialSettingDefinitionSettings settings =
                new DatabaseFieldSpecialSettingDefinitionNormalSettings();
            constructorTestHelper.ConstructorSuccess(
                factory: () => new DatabaseFieldSpecialSettingDefinition(settings),
                instanceVerifier: ValueVerifier.AreItemEquals(settings)
            );
        }

        /// <summary>
        ///     settings が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_IDatabaseFieldSpecialSettingDefinitionSettings_Normal_Failure_NullArgs()
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseFieldSpecialSettingDefinition(
                    (IDatabaseFieldSpecialSettingDefinitionSettings)null!
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     設定DTOに null 要素が含まれる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_IDatabaseFieldSpecialSettingDefinitionSettings_Normal_Failure_HasNullItem()
        {
            var settings = new DatabaseFieldSpecialSettingDefinitionNormalSettings
            {
                InitValue = null!,
            };
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseFieldSpecialSettingDefinition(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #endregion

        #region Methods

        // 各メソッドは委譲先に処理を移譲するだけのためテストしない
        // DeepClone のみ、通常のModelクラスとは異なる処理のため例外としてテスト実施

        #region DeepClone

        /// <summary>
        ///     <para>ディープコピーがコピー元と同一値であること。</para>
        ///     <para>コピーしたインスタンスを編集した場合、元のインスタンスが変化しないこと。</para>
        /// </summary>
        [Test]
        public static void DeepCloneTest_Normal_Success_Mutable()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionNormal(
                new DatabaseFieldSpecialSettingDefinitionNormalSettings
                {
                    InitValue = new DatabaseValueInt(123),
                }
            );
            deepCloneTestHelper.DeepClone(
                instance,
                resultValueVerifier: null
            );

            var cloned = instance.DeepClone();
            cloned.InitValue = new DatabaseValueInt(456);
            Assert.AreEqual(cloned.InitValue, new DatabaseValueInt(456));
            Assert.AreEqual(instance.InitValue, new DatabaseValueInt(123));
        }

        /// <summary>
        ///     ディープコピーがコピー元と同一値であること。
        /// </summary>
        [Test]
        public static void DeepCloneTest_Normal_Success_Immutable()
        {
            ReadOnlyDatabaseFieldSpecialSettingDefinitionNormal instance =
                new DatabaseFieldSpecialSettingDefinitionNormal(
                    new DatabaseFieldSpecialSettingDefinitionNormalSettings
                    {
                        InitValue = new DatabaseValueInt(123),
                    }
                );
            deepCloneTestHelper.DeepClone(
                instance,
                resultValueVerifier: null
            );
        }

        #endregion

        #endregion

        #region Operations

        #region From

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void ImplicitOperatorFromTest_Normal_NonNull()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionNormal();
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => instance,
                resultValueVerifier: ValueVerifier<DatabaseFieldSpecialSettingDefinition>.AreItemEquals(instance)
            );
        }

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void ImplicitOperatorFromTest_Normal_Null()
        {
            DatabaseFieldSpecialSettingDefinitionNormal? instance = null;
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => instance,
                resultValueVerifier: ValueVerifier<DatabaseFieldSpecialSettingDefinition?>.IsNull()
            );
        }

        #endregion

        #endregion

        #endregion

        #region LoadFile

        #region Properties

        #region SettingType

        /// <summary>
        ///     プロパティ SettingType の取得に成功すること。
        /// </summary>
        [Test]
        public static void SettingTypeGetTest_LoadFile_Success()
        {
            var instance = new DatabaseFieldSpecialSettingDefinition(
                new DatabaseFieldSpecialSettingDefinitionLoadFile()
            );
            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.SettingType,
                getValueVerifier: ValueVerifier<DatabaseFieldSpecialSettingType>.AreEquals(
                    DatabaseFieldSpecialSettingType.LoadFile
                )
            );
        }

        #endregion

        #region DefaultType

        /// <summary>
        ///     プロパティ DefaultType の取得に成功すること。
        /// </summary>
        [Test]
        public static void DefaultTypeGetTest_LoadFile_Success()
        {
            var instance = new DatabaseFieldSpecialSettingDefinition(
                new DatabaseFieldSpecialSettingDefinitionLoadFile()
            );
            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.DefaultType,
                getValueVerifier: ValueVerifier<DatabaseFieldType>.AreEquals(DatabaseFieldType.String)
            );
        }

        #endregion

        #region InitValue

        /// <summary>
        ///     プロパティ InitValue の取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void InitValueGetAndSetTest_LoadFile_Success()
        {
            var initValue = new DatabaseValueInt(64);
            var instance = new DatabaseFieldSpecialSettingDefinition(
                new DatabaseFieldSpecialSettingDefinitionLoadFile
                {
                    InitValue = initValue,
                }
            );
            var setItem = new DatabaseValueInt(128);

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DatabaseFieldSpecialSettingDefinition.InitValue),
                setItem,
                isValueEqualsBefore: false,
                setter: (x, v) => x.InitValue = v,
                getter: x => x.InitValue,
                getValueVerifier: ValueVerifier.AreEquals(setItem)
            );
        }

        /// <summary>
        ///     プロパティ InitValue に null を設定した場合、
        ///     PropertyNullException が発生すること。
        /// </summary>
        [Test]
        public static void InitValueSetTest_LoadFile_Failure_PropertyNullException()
        {
            var instance = new DatabaseFieldSpecialSettingDefinition(
                new DatabaseFieldSpecialSettingDefinitionLoadFile()
            );

            propertyTestHelper.PropertySetFailure(
                instance,
                setItem: (DatabaseValueInt)null!,
                setter: (x, v) => x.InitValue = v,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(PropertyNullException))
            );
        }

        #endregion

        #endregion

        #region Constructors

        #region IDatabaseFieldSpecialSettingDefinitionSettings

        /// <summary>
        ///     コンストラクタが正常に終了すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_IDatabaseFieldSpecialSettingDefinitionSettings_LoadFile_Success()
        {
            IDatabaseFieldSpecialSettingDefinitionSettings settings =
                new DatabaseFieldSpecialSettingDefinitionLoadFileSettings();
            constructorTestHelper.ConstructorSuccess(
                factory: () => new DatabaseFieldSpecialSettingDefinition(settings),
                instanceVerifier: ValueVerifier.AreItemEquals(settings)
            );
        }

        /// <summary>
        ///     settings が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_IDatabaseFieldSpecialSettingDefinitionSettings_LoadFile_Failure_NullArgs()
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseFieldSpecialSettingDefinition(
                    (IDatabaseFieldSpecialSettingDefinitionSettings)null!
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     設定DTOに null 要素が含まれる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_IDatabaseFieldSpecialSettingDefinitionSettings_LoadFile_Failure_HasNullItem()
        {
            var settings = new DatabaseFieldSpecialSettingDefinitionLoadFileSettings
            {
                FolderName = null!,
            };
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseFieldSpecialSettingDefinition(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #endregion

        #region Methods

        // 各メソッドは委譲先に処理を移譲するだけのためテストしない
        // DeepClone のみ、通常のModelクラスとは異なる処理のため例外としてテスト実施

        #region DeepClone

        /// <summary>
        ///     <para>ディープコピーがコピー元と同一値であること。</para>
        ///     <para>コピーしたインスタンスを編集した場合、元のインスタンスが変化しないこと。</para>
        /// </summary>
        [Test]
        public static void DeepCloneTest_LoadFile_Success_Mutable()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionLoadFile(
                new DatabaseFieldSpecialSettingDefinitionLoadFileSettings
                {
                    InitValue = new DatabaseValueInt(123),
                    FolderName = "Test/Dir/Name",
                }
            );
            deepCloneTestHelper.DeepClone(
                instance,
                resultValueVerifier: null
            );

            var cloned = instance.DeepClone();
            cloned.FolderName = "Other/Dir/Name";
            Assert.AreEqual(cloned.FolderName, new DBSettingFolderName("Other/Dir/Name"));
            Assert.AreEqual(instance.FolderName, new DBSettingFolderName("Test/Dir/Name"));
        }

        /// <summary>
        ///     ディープコピーがコピー元と同一値であること。
        /// </summary>
        [Test]
        public static void DeepCloneTest_LoadFile_Success_Immutable()
        {
            ReadOnlyDatabaseFieldSpecialSettingDefinitionLoadFile instance =
                new DatabaseFieldSpecialSettingDefinitionLoadFile(
                    new DatabaseFieldSpecialSettingDefinitionLoadFileSettings
                    {
                        InitValue = new DatabaseValueInt(123),
                        FolderName = "Test/Dir/Name",
                    }
                );
            deepCloneTestHelper.DeepClone(
                instance,
                resultValueVerifier: null
            );
        }

        #endregion

        #endregion

        #region Operations

        #region From

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void ImplicitOperatorFromTest_LoadFile_NonNull()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionLoadFile();
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => instance,
                resultValueVerifier: ValueVerifier<DatabaseFieldSpecialSettingDefinition>.AreItemEquals(instance)
            );
        }

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void ImplicitOperatorFromTest_LoadFile_Null()
        {
            DatabaseFieldSpecialSettingDefinitionLoadFile? instance = null;
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => instance,
                resultValueVerifier: ValueVerifier<DatabaseFieldSpecialSettingDefinition?>.IsNull()
            );
        }

        #endregion

        #endregion

        #endregion

        #region DatabaseReference

        #region Properties

        #region SettingType

        /// <summary>
        ///     プロパティ SettingType の取得に成功すること。
        /// </summary>
        [Test]
        public static void SettingTypeGetTest_DatabaseReference_Success()
        {
            var instance = new DatabaseFieldSpecialSettingDefinition(
                new DatabaseFieldSpecialSettingDefinitionDatabaseReference()
            );
            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.SettingType,
                getValueVerifier: ValueVerifier<DatabaseFieldSpecialSettingType>.AreEquals(
                    DatabaseFieldSpecialSettingType.ReferDatabase
                )
            );
        }

        #endregion

        #region DefaultType

        /// <summary>
        ///     プロパティ DefaultType の取得に成功すること。
        /// </summary>
        [Test]
        public static void DefaultTypeGetTest_DatabaseReference_Success()
        {
            var instance = new DatabaseFieldSpecialSettingDefinition(
                new DatabaseFieldSpecialSettingDefinitionDatabaseReference()
            );
            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.DefaultType,
                getValueVerifier: ValueVerifier<DatabaseFieldType>.AreEquals(DatabaseFieldType.Int)
            );
        }

        #endregion

        #region InitValue

        /// <summary>
        ///     プロパティ InitValue の取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void InitValueGetAndSetTest_DatabaseReference_Success()
        {
            var initValue = new DatabaseValueInt(64);
            var instance = new DatabaseFieldSpecialSettingDefinition(
                new DatabaseFieldSpecialSettingDefinitionDatabaseReference
                {
                    InitValue = initValue,
                }
            );
            var setItem = new DatabaseValueInt(128);

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DatabaseFieldSpecialSettingDefinition.InitValue),
                setItem,
                isValueEqualsBefore: false,
                setter: (x, v) => x.InitValue = v,
                getter: x => x.InitValue,
                getValueVerifier: ValueVerifier.AreEquals(setItem)
            );
        }

        /// <summary>
        ///     プロパティ InitValue に null を設定した場合、
        ///     PropertyNullException が発生すること。
        /// </summary>
        [Test]
        public static void InitValueSetTest_DatabaseReference_Failure_PropertyNullException()
        {
            var instance = new DatabaseFieldSpecialSettingDefinition(
                new DatabaseFieldSpecialSettingDefinitionDatabaseReference()
            );

            propertyTestHelper.PropertySetFailure(
                instance,
                setItem: (DatabaseValueInt)null!,
                setter: (x, v) => x.InitValue = v,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(PropertyNullException))
            );
        }

        #endregion

        #endregion

        #region Constructors

        #region IDatabaseFieldSpecialSettingDefinitionSettings

        /// <summary>
        ///     コンストラクタが正常に終了すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_IDatabaseFieldSpecialSettingDefinitionSettings_DatabaseReference_Success()
        {
            IDatabaseFieldSpecialSettingDefinitionSettings settings =
                new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings();
            constructorTestHelper.ConstructorSuccess(
                factory: () => new DatabaseFieldSpecialSettingDefinition(settings),
                instanceVerifier: ValueVerifier.AreItemEquals(settings)
            );
        }

        /// <summary>
        ///     settings が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void
            ConstructorTest_IDatabaseFieldSpecialSettingDefinitionSettings_DatabaseReference_Failure_NullArgs()
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseFieldSpecialSettingDefinition(
                    (IDatabaseFieldSpecialSettingDefinitionSettings)null!
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     設定DTOに null 要素が含まれる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void
            ConstructorTest_IDatabaseFieldSpecialSettingDefinitionSettings_DatabaseReference_Failure_HasNullItem()
        {
            var settings = new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
            {
                InitValue = null!,
            };
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseFieldSpecialSettingDefinition(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #endregion

        #region Methods

        // 各メソッドは委譲先に処理を移譲するだけのためテストしない
        // DeepClone のみ、通常のModelクラスとは異なる処理のため例外としてテスト実施

        #region DeepClone

        /// <summary>
        ///     <para>ディープコピーがコピー元と同一値であること。</para>
        ///     <para>コピーしたインスタンスを編集した場合、元のインスタンスが変化しないこと。</para>
        /// </summary>
        [Test]
        public static void DeepCloneTest_DatabaseReference_Success_Mutable()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(
                new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
                {
                    InitValue = new DatabaseValueInt(123),
                    DatabaseReferKind = DatabaseReferType.System,
                    DatabaseDbTypeId = 7,
                    IsUseAdditionalItems = true,
                    AdditionalCase1 = "Case 1",
                    AdditionalCase2 = "Case 2",
                    AdditionalCase3 = "Case 3",
                }
            );
            deepCloneTestHelper.DeepClone(
                instance,
                resultValueVerifier: null
            );

            var cloned = instance.DeepClone();
            cloned.DatabaseReferKind = DatabaseReferType.User;
            Assert.AreEqual(cloned.DatabaseReferKind, DatabaseReferType.User);
            Assert.AreEqual(instance.DatabaseReferKind, DatabaseReferType.System);
        }

        /// <summary>
        ///     ディープコピーがコピー元と同一値であること。
        /// </summary>
        [Test]
        public static void DeepCloneTest_DatabaseReference_Success_Immutable()
        {
            ReadOnlyDatabaseFieldSpecialSettingDefinitionDatabaseReference instance =
                new DatabaseFieldSpecialSettingDefinitionDatabaseReference(
                    new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
                    {
                        InitValue = new DatabaseValueInt(123),
                        DatabaseReferKind = DatabaseReferType.System,
                        DatabaseDbTypeId = 7,
                        IsUseAdditionalItems = true,
                        AdditionalCase1 = "Case 1",
                        AdditionalCase2 = "Case 2",
                        AdditionalCase3 = "Case 3",
                    }
                );
            deepCloneTestHelper.DeepClone(
                instance,
                resultValueVerifier: null
            );
        }

        #endregion

        #endregion

        #region Operations

        #region From

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void ImplicitOperatorFromTest_DatabaseReference_NonNull()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference();
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => instance,
                resultValueVerifier: ValueVerifier<DatabaseFieldSpecialSettingDefinition>.AreItemEquals(instance)
            );
        }

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void ImplicitOperatorFromTest_DatabaseReference_Null()
        {
            DatabaseFieldSpecialSettingDefinitionDatabaseReference? instance = null;
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => instance,
                resultValueVerifier: ValueVerifier<DatabaseFieldSpecialSettingDefinition?>.IsNull()
            );
        }

        #endregion

        #endregion

        #endregion

        #region Manual

        #region Properties

        #region SettingType

        /// <summary>
        ///     プロパティ SettingType の取得に成功すること。
        /// </summary>
        [Test]
        public static void SettingTypeGetTest_Manual_Success()
        {
            var instance = new DatabaseFieldSpecialSettingDefinition(
                new DatabaseFieldSpecialSettingDefinitionManual()
            );
            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.SettingType,
                getValueVerifier: ValueVerifier<DatabaseFieldSpecialSettingType>.AreEquals(
                    DatabaseFieldSpecialSettingType.Manual
                )
            );
        }

        #endregion

        #region DefaultType

        /// <summary>
        ///     プロパティ DefaultType の取得に成功すること。
        /// </summary>
        [Test]
        public static void DefaultTypeGetTest_Manual_Success()
        {
            var instance = new DatabaseFieldSpecialSettingDefinition(
                new DatabaseFieldSpecialSettingDefinitionManual()
            );
            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.DefaultType,
                getValueVerifier: ValueVerifier<DatabaseFieldType>.AreEquals(DatabaseFieldType.Int)
            );
        }

        #endregion

        #region InitValue

        /// <summary>
        ///     プロパティ InitValue の取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void InitValueGetAndSetTest_Manual_Success()
        {
            var initValue = new DatabaseValueInt(64);
            var instance = new DatabaseFieldSpecialSettingDefinition(
                new DatabaseFieldSpecialSettingDefinitionManual
                {
                    InitValue = initValue,
                }
            );
            var setItem = new DatabaseValueInt(128);

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DatabaseFieldSpecialSettingDefinition.InitValue),
                setItem,
                isValueEqualsBefore: false,
                setter: (x, v) => x.InitValue = v,
                getter: x => x.InitValue,
                getValueVerifier: ValueVerifier.AreEquals(setItem)
            );
        }

        /// <summary>
        ///     プロパティ InitValue に null を設定した場合、
        ///     PropertyNullException が発生すること。
        /// </summary>
        [Test]
        public static void InitValueSetTest_Manual_Failure_PropertyNullException()
        {
            var instance = new DatabaseFieldSpecialSettingDefinition(
                new DatabaseFieldSpecialSettingDefinitionManual()
            );

            propertyTestHelper.PropertySetFailure(
                instance,
                setItem: (DatabaseValueInt)null!,
                setter: (x, v) => x.InitValue = v,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(PropertyNullException))
            );
        }

        #endregion

        #endregion

        #region Constructors

        #region IDatabaseFieldSpecialSettingDefinitionSettings

        /// <summary>
        ///     コンストラクタが正常に終了すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_IDatabaseFieldSpecialSettingDefinitionSettings_Manual_Success()
        {
            IDatabaseFieldSpecialSettingDefinitionSettings settings =
                new DatabaseFieldSpecialSettingDefinitionManualSettings();
            constructorTestHelper.ConstructorSuccess(
                factory: () => new DatabaseFieldSpecialSettingDefinition(settings),
                instanceVerifier: ValueVerifier.AreItemEquals(settings)
            );
        }

        /// <summary>
        ///     settings が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_IDatabaseFieldSpecialSettingDefinitionSettings_Manual_Failure_NullArgs()
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseFieldSpecialSettingDefinition(
                    (IDatabaseFieldSpecialSettingDefinitionSettings)null!
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     設定DTOに null 要素が含まれる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_IDatabaseFieldSpecialSettingDefinitionSettings_Manual_Failure_HasNullItem()
        {
            var settings = new DatabaseFieldSpecialSettingDefinitionManualSettings
            {
                InitValue = null!,
            };
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseFieldSpecialSettingDefinition(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #endregion

        #region Methods

        #region DeepClone

        /// <summary>
        ///     <para>ディープコピーがコピー元と同一値であること。</para>
        ///     <para>コピーしたインスタンスを編集した場合、元のインスタンスが変化しないこと。</para>
        /// </summary>
        [Test]
        public static void DeepCloneTest_Manual_Success_Mutable()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionManual(
                new DatabaseFieldSpecialSettingDefinitionManualSettings
                {
                    InitValue = new DatabaseValueInt(123),
                    SpecialCases = new DatabaseValueCaseListSettings(
                        new DatabaseValueCase[]
                        {
                            new(1, "Case 1"),
                            new(3, "Case 3"),
                            new(5, "Case 5"),
                        }
                    ),
                }
            );
            deepCloneTestHelper.DeepClone(
                instance,
                resultValueVerifier: null
            );

            var cloned = instance.DeepClone();
            cloned.InitValue = new DatabaseValueInt(456);
            Assert.AreEqual(cloned.InitValue, new DatabaseValueInt(456));
            Assert.AreEqual(instance.InitValue, new DatabaseValueInt(123));
        }

        /// <summary>
        ///     ディープコピーがコピー元と同一値であること。
        /// </summary>
        [Test]
        public static void DeepCloneTest_Manual_Success_Immutable()
        {
            ReadOnlyDatabaseFieldSpecialSettingDefinitionManual instance =
                new DatabaseFieldSpecialSettingDefinitionManual(
                    new DatabaseFieldSpecialSettingDefinitionManualSettings
                    {
                        InitValue = new DatabaseValueInt(123),
                        SpecialCases = new DatabaseValueCaseListSettings(
                            new DatabaseValueCase[]
                            {
                                new(1, "Case 1"),
                                new(3, "Case 3"),
                                new(5, "Case 5"),
                            }
                        ),
                    }
                );
            deepCloneTestHelper.DeepClone(
                instance,
                resultValueVerifier: null
            );
        }

        #endregion

        #endregion

        #region Operations

        #region From

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void ImplicitOperatorFromTest_Manual_NonNull()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionManual();
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => instance,
                resultValueVerifier: ValueVerifier<DatabaseFieldSpecialSettingDefinition>.AreItemEquals(instance)
            );
        }

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void ImplicitOperatorFromTest_Manual_Null()
        {
            DatabaseFieldSpecialSettingDefinitionManual? instance = null;
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => instance,
                resultValueVerifier: ValueVerifier<DatabaseFieldSpecialSettingDefinition?>.IsNull()
            );
        }

        #endregion

        #endregion

        #endregion

        #region Common

        #region Constructors

        #region IDatabaseFieldSpecialSettingDefinitionSettings

        /// <summary>
        ///     settings が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_IDatabaseFieldSpecialSettingDefinitionSettings_Failure_NullArgs()
        {
            IDatabaseFieldSpecialSettingDefinitionSettings settings = null!;
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseFieldSpecialSettingDefinition(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion

        #endregion

        #endregion
    }
}
