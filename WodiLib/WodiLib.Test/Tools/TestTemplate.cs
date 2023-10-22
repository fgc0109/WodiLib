// ========================================
// Project Name : WodiLib.Test
// File Name    : TestTemplate.ts.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;

namespace WodiLib.Test.Tools
{
    /// <summary>
    ///     テスト用テンプレート処理
    /// </summary>
    internal static class TestTemplate
    {
        #region Constructor

        /// <summary>
        ///     コンストラクタテスト
        /// </summary>
        /// <remarks>
        ///     以下の手順のテストを行う。
        ///     <ul>
        ///         <li>コンストラクタ実行によりインスタンスを生成する。</li>
        ///         <li>インスタンス生成によるエラーの有無をテスト</li>
        ///         <li>生成されたインスタンスまたはデフォルト値を返却</li>
        ///     </ul>
        /// </remarks>
        /// <param name="factory">コンストラクタ実行処理</param>
        /// <param name="expectedThrowCreateNewInstance">コンストラクタエラー有無</param>
        /// <param name="verification">
        ///     コンストラクタで生成されたインスタンスの検証処理<br/>
        ///     エラー時には <see cref="Assert"/> などを利用して例外を発生させる。
        /// </param>
        /// <param name="logger"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static void Constructor<T>(
            Func<T> factory,
            bool expectedThrowCreateNewInstance,
            Action<T>? verification,
            Logger logger
        )
        {
            var errorOccured = false;
            try
            {
                var instance = factory();
                verification?.Invoke(instance);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }
            finally
            {
                // エラー有無が意図した結果であること
                Assert.AreEqual(expectedThrowCreateNewInstance, errorOccured);
            }
        }

        #endregion

        #region Property

        /// <summary>
        ///     プロパティ値編集 &amp; 取得のテスト
        /// </summary>
        /// <remarks>
        ///     以下の手順のテストを行う。
        ///     <ul>
        ///         <li>プロパティに対して値の編集を試みる</li>
        ///         <li>値編集によるエラーの有無をテスト</li>
        ///         <li>プロパティ値編集によりエラーが発生した場合処理を終了</li>
        ///         <li>既値と異なる値が編集された場合、プロパティ変更通知が行われていることをテスト</li>
        ///         <li>既値と同一値が編集された場合、プロパティ変更通知が行われていないことをテスト</li>
        ///         <li>プロパティ値の取得を試みる</li>
        ///         <li>プロパティ値取得によるエラーの有無をテスト</li>
        ///         <li>プロパティ変更通知が行われていないことをテスト</li>
        ///         <li>プロパティ値取得によりエラーが発生した場合処理を終了</li>
        ///         <li>編集したプロパティ値と取得したプロパティ値が同値であることをテスト</li>
        ///     </ul>
        /// </remarks>
        /// <param name="createInstance">テスト対象のインスタンス生成処理</param>
        /// <param name="propertyName">テスト対象のプロパティ名</param>
        /// <param name="setItem">プロパティに編集する値</param>
        /// <param name="isEqualSetItemBeforePropertyValue">編集する値と編集前の値が同値であるか</param>
        /// <param name="propertySetter">プロパティ編集処理</param>
        /// <param name="expectedThrowActPropertySet">プロパティ編集時例外有無期待値</param>
        /// <param name="propertyGetter">プロパティ取得処理</param>
        /// <param name="expectedThrowActPropertyGet">プロパティ取得処理</param>
        /// <param name="itemEqualityComparer">設定値と取得値の比較処理</param>
        /// <param name="logger">ロガー</param>
        /// <typeparam name="TTarget">テスト対象インスタンス型</typeparam>
        /// <typeparam name="TItem">プロパティに編集する値型</typeparam>
        public static void PropertyGetAndSet<TTarget, TItem>(
            Func<TTarget> createInstance,
            string propertyName,
            TItem setItem,
            bool isEqualSetItemBeforePropertyValue,
            Action<TTarget, TItem> propertySetter,
            bool expectedThrowActPropertySet,
            Func<TTarget, TItem> propertyGetter,
            bool expectedThrowActPropertyGet,
            Func<TItem, TItem, bool> itemEqualityComparer,
            Logger logger
        )
        {
            PropertySet(
                createInstance,
                propertyName,
                setItem,
                isEqualSetItemBeforePropertyValue,
                propertySetter,
                expectedThrowActPropertySet,
                logger
            );

            var getValueVerification = new Action<TItem>(getItem => itemEqualityComparer(setItem, getItem));
            PropertyGet(createInstance, propertyGetter, expectedThrowActPropertyGet, getValueVerification, logger);
        }

