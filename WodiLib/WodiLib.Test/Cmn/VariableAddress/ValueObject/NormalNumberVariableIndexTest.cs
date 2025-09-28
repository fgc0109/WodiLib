using System;
using Commons;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn
{
    [TestFixture]
    public class NormalNumberVariableIndexTest
    {
        private static Logger logger = null!;

        private static ConstructorTestHelper constructorTestHelper = null!;
        private static PureFunctionTestHelper pureFunctionTestHelper = null!;
        private static StaticFunctionTestHelper staticFunctionTestHelper = null!;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();

            constructorTestHelper = new ConstructorTestHelper(logger);
            pureFunctionTestHelper = new PureFunctionTestHelper(logger);
            staticFunctionTestHelper = new StaticFunctionTestHelper(logger);
        }

        #region Constructor

        /// <summary>
        ///     コンストラクタが正常に終了すること。
        /// </summary>
        [TestCase(0)]
        [TestCase(99999)]
        public static void ConstructorIntTest_Success(int value)
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new NormalNumberVariableIndex(value),
                instanceVerifier: new ValueVerifier<NormalNumberVariableIndex>(instance =>
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
        [TestCase(100000)]
        public static void ConstructorIntTest_Failure_OutOfRange(int value)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new NormalNumberVariableIndex(value),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region Cast

        #region From

        /// <summary>
        ///     int から NormalNumberVariableIndex に暗黙的型変換できること。
        /// </summary>
        [TestCase(0)]
        [TestCase(99999)]
        public static void CastIntToNormalNumberVariableIndexTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess<NormalNumberVariableIndex>(
                execFunc: () => value,
                resultValueVerifier: new ValueVerifier<NormalNumberVariableIndex>(actual =>
                    {
                        Assert.AreEqual(actual.RawValue, value);
                    }
                )
            );
        }

        /// <summary>
        ///     許容範囲外の値から NormalNumberVariableIndex に暗黙的型変換したとき
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(100000)]
        public static void CastIntToNormalNumberVariableIndexTest_Failure_OutOfRange(int value)
        {
            staticFunctionTestHelper.StaticFuncFailure<NormalNumberVariableIndex>(
                execFunc: () => value,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region To

        /// <summary>
        ///     NormalNumberVariableIndex から int に暗黙的型変換できること
        /// </summary>
        [TestCase(0)]
        [TestCase(99999)]
        public static void CastNormalNumberVariableIndexToIntTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess<int>(
                execFunc: () => new NormalNumberVariableIndex(value),
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
        [TestCase(0, 99999, false)]
        public static void OperatorEqualTest(int left, int right, bool expected)
        {
            var leftValue = (NormalNumberVariableIndex)left;
            var rightValue = (NormalNumberVariableIndex)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue == rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     等価比較演算 != が意図した値を返すこと
        /// </summary>
        [TestCase(0, 0, false)]
        [TestCase(0, 99999, true)]
        public static void OperatorNotEqualTest(int left, int right, bool expected)
        {
            var leftValue = (NormalNumberVariableIndex)left;
            var rightValue = (NormalNumberVariableIndex)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue != rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと
        /// </summary>
        [TestCase(0, 0, true)]
        [TestCase(0, 99999, false)]
        public static void OperatorEqualsTest(int left, int right, bool expected)
        {
            var leftValue = (NormalNumberVariableIndex)left;
            var rightValue = (NormalNumberVariableIndex)right;

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
