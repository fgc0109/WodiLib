using System;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn.ValueObject
{
    [TestFixture]
    public class ConditionRightTest : TestFixtureBase
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
        [TestCase(-999999)]
        [TestCase(999999)]
        public static void ConstructorIntTest_Success(int value)
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new ConditionRight(value),
                instanceVerifier: new ValueVerifier<ConditionRight>(instance =>
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
        [TestCase(-1000000)]
        [TestCase(1000000)]
        public static void ConstructorIntTest_Failure_OutOfRange(int value)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new ConditionRight(value),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region Cast

        #region From

        /// <summary>
        ///     int から ConditionRight に暗黙的型変換できること。
        /// </summary>
        [TestCase(-999999)]
        [TestCase(999999)]
        public static void CastIntToConditionRightTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => value,
                resultValueVerifier: new ValueVerifier<ConditionRight>(actual =>
                    {
                        Assert.AreEqual(actual.RawValue, value);
                    }
                )
            );
        }

        /// <summary>
        ///     許容範囲外の値から ConditionRight に暗黙的型変換した場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1000000)]
        [TestCase(1000000)]
        public static void CastIntToConditionRightTest_Failure_OutOfRange(int value)
        {
            staticFunctionTestHelper.StaticFuncFailure<ConditionRight>(
                execFunc: () => value,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region To

        /// <summary>
        ///     ConditionRight から int に暗黙的型変換できること。
        /// </summary>
        [TestCase(-999999)]
        [TestCase(999999)]
        public static void CastConditionRightToIntTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => new ConditionRight(value),
                resultValueVerifier: ValueVerifier<int>.AreEquals(value)
            );
        }

        #endregion

        #endregion

        #region Operation

        #region Equal / Equals(Method)

        /// <summary>
        ///     等価比較演算 == が意図した値を返すこと
        /// </summary>
        [TestCase(-999999, -999999, true)]
        [TestCase(-999999, 999999, false)]
        public static void OperatorEqualTest(int left, int right, bool expected)
        {
            var leftValue = (ConditionRight)left;
            var rightValue = (ConditionRight)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue == rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     等価比較演算 != が意図した値を返すこと
        /// </summary>
        [TestCase(-999999, -999999, false)]
        [TestCase(-999999, 999999, true)]
        public static void OperatorNotEqualTest(int left, int right, bool expected)
        {
            var leftValue = (ConditionRight)left;
            var rightValue = (ConditionRight)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue != rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと
        /// </summary>
        [TestCase(-999999, -999999, true)]
        [TestCase(-999999, 999999, false)]
        public static void OperatorEqualsTest(int left, int right, bool expected)
        {
            var leftValue = (ConditionRight)left;
            var rightValue = (ConditionRight)right;

            pureFunctionTestHelper.PureFuncSuccess(
                instance: leftValue,
                execFunc: target => target.Equals(rightValue),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #endregion
    }
}
