using System;
using System.Linq;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn.Factory
{
    [TestFixture]
    public class VariableAddressFactoryTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region public

        private static readonly object?[][] CreateTestCaseSource =
        {
            new object?[] { int.MinValue, false, null },
            new object?[] { -2000000000, false, null },
            new object?[] { 999999, false, null },
            new object?[] { 1000000, true, typeof(MapEventVariableAddress) },
            new object?[] { 1099999, true, typeof(MapEventVariableAddress) },
            new object?[] { 1100000, true, typeof(ThisMapEventVariableAddress) },
            new object?[] { 1100009, true, typeof(ThisMapEventVariableAddress) },
            new object?[] { 1100010, false, null },
            new object?[] { 1599999, false, null },
            new object?[] { 1600000, true, typeof(ThisCommonEventVariableAddress) },
            new object?[] { 1600099, true, typeof(ThisCommonEventVariableAddress) },
            new object?[] { 1600100, false, null },
            new object?[] { 1999999, false, null },
            new object?[] { 2000000, true, typeof(NormalNumberVariableAddress) },
            new object?[] { 2099999, true, typeof(NormalNumberVariableAddress) },
            new object?[] { 2100000, true, typeof(SpareNumberVariableAddress) },
            new object?[] { 2999999, true, typeof(SpareNumberVariableAddress) },
            new object?[] { 3000000, true, typeof(StringVariableAddress) },
            new object?[] { 3999999, true, typeof(StringVariableAddress) },
            new object?[] { 4000000, false, null },
            new object?[] { 7999999, false, null },
            new object?[] { 8000000, true, typeof(RandomVariableAddress) },
            new object?[] { 8999999, true, typeof(RandomVariableAddress) },
            new object?[] { 9000000, true, typeof(SystemVariableAddress) },
            new object?[] { 9099999, true, typeof(SystemVariableAddress) },
            new object?[] { 9100000, true, typeof(EventInfoAddress) },
            new object?[] { 9179999, true, typeof(EventInfoAddress) },
            new object?[] { 9180000, true, typeof(HeroInfoAddress) },
            new object?[] { 9180009, true, typeof(HeroInfoAddress) },
            new object?[] { 9180010, true, typeof(MemberInfoAddress) },
            new object?[] { 9180059, true, typeof(MemberInfoAddress) },
            new object?[] { 9180060, false, null },
            new object?[] { 9189999, false, null },
            new object?[] { 9190000, true, typeof(ThisMapEventInfoAddress) },
            new object?[] { 9199999, true, typeof(ThisMapEventInfoAddress) },
            new object?[] { 9200000, false, null },
            new object?[] { 9899999, false, null },
            new object?[] { 9900000, true, typeof(SystemStringVariableAddress) },
            new object?[] { 9999999, true, typeof(SystemStringVariableAddress) },
            new object?[] { 10000000, false, null },
            new object?[] { 14899999, false, null },
            new object?[] { 15000000, true, typeof(CommonEventVariableAddress) },
            new object?[] { 15999999, true, typeof(CommonEventVariableAddress) },
            new object?[] { 16000000, false, null },
            new object?[] { 999999999, false, null },
            new object?[] { 1000000000, true, typeof(UserDatabaseAddress) },
            new object?[] { 1099999999, true, typeof(UserDatabaseAddress) },
            new object?[] { 1100000000, true, typeof(ChangeableDatabaseAddress) },
            new object?[] { 1199999999, true, typeof(ChangeableDatabaseAddress) },
            new object?[] { 1200000000, false, null },
            new object?[] { 1299999999, false, null },
            new object?[] { 1300000000, true, typeof(SystemDatabaseAddress) },
            new object?[] { 1399999999, true, typeof(SystemDatabaseAddress) },
            new object?[] { 1400000000, false, null },
            new object?[] { 2000000000, false, null },
            new object?[] { int.MaxValue, false, null },
        };

        #region TryCreate

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        /// <param name="value">チェック対象</param>
        /// <param name="expected">期待する結果</param>
        /// <param name="expectedCreatedInstanceType">返却された変換値の期待型</param>
        [TestCaseSource(nameof(CreateTestCaseSource))]
        public static void TryCreateTest(int value, bool expected, Type expectedCreatedInstanceType)
        {
            VariableAddress? result = null;
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => VariableAddressFactory.TryCreate(value, out result),
                resultValueVerifier: ValueVerifier<bool>.AreEquals(expected)
            );

            // 変換値が期待する型であること
            if (result is not null)
            {
                ValueVerifier<VariableAddress>.IsType(expectedCreatedInstanceType).Verify(result);
            }
        }

        #endregion

        #region Create

        private static object?[][] CreateTestCaseSource_SuccessPattern =>
            CreateTestCaseSource.Where(@case => (bool)@case[1]!).ToArray();

        /// <summary>
        ///     意図した変数アドレス値のインスタンスが取得されること
        /// </summary>
        /// <param name="value">対象</param>
        /// <param name="_"></param>
        /// <param name="expectedCreatedInstanceType">期待する返却値型</param>
        [TestCaseSource(nameof(CreateTestCaseSource_SuccessPattern))]
        public static void CreateTest_Success(int value, bool _, Type expectedCreatedInstanceType)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => VariableAddressFactory.Create(value),
                resultValueVerifier: ValueVerifier<VariableAddress>.IsType(expectedCreatedInstanceType)
            );
        }

        private static object?[][] CreateTestCaseSource_FailurePattern =>
            CreateTestCaseSource.Where(@case => !(bool)@case[1]!).ToArray();

        /// <summary>
        ///     変数アドレス値ではない値の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        /// <param name="value">対象</param>
        /// <param name="_"></param>
        /// <param name="__"></param>
        [TestCaseSource(nameof(CreateTestCaseSource_FailurePattern))]
        public static void CreateTest_Failure(int value, bool _, Type __)
        {
            staticFunctionTestHelper.StaticFuncFailure(
                execFunc: () => VariableAddressFactory.Create(value),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #endregion
    }
}
