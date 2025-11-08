using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Extension
{
    [TestFixture]
    public class ChangeableDatabaseAddressExtensionTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region GetTypeId

        /// <summary>
        ///     意図した値が取得できること。
        /// </summary>
        /// <param name="src">取得元</param>
        /// <param name="expected">期待する値</param>
        [TestCase(1100000102, 0)]
        [TestCase(1124012345, 24)]
        public static void GetTypeIdTest_Success(int src, int expected)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => ChangeableDatabaseAddressExtension.GetTypeId(src),
                resultValueVerifier: ValueVerifier<TypeId>.AreEquals(new TypeId(expected))
            );
        }

        /// <summary>
        ///     引数 src が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void GetTypeIdTest_Failure_ArgumentNull()
        {
            staticFunctionTestHelper.StaticFuncFailure(
                execFunc: () => ChangeableDatabaseAddressExtension.GetTypeId(null!),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion

        #region GetDataId

        /// <summary>
        ///     意図した値が取得できること。
        /// </summary>
        /// <param name="src">取得元</param>
        /// <param name="expected">期待する値</param>
        [TestCase(1100000102, 1)]
        [TestCase(1124012345, 123)]
        public static void GetDataIdTest_Success(int src, int expected)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => ChangeableDatabaseAddressExtension.GetDataId(src),
                resultValueVerifier: ValueVerifier<DataId>.AreEquals(new DataId(expected))
            );
        }

        /// <summary>
        ///     引数 src が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void GetDataIdTest_Failure_ArgumentNull()
        {
            staticFunctionTestHelper.StaticFuncFailure(
                execFunc: () => ChangeableDatabaseAddressExtension.GetDataId(null!),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion

        #region GetFieldId

        /// <summary>
        ///     意図した値が取得できること。
        /// </summary>
        /// <param name="src">取得元</param>
        /// <param name="expected">期待する値</param>
        [TestCase(1100000102, 2)]
        [TestCase(1124012345, 45)]
        public static void GetFieldIdTest_Success(int src, int expected)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => ChangeableDatabaseAddressExtension.GetFieldId(src),
                resultValueVerifier: ValueVerifier<FieldId>.AreEquals(new FieldId(expected))
            );
        }

        /// <summary>
        ///     引数 src が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void GetFieldIdTest_Failure_ArgumentNull()
        {
            staticFunctionTestHelper.StaticFuncFailure(
                execFunc: () => ChangeableDatabaseAddressExtension.GetFieldId(null!),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion
    }
}
