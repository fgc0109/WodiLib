using System;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn.ValueObject
{
    [TestFixture]
    public class SideLengthTest : TestFixtureBase
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
        [TestCase(int.MaxValue)]
        public static void ConstructorIntTest_Success(int value)
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new SideLength(value),
                instanceVerifier: new ValueVerifier<SideLength>(instance =>
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
        public static void ConstructorIntTest_Failure_OutOfRange(int value)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new SideLength(value),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region Cast

        #region From

        /// <summary>
        ///     int から SideLength に暗黙的型変換できること。
        /// </summary>
        [TestCase(0)]
        [TestCase(int.MaxValue)]
        public static void CastIntToSideLengthTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => value,
                resultValueVerifier: new ValueVerifier<SideLength>(actual =>
                    {
                        Assert.AreEqual(actual.RawValue, value);
                    }
                )
            );
        }

        /// <summary>
        ///     許容範囲外の値から SideLength に暗黙的型変換した場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        public static void CastIntToSideLengthTest_Failure_OutOfRange(int value)
        {
            staticFunctionTestHelper.StaticFuncFailure<SideLength>(
                execFunc: () => value,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region To

        /// <summary>
        ///     SideLength から int に暗黙的型変換できること
        /// </summary>
        [TestCase(0)]
        [TestCase(int.MaxValue)]
        public static void CastSideLengthToIntTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => new SideLength(value),
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
        [TestCase(0, 0, true)]
        [TestCase(0, int.MaxValue, false)]
        public static void OperatorEqualTest(int left, int right, bool expected)
        {
            var leftValue = (SideLength)left;
            var rightValue = (SideLength)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue == rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     等価比較演算 != が意図した値を返すこと
        /// </summary>
        [TestCase(0, 0, false)]
        [TestCase(0, int.MaxValue, true)]
        public static void OperatorNotEqualTest(int left, int right, bool expected)
        {
            var leftValue = (SideLength)left;
            var rightValue = (SideLength)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue != rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと
        /// </summary>
        [TestCase(0, 0, true)]
        [TestCase(0, int.MaxValue, false)]
        public static void OperatorEqualsTest(int left, int right, bool expected)
        {
            var leftValue = (SideLength)left;
            var rightValue = (SideLength)right;

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
