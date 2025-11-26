using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn.Enum
{
    [TestFixture]
    public class VariableAddressValueTypeTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Constants

        /// <summary>
        ///     ID が正しく取得できること。
        /// </summary>
        [Test]
        public static void ConstValueTest_Id()
        {
            Assert.AreEqual(VariableAddressValueType.Numeric.Id, "Numeric");
            Assert.AreEqual(VariableAddressValueType.String.Id, "String");
            Assert.AreEqual(VariableAddressValueType.Both.Id, "Both");
        }

        #endregion

        #region Methods

        #region CheckTypeInclude

        private static readonly object[][] CheckTypeInclude_TestCaseSource = new[]
        {
            // [instance, target, expected]
            new object[] { VariableAddressValueType.Numeric, VariableAddressValueType.Numeric, true },
            new object[] { VariableAddressValueType.Numeric, VariableAddressValueType.String, false },
            new object[] { VariableAddressValueType.Numeric, VariableAddressValueType.Both, false },
            new object[] { VariableAddressValueType.String, VariableAddressValueType.Numeric, false },
            new object[] { VariableAddressValueType.String, VariableAddressValueType.String, true },
            new object[] { VariableAddressValueType.String, VariableAddressValueType.Both, false },
            new object[] { VariableAddressValueType.Both, VariableAddressValueType.Numeric, true },
            new object[] { VariableAddressValueType.Both, VariableAddressValueType.String, true },
            new object[] { VariableAddressValueType.Both, VariableAddressValueType.Both, true },
        };

        /// <summary>
        ///     Code 文字列から VariableAddressValueType インスタンスが正しく取得できること。
        /// </summary>
        [TestCaseSource(nameof(CheckTypeInclude_TestCaseSource))]
        public static void CheckTypeIncludeTest(
            VariableAddressValueType instance,
            VariableAddressValueType target,
            bool expected
        )
        {
            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: value => value.CheckTypeInclude(target),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #endregion
    }
}
