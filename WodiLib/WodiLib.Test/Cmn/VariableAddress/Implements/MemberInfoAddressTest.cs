using System;
using Commons;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn
{
    [TestFixture]
    public class MemberInfoAddressTest
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
                instance: new MemberInfoAddress(),
                getter: target => target.ValueType,
                getValueVerifier: ValueVerifier<VariableAddressValueType>.AreEquals(VariableAddressValueType.Numeric)
            );
        }

        #endregion

        #region MemberId

        private static readonly object[] MemberIdGetterTestCaseSource =
        {
            // [value, expected]
            new object[] { new MemberInfoAddress(9180010), new MemberId(1) },
            new object[] { new MemberInfoAddress(9180021), new MemberId(2) },
            new object[] { new MemberInfoAddress(9180033), new MemberId(3) },
            new object[] { new MemberInfoAddress(9180046), new MemberId(4) },
            new object[] { new MemberInfoAddress(9180059), new MemberId(5) },
        };

        /// <summary>
        ///     意図した値が取得できること。
        /// </summary>
        /// <param name="instance">処理対象</param>
        /// <param name="expected">期待する値</param>
        [TestCaseSource(nameof(MemberIdGetterTestCaseSource))]
        public static void MemberIdGetterTest(MemberInfoAddress instance, MemberId expected)
        {
            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.MemberId,
                getValueVerifier: ValueVerifier<MemberId>.AreEquals(expected)
            );
        }

        #endregion

        #region InfoType

        private static readonly object[] InfoTypeGetterTestCaseSource =
        {
            // [value, expected]
            new object[] { new MemberInfoAddress(9180010), InfoAddressInfoType.PositionX },
            new object[] { new MemberInfoAddress(9180011), InfoAddressInfoType.PositionY },
            new object[] { new MemberInfoAddress(9180012), InfoAddressInfoType.PositionXPrecise },
            new object[] { new MemberInfoAddress(9180013), InfoAddressInfoType.PositionYPrecise },
            new object[] { new MemberInfoAddress(9180014), InfoAddressInfoType.Height },
            new object[] { new MemberInfoAddress(9180015), InfoAddressInfoType.ShadowGraphicId },
            new object[] { new MemberInfoAddress(9180016), InfoAddressInfoType.Direction },
            new object[] { new MemberInfoAddress(9180017), InfoAddressInfoType.Empty },
            new object[] { new MemberInfoAddress(9180018), InfoAddressInfoType.Empty },
            new object[] { new MemberInfoAddress(9180019), InfoAddressInfoType.CharacterGraphicName },
        };

        /// <summary>
        ///     意図した値が取得できること。
        /// </summary>
        /// <param name="instance">処理対象</param>
        /// <param name="expected">期待する値</param>
        [TestCaseSource(nameof(InfoTypeGetterTestCaseSource))]
        public static void InfoTypeGetterTest(MemberInfoAddress instance, InfoAddressInfoType expected)
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
        [TestCase(9180010)]
        [TestCase(9180059)]
        public static void ConstructorIntTest_Success(int value)
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new MemberInfoAddress(value),
                instanceVerifier: new ValueVerifier<MemberInfoAddress>(instance =>
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
        [TestCase(9180009)]
        [TestCase(9180060)]
        public static void ConstructorIntTest_Failure_OutOfRange(int value)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new MemberInfoAddress(value),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region Cast

        #region From

        /// <summary>
        ///     int から MemberInfoAddress に暗黙的型変換できること。
        /// </summary>
        [TestCase(9180010)]
        [TestCase(9180059)]
        public static void CastIntToMemberInfoAddressTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => value,
                resultValueVerifier: new ValueVerifier<MemberInfoAddress>(actual =>
                    {
                        Assert.AreEqual(actual.RawValue, value);
                    }
                )
            );
        }

        /// <summary>
        ///     許容範囲外の値から MemberInfoAddress に暗黙的型変換した場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(9180009)]
        [TestCase(9180060)]
        public static void CastIntToMemberInfoAddressTest_Failure_OutOfRange(int value)
        {
            staticFunctionTestHelper.StaticFuncFailure<MemberInfoAddress>(
                execFunc: () => value,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region To

        /// <summary>
        ///     MemberInfoAddress から int に暗黙的型変換できること。
        /// </summary>
        [TestCase(9180010)]
        [TestCase(9180059)]
        public static void CastMemberInfoAddressToIntTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => new MemberInfoAddress(value),
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
        [TestCase(9180010, 9180010, true)]
        [TestCase(9180010, 9180059, false)]
        public static void OperatorEqualTest(int left, int right, bool expected)
        {
            var leftValue = (MemberInfoAddress)left;
            var rightValue = (MemberInfoAddress)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue == rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     等価比較演算 != が意図した値を返すこと。
        /// </summary>
        [TestCase(9180010, 9180010, false)]
        [TestCase(9180010, 9180059, true)]
        public static void OperatorNotEqualTest(int left, int right, bool expected)
        {
            var leftValue = (MemberInfoAddress)left;
            var rightValue = (MemberInfoAddress)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue != rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと。
        /// </summary>
        [TestCase(9180010, 9180010, true)]
        [TestCase(9180010, 9180059, false)]
        public static void OperatorEqualsTest(int left, int right, bool expected)
        {
            var leftValue = (MemberInfoAddress)left;
            var rightValue = (MemberInfoAddress)right;

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
