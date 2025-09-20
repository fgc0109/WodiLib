using System;
using System.Collections.Generic;
using System.ComponentModel;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;

namespace WodiLib.Test.Tools
{
    /// <summary>
    ///     戻り値のない純粋メソッドテスト用テンプレート処理を定義したクラス
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
    ///                 プロパティ変更通知が行われていないことを検証する。
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
    internal class PureActionTestHelper : TestHelperBase
    {
        public PureActionTestHelper(Logger logger) : base(logger)
        {
        }

        /// <summary>
        ///     純粋メソッドのテスト(戻り値なし)（成功パターン）
        /// </summary>
        /// <param name="instance">テスト対象のインスタンス</param>
        /// <param name="execAction">メソッド実行処理</param>
        /// <returns></returns>
        public void PureActionSuccess<TTarget>(
            TTarget instance,
            Action<TTarget> execAction
        ) => PureActionInternal(
            instance,
            execAction,
            expectedFailure: false,
            verifyException: null
        );

        /// <summary>
        ///     純粋メソッドのテスト(戻り値なし)（失敗パターン）
        /// </summary>
        /// <param name="instance">テスト対象のインスタンス</param>
        /// <param name="execAction">メソッド実行処理</param>
        /// <param name="verifyException">
        ///     例外発生時の例外検証処理。<br/>
        ///     <br/>
        ///     <see langword="null"/> の場合、例外が発生しても検証処理を行わない。
        /// </param>
        public void PureActionFailure<TTarget>(
            TTarget instance,
            Action<TTarget> execAction,
            ValueVerifier<Exception>? verifyException = null
        ) => PureActionInternal(
            instance,
            execAction,
            expectedFailure: true,
            verifyException
        );

        private void PureActionInternal<TTarget>(
            TTarget instance,
            Action<TTarget> execAction,
            bool expectedFailure,
            ValueVerifier<Exception>? verifyException = null
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
            Exception? exception = null;
            try
            {
                execAction.Invoke(instance);
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
                // 発生したエラーの検証処理
                verifyException?.Verify(exception!);
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
