using System;
using System.Collections.Generic;
using System.ComponentModel;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Test.Tools
{
    /// <summary>
    ///     戻り値のある純粋メソッドテスト用テンプレート処理を定義したクラス
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
    ///         <item>
    ///             <description>
    ///                 メソッド実行によりプロパティ変更通知が発火していないことを検証する。
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 （対象が IDeepCloneable を実装する場合のみ）
    ///                 メソッド実行前後で状態が変化していないことを検証する。
    ///             </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    internal class PureFunctionTestHelper : TestHelperBase
    {
        public PureFunctionTestHelper(WodiLibLogger logger) : base(logger)
        {
        }

        /// <summary>
        ///     純粋メソッドのテスト(戻り値あり)（成功パターン）
        /// </summary>
        /// <param name="instance">テスト対象のインスタンス</param>
        /// <param name="execFunc">メソッド実行処理</param>
        /// <param name="resultValueVerifier">
        ///     戻り値の検証処理<br/>
        ///     エラー時には <see cref="Assert"/> などを利用して例外を発生させる。
        /// </param>
        /// <returns></returns>
        public void PureFuncSuccess<TTarget, TResult>(
            TTarget instance,
            Func<TTarget, TResult> execFunc,
            ValueVerifier<TResult>? resultValueVerifier = null
        ) => PureFuncInternal(
            instance,
            execFunc,
            expectedFailure: false,
            resultValueVerifier,
            exceptionVerifier: null
        );

        /// <summary>
        ///     純粋メソッドのテスト(戻り値あり)（失敗パターン）
        /// </summary>
        /// <param name="instance">テスト対象のインスタンス</param>
        /// <param name="execFunc">メソッド実行処理</param>
        /// <param name="exceptionVerifier">
        ///     例外発生時の例外検証処理。<br/>
        ///     <br/>
        ///     <see langword="null"/> の場合、例外が発生しても検証処理を行わない。
        /// </param>
        /// <returns></returns>
        public void PureFuncFailure<TTarget, TResult>(
            TTarget instance,
            Func<TTarget, TResult> execFunc,
            ValueVerifier<Exception>? exceptionVerifier = null
        ) => PureFuncInternal(
            instance,
            execFunc,
            expectedFailure: true,
            resultValueVerifier: null,
            exceptionVerifier
        );

        private void PureFuncInternal<TTarget, TResult>(
            TTarget instance,
            Func<TTarget, TResult> execFunc,
            bool expectedFailure,
            ValueVerifier<TResult>? resultValueVerifier = null,
            ValueVerifier<Exception>? exceptionVerifier = null
        )
        {
            var changedPropertyList = new List<string>();
            if (instance is INotifyPropertyChanged propertyChangedNotifiable)
            {
                propertyChangedNotifiable.PropertyChanged += (_, args) =>
                {
                    changedPropertyList.Add(args.PropertyName!);
                };
            }

            TTarget? original = default;
            if (instance is IDeepCloneable<TTarget> deepCloneable)
            {
                original = deepCloneable.DeepClone();
            }

            var errorOccured = false;
            TResult result = default!;
            Exception? exception = null;
            try
            {
                result = execFunc(instance);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
                exception = ex;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(expectedFailure, errorOccured, exception?.ToString());

            if (errorOccured)
            {
                // エラーが発生した場合、発生したエラーの検証処理
                exceptionVerifier?.Verify(exception!);
                return;
            }
            else
            {
                // 戻り値が意図した値であること
                resultValueVerifier?.Verify(result);
            }

            // 状態が変化していないこと
            if (original is not null)
            {
                if (instance is IEqualityComparable<TTarget> comparableInstance)
                {
                    CustomAssert.AreItemEquals(comparableInstance, original);
                }
                else
                {
                    Assert.AreEqual(original, instance);
                }
            }

            // プロパティ変更通知が発火していないこと
            Assert.IsEmpty(changedPropertyList);
        }
    }
}
