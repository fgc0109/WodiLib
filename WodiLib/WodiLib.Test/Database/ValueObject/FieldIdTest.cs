using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.ValueObject
{
    [TestFixture]
    public class FieldIdTest : TestFixtureBase
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
        [TestCase(0)]
        [TestCase(99)]
        public static void ConstructorIntTest_Success(int value)
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new FieldId(value),
                instanceVerifier: new ValueVerifier<FieldId>(instance =>
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
        [TestCase(-1)]
        [TestCase(100)]
        public static void ConstructorIntTest_Failure_OutOfRange(int value)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new FieldId(value),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region Cast

        #region From

        /// <summary>
        ///     int から FieldId に暗黙的型変換できること。
        /// </summary>
        [TestCase(0)]
        [TestCase(99)]
        public static void CastIntToFieldIdTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => value,
                resultValueVerifier: ValueVerifier<FieldId>.AreEquals(value)
            );
        }

        /// <summary>
        ///     許容範囲外の値から FieldId に暗黙的型変換した場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(100)]
        public static void CastIntToFieldIdTest_Failure_OutOfRange(int value)
        {
            staticFunctionTestHelper.StaticFuncFailure<FieldId>(
                execFunc: () => value,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region To

        /// <summary>
        ///     FieldId から int に暗黙的型変換できること。
        /// </summary>
        [TestCase(0)]
        [TestCase(99)]
        public static void CastFieldIdToIntTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => new FieldId(value),
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
        [TestCase(0, 0, true)]
        [TestCase(0, 99, false)]
        public static void OperatorEqualTest(int left, int right, bool expected)
        {
            var leftValue = (FieldId)left;
            var rightValue = (FieldId)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue == rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     等価比較演算 != が意図した値を返すこと。
        /// </summary>
        [TestCase(0, 0, false)]
        [TestCase(0, 99, true)]
        public static void OperatorNotEqualTest(int left, int right, bool expected)
        {
            var leftValue = (FieldId)left;
            var rightValue = (FieldId)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue != rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと。
        /// </summary>
        [TestCase(0, 0, true)]
        [TestCase(0, 99, false)]
        public static void OperatorEqualsTest(int left, int right, bool expected)
        {
            var leftValue = (FieldId)left;
            var rightValue = (FieldId)right;

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
        public static void EqualsTest_FieldId(int left, int? right, bool expected)
        {
            var leftValue = (FieldId)left;
            var rightValue = (FieldId?)right;

            equalsTestHelper.Equals(
                leftValue,
                rightValue,
                expected
            );
        }

        private static readonly object?[][] EqualsObjectTestCaseSource =
        {
            // [left, right, expected]
            new object?[] { 1, new FieldId(1), true },
            new object?[] { 1, new FieldId(2), false },
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
            var leftValue = (FieldId)left;

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
