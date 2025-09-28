using System;
using Commons;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn
{
    [TestFixture]
    public class RandomVariableAddressTest
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
                instance: new RandomVariableAddress(),
                getter: target => target.ValueType,
                getValueVerifier: ValueVerifier<VariableAddressValueType>.AreEquals(VariableAddressValueType.Numeric)
            );
        }

        #endregion

        #region RandomValue

        private static readonly object[][] RandomValueGetterTestCaseSource =
        {
            // [instance, expected]
            new object[] { new RandomVariableAddress(8000000), new RandomVariableValue(0) },
            new object[] { new RandomVariableAddress(8000001), new RandomVariableValue(1) },
            new object[] { new RandomVariableAddress(8999999), new RandomVariableValue(999999) },
        };

        /// <summary>
        ///     意図した値が取得できること。
        /// </summary>
        [TestCaseSource(nameof(RandomValueGetterTestCaseSource))]
        public static void RandomValueGetterTest(RandomVariableAddress instance, RandomVariableValue expected)
        {
            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.RandomValue,
                getValueVerifier: ValueVerifier<RandomVariableValue>.AreEquals(expected)
            );
        }

        #endregion

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        ///     コンストラクタが正常に終了すること。
        /// </summary>
        [TestCase(8000000)]
        [TestCase(8999999)]
        public static void ConstructorIntTest_Success(int value)
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new RandomVariableAddress(value),
                instanceVerifier: new ValueVerifier<RandomVariableAddress>(instance =>
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
        [TestCase(7999999)]
        [TestCase(9000000)]
        public static void ConstructorIntTest_Failure_OutOfRange(int value)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new RandomVariableAddress(value),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region Cast

        #region From

        /// <summary>
        ///     int から RandomVariableAddress に暗黙的型変換できること。
        /// </summary>
        [TestCase(8000000)]
        [TestCase(8999999)]
        public static void CastIntToRandomVariableAddressTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => value,
                resultValueVerifier: new ValueVerifier<RandomVariableAddress>(actual =>
                    {
                        Assert.AreEqual(actual.RawValue, value);
                    }
                )
            );
        }

        /// <summary>
        ///     許容範囲外の値から RandomVariableAddress に暗黙的型変換した場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(7999999)]
        [TestCase(9000000)]
        public static void CastIntToRandomVariableAddressTest_Failure_OutOfRange(int value)
        {
            staticFunctionTestHelper.StaticFuncFailure<RandomVariableAddress>(
                execFunc: () => value,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region To

        /// <summary>
        ///     RandomVariableAddress から int に暗黙的型変換できること。
        /// </summary>
        [TestCase(8000000)]
        [TestCase(8999999)]
        public static void CastRandomVariableAddressToIntTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => new RandomVariableAddress(value),
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
        [TestCase(8000000, 8000000, true)]
        [TestCase(8000000, 8999999, false)]
        public static void OperatorEqualTest(int left, int right, bool expected)
        {
            var leftValue = (RandomVariableAddress)left;
            var rightValue = (RandomVariableAddress)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue == rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     等価比較演算 != が意図した値を返すこと。
        /// </summary>
        [TestCase(8000000, 8000000, false)]
        [TestCase(8000000, 8999999, true)]
        public static void OperatorNotEqualTest(int left, int right, bool expected)
        {
            var leftValue = (RandomVariableAddress)left;
            var rightValue = (RandomVariableAddress)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue != rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと。
        /// </summary>
        [TestCase(8000000, 8000000, true)]
        [TestCase(8000000, 8999999, false)]
        public static void OperatorEqualsTest(int left, int right, bool expected)
        {
            var leftValue = (RandomVariableAddress)left;
            var rightValue = (RandomVariableAddress)right;

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
