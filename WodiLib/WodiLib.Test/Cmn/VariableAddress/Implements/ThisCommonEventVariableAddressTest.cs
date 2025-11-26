using System;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn.Implements
{
    [TestFixture]
    public class ThisCommonEventVariableAddressTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region properties

        #region public

        #region ValueType

        /// <summary>
        ///     意図した値が取得できること。
        /// </summary>
        [Test]
        public static void ValueTypeGetterTest()
        {
            propertyTestHelper.PropertyGetSuccess(
                instance: new ThisCommonEventVariableAddress(),
                getter: target => target.ValueType,
                getValueVerifier: ValueVerifier<VariableAddressValueType>.AreEquals(VariableAddressValueType.Numeric)
            );
        }

        #endregion

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        ///     コンストラクタが正常に終了すること。
        /// </summary>
        [TestCase(1600000)]
        [TestCase(1600099)]
        public static void ConstructorIntTest_Success(int value)
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new ThisCommonEventVariableAddress(value),
                instanceVerifier: new ValueVerifier<ThisCommonEventVariableAddress>(instance =>
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
        [TestCase(1599999)]
        [TestCase(1600100)]
        public static void ConstructorIntTest_Failure_OutOfRange(int value)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new ThisCommonEventVariableAddress(value),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region Cast

        #region From

        /// <summary>
        ///     int から ThisCommonEventVariableAddress に暗黙的型変換できること。
        /// </summary>
        [TestCase(1600000)]
        [TestCase(1600099)]
        public static void CastIntToThisCommonEventVariableAddressTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => value,
                resultValueVerifier: new ValueVerifier<ThisCommonEventVariableAddress>(actual =>
                    {
                        Assert.AreEqual(actual.RawValue, value);
                    }
                )
            );
        }

        /// <summary>
        ///     許容範囲外の値から ThisCommonEventVariableAddress に暗黙的型変換した場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(1599999)]
        [TestCase(1600100)]
        public static void CastIntToThisCommonEventVariableAddressTest_Failure_OutOfRange(int value)
        {
            staticFunctionTestHelper.StaticFuncFailure<ThisCommonEventVariableAddress>(
                execFunc: () => value,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region To

        /// <summary>
        ///     ThisCommonEventVariableAddress から int に暗黙的型変換できること。
        /// </summary>
        [TestCase(1600000)]
        [TestCase(1600099)]
        public static void CastThisCommonEventVariableAddressToIntTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => new ThisCommonEventVariableAddress(value),
                resultValueVerifier: new ValueVerifier<int>(actual => { Assert.AreEqual(actual, value); })
            );
        }

        #endregion

        #endregion

        #region Operation

        #region Equal / Equals(Method)

        /// <summary>
        ///     等価比較演算 == が意図した値を返すこと。
        /// </summary>
        [TestCase(1600000, 1600000, true)]
        [TestCase(1600000, 1600099, false)]
        public static void OperatorEqualTest(int left, int right, bool expected)
        {
            var leftValue = (ThisCommonEventVariableAddress)left;
            var rightValue = (ThisCommonEventVariableAddress)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue == rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     等価比較演算 != が意図した値を返すこと。
        /// </summary>
        [TestCase(1600000, 1600000, false)]
        [TestCase(1600000, 1600099, true)]
        public static void OperatorNotEqualTest(int left, int right, bool expected)
        {
            var leftValue = (ThisCommonEventVariableAddress)left;
            var rightValue = (ThisCommonEventVariableAddress)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue != rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと。
        /// </summary>
        [TestCase(1600000, 1600000, true)]
        [TestCase(1600000, 1600099, false)]
        public static void OperatorEqualsTest(int left, int right, bool expected)
        {
            var leftValue = (ThisCommonEventVariableAddress)left;
            var rightValue = (ThisCommonEventVariableAddress)right;

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
