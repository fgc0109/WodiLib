// ========================================
// Project Name : WodiLib
// File Name    : ThrowHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace WodiLib.Sys
{
    /// <summary>
    ///     例外スロー用Helperクラス
    /// </summary>
    internal static class ThrowHelper
    {
        #region Validate Property

        /// <summary>
        ///     プロパティにアクセスできない場合の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="reason">アクセス不可能な理由</param>
        /// <exception cref="Sys.PropertyAccessException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidatePropertyAccess(
            [DoesNotReturnIf(true)] bool isThrow,
            string reason
        )
        {
            if (!isThrow) return;

            PropertyAccess(reason);
        }

        /// <summary>
        ///     プロパティ設定値が <see langword="null"/> でないことを検証する際の例外処理
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <exception cref="PropertyNullException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidatePropertyNotNull([DoesNotReturnIf(true)] bool isThrow, string itemName)
        {
            if (!isThrow) return;

            PropertyNotNull(itemName);
        }

        /// <summary>
        ///     列挙子に <see langword="null"/> が含まれないことを検証する際の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <exception cref="PropertyNullException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidatePropertyItemsHasNotNull([DoesNotReturnIf(true)] bool isThrow, string itemName)
        {
            if (!isThrow) return;

            PropertyItemsHasNotNull(itemName);
        }

        /// <summary>
        ///     値の範囲検証処理時の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <param name="target">検証対象値</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidatePropertyValueRange(
            [DoesNotReturnIf(true)] bool isThrow,
            string itemName,
            int target,
            IntOrStr min,
            IntOrStr max
        )
        {
            if (!isThrow) return;

            PropertyValueRange(itemName, target, min, max);
        }

        #endregion

        #region Validate Argument

        #region Null

        /// <summary>
        ///     引数が <see langword="null"/> でないことを検証する際の例外処理
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateArgumentNotNull([DoesNotReturnIf(true)] bool isThrow, string itemName)
        {
            if (!isThrow) return;

            ArgumentNotNull(itemName);
        }

        /// <summary>
        ///     引数が 空文字 でないことを検証する際の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateArgumentNotEmpty([DoesNotReturnIf(true)] bool isThrow, string itemName)
        {
            if (!isThrow) return;

            throw new ArgumentException(
                ErrorMessage.NotEmpty(itemName)
            );
        }

        /// <summary>
        ///     列挙子に <see langword="null"/> が含まれないことを検証する際の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateArgumentItemsHasNotNull([DoesNotReturnIf(true)] bool isThrow, string itemName)
        {
            if (!isThrow) return;

            ArgumentNotEmpty(itemName);
        }

        #endregion

        #region Value Compare

        /// <summary>
        ///     値が指定値以上であることの検証時の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <param name="limit">下限値</param>
        /// <param name="itemValue">検証対象値</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateArgumentValueGreaterOrEqual(
            [DoesNotReturnIf(true)] bool isThrow,
            string itemName,
            IntOrStr limit,
            int itemValue
        )
        {
            if (!isThrow) return;

            ArgumentValueGreaterOrEqual(itemName, limit, itemValue);
        }

        /// <summary>
        ///     値が指定値以下であることの検証時の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <param name="limit">上限値</param>
        /// <param name="itemValue">検証対象値</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateArgumentValueLessOrEqual(
            [DoesNotReturnIf(true)] bool isThrow,
            string itemName,
            IntOrStr limit,
            int itemValue
        )
        {
            if (!isThrow) return;

            ArgumentValueLessOrEqual(itemName, limit, itemValue);
        }

        /// <summary>
        ///     値の範囲検証処理時の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <param name="target">検証対象値</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateArgumentValueRange(
            [DoesNotReturnIf(true)] bool isThrow,
            string itemName,
            int target,
            IntOrStr min,
            IntOrStr max
        )
        {
            if (!isThrow) return;

            ArgumentValueRange(itemName, target, min, max);
        }

        /// <summary>
        ///     同値検証時の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <param name="otherName">比較対象名</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateArgumentNotEqual(
            [DoesNotReturnIf(true)] bool isThrow,
            string itemName,
            string otherName
        )
        {
            if (!isThrow) return;

            ArgumentNotEqual(itemName, otherName);
        }

        /// <summary>
        ///     同値検証時の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <param name="item">比較項目</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateArgumentNotMatch(
            [DoesNotReturnIf(true)] bool isThrow,
            string itemName,
            IntOrStr item
        )
        {
            if (!isThrow) return;

            ArgumentNotMatch(itemName, item);
        }

        /// <summary>
        ///     同値検証時の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <param name="items">比較項目一覧</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateArgumentNotMatch(
            [DoesNotReturnIf(true)] bool isThrow,
            string itemName,
            IEnumerable<IntOrStr> items
        )
        {
            if (!isThrow) return;

            ArgumentNotMatch(itemName, items);
        }

        /// <summary>
        ///     文字列に改行が含まれないことの検証時の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <param name="value">検証値</param>
        /// <exception cref="ArgumentNewLineException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateArgumentNotNewLine(
            [DoesNotReturnIf(true)] bool isThrow,
            string itemName,
            string value
        )
        {
            if (!isThrow) return;

            ArgumentNotNewLine(itemName, value);
        }

        /// <summary>
        ///     文字列が正規表現と一致することの検証時の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="value">検証値</param>
        /// <param name="regex">正規表現</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateArgumentNotRegex(
            [DoesNotReturnIf(true)] bool isThrow,
            string value,
            Regex regex
        )
        {
            if (!isThrow) return;

            ArgumentNotRegex(value, regex);
        }

        /// <summary>
        ///     サイズが指定以下であることの検証時の例外処理
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="maxSize">最大サイズ</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateOverDataSize(
            [DoesNotReturnIf(true)] bool isThrow,
            int maxSize
        )
        {
            if (!isThrow) return;

            OverDataSize(maxSize);
        }

        #endregion

        #region Validate Argument in List

        /// <summary>
        ///     リストの範囲取得メソッドでインデックスと取得数の相関チェック時の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="indexArgName">インデックス引数名</param>
        /// <param name="countArgName">取得数引数名</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateListRange(
            [DoesNotReturnIf(true)] bool isThrow,
            string indexArgName,
            string countArgName
        )
        {
            if (!isThrow) return;

            ListRange(indexArgName, countArgName);
        }

        /// <summary>
        ///     要素数が0でないことを検証する際の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <exception cref="InvalidOperationException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateListItemCountNotZero([DoesNotReturnIf(true)] bool isThrow, string itemName)
        {
            if (!isThrow) return;

            ListItemCountNotZero(itemName);
        }

        /// <summary>
        ///     引数を元にした操作によってリスト要素数が超過しないことを検証する際の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <param name="limit">要素上限数</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateListMaxItemCount(
            [DoesNotReturnIf(true)] bool isThrow,
            string itemName,
            int limit
        )
        {
            if (!isThrow) return;

            ListMaxItemCount(itemName, limit);
        }

        /// <summary>
        ///     リスト要素数が不足しないことを検証する際の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <param name="limit">要素下限数</param>
        /// <exception cref="InvalidOperationException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateListMinItemCount(
            [DoesNotReturnIf(true)] bool isThrow,
            string itemName,
            int limit
        )
        {
            if (!isThrow) return;

            ListMinItemCount(itemName, limit);
        }

        #endregion

        #region Validate Argument in Tow Dimensional List

        /// <summary>
        ///     二重リストの全行の要素数が一致することを検証する際の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="rowNum">エラー行数</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateTwoDimListInnerItemLength(
            [DoesNotReturnIf(true)] bool isThrow,
            int rowNum
        )
        {
            if (!isThrow) return;

            TwoDimListInnerItemLength(rowNum);
        }

        #endregion

        #region Suitable

        /// <summary>
        ///     引数不適切な場合の例外処理
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <param name="item">エラーメッセージ表示オブジェクト</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateArgumentUnsuitable(
            [DoesNotReturnIf(true)] bool isThrow,
            string itemName,
            object item
        )
        {
            if (!isThrow) return;

            ArgumentUnsuitable(itemName, item);
        }

        #endregion

        #region Not Execute

        /// <summary>
        ///     引数を理由に処理できない場合の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="message">エラーメッセージ</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateArgumentNotExecute(
            [DoesNotReturnIf(true)] bool isThrow,
            Func<string> message
        )
        {
            if (!isThrow) return;

            ArgumentNotExecute(message);
        }

        #endregion

        #endregion

        #region Invalid Operation

        /// <summary>
        ///     検証エラー時に <see cref="InvalidOperationException"/> を発生させる。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="message">エラーメッセージ</param>
        /// <exception cref="InvalidOperationException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void InvalidOperationIf(
            [DoesNotReturnIf(true)] bool isThrow,
            Func<string> message
        )
        {
            if (!isThrow) return;

            throw new InvalidOperationException(
                ErrorMessage.NotExecute(message())
            );
        }

        /// <summary>
        ///     検証エラー時に <see cref="InvalidOperationException"/> を発生させる。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="message">エラーメッセージ</param>
        /// <exception cref="InvalidCastException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void InvalidCastIf(
            [DoesNotReturnIf(true)] bool isThrow,
            Func<string>? message = null
        )
        {
            if (!isThrow) return;

            if (message is not null)
            {
                throw new InvalidCastException(
                    ErrorMessage.NotCast(message())
                );
            }

            throw new InvalidCastException();
        }

        #endregion

        #region NullPointer

        /// <summary>
        ///     <see langword="null"/> 検証時に <see langword="NullReferenceException"/> を発生させる。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <exception cref="NullReferenceException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateNotNull([DoesNotReturnIf(true)] bool isThrow)
        {
            if (!isThrow) return;

            throw new NullReferenceException();
        }

        #endregion

        #region NotSupport

        /// <summary>
        ///     サポートしていないメソッドの場合に例外を発生させる。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="caller">呼び出し元</param>
        /// <param name="targetName">対象メソッド名</param>
        /// <exception cref="NotSupportedException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        /// <typeparam name="T">エラーなし時の偽装戻り値</typeparam>
        public static T ObsoleteMethod<T>(
            [DoesNotReturnIf(true)] bool isThrow,
            Type caller,
            [CallerMemberName] string targetName = ""
        )
        {
            if (!isThrow) return default!;

            throw new NotSupportedException(ReflectionHelper.GetObsoleteMsg(caller, targetName));
        }

        #endregion

        #region Thrower

        [DoesNotReturn]
        public static void PropertyAccess(string reason, Exception? innerException = null)
            => throw new PropertyAccessException(
                ErrorMessage.NotAccess(reason),
                innerException
            );

        [DoesNotReturn]
        public static void PropertyNotNull(string itemName, Exception? innerException = null)
            => throw new PropertyNullException(
                ErrorMessage.NotNull(itemName),
                innerException
            );

        [DoesNotReturn]
        public static void PropertyItemsHasNotNull(string itemName, Exception? innerException = null)
            => throw new PropertyNullException(
                ErrorMessage.NotNullInList(itemName),
                innerException
            );

        [DoesNotReturn]
        public static void PropertyValueRange(
            string itemName,
            int target,
            IntOrStr min,
            IntOrStr max,
            Exception? innerException = null
        ) => throw new PropertyOutOfRangeException(
            ErrorMessage.OutOfRange(itemName, min, max, target),
            innerException
        );

        [DoesNotReturn]
        public static void ArgumentNotNull(string itemName, Exception? innerException = null)
            => throw new ArgumentNullException(
                ErrorMessage.NotNull(itemName),
                innerException
            );

        [DoesNotReturn]
        public static void ArgumentNotEmpty(string itemName, Exception? innerException = null)
            => throw new ArgumentNullException(
                ErrorMessage.NotNullInList(itemName),
                innerException
            );

        [DoesNotReturn]
        public static void ArgumentValueGreaterOrEqual(
            string itemName,
            IntOrStr limit,
            int itemValue,
            Exception? innerException = null
        ) => throw new ArgumentOutOfRangeException(
            ErrorMessage.GreaterOrEqual(itemName, limit, itemValue),
            innerException
        );

        [DoesNotReturn]
        public static void ArgumentValueLessOrEqual(
            string itemName,
            IntOrStr limit,
            int itemValue,
            Exception? innerException = null
        ) => throw new ArgumentOutOfRangeException(
            ErrorMessage.LessOrEqual(itemName, limit, itemValue),
            innerException
        );

        [DoesNotReturn]
        public static void ArgumentValueRange(
            string itemName,
            int target,
            IntOrStr min,
            IntOrStr max,
            Exception? innerException = null
        ) => throw new ArgumentOutOfRangeException(
            ErrorMessage.OutOfRange(itemName, min, max, target),
            innerException
        );

        [DoesNotReturn]
        public static void ArgumentNotEqual(
            string itemName,
            string otherName,
            Exception? innerException = null
        ) => throw new ArgumentException(
            ErrorMessage.NotEqual(itemName, otherName),
            innerException
        );

        [DoesNotReturn]
        public static void ArgumentNotMatch(
            string itemName,
            IntOrStr item,
            Exception? innerException = null
        ) => throw new ArgumentException(
            ErrorMessage.NotMatch(itemName, item),
            innerException
        );

        [DoesNotReturn]
        public static void ArgumentNotMatch(
            string itemName,
            IEnumerable<IntOrStr> items,
            Exception? innerException = null
        ) => throw new ArgumentException(
            ErrorMessage.NotMatch(itemName, items),
            innerException
        );

        [DoesNotReturn]
        public static void ArgumentNotNewLine(
            string itemName,
            string value,
            Exception? innerException = null
        ) => throw new ArgumentNewLineException(
            ErrorMessage.NotNewLine(itemName, value),
            innerException
        );

        [DoesNotReturn]
        public static void ArgumentNotRegex(
            string value,
            Regex regex,
            Exception? innerException = null
        ) => throw new ArgumentException(
            ErrorMessage.StringNotMatchRegex(value, regex),
            innerException
        );

        [DoesNotReturn]
        public static void OverDataSize(
            int maxSize,
            Exception? innerException = null
        ) => throw new ArgumentException(
            ErrorMessage.OverDataSize(maxSize),
            innerException
        );

        [DoesNotReturn]
        public static void ListRange(
            string indexArgName,
            string countArgName,
            Exception? innerException = null
        ) => throw new ArgumentException(
            ErrorMessage.ListRange(indexArgName, countArgName),
            innerException
        );

        [DoesNotReturn]
        public static void ListItemCountNotZero(
            string itemName,
            Exception? innerException = null
        ) => throw new InvalidOperationException(
            ErrorMessage.NotExecute($"{itemName}の要素が0個のため"),
            innerException
        );

        [DoesNotReturn]
        public static void ListMaxItemCount(
            string itemName,
            int limit,
            Exception? innerException = null
        ) => throw new ArgumentException(
            ErrorMessage.OverListLength(limit, itemName),
            innerException
        );

        [DoesNotReturn]
        public static void ListMinItemCount(
            string itemName,
            int limit,
            Exception? innerException = null
        ) => throw new ArgumentException(
            ErrorMessage.UnderListLength(limit, itemName),
            innerException
        );

        [DoesNotReturn]
        public static void TwoDimListInnerItemLength(
            int rowNum,
            Exception? innerException = null
        ) => throw new ArgumentException(
            ErrorMessage.TwoDimListInnerItemLength(rowNum),
            innerException
        );

        [DoesNotReturn]
        public static void ArgumentUnsuitable(
            string itemName,
            object item,
            Exception? innerException = null
        ) => throw new ArgumentException(
            ErrorMessage.Unsuitable(itemName, item),
            innerException
        );

        [DoesNotReturn]
        public static void ArgumentNotExecute(
            Func<string> message,
            Exception? innerException = null
        ) => throw new ArgumentException(
            ErrorMessage.NotExecute(message()),
            innerException
        );

        #endregion

        #region sentence structure

        public static void RethrowIfCatch(
            Action action,
            Action<Exception> rethrower
        )
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                rethrower(e);
            }
        }

        #endregion
    }
}
