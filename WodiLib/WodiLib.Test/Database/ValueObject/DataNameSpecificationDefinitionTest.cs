using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.ValueObject
{
    [TestFixture]
    public class DataNameSpecificationDefinitionTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Constants

        #region public

        #region Default

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void DefaultGetterTest()
        {
            var expected = new DataNameSpecificationDefinition(DatabaseKind.Changeable, new TypeId(0));

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DataNameSpecificationDefinition.Default,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #endregion

        #endregion

        #region Properties

        #region public

        #region DatabaseKind

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void DatabaseKindGetterTest_Success()
        {
            var expected = DatabaseKind.User;

            propertyTestHelper.PropertyGetSuccess(
                instance: new DataNameSpecificationDefinition(expected, new TypeId(2)),
                getter: target => target.DatabaseKind,
                getValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #region TypeId

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void TypeIdGetterTest_Success()
        {
            var expected = new TypeId(21);

            propertyTestHelper.PropertyGetSuccess(
                instance: new DataNameSpecificationDefinition(DatabaseKind.System, expected),
                getter: target => target.TypeId,
                getValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #endregion

        #endregion

        #region Constructors

        private static object?[][] ConstructorTest_Success_TestCaseSource =
        {
            // [dbKind, typeId, expectedDbKind, expectedTypeId]
            new object?[] { DatabaseKind.Changeable, null, DatabaseKind.Changeable, new TypeId(0) },
            new object?[] { DatabaseKind.User, new TypeId(2), DatabaseKind.User, new TypeId(2) },
            new object?[] { DatabaseKind.System, new TypeId(5), DatabaseKind.System, new TypeId(5) },
            new object?[] { null, null, DatabaseKind.Changeable, new TypeId(0) },
        };

        /// <summary>
        ///     コンストラクタが正常に終了すること。
        /// </summary>
        [TestCaseSource(nameof(ConstructorTest_Success_TestCaseSource))]
        public static void ConstructorTest_Success(
            DatabaseKind? dbKind,
            TypeId? typeId,
            DatabaseKind expectedDbKind,
            TypeId expectedTypeId
        )
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new DataNameSpecificationDefinition(dbKind, typeId),
                instanceVerifier: new ValueVerifier<DataNameSpecificationDefinition>(instance =>
                    {
                        Assert.AreEqual(expectedDbKind, instance.DatabaseKind);
                        Assert.AreEqual(expectedTypeId, instance.TypeId);
                    }
                )
            );
        }

        #endregion

        #region Operation

        #region Equal / Equals(Method)

        private static readonly object?[][] EqualsTestCaseSource =
        {
            // [left, right, expectedEqual]
            new object?[] { new DataNameSpecificationDefinition(null, null), null, false },
            new object?[]
            {
                new DataNameSpecificationDefinition(null, null), new DataNameSpecificationDefinition(null, null), true,
            },
            new object?[]
            {
                new DataNameSpecificationDefinition(DatabaseKind.Changeable, new TypeId(2)),
                new DataNameSpecificationDefinition(DatabaseKind.Changeable, new TypeId(2)), true,
            },
            new object?[]
            {
                new DataNameSpecificationDefinition(DatabaseKind.Changeable, new TypeId(2)),
                new DataNameSpecificationDefinition(DatabaseKind.Changeable, new TypeId(6)), false,
            },
            new object?[]
            {
                new DataNameSpecificationDefinition(DatabaseKind.Changeable, new TypeId(2)),
                new DataNameSpecificationDefinition(DatabaseKind.User, new TypeId(2)), false,
            },
            new object?[]
            {
                new DataNameSpecificationDefinition(DatabaseKind.Changeable, new TypeId(2)),
                new DataNameSpecificationDefinition(DatabaseKind.System, new TypeId(2)), false,
            },
        };

        /// <summary>
        ///     等価比較演算 == が意図した値を返すこと。
        /// </summary>
        [TestCaseSource(nameof(EqualsTestCaseSource))]
        public static void OperatorEqualTest(
            DataNameSpecificationDefinition left,
            DataNameSpecificationDefinition? right,
            bool expectedEqual
        )
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => left == right,
                resultValueVerifier: ValueVerifier.AreEquals(expectedEqual)
            );
        }

        /// <summary>
        ///     等価比較演算 != が意図した値を返すこと。
        /// </summary>
        [TestCaseSource(nameof(EqualsTestCaseSource))]
        public static void OperatorNotEqualTest(
            DataNameSpecificationDefinition left,
            DataNameSpecificationDefinition? right,
            bool expectedEqual
        )
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => left != right,
                resultValueVerifier: ValueVerifier.AreEquals(!expectedEqual)
            );
        }

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと。
        /// </summary>
        [TestCaseSource(nameof(EqualsTestCaseSource))]
        public static void EqualsTest_DataNameSpecificationDefinition(
            DataNameSpecificationDefinition left,
            DataNameSpecificationDefinition? right,
            bool expectedEqual
        )
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => left.Equals(right),
                resultValueVerifier: ValueVerifier.AreEquals(expectedEqual)
            );
        }

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと。
        /// </summary>
        [TestCaseSource(nameof(EqualsTestCaseSource))]
        public static void EqualsTest_Object(DataNameSpecificationDefinition left, object? right, bool expectedEqual)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => left.Equals(right),
                resultValueVerifier: ValueVerifier.AreEquals(expectedEqual)
            );
        }

        #endregion

        #endregion
    }
}
