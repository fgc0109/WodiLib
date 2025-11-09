using System;
using Commons;
using NUnit.Framework;

namespace WodiLib.Test.Tools
{
    /// <summary>
    ///     コンストラクタテスト用テンプレート処理を定義したクラス
    /// </summary>
    /// <remarks>
    ///     以下のテストを行う。
    ///     <list type="number">
    ///         <item>
    ///             <description>
    ///                 コンストラクタ実行によりインスタンスを生成し、
    ///                 エラーの有無をテスト
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 生成されたインスタンスの検証
    ///             </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    internal class ConstructorTestHelper : TestHelperBase
    {
        public ConstructorTestHelper(Logger logger) : base(logger)
        {
        }

        /// <summary>
        ///     コンストラクタテスト（成功パターン）
        /// </summary>
        /// <param name="factory">コンストラクタ実行処理</param>
        /// <param name="instanceVerifier">
        ///     コンストラクタで生成されたインスタンスの検証処理<br/>
        ///     エラー時には <see cref="Assert"/> などを利用して例外を発生させる。<br/>
        ///     <see langword="null"/>の場合、検証処理を行わない。
        /// </param>
        /// <typeparam name="T">作成するインスタンス型</typeparam>
        public void ConstructorSuccess<T>(
            Func<T> factory,
            ValueVerifier<T>? instanceVerifier = null
        ) => ConstructorInternal(
            factory,
            expectedFailure: false,
            instanceVerifier,
            exceptionVerifier: null
        );

        /// <summary>
        ///     コンストラクタテスト（失敗パターン）
        /// </summary>
        /// <param name="factory">コンストラクタ実行処理</param>
        /// <param name="exceptionVerifier">
        ///     例外発生時の例外検証処理。<br/>
        ///     <br/>
        ///     <see langword="null"/> の場合、例外が発生しても検証処理を行わない。
        /// </param>
        /// <typeparam name="T">作成するインスタンス型</typeparam>
        public void ConstructorFailure<T>(
            Func<T> factory,
            ValueVerifier<Exception>? exceptionVerifier = null
        ) => ConstructorInternal(
            factory,
            expectedFailure: true,
            instanceVerifier: null,
            exceptionVerifier
        );

        private void ConstructorInternal<T>(
            Func<T> factory,
            bool expectedFailure,
            ValueVerifier<T>? instanceVerifier = null,
            ValueVerifier<Exception>? exceptionVerifier = null
        )
        {
            var errorOccured = false;
            Exception? exception = null;
            try
            {
                var instance = factory();
                instanceVerifier?.Verify(instance);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
                exception = ex;
            }

            // エラー有無が意図した結果であること
            Assert.AreEqual(expectedFailure, errorOccured, exception?.ToString());

            // エラーが発生した場合、意図したとおりのエラーが発生していること
            if (errorOccured)
            {
                exceptionVerifier?.Verify(exception!);
            }
        }
    }
}
