using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class DatabaseFieldDefinitionSettingsDtoTransformHelperTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region TransformMetadataSettings(IDatabaseFieldDefinitionListSettings)

        /// <summary>
        ///     <see cref="IDatabaseFieldDefinitionListSettings"/> から
        ///     <see cref="IDatabaseFieldMetadataListSettings"/> への変換が正常に行われること（複数要素）。
        /// </summary>
        [Test]
        public static void TransformMetadataSettings_List_Success_MultipleElements()
        {
            var definitionListSettings = new DatabaseFieldDefinitionListSettings(
                new IDatabaseFieldDefinitionSettings[]
                {
                    new DatabaseFieldDefinitionSettings
                    {
                        FieldName = "Field1",
                        FieldType = DatabaseFieldType.Int,
                        FieldMemo = "Memo1",
                        SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionSettings(
                            new DatabaseFieldSpecialSettingDefinitionNormalSettings()
                        ),
                    },
                    new DatabaseFieldDefinitionSettings
                    {
                        FieldName = "Field2",
                        FieldType = DatabaseFieldType.String,
                        FieldMemo = "Memo2",
                        SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionSettings(
                            new DatabaseFieldSpecialSettingDefinitionNormalSettings()
                        ),
                    },
                }
            );

            var expected = new DatabaseFieldMetadataListSettings(
                new IDatabaseFieldMetadataSettings[]
                {
                    new DatabaseFieldMetadataSettings
                    {
                        FieldName = "Field1",
                        FieldMemo = "Memo1",
                        SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionSettings(
                            new DatabaseFieldSpecialSettingDefinitionNormalSettings()
                        ),
                    },
                    new DatabaseFieldMetadataSettings
                    {
                        FieldName = "Field2",
                        FieldMemo = "Memo2",
                        SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionSettings(
                            new DatabaseFieldSpecialSettingDefinitionNormalSettings()
                        ),
                    },
                }
            );

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: ()
                    => DatabaseFieldDefinitionSettingsDtoTransformHelper.TransformMetadataSettings(
                        definitionListSettings
                    ),
                resultValueVerifier: ValueVerifier<IDatabaseFieldMetadataListSettings>.AreItemEquals(expected)
            );
        }

        /// <summary>
        ///     <see cref="IDatabaseFieldDefinitionListSettings"/> から
        ///     <see cref="IDatabaseFieldMetadataListSettings"/> への変換が正常に行われること（空のリスト）。
        /// </summary>
        [Test]
        public static void TransformMetadataSettings_List_Success_EmptyList()
        {
            var definitionListSettings = new DatabaseFieldDefinitionListSettings(
                Array.Empty<IDatabaseFieldDefinitionSettings>()
            );
            var expected = new DatabaseFieldMetadataListSettings(
                Array.Empty<IDatabaseFieldMetadataSettings>()
            );

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: ()
                    => DatabaseFieldDefinitionSettingsDtoTransformHelper.TransformMetadataSettings(
                        definitionListSettings
                    ),
                resultValueVerifier: ValueVerifier<IDatabaseFieldMetadataListSettings>.AreItemEquals(expected)
            );
        }

        /// <summary>
        ///     definitionListSettings が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void TransformMetadataSettings_List_Failure_ArgumentNull()
        {
            IDatabaseFieldDefinitionListSettings definitionListSettings = null!;

            staticFunctionTestHelper.StaticFuncFailure(
                execFunc: ()
                    => DatabaseFieldDefinitionSettingsDtoTransformHelper.TransformMetadataSettings(
                        definitionListSettings
                    ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion

        #region TransformMetadataSettings(IDatabaseFieldDefinitionSettings)

        /// <summary>
        ///     <see cref="IDatabaseFieldDefinitionSettings"/> から
        ///     <see cref="IDatabaseFieldMetadataSettings"/> への変換が正常に行われること。
        /// </summary>
        [Test]
        public static void TransformMetadataSettings_Success()
        {
            var definitionSettings = new DatabaseFieldDefinitionSettings
            {
                FieldName = "TestField",
                FieldType = DatabaseFieldType.Int,
                FieldMemo = "Test Memo",
                SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionSettings(
                    new DatabaseFieldSpecialSettingDefinitionNormalSettings()
                ),
            };

            var expected = new DatabaseFieldMetadataSettings
            {
                FieldName = "TestField",
                FieldMemo = "Test Memo",
                SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionSettings(
                    new DatabaseFieldSpecialSettingDefinitionNormalSettings()
                ),
            };

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: ()
                    => DatabaseFieldDefinitionSettingsDtoTransformHelper.TransformMetadataSettings(definitionSettings),
                resultValueVerifier: ValueVerifier<IDatabaseFieldMetadataSettings>.AreItemEquals(expected)
            );
        }

        /// <summary>
        ///     definitionSettings が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void TransformMetadataSettings_Failure_ArgumentNull()
        {
            IDatabaseFieldDefinitionSettings definitionSettings = null!;

            staticFunctionTestHelper.StaticFuncFailure(
                execFunc: ()
                    => DatabaseFieldDefinitionSettingsDtoTransformHelper.TransformMetadataSettings(definitionSettings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion
    }
}
