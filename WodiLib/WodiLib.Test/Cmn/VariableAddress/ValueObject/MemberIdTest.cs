using System;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn.ValueObject
{
    [TestFixture]
    public class MemberIdTest : TestFixtureBase
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
        [TestCase(1)]
        [TestCase(5)]
        public static void ConstructorIntTest_Success(int value)
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new MemberId(value),
                instanceVerifier: new ValueVerifier<MemberId>(instance =>
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
        [TestCase(0)]
        [TestCase(6)]
        public static void ConstructorIntTest_Failure_OutOfRange(int value)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new MemberId(value),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region Cast

        #region From

        /// <summary>
        ///     int から MemberId に暗黙的型変換できること。
        /// </summary>
        [TestCase(1)]
        [TestCase(5)]
        public static void CastIntToMemberIdTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => value,
                resultValueVerifier: new ValueVerifier<MemberId>(actual => { Assert.AreEqual(actual.RawValue, value); })
            );
        }

        /// <summary>
        ///     許容範囲外の値から MemberId に暗黙的型変換したとき
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(0)]
        [TestCase(6)]
        public static void CastIntToMemberIdTest_Failure_OutOfRange(int value)
        {
            staticFunctionTestHelper.StaticFuncFailure<MemberId>(
                execFunc: () => value,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region To

        /// <summary>
        ///     MemberId から int に暗黙的型変換できること。
        /// </summary>
        [TestCase(1)]
        [TestCase(5)]
        public static void CastMemberIdToIntTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => new MemberId(value),
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
        [TestCase(1, 1, true)]
        [TestCase(1, 5, false)]
        public static void OperatorEqualTest(int left, int right, bool expected)
        {
            var leftValue = (MemberId)left;
            var rightValue = (MemberId)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue == rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     等価比較演算 != が意図した値を返すこと
        /// </summary>
        [TestCase(1, 1, false)]
        [TestCase(1, 5, true)]
        public static void OperatorNotEqualTest(int left, int right, bool expected)
        {
            var leftValue = (MemberId)left;
            var rightValue = (MemberId)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue != rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと
        /// </summary>
        [TestCase(1, 1, true)]
        [TestCase(1, 5, false)]
        public static void OperatorEqualsTest(int left, int right, bool expected)
        {
            var leftValue = (MemberId)left;
            var rightValue = (MemberId)right;

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
