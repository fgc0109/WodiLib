using System;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn.Implements
{
    [TestFixture]
    public class StringVariableAddressTest : TestFixtureBase
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
                instance: new StringVariableAddress(),
                getter: target => target.ValueType,
                getValueVerifier: ValueVerifier<VariableAddressValueType>.AreEquals(VariableAddressValueType.String)
            );
        }

        #endregion

        #region VariableIndex

        private static readonly object[][] VariableIndexGetterTestCaseSource =
        {
            // [instance, expected]
            new object[] { new StringVariableAddress(3000000), new StringVariableIndex(0) },
            new object[] { new StringVariableAddress(3000001), new StringVariableIndex(1) },
            new object[] { new StringVariableAddress(3999999), new StringVariableIndex(999999) },
        };

        /// <summary>
        ///     意図した値が取得できること。
        /// </summary>
        [TestCaseSource(nameof(VariableIndexGetterTestCaseSource))]
        public static void VariableIndexGetterTest(StringVariableAddress instance, StringVariableIndex expected)
        {
            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.VariableIndex,
                getValueVerifier: ValueVerifier<StringVariableIndex>.AreEquals(expected)
            );
        }

        #endregion

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        ///     コンストラクタが正常に終了すること。
        /// </summary>
        [TestCase(3000000)]
        [TestCase(3999999)]
        public static void ConstructorIntTest_Success(int value)
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new StringVariableAddress(value),
                instanceVerifier: new ValueVerifier<StringVariableAddress>(instance =>
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
        [TestCase(2999999)]
        [TestCase(4000000)]
        public static void ConstructorIntTest_Failure_OutOfRange(int value)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new StringVariableAddress(value),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region Cast

        #region From

        /// <summary>
        ///     int から StringVariableAddress に暗黙的型変換できること。
        /// </summary>
        [TestCase(3000000)]
        [TestCase(3999999)]
        public static void CastIntToStringVariableAddressTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => value,
                resultValueVerifier: new ValueVerifier<StringVariableAddress>(actual =>
                    {
                        Assert.AreEqual(actual.RawValue, value);
                    }
                )
            );
        }

        /// <summary>
        ///     許容範囲外の値から StringVariableAddress に暗黙的型変換した場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(2999999)]
        [TestCase(4000000)]
        public static void CastIntToStringVariableAddressTest_Failure_OutOfRange(int value)
        {
            staticFunctionTestHelper.StaticFuncFailure<StringVariableAddress>(
                execFunc: () => value,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region To

        /// <summary>
        ///     StringVariableAddress から int に暗黙的型変換できること。
        /// </summary>
        [TestCase(3000000)]
        [TestCase(3999999)]
        public static void CastStringVariableAddressToIntTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => new StringVariableAddress(value),
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
        [TestCase(3000000, 3000000, true)]
        [TestCase(3000000, 3999999, false)]
        public static void OperatorEqualTest(int left, int right, bool expected)
        {
            var leftValue = (StringVariableAddress)left;
            var rightValue = (StringVariableAddress)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue == rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     等価比較演算 != が意図した値を返すこと。
        /// </summary>
        [TestCase(3000000, 3000000, false)]
        [TestCase(3000000, 3999999, true)]
        public static void OperatorNotEqualTest(int left, int right, bool expected)
        {
            var leftValue = (StringVariableAddress)left;
            var rightValue = (StringVariableAddress)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue != rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと。
        /// </summary>
        [TestCase(3000000, 3000000, true)]
        [TestCase(3000000, 3999999, false)]
        public static void OperatorEqualsTest(int left, int right, bool expected)
        {
            var leftValue = (StringVariableAddress)left;
            var rightValue = (StringVariableAddress)right;

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
