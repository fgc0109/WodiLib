using Commons;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn
{
    [TestFixture]
    public class VariableAddressCheckHelperTest
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

        #region public

        #region IsVariableAddress

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        /// <param name="value">チェック対象</param>
        /// <param name="expected">期待する結果</param>
        [TestCase(999999, false)]
        [TestCase(1000000, true)] // MapEventVariableAddress
        [TestCase(1099999, true)] // MapEventVariableAddress
        [TestCase(1100000, true)] // ThisMapEventVariableAddress
        [TestCase(1100009, true)] // ThisMapEventVariableAddress
        [TestCase(1100010, false)]
        [TestCase(1599999, false)]
        [TestCase(1600000, true)] // ThisCommonEventVariableAddress
        [TestCase(1600099, true)] // ThisCommonEventVariableAddress
        [TestCase(1600100, false)]
        [TestCase(1999999, false)]
        [TestCase(2000000, true)] // NormalNumberVariableAddress
        [TestCase(2099999, true)] // NormalNumberVariableAddress
        [TestCase(2100000, true)] // SpareNumberVariableAddress
        [TestCase(2999999, true)] // SpareNumberVariableAddress
        [TestCase(3000000, true)] // StringVariableAddress
        [TestCase(3999999, true)] // StringVariableAddress
        [TestCase(4000000, false)]
        [TestCase(7999999, false)]
        [TestCase(8000000, true)] // RandomVariableAddress
        [TestCase(8999999, true)] // RandomVariableAddress
        [TestCase(9000000, true)] // SystemVariableAddress
        [TestCase(9099999, true)] // SystemVariableAddress
        [TestCase(9100000, true)] // EventInfoAddress
        [TestCase(9179999, true)] // EventInfoAddress
        [TestCase(9180000, true)] // HeroInfoAddress
        [TestCase(9180009, true)] // HeroInfoAddress
        [TestCase(9180010, true)] // MemberInfoAddress
        [TestCase(9180059, true)] // MemberInfoAddress
        [TestCase(9180060, false)]
        [TestCase(9189999, false)]
        [TestCase(9190000, true)] // ThisMapEventInfoAddress
        [TestCase(9199999, true)] // ThisMapEventInfoAddress
        [TestCase(9200000, false)]
        [TestCase(9899999, false)]
        [TestCase(9900000, true)] // SystemStringVariableAddress
        [TestCase(9999999, true)] // SystemStringVariableAddress
        [TestCase(10000000, false)]
        [TestCase(14899999, false)]
        [TestCase(15000000, true)] // CommonEventVariableAddress
        [TestCase(15999999, true)] // CommonEventVariableAddress
        [TestCase(16000000, false)]
        [TestCase(999999999, false)]
        [TestCase(1000000000, true)] // UserDatabaseAddress
        [TestCase(1099999999, true)] // UserDatabaseAddress
        [TestCase(1100000000, true)] // ChangeableDatabaseAddress
        [TestCase(1199999999, true)] // ChangeableDatabaseAddress
        [TestCase(1200000000, false)]
        [TestCase(1299999999, false)]
        [TestCase(1300000000, true)] // SystemDatabaseAddress
        [TestCase(1399999999, true)] // SystemDatabaseAddress
        [TestCase(1400000000, false)]
        public static void IsVariableAddressTest(int value, bool expected)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => VariableAddressCheckHelper.IsVariableAddress(value),
                resultValueVerifier: ValueVerifier<bool>.AreEquals(expected)
            );
        }

        #endregion

        #region IsVariableAddressSimpleCheck

        [TestCase(999999, false)]
        [TestCase(1000000, true)]
        [TestCase(2000000000, true)]
        [TestCase(2000000001, false)]
        public static void IsVariableAddressSimpleCheckTest(int value, bool expected)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => VariableAddressCheckHelper.IsVariableAddressSimpleCheck(value),
                resultValueVerifier: ValueVerifier<bool>.AreEquals(expected)
            );
        }

        #endregion

        #region IsNumericVariableAddress

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        /// <param name="value">チェック対象</param>
        /// <param name="expected">期待する結果</param>
        [TestCase(999999, false)]
        [TestCase(1000000, true)] // MapEventVariableAddress
        [TestCase(1099999, true)] // MapEventVariableAddress
        [TestCase(1100000, true)] // ThisMapEventVariableAddress
        [TestCase(1100009, true)] // ThisMapEventVariableAddress
        [TestCase(1100010, false)]
        [TestCase(1599999, false)]
        [TestCase(1600000, true)] // ThisCommonEventVariableAddress
        [TestCase(1600099, true)] // ThisCommonEventVariableAddress
        [TestCase(1600100, false)]
        [TestCase(1999999, false)]
        [TestCase(2000000, true)] // NormalNumberVariableAddress
        [TestCase(2099999, true)] // NormalNumberVariableAddress
        [TestCase(2100000, true)] // SpareNumberVariableAddress
        [TestCase(2999999, true)] // SpareNumberVariableAddress
        [TestCase(3000000, false)] // StringVariableAddress
        [TestCase(3999999, false)] // StringVariableAddress
        [TestCase(4000000, false)]
        [TestCase(7999999, false)]
        [TestCase(8000000, true)] // RandomVariableAddress
        [TestCase(8999999, true)] // RandomVariableAddress
        [TestCase(9000000, true)] // SystemVariableAddress
        [TestCase(9099999, true)] // SystemVariableAddress
        [TestCase(9100000, true)] // EventInfoAddress
        [TestCase(9179999, true)] // EventInfoAddress
        [TestCase(9180000, true)] // HeroInfoAddress
        [TestCase(9180009, true)] // HeroInfoAddress
        [TestCase(9180010, true)] // MemberInfoAddress
        [TestCase(9180059, true)] // MemberInfoAddress
        [TestCase(9180060, false)]
        [TestCase(9189999, false)]
        [TestCase(9190000, true)] // ThisMapEventInfoAddress
        [TestCase(9199999, true)] // ThisMapEventInfoAddress
        [TestCase(9200000, false)]
        [TestCase(9899999, false)]
        [TestCase(9900000, false)] // SystemStringVariableAddress
        [TestCase(9999999, false)] // SystemStringVariableAddress
        [TestCase(10000000, false)]
        [TestCase(14899999, false)]
        [TestCase(15000000, true)] // CommonEventVariableAddress
        [TestCase(15999999, true)] // CommonEventVariableAddress
        [TestCase(16000000, false)]
        [TestCase(999999999, false)]
        [TestCase(1000000000, true)] // UserDatabaseAddress
        [TestCase(1099999999, true)] // UserDatabaseAddress
        [TestCase(1100000000, true)] // ChangeableDatabaseAddress
        [TestCase(1199999999, true)] // ChangeableDatabaseAddress
        [TestCase(1200000000, false)]
        [TestCase(1299999999, false)]
        [TestCase(1300000000, true)] // SystemDatabaseAddress
        [TestCase(1399999999, true)] // SystemDatabaseAddress
        [TestCase(1400000000, false)]
        public static void IsNumericVariableAddressTest(int value, bool expected)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => VariableAddressCheckHelper.IsNumericVariableAddress(value),
                resultValueVerifier: ValueVerifier<bool>.AreEquals(expected)
            );
        }

        #endregion

        #endregion
    }
}
