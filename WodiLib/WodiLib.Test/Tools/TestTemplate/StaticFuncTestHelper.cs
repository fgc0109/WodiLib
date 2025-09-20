using System;
using Commons;
using NUnit.Framework;

namespace WodiLib.Test.Tools
{
    /// <summary>
    ///     戻り値のある静的関数テスト用テンプレート処理を定義したクラス
    /// </summary>
    /// <remarks>
    ///     以下の手順のテストを行う。
    ///     <list type="number">
    ///         <item>
    ///             <description>
    ///                 メソッドを実行し、エラーの有無を検証する。
    ///                 エラー発生時はここで終了。
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 返戻値が意図した値であることを検証する。
    ///             </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    internal class StaticFunctionTestHelper : TestHelperBase
    {
        public StaticFunctionTestHelper(Logger logger) : base(logger)
        {
        }

        /// <summary>
        ///     静的関数のテスト（成功パターン）
        /// </summary>
        /// <param name="execFunc">メソッド実行処理</param>
        /// <param name="resultValueVerifier">
        ///     メソッド戻り値の検証処理<br/>
        ///     エラー時には <see cref="Assert"/> などを利用して例外を発生させる。<br/>
        ///     <br/>
        ///     <see langword="null"/> の場合、検証処理を行わない。
        /// </param>
        /// <typeparam name="TResult">メソッド返却型</typeparam>
        public void StaticFuncSuccess<TResult>(
            Func<TResult> execFunc,
            ValueVerifier<TResult>? resultValueVerifier = null
        ) => StaticFuncInternal(
            execFunc,
            expectedFailure: false,
            resultValueVerifier,
            exceptionVerifier: null
        );

        /// <summary>
        ///     静的関数のテスト（失敗パターン）
        /// </summary>
        /// <param name="execFunc">メソッド実行処理</param>
        /// <param name="exceptionVerifier">
        ///     例外発生時の例外検証処理。<br/>
        ///     <br/>
        ///     <see langword="null"/> の場合、検証処理を行わない。
        /// </param>
        /// <typeparam name="TResult">メソッド返却型</typeparam>
        public void StaticFuncFailure<TResult>(
            Func<TResult> execFunc,
            ValueVerifier<Exception>? exceptionVerifier = null
        ) => StaticFuncInternal(
            execFunc,
            expectedFailure: true,
            resultValueVerifier: null,
            exceptionVerifier
        );

        private void StaticFuncInternal<TResult>(
            Func<TResult> execFunc,
            bool expectedFailure,
            ValueVerifier<TResult>? resultValueVerifier = null,
            ValueVerifier<Exception>? exceptionVerifier = null
        )
        {
            TResult execResult = default!;
            Exception exception = null!;
            var errorOccured = false;
            try
            {
                execResult = execFunc();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
                exception = ex;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(
                expectedFailure,
                errorOccured,
                $"expectedFailure not eq errorOccured. ({expectedFailure}, {errorOccured})"
            );

            // エラーが発生した場合、発生したエラーの検証処理
            if (errorOccured)
            {
                exceptionVerifier?.Verify(exception);
                return;
            }

            // 取得した値が意図した値であること
            resultValueVerifier?.Verify(execResult);
        }
    }
}
