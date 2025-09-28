using System;
using Commons;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn.Enum
{
    [TestFixture]
    public class InfoAddressInfoTypeTest
    {
        private static Logger logger = null!;

        private static StaticFunctionTestHelper staticFunctionTestHelper = null!;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();

            staticFunctionTestHelper = new StaticFunctionTestHelper(logger);
        }

        #region Constants

        /// <summary>
        ///     ID が正しく取得できること。
        /// </summary>
        [Test]
        public static void ConstValueTest_Id()
        {
            Assert.AreEqual(InfoAddressInfoType.PositionX.Id, "PositionX");
            Assert.AreEqual(InfoAddressInfoType.PositionY.Id, "PositionY");
            Assert.AreEqual(InfoAddressInfoType.PositionXPrecise.Id, "PositionXPrecise");
            Assert.AreEqual(InfoAddressInfoType.PositionYPrecise.Id, "PositionYPrecise");
            Assert.AreEqual(InfoAddressInfoType.Height.Id, "Height");
            Assert.AreEqual(InfoAddressInfoType.ShadowGraphicId.Id, "ShadowGraphicId");
            Assert.AreEqual(InfoAddressInfoType.Direction.Id, "Direction");
            Assert.AreEqual(InfoAddressInfoType.CharacterGraphicName.Id, "CharacterGraphicName");
            Assert.AreEqual(InfoAddressInfoType.Empty.Id, "Empty");
        }

        /// <summary>
        ///     Code が正しく取得できること。
        /// </summary>
        [Test]
        public static void ConstValueTest_Code()
        {
            Assert.AreEqual(InfoAddressInfoType.PositionX.Code, 0);
            Assert.AreEqual(InfoAddressInfoType.PositionY.Code, 1);
            Assert.AreEqual(InfoAddressInfoType.PositionXPrecise.Code, 2);
            Assert.AreEqual(InfoAddressInfoType.PositionYPrecise.Code, 3);
            Assert.AreEqual(InfoAddressInfoType.Height.Code, 4);
            Assert.AreEqual(InfoAddressInfoType.ShadowGraphicId.Code, 5);
            Assert.AreEqual(InfoAddressInfoType.Direction.Code, 6);
            Assert.AreEqual(InfoAddressInfoType.CharacterGraphicName.Code, 9);
            Assert.AreEqual(InfoAddressInfoType.Empty.Code, 7);
        }

        #endregion

        #region Methods

        #region FromCode

        private static readonly object[][] FromCodeTest_Success_TestCaseSource = new[]
        {
            // [code, expected]
            new object[] { 0, InfoAddressInfoType.PositionX },
            new object[] { 1, InfoAddressInfoType.PositionY },
            new object[] { 2, InfoAddressInfoType.PositionXPrecise },
            new object[] { 3, InfoAddressInfoType.PositionYPrecise },
            new object[] { 4, InfoAddressInfoType.Height },
            new object[] { 5, InfoAddressInfoType.ShadowGraphicId },
            new object[] { 6, InfoAddressInfoType.Direction },
            new object[] { 7, InfoAddressInfoType.Empty },
            new object[] { 8, InfoAddressInfoType.Empty },
            new object[] { 9, InfoAddressInfoType.CharacterGraphicName },
        };

        /// <summary>
        ///     Code 文字列から InfoAddressInfoType インスタンスが正しく取得できること。
        /// </summary>
        [TestCaseSource(nameof(FromCodeTest_Success_TestCaseSource))]
        public static void FromCodeTest_Success(int code, InfoAddressInfoType expected)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => InfoAddressInfoType.FromCode(code),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     引数 code が定義されていない値の場合、ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [Test]
        public static void FromCodeTest_Failure_ArgumentInappropriate()
        {
            staticFunctionTestHelper.StaticFuncFailure(
                execFunc: () => InfoAddressInfoType.FromCode(-1),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #endregion
    }
}
