using System;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn.Implements
{
    [TestFixture]
    public class HeroInfoAddressTest : TestFixtureBase
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
                instance: new HeroInfoAddress(),
                getter: target => target.ValueType,
                getValueVerifier: ValueVerifier<VariableAddressValueType>.AreEquals(VariableAddressValueType.Numeric)
            );
        }

        #endregion

        #region InfoType

        private static readonly object[] InfoTypeGetterTestCaseSource =
        {
            // [value, expected]
            new object[] { new HeroInfoAddress(9180000), InfoAddressInfoType.PositionX },
            new object[] { new HeroInfoAddress(9180001), InfoAddressInfoType.PositionY },
            new object[] { new HeroInfoAddress(9180002), InfoAddressInfoType.PositionXPrecise },
            new object[] { new HeroInfoAddress(9180003), InfoAddressInfoType.PositionYPrecise },
            new object[] { new HeroInfoAddress(9180004), InfoAddressInfoType.Height },
            new object[] { new HeroInfoAddress(9180005), InfoAddressInfoType.ShadowGraphicId },
            new object[] { new HeroInfoAddress(9180006), InfoAddressInfoType.Direction },
            new object[] { new HeroInfoAddress(9180007), InfoAddressInfoType.Empty },
            new object[] { new HeroInfoAddress(9180008), InfoAddressInfoType.Empty },
            new object[] { new HeroInfoAddress(9180009), InfoAddressInfoType.CharacterGraphicName },
        };

        /// <summary>
        ///     意図した値が取得できること。
        /// </summary>
        /// <param name="instance">処理対象</param>
        /// <param name="expected">期待する値</param>
        [TestCaseSource(nameof(InfoTypeGetterTestCaseSource))]
        public static void InfoTypeGetterTest(HeroInfoAddress instance, InfoAddressInfoType expected)
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
        [TestCase(9180000)]
        [TestCase(9180009)]
        public static void ConstructorIntTest_Success(int value)
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new HeroInfoAddress(value),
                instanceVerifier: new ValueVerifier<HeroInfoAddress>(instance =>
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
        [TestCase(9179999)]
        [TestCase(9180010)]
        public static void ConstructorIntTest_Failure_OutOfRange(int value)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new HeroInfoAddress(value),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region Cast

        #region From

        /// <summary>
        ///     int から HeroInfoAddress に暗黙的型変換できること。
        /// </summary>
        [TestCase(9180000)]
        [TestCase(9180009)]
        public static void CastIntToHeroInfoAddressTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => value,
                resultValueVerifier: new ValueVerifier<HeroInfoAddress>(actual =>
                    {
                        Assert.AreEqual(actual.RawValue, value);
                    }
                )
            );
        }

        /// <summary>
        ///     許容範囲外の値から HeroInfoAddress に暗黙的型変換した場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(9179999)]
        [TestCase(9180010)]
        public static void CastIntToHeroInfoAddressTest_Failure_OutOfRange(int value)
        {
            staticFunctionTestHelper.StaticFuncFailure<HeroInfoAddress>(
                execFunc: () => value,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region To

        /// <summary>
        ///     HeroInfoAddress から int に暗黙的型変換できること。
        /// </summary>
        [TestCase(9180000)]
        [TestCase(9180009)]
        public static void CastHeroInfoAddressToIntTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => new HeroInfoAddress(value),
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
        [TestCase(9180000, 9180000, true)]
        [TestCase(9180000, 9180009, false)]
        public static void OperatorEqualTest(int left, int right, bool expected)
        {
            var leftValue = (HeroInfoAddress)left;
            var rightValue = (HeroInfoAddress)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue == rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     等価比較演算 != が意図した値を返すこと。
        /// </summary>
        [TestCase(9180000, 9180000, false)]
        [TestCase(9180000, 9180009, true)]
        public static void OperatorNotEqualTest(int left, int right, bool expected)
        {
            var leftValue = (HeroInfoAddress)left;
            var rightValue = (HeroInfoAddress)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue != rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと。
        /// </summary>
        [TestCase(9180000, 9180000, true)]
        [TestCase(9180000, 9180009, false)]
        public static void OperatorEqualsTest(int left, int right, bool expected)
        {
            var leftValue = (HeroInfoAddress)left;
            var rightValue = (HeroInfoAddress)right;

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
