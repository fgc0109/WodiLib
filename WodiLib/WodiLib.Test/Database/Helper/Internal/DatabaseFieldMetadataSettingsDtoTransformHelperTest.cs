using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class DatabaseFieldMetadataSettingsDtoTransformHelperTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region TransformMetadataSettings(IDatabaseFieldMetadataSettings)

        /// <summary>
        ///     <see cref="IDatabaseFieldMetadataSettings"/> から
        ///     <see cref="IDatabaseFieldDefinitionSettings"/> への変換が正常に行われること。
        /// </summary>
        [Test]
        public static void TransformMetadataSettings_Success()
        {
            var definitionSettings = new DatabaseFieldMetadataSettings
            {
                FieldName = "TestField",
                FieldMemo = "Test Memo",
                SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionSettings(
                    new DatabaseFieldSpecialSettingDefinitionNormalSettings()
                ),
            };

            var expected = new DatabaseFieldDefinitionSettings
            {
                FieldName = "TestField",
                FieldType = DatabaseFieldType.Int,
                FieldMemo = "Test Memo",
                SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionSettings(
                    new DatabaseFieldSpecialSettingDefinitionNormalSettings()
                ),
            };

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: ()
                    => DatabaseFieldMetadataSettingsDtoTransformHelper.TransformMetadataSettings(
                        definitionSettings,
                        DatabaseFieldType.Int
                    ),
                resultValueVerifier: ValueVerifier<IDatabaseFieldDefinitionSettings>.AreItemEquals(expected)
            );
        }

        /// <summary>
        ///     引数 が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [TestCase("definitionSettings")]
        [TestCase("fieldType")]
        public static void TransformMetadataSettings_Failure_ArgumentNull(string nullArgName)
        {
            var definitionSettings = new DatabaseFieldMetadataSettings
            {
                FieldName = "TestField",
                FieldMemo = "Test Memo",
                SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionSettings(
                    new DatabaseFieldSpecialSettingDefinitionNormalSettings()
                ),
            };
            var fieldType = DatabaseFieldType.Int;

            staticFunctionTestHelper.StaticFuncFailure(
                execFunc: ()
                    => DatabaseFieldMetadataSettingsDtoTransformHelper.TransformMetadataSettings(
                        nullArgName == "definitionSettings"
                            ? null!
                            : definitionSettings,
                        nullArgName == "fieldType"
                            ? null!
                            : fieldType
                    ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion
    }
}
