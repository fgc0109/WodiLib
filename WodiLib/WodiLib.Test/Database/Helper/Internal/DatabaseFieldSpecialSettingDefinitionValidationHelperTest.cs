using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class DatabaseFieldSpecialSettingDefinitionValidationHelperTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region ValidateDefinitionAndTypeAsArgs

        #region From DatabaseFieldSpecialSettingDefinitionSettings

        private static object[][] ValidateDefinitionAndTypeAsArgsFromUnionTest_Success_TestCaseSource =
        {
            // [definitionSettings, type]
            new object[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettings(
                    new DatabaseFieldSpecialSettingDefinitionNormalSettings()
                ),
                DatabaseFieldType.Int,
            },
            new object[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettings(
                    new DatabaseFieldSpecialSettingDefinitionNormalSettings()
                ),
                DatabaseFieldType.String,
            },
            new object[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettings(
                    new DatabaseFieldSpecialSettingDefinitionLoadFileSettings()
                ),
                DatabaseFieldType.String,
            },
            new object[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettings(
                    new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings()
                ),
                DatabaseFieldType.Int,
            },
            new object[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettings(
                    new DatabaseFieldSpecialSettingDefinitionManualSettings()
                ),
                DatabaseFieldType.Int,
            },
        };

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCaseSource(nameof(ValidateDefinitionAndTypeAsArgsFromUnionTest_Success_TestCaseSource))]
        public static void ValidateDefinitionAndTypeAsArgsFromUnionTest_Success(
            DatabaseFieldSpecialSettingDefinitionSettings definitionSettings,
            DatabaseFieldType type
        )
        {
            staticActionTestHelper.StaticActionSuccess(
                execAction: () => DatabaseFieldSpecialSettingDefinitionValidationHelper.ValidateDefinitionAndTypeAsArgs(
                    (nameof(definitionSettings), definitionSettings),
                    (nameof(type), type)
                )
            );
        }

        /// <summary>
        ///     引数 definitionSettings, type が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [TestCase("definitionSettings")]
        [TestCase("type")]
        public static void ValidateDefinitionAndTypeAsArgsFromUnionTest_Failure_ArgumentNull(string nullArgName)
        {
            var definitionSettings = nullArgName == "definitionSettings"
                ? null!
                : new NamedValue<IDatabaseFieldSpecialSettingDefinitionSettings>(
                    "definitionSettings",
                    new DatabaseFieldSpecialSettingDefinitionSettings(
                        new DatabaseFieldSpecialSettingDefinitionNormalSettings()
                    )
                );
            var type = nullArgName == "type"
                ? null!
                : new NamedValue<DatabaseFieldType>(
                    "type",
                    DatabaseFieldType.Int
                );

            staticActionTestHelper.StaticActionFailure(
                execAction: () => DatabaseFieldSpecialSettingDefinitionValidationHelper.ValidateDefinitionAndTypeAsArgs(
                    definitionSettings,
                    type
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        private static object[][] ValidateDefinitionAndTypeAsArgsFromUnionTest_Failure_CannotChangeType_TestCaseSource =
        {
            // [definitionSettings, type]
            new object[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettings(
                    new DatabaseFieldSpecialSettingDefinitionLoadFileSettings()
                ),
                DatabaseFieldType.Int,
            },
            new object[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettings(
                    new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings()
                ),
                DatabaseFieldType.String,
            },
            new object[]
            {
                new DatabaseFieldSpecialSettingDefinitionSettings(
                    new DatabaseFieldSpecialSettingDefinitionManualSettings()
                ),
                DatabaseFieldType.String,
            },
        };

        /// <summary>
        ///     引数 type の値種別に変更できないDB項目特殊設定の場合、
        ///     InvalidOperationException が発生すること。
        /// </summary>
        [TestCaseSource(nameof(ValidateDefinitionAndTypeAsArgsFromUnionTest_Failure_CannotChangeType_TestCaseSource))]
        public static void ValidateDefinitionAndTypeAsArgsFromUnionTest_Failure_CannotChangeType(
            DatabaseFieldSpecialSettingDefinitionSettings definitionSettings,
            DatabaseFieldType type
        )
        {
            staticActionTestHelper.StaticActionFailure(
                execAction: () => DatabaseFieldSpecialSettingDefinitionValidationHelper.ValidateDefinitionAndTypeAsArgs(
                    (nameof(definitionSettings), definitionSettings),
                    (nameof(type), type)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(InvalidOperationException))
            );
        }

        #endregion

        #region From IReadOnlyDatabaseFieldSpecialSettingDefinition

        private static object[][] ValidateDefinitionAndTypeAsArgsFromDefinitionTest_Success_TestCaseSource =
        {
            // [definition, type]
            new object[]
            {
                new DatabaseFieldSpecialSettingDefinitionNormal(),
                DatabaseFieldType.Int,
            },
            new object[]
            {
                new DatabaseFieldSpecialSettingDefinitionNormal(),
                DatabaseFieldType.String,
            },
            new object[]
            {
                new DatabaseFieldSpecialSettingDefinitionLoadFile(),
                DatabaseFieldType.String,
            },
            new object[]
            {
                new DatabaseFieldSpecialSettingDefinitionDatabaseReference(),
                DatabaseFieldType.Int,
            },
            new object[]
            {
                new DatabaseFieldSpecialSettingDefinitionManual(),
                DatabaseFieldType.Int,
            },
        };

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCaseSource(nameof(ValidateDefinitionAndTypeAsArgsFromDefinitionTest_Success_TestCaseSource))]
        public static void ValidateDefinitionAndTypeAsArgsFromDefinitionTest_Success(
            IReadOnlyDatabaseFieldSpecialSettingDefinition definitionSettings,
            DatabaseFieldType type
        )
        {
            staticActionTestHelper.StaticActionSuccess(
                execAction: () => DatabaseFieldSpecialSettingDefinitionValidationHelper.ValidateDefinitionAndTypeAsArgs(
                    new NamedValue<IReadOnlyDatabaseFieldSpecialSettingDefinition>(
                        nameof(definitionSettings),
                        definitionSettings
                    ),
                    (nameof(type), type)
                )
            );
        }

        /// <summary>
        ///     引数 definitionSettings, type が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [TestCase("definitionSettings")]
        [TestCase("type")]
        public static void ValidateDefinitionAndTypeAsArgsFromDefinitionTest_Failure_ArgumentNull(string nullArgName)
        {
            var definitionSettings = nullArgName == "definitionSettings"
                ? null!
                : new NamedValue<IReadOnlyDatabaseFieldSpecialSettingDefinition>(
                    "definitionSettings",
                    new DatabaseFieldSpecialSettingDefinitionNormal()
                );
            var type = nullArgName == "type"
                ? null!
                : new NamedValue<DatabaseFieldType>(
                    "type",
                    DatabaseFieldType.Int
                );

            staticActionTestHelper.StaticActionFailure(
                execAction: () => DatabaseFieldSpecialSettingDefinitionValidationHelper.ValidateDefinitionAndTypeAsArgs(
                    definitionSettings,
                    type
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        private static object[][]
            ValidateDefinitionAndTypeAsArgsFromDefinitionTest_Failure_CannotChangeType_TestCaseSource =
            {
                // [definitionSettings, type]
                new object[]
                {
                    new DatabaseFieldSpecialSettingDefinitionLoadFile(),
                    DatabaseFieldType.Int,
                },
                new object[]
                {
                    new DatabaseFieldSpecialSettingDefinitionDatabaseReference(),
                    DatabaseFieldType.String,
                },
                new object[]
                {
                    new DatabaseFieldSpecialSettingDefinitionManual(),
                    DatabaseFieldType.String,
                },
            };

        /// <summary>
        ///     引数 type の値種別に変更できないDB項目特殊設定の場合、
        ///     InvalidOperationException が発生すること。
        /// </summary>
        [TestCaseSource(
            nameof(ValidateDefinitionAndTypeAsArgsFromDefinitionTest_Failure_CannotChangeType_TestCaseSource)
        )]
        public static void ValidateDefinitionAndTypeAsArgsFromDefinitionTest_Failure_CannotChangeType(
            IReadOnlyDatabaseFieldSpecialSettingDefinition definitionSettings,
            DatabaseFieldType type
        )
        {
            staticActionTestHelper.StaticActionFailure(
                execAction: () => DatabaseFieldSpecialSettingDefinitionValidationHelper.ValidateDefinitionAndTypeAsArgs(
                    new NamedValue<IReadOnlyDatabaseFieldSpecialSettingDefinition>(
                        nameof(definitionSettings),
                        definitionSettings
                    ),
                    (nameof(type), type)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(InvalidOperationException))
            );
        }

        #endregion

        #endregion
    }
}
