// ========================================
// Project Name : WodiLib
// File Name    : WeakReferenceExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <summary>
    ///     <see cref="WeakReference{T}"/> 拡張クラス
    /// </summary>
    internal static class WeakReferenceExtension
    {
        internal delegate void GetSuccessCallback<in T>(T target);

        internal delegate void GetSuccessCallback<in T1, in T2>(T1 target1, T2 target2);

        internal delegate void GetSuccessCallback<in T1, in T2, in T3>(T1 target1, T2 target2, T3 target3);

        /// <summary>
        ///     弱参照からターゲットオブジェクトの取得を試行し、
        ///     成功した場合に指定されたコールバック処理を実行する。
        /// </summary>
        /// <typeparam name="T">弱参照で保持されるオブジェクトの型</typeparam>
        /// <param name="weakReference">対象の弱参照</param>
        /// <param name="callback">ターゲット取得成功時に実行されるコールバック処理</param>
        /// <remarks>
        ///     弱参照からターゲットオブジェクトが正常に取得できた場合のみ、
        ///     指定されたコールバック処理が実行される。
        ///     ターゲットオブジェクトがガベージコレクションにより回収済みの場合、
        ///     処理は行われない。
        /// </remarks>
        internal static void TryGetTarget<T>(this WeakReference<T> weakReference, GetSuccessCallback<T> callback)
            where T : class
        {
            if (weakReference.TryGetTarget(out var target))
            {
                callback(target);
            }
        }

        /// <seealso cref="TryGetTarget{T}"/>
        internal static void TryGetTarget<T1, T2>(
            this (WeakReference<T1> ref1, WeakReference<T2> ref2) weakReferences,
            GetSuccessCallback<T1, T2> callback
        )
            where T1 : class
            where T2 : class
        {
            if (weakReferences.ref1.TryGetTarget(out var target1)
                && weakReferences.ref2.TryGetTarget(out var target2)
               )
            {
                callback(target1, target2);
            }
        }
    }
}
