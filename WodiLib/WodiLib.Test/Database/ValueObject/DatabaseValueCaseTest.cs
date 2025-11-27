using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.ValueObject
{
    [TestFixture]
    public class DatabaseValueCaseTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Constants

        #region public

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void DefaultGetterTest()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseValueCase.Default,
                resultValueVerifier: new ValueVerifier<DatabaseValueCase>(instance =>
                    {
                        Assert.IsTrue(instance.CaseNumber == 0);
                        Assert.IsTrue(instance.Description == "");
                    }
                )
            );
        }

        #endregion

        #endregion

        #region Properties

        #region public

        #region CaseNumber

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void CaseNumberGetterTest()
        {
            const int caseNumber = 20;
            var instance = new DatabaseValueCase(caseNumber, $"{caseNumber}");
            var expected = new DatabaseValueCaseNumber(caseNumber);

            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.CaseNumber,
                getValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #region Description

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void DescriptionGetterTest()
        {
            const int caseNumber = 20;
            var instance = new DatabaseValueCase(caseNumber, $"{caseNumber}");
            var expected = new DatabaseValueCaseDescription($"{caseNumber}");

            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.Description,
                getValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #endregion

        #endregion

        #region Constructors

        #region NoParams

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void ConstructorTest_NoParams_Success()
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new DatabaseValueCase(),
                instanceVerifier: new ValueVerifier<DatabaseValueCase>(instance =>
                    {
                        Assert.AreEqual(0, instance.CaseNumber.RawValue);
                        Assert.AreEqual("", instance.Description.RawValue);
                    }
                )
            );
        }

        #endregion

        #region From CaseNumber & Description

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [Test]
        public static void ConstructorTest_FromCaseNumberAndDescription_Success()
        {
            var caseNumber = new DatabaseValueCaseNumber(20);
            var description = new DatabaseValueCaseDescription("Value");

            constructorTestHelper.ConstructorSuccess(
                factory: () => new DatabaseValueCase(caseNumber, description),
                instanceVerifier: new ValueVerifier<DatabaseValueCase>(instance =>
                    {
                        Assert.AreEqual(caseNumber, instance.CaseNumber);
                        Assert.AreEqual(description, instance.Description);
                    }
                )
            );
        }

        /// <summary>
        ///     caseNumber, description が null の場合、
        ///     ArgumentNullExceptoin が発生すること。
        /// </summary>
        [TestCase("caseNumber")]
        [TestCase("description")]
        public static void ConstructorTest_FromCaseNumberAndDescription_Failure_NullArgs(string nullArgName)
        {
            var caseNumber = nullArgName == "caseNumber"
                ? null!
                : new DatabaseValueCaseNumber(20);
            var description = nullArgName == "description"
                ? null!
                : new DatabaseValueCaseDescription("Value");

            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseValueCase(caseNumber, description),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion

        #endregion

        #region Methods

        #region public

        #region GetHashCode

        [Test]
        public static void GetHashCodeTest()
        {
            var instance1 = new DatabaseValueCase(1, "Case 1");
            var instance2 = new DatabaseValueCase(1, "Case 1");
            var instance3 = new DatabaseValueCase(2, "Case 2");
            var instance4 = new DatabaseValueCase(1, "Case 2");
            var instance5 = new DatabaseValueCase(2, "Case 1");

            // 同じ値は同じハッシュコードを返すこと
            Assert.AreEqual(instance1.GetHashCode(), instance2.GetHashCode());

            // 異なる値は異なるハッシュコードを返すこと
            Assert.AreNotEqual(instance1.GetHashCode(), instance3.GetHashCode());
            Assert.AreNotEqual(instance1.GetHashCode(), instance4.GetHashCode());
            Assert.AreNotEqual(instance1.GetHashCode(), instance5.GetHashCode());
        }

        #endregion

        #endregion

        #endregion
    }
}
