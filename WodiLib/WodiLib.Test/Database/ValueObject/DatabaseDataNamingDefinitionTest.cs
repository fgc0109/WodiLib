using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.ValueObject
{
    [TestFixture]
    public class DatabaseDataNamingDefinitionTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Properties

        #region Default

        /// <summary>
        ///     意図した値が取得できること。
        /// </summary>
        [Test]
        public static void DefaultGetterTest_Success()
        {
            var expected = new DatabaseDataNamingDefinition(DatabaseDataNamingType.Manual);

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseDataNamingDefinition.Default,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #region NamingType

        private static readonly object[][] NamingTypeGetterTest_Success_TestCaseSource =
        {
            // [namingDefinition, expected]
            new object[] { DatabaseDataNamingDefinition.BuildManual(), DatabaseDataNamingType.Manual },
            new object[]
                { DatabaseDataNamingDefinition.BuildFirstStringData(), DatabaseDataNamingType.FirstStringData },
            new object[] { DatabaseDataNamingDefinition.BuildEqualBefore(), DatabaseDataNamingType.EqualBefore },
            new object[]
            {
                DatabaseDataNamingDefinition.BuildDesignatedType(DatabaseKind.User, 1),
                DatabaseDataNamingType.DesignatedType,
            },
            new object[]
            {
                DatabaseDataNamingDefinition.BuildDesignatedType(DatabaseKind.System, 1),
                DatabaseDataNamingType.DesignatedType,
            },
        };

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [TestCaseSource(nameof(NamingTypeGetterTest_Success_TestCaseSource))]
        public static void NamingTypeGetterTest_Success(
            DatabaseDataNamingDefinition namingDefinition,
            DatabaseDataNamingType expected
        )
        {
            propertyTestHelper.PropertyGetSuccess(
                instance: namingDefinition,
                getter: target => target.NamingType,
                getValueVerifier: ValueVerifier<DatabaseDataNamingType>.AreEquals(expected)
            );
        }

        #endregion

        #region DBKind

        private static readonly object?[][] DBKindGetterTest_Success_TestCaseSource =
        {
            // [namingDefinition, expected]
            new object?[] { DatabaseDataNamingDefinition.BuildManual(), null },
            new object?[] { DatabaseDataNamingDefinition.BuildFirstStringData(), null },
            new object?[] { DatabaseDataNamingDefinition.BuildEqualBefore(), null },
            new object?[]
            {
                DatabaseDataNamingDefinition.BuildDesignatedType(DatabaseKind.User, 1),
                DatabaseKind.User,
            },
            new object?[]
            {
                DatabaseDataNamingDefinition.BuildDesignatedType(DatabaseKind.Changeable, 3),
                DatabaseKind.Changeable,
            },
            new object?[]
            {
                DatabaseDataNamingDefinition.BuildDesignatedType(DatabaseKind.System, 18),
                DatabaseKind.System,
            },
        };

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [TestCaseSource(nameof(DBKindGetterTest_Success_TestCaseSource))]
        public static void DBKindGetterTest_Success(
            DatabaseDataNamingDefinition namingDefinition,
            DatabaseKind? expected
        )
        {
            propertyTestHelper.PropertyGetSuccess(
                instance: namingDefinition,
                getter: target => target.DBKind,
                getValueVerifier: ValueVerifier<DatabaseKind?>.AreEquals(expected)
            );
        }

        #endregion

        #region TypeId

        private static readonly object?[][] TypeIdGetterTest_Success_TestCaseSource =
        {
            // [namingDefinition, expected]
            new object?[] { DatabaseDataNamingDefinition.BuildManual(), null },
            new object?[] { DatabaseDataNamingDefinition.BuildFirstStringData(), null },
            new object?[] { DatabaseDataNamingDefinition.BuildEqualBefore(), null },
            new object?[]
            {
                DatabaseDataNamingDefinition.BuildDesignatedType(DatabaseKind.User, 1),
                new TypeId(1),
            },
            new object?[]
            {
                DatabaseDataNamingDefinition.BuildDesignatedType(DatabaseKind.Changeable, 4),
                new TypeId(4),
            },
            new object?[]
            {
                DatabaseDataNamingDefinition.BuildDesignatedType(DatabaseKind.System, 21),
                new TypeId(21),
            },
        };

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [TestCaseSource(nameof(TypeIdGetterTest_Success_TestCaseSource))]
        public static void TypeIdGetterTest_Success(
            DatabaseDataNamingDefinition namingDefinition,
            TypeId? expected
        )
        {
            propertyTestHelper.PropertyGetSuccess(
                instance: namingDefinition,
                getter: target => target.TypeId,
                getValueVerifier: ValueVerifier<TypeId?>.AreEquals(expected)
            );
        }

        #endregion

        #endregion

        #region Constructor

        #region From DataNamingType & DatabaseKind & TypeId

        private static readonly object?[][]
            ConstructorTest_FromDataNamingAndDatabaseKindAndTypeId_Success_TestCaseSource =
            {
                // [namingType, dBKind, typeId, expectedNamingType, expectedDBKind, expectedTypeId]
                new object?[] { null, null, null, DatabaseDataNamingType.Manual, null, null },
                new object?[]
                    { DatabaseDataNamingType.EqualBefore, null, null, DatabaseDataNamingType.EqualBefore, null, null },
                new object?[]
                {
                    DatabaseDataNamingType.FirstStringData, null, null, DatabaseDataNamingType.FirstStringData, null,
                    null,
                },
                new object?[] { DatabaseDataNamingType.Manual, null, null, DatabaseDataNamingType.Manual, null, null },
                new object?[]
                {
                    DatabaseDataNamingType.Manual, DatabaseKind.Changeable, new TypeId(3),
                    DatabaseDataNamingType.Manual, null, null,
                },
                new object?[]
                {
                    DatabaseDataNamingType.DesignatedType, DatabaseKind.User, new TypeId(0),
                    DatabaseDataNamingType.DesignatedType, DatabaseKind.User, new TypeId(0),
                },
                new object?[]
                {
                    DatabaseDataNamingType.DesignatedType, DatabaseKind.System, new TypeId(5),
                    DatabaseDataNamingType.DesignatedType, DatabaseKind.System, new TypeId(5),
                },
                new object?[]
                {
                    DatabaseDataNamingType.DesignatedType, DatabaseKind.Changeable, new TypeId(2),
                    DatabaseDataNamingType.DesignatedType, DatabaseKind.Changeable, new TypeId(2),
                },
            };

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [TestCaseSource(nameof(ConstructorTest_FromDataNamingAndDatabaseKindAndTypeId_Success_TestCaseSource))]
        public static void ConstructorTest_FromDataNamingAndDatabaseKindAndTypeId_Success(
            DatabaseDataNamingType? namingType,
            DatabaseKind? dBKind,
            TypeId? typeId,
            DatabaseDataNamingType? expectedNamingType,
            DatabaseKind? expectedDBKind,
            TypeId? expectedTypeId
        )
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new DatabaseDataNamingDefinition(namingType, dBKind, typeId),
                instanceVerifier: new ValueVerifier<DatabaseDataNamingDefinition>(instance =>
                    {
                        Assert.IsTrue(instance.NamingType == expectedNamingType);
                        Assert.IsTrue(instance.DBKind == expectedDBKind);
                        Assert.IsTrue(instance.TypeId == expectedTypeId);
                    }
                )
            );
        }

        /// <summary>
        ///     namingType が DesignatedType の場合かつ dBKind, typeId が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [TestCase("dbKind")]
        [TestCase("typeId")]
        public static void ConstructorTest_FromDataNamingAndDatabaseKindAndTypeId_Failure_NullArgs(string nullArgName)
        {
            var namingType = DatabaseDataNamingType.DesignatedType;
            var dbKind = nullArgName == "dbKind"
                ? null
                : DatabaseKind.User;
            var typeId = nullArgName == "typeId"
                ? null
                : new TypeId(1);

            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseDataNamingDefinition(namingType, dbKind, typeId),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion

        #region From DatabaseDataNamingType & DataNameSpecificationDefinition

        private static readonly object?[][]
            ConstructorTest_FromDatabaseDataNamingAndDataNameSpecificationDefinition_Success_TestCaseSource =
            {
                // [namingType, referDatabaseDefinition, expectedNamingType, expectedDBKind, expectedTypeId]
                new object?[] { null, null, DatabaseDataNamingType.Manual, null, null },
                new object?[] { DatabaseDataNamingType.Manual, null, DatabaseDataNamingType.Manual, null, null },
                new object?[]
                {
                    DatabaseDataNamingType.FirstStringData, null, DatabaseDataNamingType.FirstStringData, null, null,
                },
                new object?[]
                    { DatabaseDataNamingType.EqualBefore, null, DatabaseDataNamingType.EqualBefore, null, null },
                new object?[]
                {
                    DatabaseDataNamingType.DesignatedType,
                    new DataNameSpecificationDefinition(DatabaseKind.Changeable, new TypeId(1)),
                    DatabaseDataNamingType.DesignatedType, DatabaseKind.Changeable, new TypeId(1),
                },
            };

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [TestCaseSource(
            nameof(ConstructorTest_FromDatabaseDataNamingAndDataNameSpecificationDefinition_Success_TestCaseSource)
        )]
        public static void ConstructorTest_FromDatabaseDataNamingAndDataNameSpecificationDefinition_Success(
            DatabaseDataNamingType? namingType,
            DataNameSpecificationDefinition? referDatabaseDefinition,
            DatabaseDataNamingType? expectedNamingType,
            DatabaseKind? expectedDBKind,
            TypeId? expectedTypeId
        )
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new DatabaseDataNamingDefinition(namingType, referDatabaseDefinition),
                instanceVerifier: new ValueVerifier<DatabaseDataNamingDefinition>(instance =>
                    {
                        Assert.IsTrue(instance.NamingType == expectedNamingType);
                        Assert.IsTrue(instance.DBKind == expectedDBKind);
                        Assert.IsTrue(instance.TypeId == expectedTypeId);
                    }
                )
            );
        }

        /// <summary>
        ///     namingType が DesignatedType の場合かつ referDatabaseDefinition が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test()]
        public static void ConstructorTest_FromDatabaseDataNamingAndDataNameSpecificationDefinition_Failure_NullArgs()
        {
            var namingType = DatabaseDataNamingType.DesignatedType;

            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseDataNamingDefinition(namingType, null!),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion

        #region Methods

        #region public

        #region BuildManual

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void BuildManualTest_Success()
        {
            var expected = new DatabaseDataNamingDefinition(DatabaseDataNamingType.Manual);

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseDataNamingDefinition.BuildManual(),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #region BuildFirstStringData

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void BuildFirstStringDataTest_Success()
        {
            var expected = new DatabaseDataNamingDefinition(DatabaseDataNamingType.FirstStringData);

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseDataNamingDefinition.BuildFirstStringData(),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #region BuildEqualBefore

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void BuildEqualBeforeTest_Success()
        {
            var expected = new DatabaseDataNamingDefinition(DatabaseDataNamingType.EqualBefore);

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseDataNamingDefinition.BuildEqualBefore(),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #region BuildDesignatedType

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void BuildDesignatedTypeTest_Success()
        {
            var dbKind = DatabaseKind.User;
            var typeId = new TypeId(1);
            var expected = new DatabaseDataNamingDefinition(DatabaseDataNamingType.DesignatedType, dbKind, typeId);

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseDataNamingDefinition.BuildDesignatedType(dbKind, typeId),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     dbKind, typeId が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [TestCase("dbKind")]
        [TestCase("typeId")]
        public static void BuildDesignatedTypeTest_Failure_NullArgs(string nullArgName)
        {
            var dbKind = nullArgName == "dbKind"
                ? null!
                : DatabaseKind.User;
            var typeId = nullArgName == "typeId"
                ? null!
                : new TypeId(1);

            staticFunctionTestHelper.StaticFuncFailure(
                execFunc: () => DatabaseDataNamingDefinition.BuildDesignatedType(dbKind, typeId),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion

        #region Equals

        #region DatabaseDataNamingDefinition

        private static readonly object?[][] EquqlsTest_DatabaseDataNamingDefinition_TestCaseSource =
        {
            // [left, right, expected]
            new object?[] { DatabaseDataNamingDefinition.BuildManual(), null, false },
            new object?[]
                { DatabaseDataNamingDefinition.BuildManual(), DatabaseDataNamingDefinition.BuildManual(), true },
            new object?[]
            {
                DatabaseDataNamingDefinition.BuildManual(), DatabaseDataNamingDefinition.BuildFirstStringData(), false,
            },
            new object?[]
                { DatabaseDataNamingDefinition.BuildManual(), DatabaseDataNamingDefinition.BuildEqualBefore(), false },
            new object?[]
            {
                DatabaseDataNamingDefinition.BuildManual(),
                DatabaseDataNamingDefinition.BuildDesignatedType(DatabaseKind.User, 1), false,
            },
            new object?[]
            {
                DatabaseDataNamingDefinition.BuildManual(),
                DatabaseDataNamingDefinition.BuildDesignatedType(DatabaseKind.System, 1), false,
            },
            new object?[]
            {
                DatabaseDataNamingDefinition.BuildManual(),
                DatabaseDataNamingDefinition.BuildDesignatedType(DatabaseKind.Changeable, 1), false,
            },
            new object?[]
            {
                DatabaseDataNamingDefinition.BuildFirstStringData(), DatabaseDataNamingDefinition.BuildManual(), false,
            },
            new object?[]
            {
                DatabaseDataNamingDefinition.BuildDesignatedType(DatabaseKind.User, 1),
                DatabaseDataNamingDefinition.BuildDesignatedType(DatabaseKind.User, 1), true,
            },
            new object?[]
            {
                DatabaseDataNamingDefinition.BuildDesignatedType(DatabaseKind.User, 1),
                DatabaseDataNamingDefinition.BuildDesignatedType(DatabaseKind.User, 2), false,
            },
            new object?[]
            {
                DatabaseDataNamingDefinition.BuildDesignatedType(DatabaseKind.User, 1),
                DatabaseDataNamingDefinition.BuildDesignatedType(DatabaseKind.System, 1), false,
            },
        };

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと。
        /// </summary>
        [TestCaseSource(nameof(EquqlsTest_DatabaseDataNamingDefinition_TestCaseSource))]
        public static void EquqlsTest_DatabaseDataNamingDefinition(
            DatabaseDataNamingDefinition left,
            DatabaseDataNamingDefinition? right,
            bool expected
        )
        {
            equalsTestHelper.Equals(
                left,
                right,
                expected
            );
        }

        #endregion

        #region Object

        private static readonly object?[][] EquqlsTest_Object_TestCaseSource =
        {
            // [left, right, expected]
            new object?[] { DatabaseDataNamingDefinition.BuildManual(), null, false },
            new object?[]
                { DatabaseDataNamingDefinition.BuildManual(), DatabaseDataNamingDefinition.BuildManual(), true },
            new object?[]
                { DatabaseDataNamingDefinition.BuildManual(), DatabaseDataNamingDefinition.BuildEqualBefore(), false },
            new object?[] { DatabaseDataNamingDefinition.BuildManual(), "10", false },
        };

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと。
        /// </summary>
        [TestCaseSource(nameof(EquqlsTest_Object_TestCaseSource))]
        public static void EquqlsTest_Object(DatabaseDataNamingDefinition left, object? right, bool expected)
        {
            equalsTestHelper.Equals(
                left,
                right,
                expected
            );
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #endregion
    }
}
