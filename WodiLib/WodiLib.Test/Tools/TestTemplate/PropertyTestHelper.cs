using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Commons;
using NUnit.Framework;

namespace WodiLib.Test.Tools
{
    /// <summary>
    ///     プロパティテスト用テンプレート処理を定義したクラス
    /// </summary>
    /// <remarks>
    ///     以下の手順のテストを行う。
    ///     <list type="number">
    ///         <item>
    ///             <description>
    ///                 プロパティの値を編集し、エラーの有無を検証する。
    ///                 エラー発生時はここで終了。
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 既値と異なる値が編集された場合、
    ///                 プロパティ変更通知が行われていることを検証する。<br/>
    ///                 既値と同じ値が編集された場合、
    ///                 プロパティ変更通知が行われていないことを検証する。
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 再度プロパティの値を編集し、
    ///                 プロパティ変更通知が行われていないことを検証する。
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 プロパティの値を取得し、エラーの有無を検証する。
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
    ///                 編集したプロパティ値と取得したプロパティ値が同値であることを検証する。
    ///             </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    internal class PropertyTestHelper : TestHelperBase
    {
        public PropertyTestHelper(Logger logger) : base(logger)
        {
        }

        #region PropertyGetAndSet

        /// <summary>
        ///     プロパティ値編集 &amp; 取得のテスト（成功パターン）
        /// </summary>
        /// <param name="instance">テスト対象のインスタンス</param>
        /// <param name="propertyName">テスト対象のプロパティ名</param>
        /// <param name="setItem">プロパティに編集する値</param>
        /// <param name="isValueEqualsBefore">編集する値と編集前の値が同値であるか</param>
        /// <param name="setter">プロパティ編集処理</param>
        /// <param name="getter">プロパティ取得処理</param>
        /// <param name="expectedNotifyProperties">
        ///     期待する通知プロパティ名リスト（通知される順番もテストする）。<br/>
        ///     <br/>
        ///     <see langword="null"/> を指定した場合、編集対象プロパティ以外の通知チェックを行わない。<br/>
        ///     編集対象のプロパティ名については <paramref name="propertyName"/> と
        ///     <paramref name="isValueEqualsBefore"/> でチェックする。
        /// </param>
        /// <param name="instanceVerifier">
        ///     プロパティ編集後の処理対象インスタンス状態検証処理<br/>
        ///     エラー時には <see cref="Assert"/> などを利用して例外を発生させる。
        /// </param>
        /// <param name="getValueVerifier">
        ///     プロパティから取得した値の検証処理<br/>
        ///     エラー時には <see cref="Assert"/> などを利用して例外を発生させる。
        /// </param>
        /// <typeparam name="TTarget">テスト対象インスタンス型</typeparam>
        /// <typeparam name="TItem">プロパティに編集する値型</typeparam>
        public void PropertyGetAndSetSuccess<TTarget, TItem>(
            TTarget instance,
            string propertyName,
            TItem setItem,
            bool isValueEqualsBefore,
            Action<TTarget, TItem> setter,
            Func<TTarget, TItem> getter,
            IEnumerable<string>? expectedNotifyProperties = null,
            ValueVerifier<TTarget>? instanceVerifier = null,
            ValueVerifier<TItem>? getValueVerifier = null
        )
        {
            PropertySetSuccess(
                instance,
                propertyName,
                setItem,
                isValueEqualsBefore,
                setter,
                expectedNotifyProperties,
                instanceVerifier
            );
            PropertyGetSuccess(
                instance,
                getter,
                getValueVerifier
            );
        }

        #endregion

        #region PropertyGet

        /// <summary>
        ///     プロパティ値取得のテスト（成功パターン）
        /// </summary>
        /// <param name="instance">テスト対象のインスタンス</param>
        /// <param name="getter">プロパティ取得処理</param>
        /// <param name="getValueVerifier">
        ///     プロパティから取得した値の検証処理<br/>
        ///     エラー時には <see cref="Assert"/> などを利用して例外を発生させる。
        /// </param>
        /// <typeparam name="TTarget">テスト対象インスタンス型</typeparam>
        /// <typeparam name="TItem">プロパティから取得する値型</typeparam>
        public void PropertyGetSuccess<TTarget, TItem>(
            TTarget instance,
            Func<TTarget, TItem> getter,
            ValueVerifier<TItem>? getValueVerifier = null
        ) => PropertyGetInternal(
            instance,
            getter,
            expectedFailure: false,
            getValueVerifier,
            exceptionVerifier: null // 使わないため適当な設定で良い
        );

        /// <summary>
        ///     プロパティ値取得のテスト（失敗パターン）
        /// </summary>
        /// <param name="instance">テスト対象のインスタンス</param>
        /// <param name="getter">プロパティ取得処理</param>
        /// <param name="exceptionVerifier">
        ///     例外発生時の例外検証処理。<br/>
        ///     <br/>
        ///     <see langword="null"/> の場合、例外が発生しても検証処理を行わない。
        /// </param>
        /// <typeparam name="TTarget"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        public void PropertyGetFailure<TTarget, TItem>(
            TTarget instance,
            Func<TTarget, TItem> getter,
            ValueVerifier<Exception>? exceptionVerifier = null
        ) => PropertyGetInternal(
            instance,
            getter,
            expectedFailure: true,
            getValueVerifier: null, // 使わないため適当な設定で良い
            exceptionVerifier
        );