        /// <summary>
        ///     プロパティ値取得のテスト
        /// </summary>
        /// <remarks>
        ///     以下の手順のテストを行う。
        ///     <ul>
        ///         <li>プロパティ値の取得を試みる</li>
        ///         <li>プロパティ値取得によりエラーが発生しないことをテスト</li>
        ///         <li>プロパティ値取得によるエラーの有無をテスト</li>
        ///         <li>プロパティ変更通知が行われていないことをテスト</li>
        ///         <li>プロパティ値取得によりエラーが発生した場合処理を終了</li>
        ///         <li>取得したプロパティ値が同値であることをテスト</li>
        ///     </ul>
        /// </remarks>
        /// <param name="createInstance">テスト対象のインスタンス生成処理</param>
        /// <param name="propertyGetter">プロパティ取得処理</param>
        /// <param name="expectedThrowActPropertyGet">プロパティ取得処理</param>
        /// <param name="getValueVerification">
        ///     プロパティから取得した値の検証処理<br/>
        ///     エラー時には <see cref="Assert"/> などを利用して例外を発生させる。
        /// </param>
        /// <param name="logger">ロガー</param>
        /// <typeparam name="TTarget"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        public static void PropertyGet<TTarget, TItem>(
            Func<TTarget> createInstance,
            Func<TTarget, TItem> propertyGetter,
            bool expectedThrowActPropertyGet,
            Action<TItem>? getValueVerification,
            Logger logger
        )
        {
            var instance = createInstance.Invoke();
            var changedPropertyList = new List<string>();
            var propertyChangedNotifiable = instance as INotifyPropertyChanged;
            if (propertyChangedNotifiable is not null)
            {
                propertyChangedNotifiable.PropertyChanged += (_, args) =>
                {
                    changedPropertyList.Add(args.PropertyName);
                };
            }

            TItem getResult = default!;
            var errorOccured = false;
            try
            {
                getResult = propertyGetter(instance);
            }
            catch (Exception e)
            {
                logger.Error(e.StackTrace);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(expectedThrowActPropertyGet, errorOccured);

            // プロパティ変更通知が発火していないこと
            if (propertyChangedNotifiable is not null)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }

            if (errorOccured)
            {
                return;
            }

            // 取得した要素の検証処理
            getValueVerification?.Invoke(getResult);
        }

        /// <summary>
        ///     プロパティ値編集のテスト
        /// </summary>
        /// <remarks>
        ///     以下の手順のテストを行う。
        ///     <ul>
        ///         <li>プロパティに対して値の編集を試みる</li>
        ///         <li>値編集によるエラーの有無をテスト</li>
        ///         <li>既値と異なる値が編集された場合、プロパティ変更通知が行われていることをテスト</li>
        ///         <li>既値と同一値が編集された場合、プロパティ変更通知が行われていないことをテスト</li>
        ///         <li>プロパティに対して同じ値の編集を試みる</li>
        ///         <li>プロパティ変更通知が行われていないことをテスト</li>
        ///     </ul>
        /// </remarks>
        /// <param name="createInstance">テスト対象のインスタンス生成処理</param>
        /// <param name="propertyName">テスト対象のプロパティ名</param>
        /// <param name="setItem">プロパティに編集する値</param>
        /// <param name="isEqualSetItemBeforePropertyValue">編集する値と編集前の値が同値であるか</param>
        /// <param name="propertySetter">プロパティ編集処理</param>
        /// <param name="expectedThrowActPropertySet">プロパティ編集時例外有無期待値</param>
        /// <param name="logger">ロガー</param>
        /// <typeparam name="TTarget">テスト対象インスタンス型</typeparam>
        /// <typeparam name="TItem">プロパティに編集する値型</typeparam>
        public static void PropertySet<TTarget, TItem>(
            Func<TTarget> createInstance,
            string propertyName,
            TItem setItem,
            bool isEqualSetItemBeforePropertyValue,
            Action<TTarget, TItem> propertySetter,
            bool expectedThrowActPropertySet,
            Logger logger
        )
        {
            var instance = createInstance.Invoke();
            var changedPropertyList = new List<string>();
            var propertyChangedNotifiable = instance as INotifyPropertyChanged;
            if (propertyChangedNotifiable is not null)
            {
                propertyChangedNotifiable.PropertyChanged += (_, args) =>
                {
                    changedPropertyList.Add(args.PropertyName);
                };
            }

            var errorOccured = false;
            try
            {
                propertySetter(instance, setItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(expectedThrowActPropertySet, errorOccured);

            if (propertyChangedNotifiable is not null)
            {
                if (errorOccured)
                {
                    // プロパティ変更通知が発火していないこと
                    Assert.AreEqual(changedPropertyList.Count, 0);
                }
                else
                {
                    if (isEqualSetItemBeforePropertyValue)
                    {
                        // プロパティ変更通知が発火していないこと
                        Assert.AreEqual(changedPropertyList.Count, 0);
                    }
                    else
                    {
                        // プロパティ変更通知が発火していること
                        Assert.AreEqual(changedPropertyList.Count, 1);
                        Assert.AreEqual(changedPropertyList[0], propertyName);
                    }
                }
            }
        }

        #endregion

        #region Method

        /// <summary>
        ///     純粋メソッドのテスト
        /// </summary>
        /// <remarks>
        ///     以下の手順のテストを行う。
        ///     <ul>
        ///         <li>メソッドを実行する</li>
        ///         <li>メソッド実行によるエラーの有無をテスト</li>
        ///         <li>メソッド実行によりプロパティ変更通知が発火していないことをテスト</li>
        ///     </ul>
        /// </remarks>
        /// <param name="instance">テスト対象のインスタンス</param>
        /// <param name="execFunc">メソッド実行処理</param>
        /// <param name="expectedThrowExecute">メソッド実行時例外有無期待値</param>
        /// <param name="logger">ロガー</param>
        /// <typeparam name="TTarget">テスト対象インスタンス型</typeparam>
        /// <typeparam name="TResult">メソッド返却型</typeparam>
        /// <returns></returns>
        public static void PureMethod<TTarget, TResult>(
            TTarget instance,
            Func<TTarget, TResult> execFunc,
            bool expectedThrowExecute,
            Logger logger
        ) => PureMethod(
            createInstance: () => instance,
            execFunc,
            expectedThrowExecute,
            logger
        );

        /// <summary>
        ///     純粋メソッドのテスト
        /// </summary>
        /// <remarks>
        ///     以下の手順のテストを行う。
        ///     <ul>
        ///         <li>メソッドを実行する</li>
        ///         <li>メソッド実行によるエラーの有無をテスト</li>
        ///         <li>メソッド実行によりプロパティ変更通知が発火していないことをテスト</li>
        ///     </ul>
        /// </remarks>
        /// <param name="createInstance">テスト対象のインスタンス生成処理</param>
        /// <param name="execFunc">メソッド実行処理</param>
        /// <param name="expectedThrowExecute">メソッド実行時例外有無期待値</param>
        /// <param name="logger">ロガー</param>
        /// <typeparam name="TTarget">テスト対象インスタンス型</typeparam>
        /// <typeparam name="TResult">メソッド返却型</typeparam>
        /// <returns></returns>
        public static void PureMethod<TTarget, TResult>(
            Func<TTarget> createInstance,
            Func<TTarget, TResult> execFunc,
            bool expectedThrowExecute,
            Logger logger
        )
            => PureMethod(createInstance, execFunc, expectedThrowExecute, resultValueVerification: null, logger);

        /// <summary>
        ///     純粋メソッドのテスト
        /// </summary>
        /// <remarks>
        ///     以下の手順のテストを行う。
        ///     <ul>
        ///         <li>メソッドを実行する</li>
        ///         <li>メソッド実行によるエラーの有無をテスト</li>
        ///         <li>メソッド実行によりプロパティ変更通知が発火していないことをテスト</li>
        ///         <li>メソッド実行によりエラーが発生していない場合、取得した値が意図した値であることをテスト</li>
        ///     </ul>
        /// </remarks>
        /// <param name="instance">テスト対象のインスタンス</param>
        /// <param name="execFunc">メソッド実行処理</param>
        /// <param name="expectedThrowExecute">メソッド実行時例外有無期待値</param>
        /// <param name="resultValueVerification">
        ///     メソッド戻り値の検証処理<br/>
        ///     エラー時には <see cref="Assert"/> などを利用して例外を発生させる。
        /// </param>
        /// <param name="logger">ロガー</param>
        /// <typeparam name="TTarget">テスト対象インスタンス型</typeparam>
        /// <typeparam name="TResult">メソッド返却型</typeparam>
        /// <returns></returns>
        public static void PureMethod<TTarget, TResult>(
            TTarget instance,
            Func<TTarget, TResult> execFunc,
            bool expectedThrowExecute,
            Action<TResult>? resultValueVerification,
            Logger logger
        ) => PureMethod(
            createInstance: () => instance,
            execFunc,
            expectedThrowExecute,
            resultValueVerification,
            logger
        );

        /// <summary>
        ///     純粋メソッドのテスト
        /// </summary>
        /// <remarks>
        ///     以下の手順のテストを行う。
        ///     <ul>
        ///         <li>メソッドを実行する</li>
        ///         <li>メソッド実行によるエラーの有無をテスト</li>
        ///         <li>メソッド実行によりプロパティ変更通知が発火していないことをテスト</li>
        ///         <li>メソッド実行によりエラーが発生していない場合、取得した値が意図した値であることをテスト</li>
        ///     </ul>
        /// </remarks>
        /// <param name="createInstance">テスト対象のインスタンス生成処理</param>
        /// <param name="execFunc">メソッド実行処理</param>
        /// <param name="expectedThrowExecute">メソッド実行時例外有無期待値</param>
        /// <param name="resultValueVerification">
        ///     メソッド戻り値の検証処理<br/>
        ///     エラー時には <see cref="Assert"/> などを利用して例外を発生させる。
        /// </param>
        /// <param name="logger">ロガー</param>
        /// <typeparam name="TTarget">テスト対象インスタンス型</typeparam>
        /// <typeparam name="TResult">メソッド返却型</typeparam>
        /// <returns></returns>
        public static void PureMethod<TTarget, TResult>(
            Func<TTarget> createInstance,
            Func<TTarget, TResult> execFunc,
            bool expectedThrowExecute,
            Action<TResult>? resultValueVerification,
            Logger logger
        )
        {
            var instance = createInstance.Invoke();
            var notifyChangeable = instance as INotifyPropertyChanged;
            var changedPropertyList = new List<string>();
            if (notifyChangeable is not null)
            {
                notifyChangeable.PropertyChanged += (_, args) => { changedPropertyList.Add(args.PropertyName); };
            }

            TResult execResult = default!;
            var errorOccured = false;
            try
            {
                execResult = execFunc(instance);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(expectedThrowExecute, errorOccured);

            if (notifyChangeable is not null)
            {
                // プロパティ変更通知が発火していないこと
                Assert.AreEqual(changedPropertyList.Count, 0);
            }

            if (errorOccured)
            {
                return;
            }

            // 取得した値が意図した値であること
            resultValueVerification?.Invoke(execResult);
        }

        /// <summary>
        ///     純粋メソッドのテスト
        /// </summary>
        /// <remarks>
        ///     以下の手順のテストを行う。
        ///     <ul>
        ///         <li>メソッドを実行する</li>
        ///         <li>メソッド実行によるエラーの有無をテスト</li>
        ///         <li>メソッド実行によりプロパティ変更通知が発火していないことをテスト</li>
        ///         <li>メソッド実行によりエラーが発生していない場合、取得した値が意図した値であることをテスト</li>
        ///     </ul>
        /// </remarks>
        /// <param name="createInstance">テスト対象のインスタンス生成処理</param>
        /// <param name="execAction">メソッド実行処理</param>
        /// <param name="expectedThrowExecute">メソッド実行時例外有無期待値</param>
        /// <param name="logger">ロガー</param>
        /// <typeparam name="TTarget">テスト対象インスタンス型</typeparam>
        /// <returns></returns>
        public static void PureMethod<TTarget>(
            Func<TTarget> createInstance,
            Action<TTarget> execAction,
            bool expectedThrowExecute,
            Logger logger
        )
        {
            var instance = createInstance.Invoke();
            var notifyChangeable = instance as INotifyPropertyChanged;
            var changedPropertyList = new List<string>();
            if (notifyChangeable is not null)
            {
                notifyChangeable.PropertyChanged += (_, args) => { changedPropertyList.Add(args.PropertyName); };
            }

            var errorOccured = false;
            try
            {
                execAction(instance);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(expectedThrowExecute, errorOccured);

            if (notifyChangeable is not null)
            {
                // プロパティ変更通知が発火していないこと
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
        }

        /// <summary>
        ///     純粋メソッドのテスト
        /// </summary>
        /// <remarks>
        ///     以下の手順のテストを行う。
        ///     <ul>
        ///         <li>メソッドを実行する</li>
        ///         <li>メソッド実行によるエラーの有無をテスト</li>
        ///     </ul>
        /// </remarks>
        /// <param name="execAction">メソッド実行処理</param>
        /// <param name="expectedThrowExecute">メソッド実行時例外有無期待値</param>
        /// <param name="logger">ロガー</param>
        /// <returns></returns>
        public static void PureMethod(
            Action execAction,
            bool expectedThrowExecute,
            Logger logger
        )
        {
            var errorOccured = false;
            try
            {
                execAction();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(expectedThrowExecute, errorOccured);
        }

        /// <summary>
        ///     非純粋メソッドのテスト(戻り値あり)
        /// </summary>
        /// <remarks>
        ///     以下の手順のテストを行う。
        ///     <ul>
        ///         <li>メソッドを実行する</li>
        ///         <li>メソッド実行によるエラーの有無をテスト</li>
        ///         <li>ソッド実行によりエラーが発生していない場合、メソッド実行によりプロパティ変更通知が意図したとおり発火していることをテスト</li>
        ///         <li>メソッド実行によりエラーが発生していない場合、実行結果が意図した値であることをテスト</li>
        ///         <li>メソッド実行によりエラーが発生していない場合、実行の状態が意図した状態であることをテスト</li>
        ///         <li>ソッド実行によりエラーが発生した場合、メソッド実行によりプロパティ変更通知が発火していないことをテスト</li>
        ///         <li>メソッド実行によりエラーが発生した場合、メソッド実行前後で状態が変化していないことをテスト</li>
        ///     </ul>
        /// </remarks>
        /// <param name="createInstance">テスト対象のインスタンス生成処理</param>
        /// <param name="execFunc">メソッド実行処理</param>
        /// <param name="expectedThrowExecute">メソッド実行時例外有無期待値</param>
        /// <param name="resultValueVerification">
        ///     戻り値の検証処理<br/>
        ///     エラー時には <see cref="Assert"/> などを利用して例外を発生させる。
        /// </param>
        /// <param name="expectedNotifyPropertyChange">期待するプロパティ変更通知</param>
        /// <param name="instanceVerification">
        ///     処理対象インスタンスの状態検証処理<br/>
        ///     エラー時には <see cref="Assert"/> などを利用して例外を発生させる。
        /// </param>
        /// <param name="logger">ロガー</param>
        /// <typeparam name="TTarget"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static void MutableMethod<TTarget, TResult>(
            Func<TTarget> createInstance,
            Func<TTarget, TResult> execFunc,
            bool expectedThrowExecute,
            Action<TResult>? resultValueVerification,
            IEnumerable<string> expectedNotifyPropertyChange,
            Action<TTarget>? instanceVerification,
            Logger logger
        )
        {
            var instance = createInstance.Invoke();
            var changedPropertyList = new List<string>();
            var propertyChangedNotifiable = instance as INotifyPropertyChanged;
            if (propertyChangedNotifiable is not null)
            {
                propertyChangedNotifiable.PropertyChanged += (_, args) =>
                {
                    changedPropertyList.Add(args.PropertyName);
                };
            }

            TResult execResult = default!;
            var errorOccured = false;
            try
            {
                execResult = execFunc(instance);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(expectedThrowExecute, errorOccured);

            if (errorOccured)
            {
                return;
            }

            // 取得した値が意図した値であること
            resultValueVerification?.Invoke(execResult);

            if (propertyChangedNotifiable is not null)
            {
                // プロパティ変更通知が発火していること
                var expectedNotifyPropertyChangeList = expectedNotifyPropertyChange.ToList();

                AssertEqualsNotifiedPropertyNames(
                    changedPropertyList,
                    expectedNotifyPropertyChangeList
                );
            }

            // 実行後のインスタンスが意図した状態であること
            instanceVerification?.Invoke(instance);
        }

        /// <summary>
        ///     非純粋メソッドのテスト(戻り値なし)
        /// </summary>
        /// <remarks>
        ///     以下の手順のテストを行う。
        ///     <ul>
        ///         <li>メソッドを実行する</li>
        ///         <li>メソッド実行によるエラーの有無をテスト</li>
        ///         <li>ソッド実行によりエラーが発生していない場合、メソッド実行によりプロパティ変更通知が意図したとおり発火していることをテスト</li>
        ///         <li>メソッド実行によりエラーが発生していない場合、実行結果が意図した値であることをテスト</li>
        ///         <li>メソッド実行によりエラーが発生していない場合、実行の状態が意図した状態であることをテスト</li>
        ///         <li>ソッド実行によりエラーが発生した場合、メソッド実行によりプロパティ変更通知が発火していないことをテスト</li>
        ///         <li>メソッド実行によりエラーが発生した場合、メソッド実行前後で状態が変化していないことをテスト</li>
        ///     </ul>
        /// </remarks>
        /// <param name="instance">テスト対象のインスタンス</param>
        /// <param name="execAction">メソッド実行処理</param>
        /// <param name="expectedThrowExecute">メソッド実行時例外有無期待値</param>
        /// <param name="expectedNotifyPropertyChange">期待するプロパティ変更通知</param>
        /// <param name="instanceVerification">
        ///     処理対象インスタンスの状態検証処理<br/>
        ///     エラー時には <see cref="Assert"/> などを利用して例外を発生させる。
        /// </param>
        /// <param name="logger">ロガー</param>
        /// <typeparam name="TTarget"></typeparam>
        /// <returns>エラー有無</returns>
        public static void MutableMethod<TTarget>(
            TTarget instance,
            Action<TTarget> execAction,
            bool expectedThrowExecute,
            IEnumerable<string> expectedNotifyPropertyChange,
            Action<TTarget>? instanceVerification,
            Logger logger
        ) => MutableMethod(
            createInstance: () => instance,
            execAction,
            expectedThrowExecute,
            expectedNotifyPropertyChange,
            instanceVerification,
            logger
        );

        /// <summary>
        ///     非純粋メソッドのテスト(戻り値なし)
        /// </summary>
        /// <remarks>
        ///     以下の手順のテストを行う。
        ///     <ul>
        ///         <li>メソッドを実行する</li>
        ///         <li>メソッド実行によるエラーの有無をテスト</li>
        ///         <li>ソッド実行によりエラーが発生していない場合、メソッド実行によりプロパティ変更通知が意図したとおり発火していることをテスト</li>
        ///         <li>メソッド実行によりエラーが発生していない場合、実行結果が意図した値であることをテスト</li>
        ///         <li>メソッド実行によりエラーが発生していない場合、実行の状態が意図した状態であることをテスト</li>
        ///         <li>ソッド実行によりエラーが発生した場合、メソッド実行によりプロパティ変更通知が発火していないことをテスト</li>
        ///         <li>メソッド実行によりエラーが発生した場合、メソッド実行前後で状態が変化していないことをテスト</li>
        ///     </ul>
        /// </remarks>
        /// <param name="createInstance">テスト対象のインスタンス生成処理</param>
        /// <param name="execAction">メソッド実行処理</param>
        /// <param name="expectedThrowExecute">メソッド実行時例外有無期待値</param>
        /// <param name="expectedNotifyPropertyChange">期待するプロパティ変更通知</param>
        /// <param name="instanceVerification">
        ///     処理対象インスタンスの状態検証処理<br/>
        ///     エラー時には <see cref="Assert"/> などを利用して例外を発生させる。
        /// </param>
        /// <param name="logger">ロガー</param>
        /// <typeparam name="TTarget"></typeparam>
        /// <returns>エラー有無</returns>
        public static void MutableMethod<TTarget>(
            Func<TTarget> createInstance,
            Action<TTarget> execAction,
            bool expectedThrowExecute,
            IEnumerable<string> expectedNotifyPropertyChange,
            Action<TTarget>? instanceVerification,
            Logger logger
        )
        {
            var instance = createInstance.Invoke();
            var changedPropertyList = new List<string>();
            var propertyChangedNotifiable = instance as INotifyPropertyChanged;
            if (propertyChangedNotifiable is not null)
            {
                propertyChangedNotifiable.PropertyChanged += (_, args) =>
                {
                    changedPropertyList.Add(args.PropertyName);
                };
            }

            var errorOccured = false;
            try
            {
                execAction(instance);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(expectedThrowExecute, errorOccured);

            if (errorOccured)
            {
                return;
            }

            if (propertyChangedNotifiable is not null)
            {
                // プロパティ変更通知が発火していること
                var expectedNotifyPropertyChangeList = expectedNotifyPropertyChange.ToList();

                AssertEqualsNotifiedPropertyNames(
                    expectedNotifyPropertyChangeList,
                    changedPropertyList
                );
            }

            // 実行後のインスタンスが意図した状態であること
            instanceVerification?.Invoke(instance);
        }

        /// <summary>
        ///     静的メソッドのテスト
        /// </summary>
        /// <remarks>
        ///     以下の手順のテストを行う。
        ///     <ul>
        ///         <li>メソッドを実行する</li>
        ///         <li>メソッド実行によるエラーの有無をテスト</li>
        ///         <li>取得した値が意図した値であることをテスト</li>
        ///     </ul>
        /// </remarks>
        /// <param name="execFunc">メソッド実行処理</param>
        /// <param name="expectedThrowExecute">メソッド実行時例外有無期待値</param>
        /// <param name="resultValueVerification">
        ///     メソッド戻り値の検証処理<br/>
        ///     エラー時には <see cref="Assert"/> などを利用して例外を発生させる。
        /// </param>
        /// <param name="logger">ロガー</param>
        /// <typeparam name="TResult">メソッド返却型</typeparam>
        /// <returns></returns>
        public static void StaticMethod<TResult>(
            Func<TResult> execFunc,
            bool expectedThrowExecute,
            Action<TResult>? resultValueVerification,
            Logger logger
        )
        {
            TResult execResult = default!;
            var errorOccured = false;
            try
            {
                execResult = execFunc();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(expectedThrowExecute, errorOccured);

            if (errorOccured)
            {
                return;
            }

            // 取得した値が意図した値であること
            resultValueVerification?.Invoke(execResult);
        }

        /// <summary>
        ///     比較処理のテスト
        /// </summary>
        /// <remarks>
        ///     以下の手順のテストを行う。
        ///     <ul>
        ///         <li>比較メソッドを実行する</li>
        ///         <li>メソッド実行によりエラーが発生しないことをテスト</li>
        ///         <li>メソッド実行によりプロパティ変更通知が発火していないことをテスト</li>
        ///         <li>メソッド実行前後で状態が変化していないことをテスト</li>
        ///         <li>結果が意図した値であることをテスト</li>
        ///     </ul>
        /// </remarks>
        /// <param name="left">比較対象の左辺インスタンス</param>
        /// <param name="right">比較対象の右辺インスタンス</param>
        /// <param name="expected">期待する比較結果</param>
        /// <param name="logger">ロガー</param>
        /// <typeparam name="TTarget">テスト対象インスタンス型</typeparam>
        /// <typeparam name="TComp">テスト対象比較相手型</typeparam>
        public static void ItemEquals<TTarget, TComp>(
            TTarget left,
            TComp? right,
            bool expected,
            Logger logger
        )
            where TTarget : IEqualityComparable<TComp>
            => ItemEquals(
                createLeftItem: () => left,
                createRightItem: () => right,
                expected,
                logger
            );

        /// <summary>
        ///     比較処理のテスト
        /// </summary>
        /// <remarks>
        ///     以下の手順のテストを行う。
        ///     <ul>
        ///         <li>比較メソッドを実行する</li>
        ///         <li>メソッド実行によりエラーが発生しないことをテスト</li>
        ///         <li>メソッド実行によりプロパティ変更通知が発火していないことをテスト</li>
        ///         <li>メソッド実行前後で状態が変化していないことをテスト</li>
        ///         <li>結果が意図した値であることをテスト</li>
        ///     </ul>
        /// </remarks>
        /// <param name="createLeftItem">比較対象の左辺インスタンス生成処理</param>
        /// <param name="createRightItem">比較対象の右辺インスタンス生成処理</param>
        /// <param name="expected">期待する比較結果</param>
        /// <param name="logger">ロガー</param>
        /// <typeparam name="TTarget">テスト対象インスタンス型</typeparam>
        /// <typeparam name="TComp">テスト対象比較相手型</typeparam>
        public static void ItemEquals<TTarget, TComp>(
            Func<TTarget> createLeftItem,
            Func<TComp?> createRightItem,
            bool expected,
            Logger logger
        )
            where TTarget : IEqualityComparable<TComp>
        {
            var right = createRightItem.Invoke();

            PureMethod(
                createLeftItem,
                target => target.ItemEquals(right),
                expectedThrowExecute: false,
                resultValueVerification: actual => { Assert.AreEqual(expected, actual); },
                logger
            );
        }

        /// <summary>
        ///     DeepCloneメソッドのテスト
        /// </summary>
        /// <remarks>
        ///     以下の手順のテストを行う。
        ///     <ul>
        ///         <li>メソッドを実行する</li>
        ///         <li>メソッド実行によりエラーが発生しないことをテスト</li>
        ///         <li>メソッド実行によりプロパティ変更通知が発火していないことをテスト</li>
        ///         <li>メソッド実行前後で状態が変化していないことをテスト</li>
        ///         <li>取得した値が元のインスタンスとは別インスタンスであり、同値であることをテスト</li>
        ///     </ul>
        /// </remarks>
        /// <param name="instance">テスト対象のインスタンス</param>
        /// <param name="logger">ロガー</param>
        /// <typeparam name="TTarget">テスト対象インスタンス型</typeparam>
        /// <returns></returns>
        public static void DeepClone<TTarget>(
            TTarget instance,
            Logger logger
        )
            where TTarget : IDeepCloneable<TTarget>, IEqualityComparable<TTarget>
            => DeepClone(
                createInstance: () => instance,
                logger
            );

        /// <summary>
        ///     DeepCloneメソッドのテスト
        /// </summary>
        /// <remarks>
        ///     以下の手順のテストを行う。
        ///     <ul>
        ///         <li>メソッドを実行する</li>
        ///         <li>メソッド実行によりエラーが発生しないことをテスト</li>
        ///         <li>メソッド実行によりプロパティ変更通知が発火していないことをテスト</li>
        ///         <li>メソッド実行前後で状態が変化していないことをテスト</li>
        ///         <li>取得した値が元のインスタンスとは別インスタンスであり、同値であることをテスト</li>
        ///     </ul>
        /// </remarks>
        /// <param name="createInstance">テスト対象のインスタンス生成処理</param>
        /// <param name="logger">ロガー</param>
        /// <typeparam name="TTarget">テスト対象インスタンス型</typeparam>
        /// <returns></returns>
        public static void DeepClone<TTarget>(
            Func<TTarget> createInstance,
            Logger logger
        )
            where TTarget : IDeepCloneable<TTarget>, IEqualityComparable<TTarget>
        {
            var instance = createInstance.Invoke();

            PureMethod(
                createInstance: () => instance,
                target => target.DeepClone(),
                expectedThrowExecute: false,
                resultValueVerification: result =>
                {
                    Assert.IsFalse(ReferenceEquals(instance, result), "ReferenceEquals(instance, result)");
                    Assert.IsTrue(result.ItemEquals(instance), "result.ItemEquals(instance)");
                },
                logger
            );
        }


        private static void AssertEqualsNotifiedPropertyNames(
            IReadOnlyCollection<string> expected,
            IReadOnlyCollection<string> actual
        )
        {
            Assert.AreEqual(expected.Count, actual.Count);
            expected.ForEach(
                expectedItem => { Assert.IsTrue(actual.Count(actualItem => expectedItem == actualItem) == 1); }
            );
        }

        #endregion

        #region StaticClass

        /// <summary>
        ///     staticクラスの関数テスト
        /// </summary>
        /// <remarks>
        ///     以下の手順のテストを行う。
        ///     <ul>
        ///         <li>関数を実行する</li>
        ///         <li>関数実行によるエラーの有無をテスト</li>
        ///         <li>関数実行によりエラーが発生していない場合、取得した値が意図した値であることをテスト</li>
        ///     </ul>
        /// </remarks>
        /// <param name="execFunc">関数実行処理</param>
        /// <param name="expectedThrowExecute">メソッド実行時例外有無期待値</param>
        /// <param name="resultValueVerification">
        ///     取得した要素が意図した値であることを検証する処理<br/>
        ///     <see langword="null"/> の場合実行しない。
        /// </param>
        /// <param name="logger">ロガー</param>
        /// <typeparam name="TResult">関数返却型</typeparam>
        public static void StaticClassFunc<TResult>(
            Func<TResult> execFunc,
            bool expectedThrowExecute,
            Action<TResult>? resultValueVerification,
            Logger logger
        )
        {
            TResult execResult = default!;
            var errorOccured = false;
            try
            {
                execResult = execFunc();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(expectedThrowExecute, errorOccured);

            // 取得した値が意図した値であること
            if (!errorOccured)
            {
                resultValueVerification?.Invoke(execResult);
            }
        }

        /// <summary>
        ///     staticクラスの関数テスト
        /// </summary>
        /// <remarks>
        ///     以下の手順のテストを行う。
        ///     <ul>
        ///         <li>関数を実行する</li>
        ///         <li>関数実行によるエラーの有無をテスト</li>
        ///         <li>関数実行によりエラーが発生していない場合、取得した値が意図した値であることをテスト</li>
        ///     </ul>
        /// </remarks>
        /// <param name="execAction">関数実行処理</param>
        /// <param name="expectedThrowExecute">メソッド実行時例外有無期待値</param>
        /// <param name="verification">
        ///     実行した結果意図した状態となったことを検証する処理<br/>
        ///     <see langword="null"/> の場合実行しない。
        /// </param>
        /// <param name="logger">ロガー</param>
        public static void StaticClassFunc(
            Action execAction,
            bool expectedThrowExecute,
            Action? verification,
            Logger logger
        )
        {
            var errorOccured = false;
            try
            {
                execAction();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(expectedThrowExecute, errorOccured);

            // 取得した値が意図した値であること
            if (!errorOccured)
            {
                verification?.Invoke();
            }
        }

        #endregion
    }
}
