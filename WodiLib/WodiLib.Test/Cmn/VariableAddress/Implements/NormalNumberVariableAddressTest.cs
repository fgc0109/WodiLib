using System;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn.Implements
{
    [TestFixture]
    public class NormalNumberVariableAddressTest : TestFixtureBase
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
                instance: new NormalNumberVariableAddress(),
                getter: target => target.ValueType,
                getValueVerifier: ValueVerifier<VariableAddressValueType>.AreEquals(VariableAddressValueType.Numeric)
            );
        }

        #endregion

        #region VariableIndex

        private static readonly object[][] VariableIndexGetterTestCaseSource =
        {
            // [instance, expected]
            new object[] { new NormalNumberVariableAddress(2000000), new NormalNumberVariableIndex(0) },
            new object[] { new NormalNumberVariableAddress(2000001), new NormalNumberVariableIndex(1) },
            new object[] { new NormalNumberVariableAddress(2000999), new NormalNumberVariableIndex(999) },
        };

        /// <summary>
        ///     意図した値が取得できること。
        /// </summary>
        [TestCaseSource(nameof(VariableIndexGetterTestCaseSource))]
        public static void VariableIndexGetterTest(
            NormalNumberVariableAddress instance,
            NormalNumberVariableIndex expected
        )
        {
            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.VariableIndex,
                getValueVerifier: ValueVerifier<NormalNumberVariableIndex>.AreEquals(expected)
            );
        }

        #endregion

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        ///     コンストラクタが正常に終了すること。
        /// </summary>
        [TestCase(2000000)]
        [TestCase(2099999)]
        public static void ConstructorIntTest_Success(int value)
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new NormalNumberVariableAddress(value),
                instanceVerifier: new ValueVerifier<NormalNumberVariableAddress>(instance =>
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
        [TestCase(1999999)]
        [TestCase(2100000)]
        public static void ConstructorIntTest_Failure_OutOfRange(int value)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new NormalNumberVariableAddress(value),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region Cast

        #region From

        /// <summary>
        ///     int から NormalNumberVariableAddress に暗黙的型変換できること。
        /// </summary>
        [TestCase(2000000)]
        [TestCase(2099999)]
        public static void CastIntToNormalNumberVariableAddressTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => value,
                resultValueVerifier: new ValueVerifier<NormalNumberVariableAddress>(actual =>
                    {
                        Assert.AreEqual(actual.RawValue, value);
                    }
                )
            );
        }

        /// <summary>
        ///     許容範囲外の値から NormalNumberVariableAddress に暗黙的型変換した場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(1999999)]
        [TestCase(2100000)]
        public static void CastIntToNormalNumberVariableAddressTest_Failure_OutOfRange(int value)
        {
            staticFunctionTestHelper.StaticFuncFailure<NormalNumberVariableAddress>(
                execFunc: () => value,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region To

        /// <summary>
        ///     NormalNumberVariableAddress から int に暗黙的型変換できること。
        /// </summary>
        [TestCase(2000000)]
        [TestCase(2099999)]
        public static void CastNormalNumberVariableAddressToIntTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => new NormalNumberVariableAddress(value),
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
        [TestCase(2000000, 2000000, true)]
        [TestCase(2000000, 2099999, false)]
        public static void OperatorEqualTest(int left, int right, bool expected)
        {
            var leftValue = (NormalNumberVariableAddress)left;
            var rightValue = (NormalNumberVariableAddress)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue == rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     等価比較演算 != が意図した値を返すこと。
        /// </summary>
        [TestCase(2000000, 2000000, false)]
        [TestCase(2000000, 2099999, true)]
        public static void OperatorNotEqualTest(int left, int right, bool expected)
        {
            var leftValue = (NormalNumberVariableAddress)left;
            var rightValue = (NormalNumberVariableAddress)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue != rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと。
        /// </summary>
        [TestCase(2000000, 2000000, true)]
        [TestCase(2000000, 2099999, false)]
        public static void OperatorEqualsTest(int left, int right, bool expected)
        {
            var leftValue = (NormalNumberVariableAddress)left;
            var rightValue = (NormalNumberVariableAddress)right;

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
