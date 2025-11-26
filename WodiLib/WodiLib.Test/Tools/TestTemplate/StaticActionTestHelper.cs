using System;
using NUnit.Framework;
using WodiLib.Sys.Cmn;

namespace WodiLib.Test.Tools
{
    /// <summary>
    ///     戻り値のない静的関数テスト用テンプレート処理を定義したクラス
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
    ///     </list>
    /// </remarks>
    internal class StaticActionTestHelper : TestHelperBase
    {
        public StaticActionTestHelper(WodiLibLogger logger) : base(logger)
        {
        }

        /// <summary>
        ///     静的関数のテスト（成功パターン）
        /// </summary>
        /// <param name="execAction">メソッド実行処理</param>
        public void StaticActionSuccess(
            Action execAction
        ) => StaticActionInternal(
            execAction,
            expectedFailure: false,
            exceptionVerifier: null
        );

        /// <summary>
        ///     静的関数のテスト（失敗パターン）
        /// </summary>
        /// <param name="execAction">メソッド実行処理</param>
        /// <param name="exceptionVerifier">
        ///     例外発生時の例外検証処理。<br/>
        ///     <br/>
        ///     <see langword="null"/> の場合、検証処理を行わない。
        /// </param>
        public void StaticActionFailure(
            Action execAction,
            ValueVerifier<Exception>? exceptionVerifier = null
        ) => StaticActionInternal(
            execAction,
            expectedFailure: true,
            exceptionVerifier
        );

        private void StaticActionInternal(
            Action execAction,
            bool expectedFailure,
            ValueVerifier<Exception>? exceptionVerifier = null
        )
        {
            Exception exception = null!;
            var errorOccured = false;
            try
            {
                execAction();
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
            }
        }
    }
}