        private void PropertyGetInternal<TTarget, TItem>(
            TTarget instance,
            Func<TTarget, TItem> getter,
            bool expectedFailure,
            ValueVerifier<TItem>? getValueVerifier = null,
            ValueVerifier<Exception>? exceptionVerifier = null
        )
        {
            var changedPropertyList = new List<string>();
            var propertyChangedNotifiable = instance as INotifyPropertyChanged;
            if (propertyChangedNotifiable is not null)
            {
                propertyChangedNotifiable.PropertyChanged += (_, args) =>
                {
                    changedPropertyList.Add(args.PropertyName!);
                };
            }

            TItem getResult = default!;
            var errorOccured = false;
            Exception exception = null!;
            try
            {
                getResult = getter(instance);
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
                errorOccured
            );

            if (errorOccured)
            {
                // エラーが発生した場合、発生したエラーの検証処理
                exceptionVerifier?.Verify(exception);
                return;
            }

            // プロパティ変更通知が発火していないこと
            if (propertyChangedNotifiable is not null)
            {
                Assert.IsEmpty(changedPropertyList);
            }

            if (errorOccured)
            {
                return;
            }

            // 取得した要素の検証処理
            getValueVerifier?.Verify(getResult);
        }

        #endregion

        #region PropertySet

        /// <summary>
        ///     プロパティ値編集のテスト（成功パターン）
        /// </summary>
        /// <param name="instance">テスト対象のインスタンス</param>
        /// <param name="propertyName">テスト対象のプロパティ名</param>
        /// <param name="setItem">プロパティに編集する値</param>
        /// <param name="isValueEqualsBefore">編集する値と編集前の値が同値であるか</param>
        /// <param name="setter">プロパティ編集処理</param>
        /// <param name="expectedNotifyProperties">
        ///     期待する通知プロパティ名リスト（通知される順番もテストする）。<br/>
        ///     <br/>
        ///     <see langword="null"/> を指定した場合、編集対象プロパティ以外の通知チェックを行わない。<br/>
        ///     編集対象のプロパティ名については <paramref name="propertyName"/> と
        ///     <paramref name="isValueEqualsBefore"/> でチェックする。
        /// </param>
        /// <param name="instanceVerifier">
        ///     プロパティ編集後の処理対象インスタンス状態検証処理<br/>
        ///     エラー時には <see cref="Assert"/> などを利用して例外を発生させる。
        /// </param>
        /// <typeparam name="TTarget">テスト対象インスタンス型</typeparam>
        /// <typeparam name="TItem">プロパティに編集する値型</typeparam>
        public void PropertySetSuccess<TTarget, TItem>(
            TTarget instance,
            string propertyName,
            TItem setItem,
            bool isValueEqualsBefore,
            Action<TTarget, TItem> setter,
            IEnumerable<string>? expectedNotifyProperties = null,
            ValueVerifier<TTarget>? instanceVerifier = null
        ) => PropertySetInternal(
            instance,
            propertyName,
            setItem,
            isValueEqualsBefore,
            setter,
            expectFailure: false,
            expectedNotifyProperties,
            instanceVerifier,
            exceptionVerifier: null // 使わないため適当な設定で良い
        );

