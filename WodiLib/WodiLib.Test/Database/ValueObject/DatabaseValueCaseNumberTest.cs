using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.ValueObject
{
    [TestFixture]
    public class DatabaseValueCaseNumberTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Constructor

        /// <summary>
        ///     コンストラクタが正常に終了すること。
        /// </summary>
        [TestCase(-9999999)]
        [TestCase(1400000000)]
        public static void ConstructorIntTest_Success(int value)
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new DatabaseValueCaseNumber(value),
                instanceVerifier: new ValueVerifier<DatabaseValueCaseNumber>(instance =>
                    {
                        // インスタンスが意図したとおり作成されること
                        Assert.AreEqual(instance.RawValue, value);
                    }
                )
            );
        }

        /// <summary>
        ///     引数に許容範囲外の値を指定した場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-10000000)]
        [TestCase(1400000001)]
        public static void ConstructorIntTest_Failure_OutOfRange(int value)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseValueCaseNumber(value),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region Cast

        #region From

        /// <summary>
        ///     int から DatabaseValueCaseNumber に暗黙的型変換できること。
        /// </summary>
        [TestCase(-9999999)]
        [TestCase(1400000000)]
        public static void CastIntToDatabaseValueCaseNumberTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => value,
                resultValueVerifier: ValueVerifier<DatabaseValueCaseNumber>.AreEquals(value)
            );
        }

        /// <summary>
        ///     許容範囲外の値から DatabaseValueCaseNumber に暗黙的型変換した場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-10000000)]
        [TestCase(1400000001)]
        public static void CastIntToDatabaseValueCaseNumberTest_Failure_OutOfRange(int value)
        {
            staticFunctionTestHelper.StaticFuncFailure<DatabaseValueCaseNumber>(
                execFunc: () => value,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region To

        /// <summary>
        ///     DatabaseValueCaseNumber から int に暗黙的型変換できること。
        /// </summary>
        [TestCase(-9999999)]
        [TestCase(1400000000)]
        public static void CastDatabaseValueCaseNumberToIntTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => new DatabaseValueCaseNumber(value),
                resultValueVerifier: ValueVerifier<int>.AreEquals(value)
            );
        }

        #endregion

        #endregion

        #region Operation

        #region Equal / Equals(Method)

        /// <summary>
        ///     等価比較演算 == が意図した値を返すこと。
        /// </summary>
        [TestCase(-9999999, -9999999, true)]
        [TestCase(-9999999, 1400000000, false)]
        public static void OperatorEqualTest(int left, int right, bool expected)
        {
            var leftValue = (DatabaseValueCaseNumber)left;
            var rightValue = (DatabaseValueCaseNumber)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue == rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     等価比較演算 != が意図した値を返すこと。
        /// </summary>
        [TestCase(-9999999, -9999999, false)]
        [TestCase(-9999999, 1400000000, true)]
        public static void OperatorNotEqualTest(int left, int right, bool expected)
        {
            var leftValue = (DatabaseValueCaseNumber)left;
            var rightValue = (DatabaseValueCaseNumber)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue != rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと。
        /// </summary>
        [TestCase(-9999999, -9999999, true)]
        [TestCase(-9999999, 1400000000, false)]
        public static void OperatorEqualsTest(int left, int right, bool expected)
        {
            var leftValue = (DatabaseValueCaseNumber)left;
            var rightValue = (DatabaseValueCaseNumber)right;

            pureFunctionTestHelper.PureFuncSuccess(
                instance: leftValue,
                execFunc: target => target.Equals(rightValue),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと。
        /// </summary>
        [TestCase(1, 1, true)]
        [TestCase(1, 2, false)]
        [TestCase(1, null, false)]
        public static void EqualsTest_DatabaseValueCaseNumber(int left, int? right, bool expected)
        {
            var leftValue = (DatabaseValueCaseNumber)left;
            var rightValue = (DatabaseValueCaseNumber?)right;

            equalsTestHelper.Equals(
                leftValue,
                rightValue,
                expected
            );
        }

        private static readonly object?[][] EqualsObjectTestCaseSource =
        {
            // [left, right, expected]
            new object?[] { 1, new DatabaseValueCaseNumber(1), true },
            new object?[] { 1, new DatabaseValueCaseNumber(2), false },
            new object?[] { 1, 1, false },
            new object?[] { 1, "1", false },
            new object?[] { 1, null, false },
        };

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと。
        /// </summary>
        [TestCaseSource(nameof(EqualsObjectTestCaseSource))]
        public static void EqualsTest_Object(int left, object? right, bool expected)
        {
            var leftValue = (DatabaseValueCaseNumber)left;

            equalsTestHelper.Equals(
                leftValue,
                right,
                expected
            );
        }

        #endregion

        #endregion
    }
}
