using System;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn.ValueObject
{
    [TestFixture]
    public class SpareNumberVariableNumberTest : TestFixtureBase
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
        [TestCase(9)]
        public static void ConstructorIntTest_Success(int value)
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new SpareNumberVariableNumber(value),
                instanceVerifier: new ValueVerifier<SpareNumberVariableNumber>(instance =>
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
        [TestCase(10)]
        public static void ConstructorIntTest_Failure_OutOfRange(int value)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new SpareNumberVariableNumber(value),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region Cast

        #region From

        /// <summary>
        ///     int から SpareNumberVariableNumber に暗黙的型変換できること。
        /// </summary>
        [TestCase(0)]
        [TestCase(9)]
        public static void CastIntToSpareNumberVariableNumberTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => value,
                resultValueVerifier: new ValueVerifier<SpareNumberVariableNumber>(actual =>
                    {
                        Assert.AreEqual(actual.RawValue, value);
                    }
                )
            );
        }

        /// <summary>
        ///     許容範囲外の値から SpareNumberVariableNumber に暗黙的型変換したとき
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(10)]
        public static void CastIntToSpareNumberVariableNumberTest_Failure_OutOfRange(int value)
        {
            staticFunctionTestHelper.StaticFuncFailure<SpareNumberVariableNumber>(
                execFunc: () => value,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region To

        /// <summary>
        ///     SpareNumberVariableNumber から int に暗黙的型変換できること。
        /// </summary>
        [TestCase(0)]
        [TestCase(9)]
        public static void CastSpareNumberVariableNumberToIntTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => new SpareNumberVariableNumber(value),
                resultValueVerifier: new ValueVerifier<int>(actual => { Assert.AreEqual(actual, value); })
            );
        }

        #endregion

        #endregion

        #region Operation

        #region Equal / Equals(Method)

        /// <summary>
        ///     等価比較演算 == が意図した値を返すこと
        /// </summary>
        [TestCase(0, 0, true)]
        [TestCase(0, 9, false)]
        public static void OperatorEqualTest(int left, int right, bool expected)
        {
            var leftValue = (SpareNumberVariableNumber)left;
            var rightValue = (SpareNumberVariableNumber)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue == rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     等価比較演算 != が意図した値を返すこと
        /// </summary>
        [TestCase(0, 0, false)]
        [TestCase(0, 9, true)]
        public static void OperatorNotEqualTest(int left, int right, bool expected)
        {
            var leftValue = (SpareNumberVariableNumber)left;
            var rightValue = (SpareNumberVariableNumber)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue != rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと
        /// </summary>
        [TestCase(0, 0, true)]
        [TestCase(0, 9, false)]
        public static void OperatorEqualsTest(int left, int right, bool expected)
        {
            var leftValue = (SpareNumberVariableNumber)left;
            var rightValue = (SpareNumberVariableNumber)right;

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
