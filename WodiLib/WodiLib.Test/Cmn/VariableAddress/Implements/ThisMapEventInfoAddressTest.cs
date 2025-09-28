using System;
using Commons;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn
{
    [TestFixture]
    public class ThisMapEventInfoAddressTest
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
                instance: new ChangeableDatabaseAddress(),
                getter: target => target.ValueType,
                getValueVerifier: ValueVerifier<VariableAddressValueType>.AreEquals(VariableAddressValueType.Numeric)
            );
        }

        #endregion

        #region InfoType

        private static readonly object[] InfoTypeGetterTestCaseSource =
        {
            // [value, expected]
            new object[] { new ThisMapEventInfoAddress(9190000), InfoAddressInfoType.PositionX },
            new object[] { new ThisMapEventInfoAddress(9190001), InfoAddressInfoType.PositionY },
            new object[] { new ThisMapEventInfoAddress(9190012), InfoAddressInfoType.PositionXPrecise },
            new object[] { new ThisMapEventInfoAddress(9190113), InfoAddressInfoType.PositionYPrecise },
            new object[] { new ThisMapEventInfoAddress(9190234), InfoAddressInfoType.Height },
            new object[] { new ThisMapEventInfoAddress(9190235), InfoAddressInfoType.ShadowGraphicId },
            new object[] { new ThisMapEventInfoAddress(9190306), InfoAddressInfoType.Direction },
            new object[] { new ThisMapEventInfoAddress(9191357), InfoAddressInfoType.Empty },
            new object[] { new ThisMapEventInfoAddress(9192568), InfoAddressInfoType.Empty },
            new object[] { new ThisMapEventInfoAddress(9199999), InfoAddressInfoType.CharacterGraphicName },
        };

        /// <summary>
        ///     意図した値が取得できること。
        /// </summary>
        /// <param name="instance">処理対象</param>
        /// <param name="expected">期待する値</param>
        [TestCaseSource(nameof(InfoTypeGetterTestCaseSource))]
        public static void InfoTypeGetterTest(ThisMapEventInfoAddress instance, InfoAddressInfoType expected)
        {
            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.InfoType,
                getValueVerifier: ValueVerifier<InfoAddressInfoType>.AreEquals(expected)
            );
        }

        #endregion

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        ///     コンストラクタが正常に終了すること。
        /// </summary>
        [TestCase(9190000)]
        [TestCase(9199999)]
        public static void ConstructorIntTest_Success(int value)
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new ThisMapEventInfoAddress(value),
                instanceVerifier: new ValueVerifier<ThisMapEventInfoAddress>(instance =>
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
        [TestCase(9189999)]
        [TestCase(9200000)]
        public static void ConstructorIntTest_Failure_OutOfRange(int value)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new ThisMapEventInfoAddress(value),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region Cast

        #region From

        /// <summary>
        ///     int から ThisMapEventInfoAddress に暗黙的型変換できること。
        /// </summary>
        [TestCase(9190000)]
        [TestCase(9199999)]
        public static void CastIntToThisMapEventInfoAddressTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => value,
                resultValueVerifier: new ValueVerifier<ThisMapEventInfoAddress>(actual =>
                    {
                        Assert.AreEqual(actual.RawValue, value);
                    }
                )
            );
        }

        /// <summary>
        ///     許容範囲外の値から ThisMapEventInfoAddress に暗黙的型変換した場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(9189999)]
        [TestCase(9200000)]
        public static void CastIntToThisMapEventInfoAddressTest_Failure_OutOfRange(int value)
        {
            staticFunctionTestHelper.StaticFuncFailure<ThisMapEventInfoAddress>(
                execFunc: () => value,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region To

        /// <summary>
        ///     ThisMapEventInfoAddress から int に暗黙的型変換できること。
        /// </summary>
        [TestCase(9190000)]
        [TestCase(9199999)]
        public static void CastThisMapEventInfoAddressToIntTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => new ThisMapEventInfoAddress(value),
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
        [TestCase(9190000, 9190000, true)]
        [TestCase(9190000, 9199999, false)]
        public static void OperatorEqualTest(int left, int right, bool expected)
        {
            var leftValue = (ThisMapEventInfoAddress)left;
            var rightValue = (ThisMapEventInfoAddress)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue == rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     等価比較演算 != が意図した値を返すこと。
        /// </summary>
        [TestCase(9190000, 9190000, false)]
        [TestCase(9190000, 9199999, true)]
        public static void OperatorNotEqualTest(int left, int right, bool expected)
        {
            var leftValue = (ThisMapEventInfoAddress)left;
            var rightValue = (ThisMapEventInfoAddress)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue != rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと。
        /// </summary>
        [TestCase(9190000, 9190000, true)]
        [TestCase(9190000, 9199999, false)]
        public static void OperatorEqualsTest(int left, int right, bool expected)
        {
            var leftValue = (ThisMapEventInfoAddress)left;
            var rightValue = (ThisMapEventInfoAddress)right;

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
