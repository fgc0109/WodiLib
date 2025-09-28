using System;
using Commons;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn
{
    [TestFixture]
    public class SystemVariableAddressTest
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
                instance: new SystemVariableAddress(),
                getter: target => target.ValueType,
                getValueVerifier: ValueVerifier<VariableAddressValueType>.AreEquals(VariableAddressValueType.Numeric)
            );
        }

        #endregion

        #region VariableIndex

        private static readonly object[][] VariableIndexGetterTestCaseSource =
        {
            // [instance, expected]
            new object[] { new SystemVariableAddress(9000000), new SystemVariableIndex(0) },
            new object[] { new SystemVariableAddress(9000001), new SystemVariableIndex(1) },
            new object[] { new SystemVariableAddress(9099999), new SystemVariableIndex(99999) },
        };

        /// <summary>
        ///     意図した値が取得できること。
        /// </summary>
        [TestCaseSource(nameof(VariableIndexGetterTestCaseSource))]
        public static void VariableIndexGetterTest(
            SystemVariableAddress instance,
            SystemVariableIndex expected
        )
        {
            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.VariableIndex,
                getValueVerifier: ValueVerifier<SystemVariableIndex>.AreEquals(expected)
            );
        }

        #endregion

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        ///     コンストラクタが正常に終了すること。
        /// </summary>
        [TestCase(9000000)]
        [TestCase(9099999)]
        public static void ConstructorIntTest_Success(int value)
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new SystemVariableAddress(value),
                instanceVerifier: new ValueVerifier<SystemVariableAddress>(instance =>
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
        [TestCase(8999999)]
        [TestCase(9100000)]
        public static void ConstructorIntTest_Failure_OutOfRange(int value)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new SystemVariableAddress(value),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region Cast

        #region From

        /// <summary>
        ///     int から SystemVariableAddress に暗黙的型変換できること。
        /// </summary>
        [TestCase(9000000)]
        [TestCase(9099999)]
        public static void CastIntToSystemVariableAddressTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => value,
                resultValueVerifier: new ValueVerifier<SystemVariableAddress>(actual =>
                    {
                        Assert.AreEqual(actual.RawValue, value);
                    }
                )
            );
        }

        /// <summary>
        ///     許容範囲外の値から SystemVariableAddress に暗黙的型変換した場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(8999999)]
        [TestCase(9100000)]
        public static void CastIntToSystemVariableAddressTest_Failure_OutOfRange(int value)
        {
            staticFunctionTestHelper.StaticFuncFailure<SystemVariableAddress>(
                execFunc: () => value,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region To

        /// <summary>
        ///     SystemVariableAddress から int に暗黙的型変換できること。
        /// </summary>
        [TestCase(9000000)]
        [TestCase(9099999)]
        public static void CastSystemVariableAddressToIntTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => new SystemVariableAddress(value),
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
        [TestCase(9000000, 9000000, true)]
        [TestCase(9000000, 9099999, false)]
        public static void OperatorEqualTest(int left, int right, bool expected)
        {
            var leftValue = (SystemVariableAddress)left;
            var rightValue = (SystemVariableAddress)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue == rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     等価比較演算 != が意図した値を返すこと。
        /// </summary>
        [TestCase(9000000, 9000000, false)]
        [TestCase(9000000, 9099999, true)]
        public static void OperatorNotEqualTest(int left, int right, bool expected)
        {
            var leftValue = (SystemVariableAddress)left;
            var rightValue = (SystemVariableAddress)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue != rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと。
        /// </summary>
        [TestCase(9000000, 9000000, true)]
        [TestCase(9000000, 9099999, false)]
        public static void OperatorEqualsTest(int left, int right, bool expected)
        {
            var leftValue = (SystemVariableAddress)left;
            var rightValue = (SystemVariableAddress)right;

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
