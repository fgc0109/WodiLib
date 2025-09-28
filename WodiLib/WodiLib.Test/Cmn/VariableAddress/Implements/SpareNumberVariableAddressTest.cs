using System;
using Commons;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn
{
    [TestFixture]
    public class SpareNumberVariableAddressTest
    {
        private static Logger logger = null!;

        private static ConstructorTestHelper constructorTestHelper = null!;
        private static PropertyTestHelper propertyTestHelper = null!;
        private static PureFunctionTestHelper pureFunctionTestHelper = null!;
        private static StaticFunctionTestHelper staticFunctionTestHelper = null!;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();

            constructorTestHelper = new ConstructorTestHelper(logger);
            propertyTestHelper = new PropertyTestHelper(logger);
            pureFunctionTestHelper = new PureFunctionTestHelper(logger);
            staticFunctionTestHelper = new StaticFunctionTestHelper(logger);
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
                instance: new SpareNumberVariableAddress(),
                getter: target => target.ValueType,
                getValueVerifier: ValueVerifier<VariableAddressValueType>.AreEquals(VariableAddressValueType.Numeric)
            );
        }

        #endregion

        #region VariableNumber

        private static readonly object[][] VariableNumberGetterTestCaseSource =
        {
            // [instance, expected]
            new object[] { new SpareNumberVariableAddress(2100000), new SpareNumberVariableNumber(1) },
            new object[] { new SpareNumberVariableAddress(2100005), new SpareNumberVariableNumber(1) },
            new object[] { new SpareNumberVariableAddress(2200090), new SpareNumberVariableNumber(2) },
            new object[] { new SpareNumberVariableAddress(2999999), new SpareNumberVariableNumber(9) },
        };

        /// <summary>
        ///     意図した値が取得できること。
        /// </summary>
        [TestCaseSource(nameof(VariableNumberGetterTestCaseSource))]
        public static void VariableNumberGetterTest(
            SpareNumberVariableAddress instance,
            SpareNumberVariableNumber expected
        )
        {
            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.VariableNumber,
                getValueVerifier: ValueVerifier<SpareNumberVariableNumber>.AreEquals(expected)
            );
        }

        #endregion

        #region VariableIndex

        private static readonly object[][] VariableIndexGetterTestCaseSource =
        {
            // [instance, expected]
            new object[] { new SpareNumberVariableAddress(2100000), new SpareNumberVariableIndex(0) },
            new object[] { new SpareNumberVariableAddress(2100005), new SpareNumberVariableIndex(5) },
            new object[] { new SpareNumberVariableAddress(2999999), new SpareNumberVariableIndex(99999) },
        };

        /// <summary>
        ///     意図した値が取得できること。
        /// </summary>
        [TestCaseSource(nameof(VariableIndexGetterTestCaseSource))]
        public static void VariableIndexGetterTest(
            SpareNumberVariableAddress instance,
            SpareNumberVariableIndex expected
        )
        {
            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.VariableIndex,
                getValueVerifier: ValueVerifier<SpareNumberVariableIndex>.AreEquals(expected)
            );
        }

        #endregion

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        ///     コンストラクタが正常に終了すること。
        /// </summary>
        [TestCase(2100000)]
        [TestCase(2999999)]
        public static void ConstructorIntTest_Success(int value)
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new SpareNumberVariableAddress(value),
                instanceVerifier: new ValueVerifier<SpareNumberVariableAddress>(instance =>
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
        [TestCase(2099999)]
        [TestCase(3000000)]
        public static void ConstructorIntTest_Failure_OutOfRange(int value)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new SpareNumberVariableAddress(value),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region Cast

        #region From

        /// <summary>
        ///     int から SpareNumberVariableAddress に暗黙的型変換できること。
        /// </summary>
        [TestCase(2100000)]
        [TestCase(2999999)]
        public static void CastIntToSpareNumberVariableAddressTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => value,
                resultValueVerifier: new ValueVerifier<SpareNumberVariableAddress>(actual =>
                    {
                        Assert.AreEqual(actual.RawValue, value);
                    }
                )
            );
        }

        /// <summary>
        ///     許容範囲外の値から SpareNumberVariableAddress に暗黙的型変換した場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(2099999)]
        [TestCase(3000000)]
        public static void CastIntToSpareNumberVariableAddressTest_Failure_OutOfRange(int value)
        {
            staticFunctionTestHelper.StaticFuncFailure<SpareNumberVariableAddress>(
                execFunc: () => value,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region To

        /// <summary>
        ///     SpareNumberVariableAddress から int に暗黙的型変換できること。
        /// </summary>
        [TestCase(2100000)]
        [TestCase(2999999)]
        public static void CastSpareNumberVariableAddressToIntTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => new SpareNumberVariableAddress(value),
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
        [TestCase(2100000, 2100000, true)]
        [TestCase(2100000, 2999999, false)]
        public static void OperatorEqualTest(int left, int right, bool expected)
        {
            var leftValue = (SpareNumberVariableAddress)left;
            var rightValue = (SpareNumberVariableAddress)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue == rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     等価比較演算 != が意図した値を返すこと。
        /// </summary>
        [TestCase(2100000, 2100000, false)]
        [TestCase(2100000, 2999999, true)]
        public static void OperatorNotEqualTest(int left, int right, bool expected)
        {
            var leftValue = (SpareNumberVariableAddress)left;
            var rightValue = (SpareNumberVariableAddress)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue != rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと。
        /// </summary>
        [TestCase(2100000, 2100000, true)]
        [TestCase(2100000, 2999999, false)]
        public static void OperatorEqualsTest(int left, int right, bool expected)
        {
            var leftValue = (SpareNumberVariableAddress)left;
            var rightValue = (SpareNumberVariableAddress)right;

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
