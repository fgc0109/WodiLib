using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.ValueObject
{
    [TestFixture]
    public class DatabaseFieldValueTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Properties

        #region Type

        private static readonly object[][] TypeGetterTest_Success_TestCaseSource =
        {
            // [instance, expected]
            new object[] { new DatabaseFieldValue(0), DatabaseFieldType.Int },
            new object[] { new DatabaseFieldValue("a"), DatabaseFieldType.String },
        };

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [TestCaseSource(nameof(TypeGetterTest_Success_TestCaseSource))]
        public static void TypeGetterTest_Success(DatabaseFieldValue instance, DatabaseFieldType expected)
        {
            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.Type,
                getValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #region IntValue

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void IntValueGetterTest_Success()
        {
            const int intValue = 205;
            var instance = new DatabaseFieldValue(intValue);
            var expected = new DatabaseValueInt(intValue);

            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.IntValue,
                getValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     文字列型の FieldValue から Int 値を取得しようとした場合、
        ///     PropertyAccessException が発生すること。
        /// </summary>
        [Test]
        public static void IntValueGetterTest_Failure_TypeMismatch()
        {
            var instance = new DatabaseFieldValue("Value");

            propertyTestHelper.PropertyGetFailure(
                instance,
                getter: target => target.IntValue,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(PropertyAccessException))
            );
        }

        #endregion

        #region StringValue

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void StringValueGetterTest_Success()
        {
            const string stringValue = "Str Value";
            var instance = new DatabaseFieldValue(stringValue);
            var expected = new DatabaseValueString(stringValue);

            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.StringValue,
                getValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     数値型の FieldValue から String 値を取得しようとした場合、
        ///     PropertyAccessException が発生すること。
        /// </summary>
        [Test]
        public static void StringValueGetterTest_Failure_TypeMismatch()
        {
            var instance = new DatabaseFieldValue(127);

            propertyTestHelper.PropertyGetFailure(
                instance,
                getter: target => target.StringValue,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(PropertyAccessException))
            );
        }

        #endregion

        #endregion

        #region Constructors

        #region From IntValue

        /// <summary>
        ///     コンストラクタが正常に終了すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_FromIntValue_Success()
        {
            var intValue = new DatabaseValueInt(75);

            constructorTestHelper.ConstructorSuccess(
                factory: () => new DatabaseFieldValue(intValue),
                instanceVerifier: new ValueVerifier<DatabaseFieldValue>(instance =>
                    {
                        Assert.AreEqual(DatabaseFieldType.Int, instance.Type);
                        Assert.AreEqual(intValue, instance.IntValue);
                    }
                )
            );
        }

        /// <summary>
        ///     intValue が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_FromIntValue_Failure_NullArgs()
        {
            DatabaseValueInt intValue = null!;

            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseFieldValue(intValue),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion

        #region From StringValue

        /// <summary>
        ///     コンストラクタが正常に終了すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_FromStringValue_Success()
        {
            var stringValue = new DatabaseValueString("FieldValue");

            constructorTestHelper.ConstructorSuccess(
                factory: () => new DatabaseFieldValue(stringValue),
                instanceVerifier: new ValueVerifier<DatabaseFieldValue>(instance =>
                    {
                        Assert.AreEqual(DatabaseFieldType.String, instance.Type);
                        Assert.AreEqual(stringValue, instance.StringValue);
                    }
                )
            );
        }

        /// <summary>
        ///     stringValue が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_FromStringValue_Failure_NullArgs()
        {
            DatabaseValueString stringValue = null!;

            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseFieldValue(stringValue),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion

        #region From DatabaseFieldType

        /// <summary>
        ///     コンストラクタが正常に終了すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_FromDatabaseFieldType_Success_FieldTypeInt()
        {
            var fieldType = DatabaseFieldType.Int;

            constructorTestHelper.ConstructorSuccess(
                factory: () => new DatabaseFieldValue(fieldType),
                instanceVerifier: new ValueVerifier<DatabaseFieldValue>(instance =>
                    {
                        Assert.AreEqual(DatabaseFieldType.Int, instance.Type);
                        Assert.AreEqual(0, instance.IntValue.RawValue);
                    }
                )
            );
        }

        /// <summary>
        ///     コンストラクタが正常に終了すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_FromDatabaseFieldType_Success_FieldTypeString()
        {
            var fieldType = DatabaseFieldType.String;

            constructorTestHelper.ConstructorSuccess(
                factory: () => new DatabaseFieldValue(fieldType),
                instanceVerifier: new ValueVerifier<DatabaseFieldValue>(instance =>
                    {
                        Assert.AreEqual(DatabaseFieldType.String, instance.Type);
                        Assert.AreEqual("", instance.StringValue.RawValue);
                    }
                )
            );
        }

        /// <summary>
        ///     fieldType が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_FromDatabaseFieldType_Failure_NullArgs()
        {
            DatabaseFieldType fieldType = null!;

            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseFieldValue(fieldType),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion

        #endregion

        #region Methods

        #region public

        #region GetDefaultValue

        private static readonly object[][] GetDefaultValueTest_Success_TestCaseSource =
        {
            // [instance, expected]
            new object[] { new DatabaseFieldValue(20), new DatabaseFieldValue(0) },
            new object[] { new DatabaseFieldValue("Item"), new DatabaseFieldValue("") },
        };

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [TestCaseSource(nameof(GetDefaultValueTest_Success_TestCaseSource))]
        public static void GetDefaultValueTest_Success(DatabaseFieldValue instance, DatabaseFieldValue expected)
        {
            pureFunctionTestHelper.PureFuncSuccess(
                instance: instance,
                execFunc: target => target.GetDefaultValue(),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #region GetHashCode

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [Test]
        public static void GetHashCodeTest_Success()
        {
            var instance1 = new DatabaseFieldValue(0);
            var instance2 = new DatabaseFieldValue(0);
            var instance3 = new DatabaseFieldValue(4);
            var instance4 = new DatabaseFieldValue("0");

            var hashCode1 = instance1.GetHashCode();
            var hashCode2 = instance2.GetHashCode();
            var hashCode3 = instance3.GetHashCode();
            var hashCode4 = instance4.GetHashCode();

            Assert.AreEqual(hashCode1, hashCode2);
            Assert.AreNotEqual(hashCode1, hashCode3);
            Assert.AreNotEqual(hashCode1, hashCode4);
        }

        #endregion

        #region Equals

        #region DatabaseFieldValue

        private static readonly object?[][] EqualsTest_DatabaseFieldValue_TestCaseSource =
        {
            // [left, right, expected]
            new object?[] { new DatabaseFieldValue(0), new DatabaseFieldValue(0), true },
            new object?[] { new DatabaseFieldValue(0), new DatabaseFieldValue("0"), false },
            new object?[] { new DatabaseFieldValue(0), null, false },
        };

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="expected"></param>
        [TestCaseSource(nameof(EqualsTest_DatabaseFieldValue_TestCaseSource))]
        public static void EqualsTest_DatabaseFieldValue(
            DatabaseFieldValue left,
            DatabaseFieldValue? right,
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
            new object?[] { new DatabaseFieldValue(0), new DatabaseFieldValue(0), true },
            new object?[] { new DatabaseFieldValue(0), new DatabaseFieldValue(1), false },
            new object?[] { new DatabaseFieldValue(0), new DatabaseFieldValue("0"), false },
            new object?[] { new DatabaseFieldValue(0), 0, false },
            new object?[] { new DatabaseFieldValue(0), null, false },
        };

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="expected"></param>
        [TestCaseSource(nameof(EqualsTest_Object_TestCaseSource))]
        public static void EqualsTest_Object(DatabaseFieldValue left, object? right, bool expected)
        {
            equalsTestHelper.Equals(
                left,
                right,
                expected
            );
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region Cast

        #region From

        #region DatabaseValueInt

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void CastFromDatabaseValueIntTest_Success()
        {
            var intValue = 98;
            var src = new DatabaseValueInt(intValue);
            var expected = new DatabaseFieldValue(intValue);

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => src,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #region DatabaseValueString

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void CastFromDatabaseValueStringTest_Success()
        {
            var stringValue = "Field Value";
            var src = new DatabaseValueString(stringValue);
            var expected = new DatabaseFieldValue(stringValue);

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => src,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #endregion

        #region To

        #region DatabaseValueInt

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void CastToDatabaseValueIntTest_Success()
        {
            var intValue = 98;
            var src = new DatabaseFieldValue(intValue);
            var expected = new DatabaseValueInt(intValue);

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => src,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #region DatabaseValueString

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void CastToDatabaseValueStringTest_Success()
        {
            var stringValue = "Field Value";
            var src = new DatabaseFieldValue(stringValue);
            var expected = new DatabaseValueString(stringValue);

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => src,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #endregion

        #endregion

        #region Operation

        private static readonly object?[][] OperatorEqualTestCaseSource =
        {
            // [left, right, expectedEqual]
            new object?[] { new DatabaseFieldValue(0), new DatabaseFieldValue(0), true },
            new object?[] { new DatabaseFieldValue(0), new DatabaseFieldValue(1), false },
            new object?[] { new DatabaseFieldValue(0), new DatabaseFieldValue("0"), false },
            new object?[] { new DatabaseFieldValue(0), null, false },
            new object?[] { null, new DatabaseFieldValue(0), false },
            new object?[] { new DatabaseFieldValue("Value"), null, false },
            new object?[] { null, new DatabaseFieldValue("Value"), false },
            new object?[] { null, null, true },
        };

        #region Equal

        /// <summary>
        ///     等価比較演算 == が意図した値を返すこと。
        /// </summary>
        [TestCaseSource(nameof(OperatorEqualTestCaseSource))]
        public static void OperatorEqualTest(DatabaseFieldValue? left, DatabaseFieldValue? right, bool expectedEqual)
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
        public static void OperatorNotEqualTest(DatabaseFieldValue? left, DatabaseFieldValue? right, bool expectedEqual)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => left != right,
                resultValueVerifier: ValueVerifier.AreEquals(!expectedEqual)
            );
        }

        #endregion

        #endregion
    }
}
