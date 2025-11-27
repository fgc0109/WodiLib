using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.BinarySerializer.Database
{
    [TestFixture]
    public class DataNameSpecificationDefinitionBinarySerializerTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Methods

        #region ToTypeCode

        private static readonly object[][] ToTypeCodeTestCaseSource = new object[][]
        {
            // [src, expected]
            new object[] { new DataNameSpecificationDefinition(DatabaseKind.Changeable, 20), 30020 },
            new object[] { new DataNameSpecificationDefinition(DatabaseKind.Changeable, 75), 30075 },
            new object[] { new DataNameSpecificationDefinition(DatabaseKind.User, 20), 20020 },
            new object[] { new DataNameSpecificationDefinition(DatabaseKind.System, 20), 10020 },
        };

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCaseSource(nameof(ToTypeCodeTestCaseSource))]
        public static void ToTypeCodeTest(DataNameSpecificationDefinition src, int expected)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DataNameSpecificationDefinitionBinarySerializer.ToTypeCode(src),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #endregion
    }
}