        /// <summary>
        ///     プロパティ値編集のテスト（失敗パターン）
        /// </summary>
        /// <param name="instance">テスト対象のインスタンス</param>
        /// <param name="setItem">プロパティに編集する値</param>
        /// <param name="setter">プロパティ編集処理</param>
        /// <param name="exceptionVerifier">
        ///     例外発生時の例外検証処理。<br/>
        ///     <br/>
        ///     <see langword="null"/> の場合、例外が発生しても検証処理を行わない。
        /// </param>
        /// <typeparam name="TTarget">テスト対象インスタンス型</typeparam>
        /// <typeparam name="TItem">プロパティに編集する値型</typeparam>
        public void PropertySetFailure<TTarget, TItem>(
            TTarget instance,
            TItem setItem,
            Action<TTarget, TItem> setter,
            ValueVerifier<Exception>? exceptionVerifier = null
        ) => PropertySetInternal(
            instance,
            propertyName: "", // 使わないため適当な設定で良い
            setItem,
            isValueEqualsBefore: false, // 使わないため適当な設定で良い
            setter,
            expectFailure: true,
            expectedNotifyProperties: null, // 使わないため適当な設定で良い
            instanceVerifier: null, // 使わないため適当な設定で良い
            exceptionVerifier
        );

        private void PropertySetInternal<TTarget, TItem>(
            TTarget instance,
            string propertyName,
            TItem setItem,
            bool isValueEqualsBefore,
            Action<TTarget, TItem> setter,
            bool expectFailure,
            IEnumerable<string>? expectedNotifyProperties = null,
            ValueVerifier<TTarget>? instanceVerifier = null,
            ValueVerifier<Exception>? exceptionVerifier = null
        )
        {
            var changedPropertyList = new List<string>();
            var propertyChangedNotifiable = instance as INotifyPropertyChanged;
            if (propertyChangedNotifiable is not null)
            {
                propertyChangedNotifiable.PropertyChanged += (_, args) =>
                {
                    changedPropertyList.Add(args.PropertyName!);
                };
            }

            var errorOccured = false;
            Exception? exception = null;
            try
            {
                setter(instance, setItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
                exception = ex;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(
                expectFailure,
                errorOccured
            );

            // エラーが発生した場合、意図したとおりのエラーが発生していること
            if (errorOccured)
            {
                exceptionVerifier?.Verify(exception!);
            }

            // ----------------------------------------
            // 以降、プロパティ変更通知を行う場合のみのテスト
            if (propertyChangedNotifiable is null)
            {
                return;
            }

            // エラーが発生している場合、プロパティ変更通知が発火していないこと
            if (errorOccured)
            {
                Assert.IsEmpty(changedPropertyList);
                return;
            }

            // ----------------------------------------
            // 以降、プロパティ編集でエラーが起きていない場合のみのテスト

            if (isValueEqualsBefore)
            {
                // 値が変化していない場合、
                //      プロパティ変更通知が発火していないこと
                Assert.IsEmpty(changedPropertyList);
            }
            else if (expectedNotifyProperties is null)
            {
                // 値が変化している場合、
                //      プロパティ変更通知が発火していること（対象プロパティのみチェック）
                Assert.AreEqual(1, changedPropertyList.Count);
                Assert.AreEqual(
                    propertyName,
                    changedPropertyList[0]
                );
            }
            else
            {
                // 値が変化している場合、
                //      プロパティ変更通知が発火していること（対象プロパティ以外もチェック）
                var expectedNotifyPropertiesArray = expectedNotifyProperties.ToArray();
                Assert.AreEqual(
                    1 + expectedNotifyPropertiesArray.Length,
                    changedPropertyList.Count,
                    $"changedPropertyList.Count not eq 1 + expectedNotifyPropertiesArray.Length.\n"
                    + $"  changedPropertyList.Count: {changedPropertyList.Count}\n"
                    + $"  1 + expectedNotifyPropertiesArray.Length: {1 + expectedNotifyPropertiesArray.Length}"
                );
                for (var i = 1; i < changedPropertyList.Count; i++)
                {
                    Assert.AreEqual(
                        expectedNotifyPropertiesArray[i - 1],
                        changedPropertyList[i],
                        $"changedPropertyList[{i}] not eq expectedNotifyPropertiesArray[{i - 1}].\n"
                        + $"  changedPropertyList[i]: {changedPropertyList[i]}\n"
                        + $"  expectedNotifyPropertiesArray[i - 1]: {expectedNotifyPropertiesArray[i - 1]}"
                    );
                }
            }

            // ----------------------------------------
            // 同じ値をセットして、変更通知が起きないことを確認
            changedPropertyList.Clear();

            // プロパティ再編集
            setter(instance, setItem);

            // プロパティ変更通知が発火していないこと
            Assert.IsEmpty(changedPropertyList);

            // ----------------------------------------
            // インスタンスの状態を検証
            instanceVerifier?.Verify(instance);
        }

        #endregion
    }
}
