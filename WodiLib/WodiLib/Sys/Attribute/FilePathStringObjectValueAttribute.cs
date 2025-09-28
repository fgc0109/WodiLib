// ========================================
// Project Name : WodiLib
// File Name    : FilePathStringObjectValueAttribute.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using System.Text.RegularExpressions;

namespace WodiLib.Sys
{
    /// <summary>
    ///     汎用ファイルパス文字列オブジェクトの設定属性（継承元クラス用）
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class FilePathStringObjectValueAttribute : CommonOneLineStringValueObjectAttribute
    {
        /// <inheritdoc/>
        [DefaultValue(32767)] // Windows の場合。 C# は文字列を UTF-16 で扱うため、単純に文字列長を取得するだけでファイルパス長の判定が可能
        public override int MaxLength { get; init; } = 0;

        /// <inheritdoc/>
        [DefaultValue(3)]
        public override int MinLength { get; init; } = 0;

        /// <inheritdoc/>
        [DefaultValue(true)]
        public override bool IsAllowEmpty { get; init; } = false;

        /// <inheritdoc/>
        [DefaultValue(RegexOptions.IgnoreCase)]
        public override RegexOptions PatternOption { get; init; } = default!;
    }
}
